using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class IntroUI : MonoBehaviour
{
    public Button StartButton, HighscoresButton, QuitButton;
    public GameObject HighscoreEntryPrefab;
    public Transform HighscoreParent;

    private void Start()
    {
        StartButton.onClick.AddListener(StartGame);
        HighscoresButton.onClick.AddListener(ShowHighscores);
        QuitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void ShowHighscores()
    {

    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

}
