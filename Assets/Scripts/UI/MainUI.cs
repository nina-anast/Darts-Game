using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainUI : MonoBehaviour
{
    public Button PauseButton;

    private void Start()
    {
        PauseButton.onClick.AddListener(Pause);
    }

    private void Pause()
    {
        SceneManager.LoadScene(0);
    }
}
