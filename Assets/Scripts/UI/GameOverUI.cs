using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

// ui for game over screen
public class GameOverUI : MonoBehaviour
{
    public Button RestartButton;
    public Button ExitButton;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Throws;

    private void Start()
    {
        RestartButton.onClick.AddListener(Restart);
        ExitButton.onClick.AddListener(Exit);
        Throws.text = $"Your throws were: {SavedData.Instance.Throws}";
    }

    private void Restart()
    {
        ResetGame();
        SceneManager.LoadScene(1);
    }

    private void Exit()
    {
        ResetGame();
        SceneManager.LoadScene(0);
    }

    // save and reset everything to start next game
    private void ResetGame()
    {
        // update singleton
        SavedData.Instance.UpdateName(Name.text);
        // save data to .json
        SavedData.Instance.Save();
        // reset singleton data
        SavedData.Instance.UpdateName("---");
        SavedData.Instance.UpdateThrows(0);
        SavedData.Instance.UpdateTime("---");
    }

}
