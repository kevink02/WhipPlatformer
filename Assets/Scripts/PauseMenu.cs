using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _screenPause;

    public void SwitchSceneToMain()
    {
        SceneManager.LoadScene(sceneName: "MainMenu");
    }
    public void ShowScreenPause()
    {
        _screenPause.SetActive(true);
        Time.timeScale = 0;
    }
    public void HideScreenPause()
    {
        _screenPause.SetActive(false);
        Time.timeScale = 1;
    }
}
