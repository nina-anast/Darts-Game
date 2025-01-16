using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

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

    private void ResetGame()
    {
        SavedData.Instance.UpdateName(Name.text);
        SavedData.Instance.Save();
        SavedData.Instance.UpdateName("---");
        SavedData.Instance.UpdateThrows(0);
        SavedData.Instance.UpdateTime("---");
    }

}
