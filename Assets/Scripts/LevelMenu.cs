using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;
    public GameObject[] levelButtons;

    private void Awake()
    {
        ButtonsToArray();

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }


        int maxUnlockableIndex = Mathf.Min(unlockedLevel, buttons.Length);

        for (int i = 0; i < maxUnlockableIndex; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "level " + levelId;
        SceneManager.LoadScene(levelName);
    }

    void ButtonsToArray()
    {
        List<Button> buttonList = new List<Button>();

        foreach (GameObject levelButton in levelButtons)
        {
            int childCount = levelButton.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Button btn = levelButton.transform.GetChild(i).gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    buttonList.Add(btn);
                }
            }
        }

        buttons = buttonList.ToArray();
    }

}
