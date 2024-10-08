//LoadingScreen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject levelMenu;
    
    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;
    
    public void LoadLevelBtn(string levelToLoad)
    {
        levelMenu.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false;

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;

            if (loadOperation.progress >= 0.9f)
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        loadOperation.allowSceneActivation = true;
    }
}
