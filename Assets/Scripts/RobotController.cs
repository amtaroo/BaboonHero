/*
using UnityEngine;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{
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

    void Start()
    {
        totalTrashCount = GameObject.FindGameObjectsWithTag("Trash").Length;
        collectedTrashCount = 0;
        FindNextTrash();
        endGamePanel.SetActive(false);
    }

    void Update()
    {
        if (carryingTrash && currentBin != null)
        {
            MoveTowardsTarget(currentBin.transform.position);

            if (Vector2.Distance(transform.position, currentBin.transform.position) < 0.1f)
            {
                DropTrash();
            }
        }
        else if (targetTrash != null)
        {
            TrashItem targetTrashItem = targetTrash.GetComponent<TrashItem>();

            // ขยะยัง active อยู่และ player ไม่ได้ถือ
            if (!targetTrash.gameObject.activeInHierarchy || targetTrashItem.isBeingHeldByPlayer)
            {
                FindNextTrash();
            }
            else
            {
                MoveTowardsTarget(targetTrash.position);

                if (Vector2.Distance(transform.position, targetTrash.position) < 0.1f)
                {
                    PickUpTrash();
                }
            }
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

            // ตรวจสอบว่า player ไม่ได้ถือขยะ
            if (!targetTrashItem.isBeingHeldByPlayer)
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

    void GoToTrashBin()
    {
        if (currentBin != null)
        {
            MoveTowardsTarget(currentBin.transform.position);
        }
    }

    void DropTrash()
    {
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
            //    endGamePanel.SetActive(true);
        }
    }
}
*/

using UnityEngine;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{
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
        if (carryingTrash && currentBin != null)
        {
            MoveTowardsTarget(currentBin.transform.position);

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
                MoveTowardsTarget(targetTrash.position);

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

            if (!targetTrashItem.isBeingHeldByPlayer)
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

    void GoToTrashBin()
    {
        if (currentBin != null)
        {
            MoveTowardsTarget(currentBin.transform.position);
        }
    }

    void DropTrash()
    {
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

            if (movementDirection != Vector3.zero)
            {
                animator.SetFloat("MoveX", movementDirection.x);
                animator.SetFloat("MoveY", movementDirection.y);
            }
            else
            {
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", 0);
            }

            lastPosition = transform.position;
        }
    }
}

