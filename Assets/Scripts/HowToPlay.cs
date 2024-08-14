using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    public GameObject HowToPlayPanel;
    public void OnClickTutorial()
    {
        HowToPlayPanel.SetActive(true);
    }
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

}
