using System.Collections;
using UnityEngine;

public class TimeFreezeManager : MonoBehaviour
{
    public static TimeFreezeManager Instance { get; private set; }
    public float freezeDuration = 5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FreezeTime()
    {
        StartCoroutine(FreezeTimeCoroutine());
    }

    private IEnumerator FreezeTimeCoroutine()
    {
        // หยุดวัตถุทั้งหมด ยกเว้นผู้เล่น
        FreezeAllObjectsExceptPlayer(true);

        // Challenge แจ้งว่าเวลาใน Timer_Challenge ถูกหยุด
        Timer_Challenge[] timers = FindObjectsOfType<Timer_Challenge>();
        foreach (var timer in timers)
        {
            timer.SetTimeFrozen(true);
        }

        // Normal อัพเดทสถานะการหยุดเวลาใน GameManager
        GameManager.Instance.SetTimeFrozen(true);

        yield return new WaitForSecondsRealtime(freezeDuration);

        // คืนค่าการเคลื่อนไหวของวัตถุทั้งหมด
        FreezeAllObjectsExceptPlayer(false);

        // Challenge แจ้งว่าเวลาใน Timer_Challenge กลับมาทำงาน
        foreach (var timer in timers)
        {
            timer.SetTimeFrozen(false);
        }

        // Normal อัพเดทการหยุดเวลาใน GameManager
        GameManager.Instance.SetTimeFrozen(false);
    }


    private void FreezeAllObjectsExceptPlayer(bool isFrozen)
    {
        Rigidbody2D[] allRigidbodies = FindObjectsOfType<Rigidbody2D>();
        Animator[] allAnimators = FindObjectsOfType<Animator>();

        foreach (Rigidbody2D rb in allRigidbodies)
        {

            if (!rb.CompareTag("Player"))
            {
                if (isFrozen)
                {
                    //    rb.velocity = Vector2.zero;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                else
                {
                    rb.constraints = RigidbodyConstraints2D.None;
                }
            }
        }

        foreach (Animator anim in allAnimators)
        {

            if (!anim.CompareTag("Player"))
            {
                anim.enabled = !isFrozen;
            }
        }

        RobotController[] allRobots = FindObjectsOfType<RobotController>();
        foreach (RobotController robot in allRobots)
        {
            robot.enabled = !isFrozen;
        }

        Hunter[] allHunters = FindObjectsOfType<Hunter>();
        foreach (Hunter hunter in allHunters)
        {
            hunter.enabled = !isFrozen;
        }
    }

}