using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] GameObject pausePopup;
    [SerializeField] GameObject map;

    void Start()
    {
        SetPausedState(false);
    }

    public void ChangePause()
    {
        bool isPaused = Time.timeScale == 0;
        SetPausedState(!isPaused);
    }

    public void SetPausedState(bool isPause)
    {
        Time.timeScale = isPause ? 0 : 1;
        pausePopup.SetActive(isPause);
        map.SetActive(!isPause);
    }

}
