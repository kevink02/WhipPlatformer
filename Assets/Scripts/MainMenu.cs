using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _screenMain;
    [SerializeField]
    private GameObject _screenSettings;

    public void SwitchSceneToLevel()
    {
        SceneManager.LoadScene(sceneName: "LevelScene");
    }
    public void ExitGame()
    {
        Debug.Log($"{name}: Exiting game");
        Application.Quit();
    }
    public void ShowScreenSettings()
    {
        _screenSettings.SetActive(true);
        // Default screen size = 1920 x 1080 -> 1920 is length, 1920 / 2 = 960 (half of length of screen), 960 / 2 = 480 (half of half the length)
        _screenMain.transform.localPosition = -480 * Vector2.right;
        _screenSettings.transform.localPosition = 480 * Vector2.right;
    }
    public void HideScreenSettings()
    {
        _screenSettings.SetActive(false);
        _screenMain.transform.localPosition = Vector2.zero;
        _screenSettings.transform.localPosition = Vector2.zero;
    }
}
