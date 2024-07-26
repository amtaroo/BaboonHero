using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    public GameObject continueButton;
    public AudioClip hiSound;
    private AudioSource audioSource;
    private int index;
    public float wordSpeed = 0.1f;
    private bool playerIsClose;
    private Coroutine typingCoroutine;
    private AudioManager audioManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
                ResumeGame();
            }
            else
            {
                dialoguePanel.SetActive(true);
                typingCoroutine = StartCoroutine(Typing());
                PauseGame();
                PlayHiSound();
            }
        }

        if (dialogueText.text == dialogue[index])
        {
            continueButton.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && dialoguePanel.activeInHierarchy && typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogue[index];
            typingCoroutine = null;
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        ResumeGame();
    }

    IEnumerator Typing()
    {
        dialogueText.text = "";
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(wordSpeed);
        }
        typingCoroutine = null;
    }

    public void NextLine()
    {
        continueButton.SetActive(false);
        if (index < dialogue.Length - 1)
        {
            index++;
            typingCoroutine = StartCoroutine(Typing());
        }
        else
        {
            zeroText();
            ResumeGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void PlayHiSound()
    {
        if (audioSource != null && hiSound != null && audioManager != null)
        {
            audioManager.PlaySFX(audioManager.NPC);
        }
    }
}
