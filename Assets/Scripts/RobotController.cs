using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    NavMeshAgent robot;
    public Transform[] trashBins;
    private Transform targetTrash;
    private TrashBin currentBin;
    private TrashType currentTrashType;
    public float speed = 2.0f;
    private bool carryingTrash = false;
    public GameObject endGamePanel;

    private int totalTrashCount;
    private int collectedTrashCount;
    public Transform holdSpot;
    private GameObject itemHolding;

    private Animator animator;
    private Vector3 lastPosition;

    // gift
    private bool hasGift = false;
    private float helpDuration = 30.0f;
    private float helpTimer = 0.0f;

    void Awake()
    {
        robot = GetComponent<NavMeshAgent>();
        robot.updateRotation = false;
        robot.updateUpAxis = false;
    }

    void Start()
    {
        totalTrashCount = GameObject.FindGameObjectsWithTag("Trash").Length;
        collectedTrashCount = 0;
        FindNextTrash();
        endGamePanel.SetActive(false);

        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        if (!hasGift)
        {

            CheckForGift();
            return;
        }

        helpTimer += Time.deltaTime;
        if (helpTimer > helpDuration)
        {

            StopHelping();
            return;
        }

        if (targetTrash == null)
        {
            FindNextTrash();
            return;
        }
        if (carryingTrash && currentBin != null)
        {
            robot.SetDestination(currentBin.transform.position);

            if (Vector2.Distance(transform.position, currentBin.transform.position) < 0.1f)
            {
                DropTrash();
            }
        }
        else if (targetTrash != null)
        {
            TrashItem targetTrashItem = targetTrash.GetComponent<TrashItem>();

            if (!targetTrash.gameObject.activeInHierarchy || targetTrashItem.isBeingHeldByPlayer)
            {
                FindNextTrash();
            }
            else
            {
                robot.SetDestination(targetTrash.position);

                if (Vector2.Distance(transform.position, targetTrash.position) < 0.1f)
                {
                    PickUpTrash();
                }
            }
        }

        UpdateAnimation();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("gift"))
        {
            Debug.Log("Robot received the gift!");
            hasGift = true;
            helpTimer = 0.0f;
            other.gameObject.SetActive(false);
        }
    }



    void CheckForGift()
    {

        GameObject gift = GameObject.FindGameObjectWithTag("gift");
        if (gift != null && Vector2.Distance(transform.position, gift.transform.position) < 0.1f)
        {
            hasGift = true;
            helpTimer = 0.0f;
            gift.SetActive(false);  
        }
    }

    void StopHelping()
    {
        hasGift = false;
        targetTrash = null;
        carryingTrash = false;
        currentBin = null;

    }

    void GoToTrashBin()
    {
        if (currentBin != null)
        {
            robot.SetDestination(currentBin.transform.position);
        }
    }

    void FindNextTrash()
    {
        GameObject[] trashObjects = GameObject.FindGameObjectsWithTag("Trash");
        float closestDistance = Mathf.Infinity;
        Transform closestTrash = null;

        foreach (GameObject trash in trashObjects)
        {
            TrashItem targetTrashItem = trash.GetComponent<TrashItem>();

            if (!targetTrashItem.isBeingHeldByPlayer && trash.activeInHierarchy)
            {
                float distance = Vector2.Distance(transform.position, trash.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTrash = trash.transform;
                }
            }
        }

        if (closestTrash != null)
        {
            targetTrash = closestTrash;
            currentTrashType = targetTrash.GetComponent<TrashItem>().trashType;
            FindTrashBinForType(currentTrashType);
        }
        else
        {
            targetTrash = null;
            CheckGameEnd();
        }
    }

    void FindTrashBinForType(TrashType trashType)
    {
        foreach (Transform bin in trashBins)
        {
            TrashBin trashBin = bin.GetComponent<TrashBin>();
            if (trashBin != null && trashBin.trashType == trashType)
            {
                currentBin = trashBin;
                return;
            }
        }
        currentBin = null;
    }

    void PickUpTrash()
    {
        carryingTrash = true;
        itemHolding = targetTrash.gameObject;
        itemHolding.SetActive(true);

        itemHolding.transform.position = holdSpot.position;
        itemHolding.transform.parent = transform;

        collectedTrashCount++;

        targetTrash = null;
        GoToTrashBin();
    }

    void DropTrash()
    {
        if (itemHolding != null)
        {
            itemHolding.SetActive(false);
            itemHolding.transform.parent = null;
        }
        carryingTrash = false;
        currentBin = null;
        FindNextTrash();
    }

    void MoveTowardsTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void CheckGameEnd()
    {
        if (collectedTrashCount >= totalTrashCount)
        {
            endGamePanel.SetActive(true);
        }
    }



    void UpdateAnimation()
    {
        if (animator != null)
        {
            Vector3 movementDirection = transform.position - lastPosition;

            float movementThreshold = 0.01f;

            if (movementDirection.sqrMagnitude > movementThreshold * movementThreshold)
            {
                animator.SetFloat("MoveX", movementDirection.x);
                animator.SetFloat("MoveY", movementDirection.y);
                animator.SetBool("isMoving", false);
            }
            else
            {
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", 0);
                animator.SetBool("isMoving", true); // เล่น animation idle
            }

            lastPosition = transform.position;
        }
    }


}
