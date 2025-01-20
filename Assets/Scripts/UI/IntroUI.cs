using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using System.Linq;

// script for ui in intro screen.
public class IntroUI : MonoBehaviour
{
    public Button StartButton, HighscoresButton, QuitButton;
    public GameObject HighscoreEntryPrefab;
    public Transform HighscoreParent;
    public GameObject HighscoreCanvas;

    private void Start()
    {
        StartButton.onClick.AddListener(StartGame);
        HighscoresButton.onClick.AddListener(ShowHighscores);
        QuitButton.onClick.AddListener(QuitGame);
    }

    // loads main game scene
    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void ShowHighscores()
    {
        // activates canvas for Highscores
        HighscoreCanvas.gameObject.SetActive(!HighscoreCanvas.gameObject.activeSelf);
        // clear
        foreach (Transform child in HighscoreParent.GetComponentInChildren<Transform>())
        {
            if (child.name == "Label" || child.name == "Vert") continue;

            Destroy(child.gameObject);
        }
        // get list of highscores from .json
        List<Highscore> data = Utilities.LoadData<List<Highscore>>("Highscores.json");
        if (data == null || data.Count == 0) return;
        // if data exist, order data with throws (min to max)
        data = data.OrderBy(highscore=>highscore.Throws).ToList();

        // for each highscore make a new entry
        // text is defined in Highscore.cs
        foreach (Highscore highscore in data)
        {
            var entry = Instantiate(HighscoreEntryPrefab, HighscoreParent);
            entry.GetComponent<TextMeshProUGUI>().text = highscore.ToString();
        }
    }

    // close game
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

}
