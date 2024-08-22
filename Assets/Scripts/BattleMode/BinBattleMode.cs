using System.Collections;
using UnityEngine;
using TMPro;
public class BinBattleMode : MonoBehaviour
{
    public TrashType acceptedTrashType;
    public TextMeshProUGUI warningText;
    public Transform playerTransform;
    public Transform monkeyTransform;
    public bool isMonkey;  // ตรวจสอบการตั้งค่านี้ให้ถูกต้อง

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        warningText.gameObject.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        TrashItem trashItem = other.GetComponent<TrashItem>();
        if (trashItem != null && !trashItem.isHeld)
        {
            if (trashItem.trashType == acceptedTrashType)
            {
                // ถูกประเภท
                HandleCorrectTrash(other.gameObject, trashItem);
            }
            else
            {
                // ผิดประเภท
                HandleIncorrectTrash(other.gameObject);
            }
        }
    }

    /*
        void HandleCorrectTrash(GameObject trashObject, TrashItem trashItem)
        {
            Destroy(trashObject);
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.correct);
            }
            Debug.Log(acceptedTrashType + " trash collected!");

            BattleModeManager.Instance.CollectTrash(true, isMonkey);  // ตรวจสอบการเรียกใช้ที่นี่
        }

        void HandleIncorrectTrash(GameObject trashObject)
        {
            Rigidbody2D rb = trashObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 bounceDirection = (trashObject.transform.position - transform.position).normalized;
                rb.AddForce(bounceDirection * 500f);
                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.incorrect);
                }
                Debug.Log("Incorrect trash type! Try again!");

                BattleModeManager.Instance.CollectTrash(false, isMonkey);  // ตรวจสอบการเรียกใช้ที่นี่
            }
        }*/
    void HandleCorrectTrash(GameObject trashObject, TrashItem trashItem)
    {
        Destroy(trashObject);
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.correct);
        }
        Debug.Log("HandleCorrectTrash: isMonkey = " + isMonkey);

        BattleModeManager.Instance.CollectTrash(true, isMonkey);
    }

    void HandleIncorrectTrash(GameObject trashObject)
    {
        Rigidbody2D rb = trashObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 bounceDirection = (trashObject.transform.position - transform.position).normalized;
            rb.AddForce(bounceDirection * 500f);
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.incorrect);
            }
            Debug.Log("HandleIncorrectTrash: isMonkey = " + isMonkey);

            BattleModeManager.Instance.CollectTrash(false, isMonkey);
        }
    }

}
