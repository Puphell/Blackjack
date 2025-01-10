using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public GameObject infoPanel;

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Info()
    {
        infoPanel.SetActive(true);
    }

    public void InfoExit()
    {
        infoPanel.SetActive(false);
    }
}
