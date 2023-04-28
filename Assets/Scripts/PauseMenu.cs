using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _screenPause;
    private static bool _isPaused;
    public delegate void DelegateVoid();
    private static DelegateVoid DoPause, UndoPause;

    private void Awake()
    {
        // Should always be false
        _isPaused = _screenPause.activeInHierarchy;
    }
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
        // Unpause the game, which prevents the game from appearing stuck upon returning back to the level from the main menu
        TogglePause();
        SceneManager.LoadScene(sceneName: "MainMenu");
    }
    public void ShowScreenPause()
    {
        _screenPause.SetActive(true);
        _isPaused = true;
        Time.timeScale = 0;
    }
    public void HideScreenPause()
    {
        _screenPause.SetActive(false);
        _isPaused = false;
        Time.timeScale = 1;
    }
    public static void TogglePause()
    {
        if (!_isPaused)
            DoPause?.Invoke();
        else
            UndoPause?.Invoke();
    }
}
