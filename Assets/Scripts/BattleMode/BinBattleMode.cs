using System.Collections;
using UnityEngine;
using TMPro;

public class BinBattleMode : MonoBehaviour
{
    public TrashType acceptedTrashType;
    public TextMeshProUGUI warningText;
    public Transform playerTransform; // This can be set to either the player or monkey
    public bool isMonkey;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        warningText.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TrashItem trashItem = other.GetComponent<TrashItem>();
        if (trashItem != null)
        {
            // Check if the trash type matches the accepted type for this bin
            if (trashItem.trashType == acceptedTrashType)
            {
                // Correct trash type
                HandleCorrectTrash(other.gameObject, trashItem);
            }
            else
            {
                // Incorrect trash type
                HandleIncorrectTrash(other.gameObject);
            }
        }
    }

    void HandleCorrectTrash(GameObject trashObject, TrashItem trashItem)
    {
        Destroy(trashObject);
        StartCoroutine(ShowMessage("Correct!", 1f, "#FFFFFF"));
        audioManager.PlaySFX(audioManager.correct);
        Debug.Log(acceptedTrashType + " trash collected!");

        BattleModeManager.Instance.CollectTrash(true, isMonkey);
    }

    void HandleIncorrectTrash(GameObject trashObject)
    {
        Rigidbody2D rb = trashObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 bounceDirection = (trashObject.transform.position - transform.position).normalized;
            rb.AddForce(bounceDirection * 500f);
            StartCoroutine(ShowMessage("Incorrect!", 1f, "#AE0000"));
            audioManager.PlaySFX(audioManager.incorrect);
            Debug.Log("Incorrect trash type! Try again!");

            BattleModeManager.Instance.CollectTrash(false, isMonkey);
        }
    }

    IEnumerator ShowMessage(string message, float duration, string hexColor)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;

        if (ColorUtility.TryParseHtmlString(hexColor, out Color textColor))
        {
            warningText.color = textColor;
        }

        float timer = 0f;
        while (timer < duration)
        {
            warningText.transform.position = Camera.main.WorldToScreenPoint(playerTransform.position + new Vector3(0, 1, 0));
            timer += Time.deltaTime;
            yield return null;
        }

        warningText.gameObject.SetActive(false);
    }
}
