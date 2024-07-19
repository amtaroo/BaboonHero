using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrashBin : MonoBehaviour
{
    public TrashType acceptedTrashType;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public TextMeshProUGUI warningText;
    public Transform playerTransform;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        warningText.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TrashItem trashItem = other.GetComponent<TrashItem>();
        if (trashItem != null)
        {
            if (trashItem.trashType == acceptedTrashType)
            {
                Destroy(other.gameObject);
                StartCoroutine(ShowMessage("Correct!", 1f, "#FFFFFF"));
                audioSource.PlayOneShot(correctSound);
                Debug.Log(acceptedTrashType + " trash collected!");

                GameManager.Instance.CollectTrash();
            }
            else
            {
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 bounceDirection = (other.transform.position - transform.position).normalized;
                    rb.AddForce(bounceDirection * 500f);
                    StartCoroutine(ShowMessage("Incorrect!", 1f, "#AE0000"));
                    audioSource.PlayOneShot(incorrectSound);
                    Debug.Log("Incorrect trash type! Try again!");
                }
            }
        }
    }

    IEnumerator ShowMessage(string message, float duration, string hexColor)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;
        Color textColor;
        if (ColorUtility.TryParseHtmlString(hexColor, out textColor))
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
