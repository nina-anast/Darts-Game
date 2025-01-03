using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class MainUI : MonoBehaviour
{
    public Button PauseButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PauseButton.onClick.AddListener(Pause);
    }

    private void Pause()
    {
        SceneManager.LoadScene(0);
    }
}
