using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using System.Linq;

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

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void ShowHighscores()
    {
        HighscoreCanvas.gameObject.SetActive(!HighscoreCanvas.gameObject.activeSelf);
        // clear
        foreach (Transform child in HighscoreParent.GetComponentInChildren<Transform>())
        {
            if (child.name == "Label" || child.name == "Vert") continue;

            Destroy(child.gameObject);
        }

        List<Highscore> data = Utilities.LoadData<List<Highscore>>("Highscores.json");
        if (data == null || data.Count == 0) return;
        data = data.OrderBy(highscore=>highscore.Throws).ToList();

        foreach (Highscore highscore in data)
        {
            var entry = Instantiate(HighscoreEntryPrefab, HighscoreParent);
            entry.GetComponent<TextMeshProUGUI>().text = highscore.ToString();
        }
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
