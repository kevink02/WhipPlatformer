using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
    }
    public void HideScreenSettings()
    {
        _screenSettings.SetActive(false);
    }
}
