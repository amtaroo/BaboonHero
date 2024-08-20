using UnityEngine;
using UnityEngine.AI;

public class BattleMonkey : MonoBehaviour
{
    NavMeshAgent robot;
    public Transform[] trashBins;
    private Transform targetTrash;
    private Transform currentBin;
    private TrashType currentTrashType;
    public float speed = 2.0f;
    private bool carryingTrash = false;

    public Transform holdSpot;
    private GameObject itemHolding;

    private Animator animator;
    private Vector3 lastPosition;

    void Awake()
    {
        robot = GetComponent<NavMeshAgent>();
        robot.updateRotation = false;
        robot.updateUpAxis = false;
    }

    void Start()
    {
        FindNextTrash();

        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        if (targetTrash == null)
        {
            FindNextTrash();
            return;
        }

        if (carryingTrash && currentBin != null)
        {
            robot.SetDestination(currentBin.position);

            if (Vector2.Distance(transform.position, currentBin.position) < 0.1f)
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
        }
    }

    void FindTrashBinForType(TrashType trashType)
    {
        foreach (Transform bin in trashBins)
        {
            BinBattleMode binBattleMode = bin.GetComponent<BinBattleMode>();
            if (binBattleMode != null && binBattleMode.acceptedTrashType == trashType)
            {
                currentBin = bin;
                return;
            }
        }
        currentBin = null;
    }

    void PickUpTrash()
    {
        Debug.Log("PickUpTrash");
        carryingTrash = true;
        itemHolding = targetTrash.gameObject;
        itemHolding.SetActive(true);

        itemHolding.transform.position = holdSpot.position;
        itemHolding.transform.parent = transform;

        //BattleModeManager.Instance.CollectTrash(true, true);

        targetTrash = null;
        GoToTrashBin();
    }


    void GoToTrashBin()
    {
        Debug.Log("GoToBin");
        if (currentBin != null)
        {
            robot.SetDestination(currentBin.position);
        }
    }

    void DropTrash()
{
    Debug.Log("DropTrash");
    if (itemHolding != null)
    {
        itemHolding.SetActive(false);
        itemHolding.transform.parent = null;

        // ให้คะแนนลิง
        BattleModeManager.Instance.CollectTrash(true, true);
    }
    carryingTrash = false;
    currentBin = null;
    FindNextTrash();
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
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", 0);
                animator.SetBool("isMoving", false); 
            }

            lastPosition = transform.position;
        }
    }
}
