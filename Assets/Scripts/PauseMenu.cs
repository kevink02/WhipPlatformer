using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _screenPause;
    public delegate void DelegateVoid();
    public static DelegateVoid DoPause, UndoPause;

    private void OnEnable()
    {
        DoPause += ShowScreenPause;
        UndoPause += HideScreenPause;
    }
    private void OnDisable()
    {
        DoPause -= ShowScreenPause;
        UndoPause -= HideScreenPause;
    }
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
