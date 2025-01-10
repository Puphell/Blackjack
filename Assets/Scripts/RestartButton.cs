using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public GameObject GameOverPanel;

    public void Restart()
    {
        GameOverPanel.SetActive(false);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
