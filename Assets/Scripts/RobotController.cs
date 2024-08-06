using UnityEngine;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{
    public Transform[] trashBins; // Array ของถังขยะ
    private Transform targetTrash; // เป้าหมายวัตถุ Trash
    private TrashBin currentBin; // ถังขยะที่กำลังไป
    private TrashType currentTrashType; // ประเภทของขยะที่ถืออยู่
    public float speed = 2.0f; // ความเร็วของลิง
    private bool carryingTrash = false; // สถานะว่าลิงกำลังถือขยะอยู่หรือไม่
    public GameObject endGamePanel; // Panel สำหรับแสดงเมื่อเกมจบ

    private int totalTrashCount;
    private int collectedTrashCount;

    void Start()
    {
        totalTrashCount = GameObject.FindGameObjectsWithTag("Trash").Length;
        collectedTrashCount = 0;
        FindNextTrash();
        endGamePanel.SetActive(false); // ซ่อน End Game panel ตอนเริ่มเกม
    }

    void Update()
    {
        if (carryingTrash && currentBin != null)
        {
            // ถ้าลิงกำลังถือขยะ ให้เดินไปที่ถังขยะที่ถูกประเภท
            MoveTowardsTarget(currentBin.transform.position);

            if (Vector2.Distance(transform.position, currentBin.transform.position) < 0.1f)
            {
                DropTrash();
            }
        }
        else if (targetTrash != null)
        {
            // ถ้าลิงยังไม่ได้ถือขยะ ให้เดินไปที่ขยะ
            MoveTowardsTarget(targetTrash.position);

            if (Vector2.Distance(transform.position, targetTrash.position) < 0.1f)
            {
                PickUpTrash();
            }
        }
    }

    void FindNextTrash()
    {
        GameObject[] trashObjects = GameObject.FindGameObjectsWithTag("Trash");
        float closestDistance = Mathf.Infinity; // เริ่มต้นด้วยค่าระยะทางที่มากที่สุด
        Transform closestTrash = null;

        foreach (GameObject trash in trashObjects)
        {
            float distance = Vector2.Distance(transform.position, trash.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTrash = trash.transform;
            }
        }

        if (closestTrash != null)
        {
            targetTrash = closestTrash; // กำหนดเป้าหมายเป็นขยะที่ใกล้ที่สุด
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
        carryingTrash = true; // กำหนดสถานะว่ากำลังถือขยะ
        targetTrash.gameObject.SetActive(false); // ซ่อนขยะไว้ (จำลองการถือ)
        collectedTrashCount++; // เพิ่มจำนวนขยะที่เก็บได้
        targetTrash = null;
        GoToTrashBin();
    }

    void GoToTrashBin()
    {
        if (currentBin != null)
        {
            MoveTowardsTarget(currentBin.transform.position); // นำทางไปยังถังขยะที่ถูกประเภท
        }
    }

    void DropTrash()
    {
        carryingTrash = false; // ลิงไม่ได้ถือขยะแล้ว
        currentBin = null;
        GameManager.Instance.CollectTrash();
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
             // แสดง End Game panel
        }
    }
}
