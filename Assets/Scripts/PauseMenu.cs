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
    }
    public void HideScreenPause()
    {
        _screenPause.SetActive(false);
    }
}
