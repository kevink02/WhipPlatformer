using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _screenPause, _buttonPause;
    private static bool _isPaused;
    public delegate void DelegateVoid();
    public static DelegateVoid DisablePause;
    private static DelegateVoid DoPause, UndoPause;

    private void Awake()
    {
        // Should always be set to false initially
        _isPaused = _screenPause.activeInHierarchy;
    }
    private void OnEnable()
    {
        DoPause += ShowScreenPause;
        UndoPause += HideScreenPause;
        DisablePause += DisablePausing;
    }
    private void OnDisable()
    {
        DoPause -= ShowScreenPause;
        UndoPause -= HideScreenPause;
        DisablePause -= DisablePausing;
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
        _buttonPause.SetActive(false);
        _isPaused = true;
        Time.timeScale = 0;
    }
    public void HideScreenPause()
    {
        _screenPause.SetActive(false);
        _buttonPause.SetActive(true);
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
    public static bool IsPaused()
    {
        return _isPaused;
    }
    public void DisablePausing()
    {
        _buttonPause.gameObject.SetActive(false);
    }
}
