using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameOverUI : MonoBehaviour
{
    public Button RestartButton;
    public Button ExitButton;

    private void Start()
    {
        RestartButton.onClick.AddListener(Restart);
        ExitButton.onClick.AddListener(Exit);
    }

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }

    private void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
