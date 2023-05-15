using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndController : MonoBehaviour
{
    [SerializeField] GameObject m_endPopup;
    [SerializeField] Timer m_timer;
    [SerializeField] TMP_Text m_resultTxt;
    [SerializeField] TMP_Text m_timeTxt;
    [SerializeField] TMP_Text m_helpsTxt;

    [Space]
    [SerializeField] string m_strWin;
    [SerializeField] string m_strLose;
    [SerializeField] string m_strTime;
    [SerializeField] string m_strHelps;

    [Space]
    [SerializeField] Color m_colorWin;
    [SerializeField] Color m_colorLose;

    [Space]
    [SerializeField] AudioSource m_audioSource;
    [SerializeField] AudioClip m_audioWin;
    [SerializeField] AudioClip m_audioLose;


    int m_helpUsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher.GameEnd += OnGameEnd;
        MapField.HelpUsed += OnHelpUsed;
    }

    private void OnDestroy()
    {
        EventDispatcher.GameEnd -= OnGameEnd;
        MapField.HelpUsed -= OnHelpUsed;
    }

    public void OnGameEnd(bool isWin)
    {
        m_endPopup.SetActive(true);
        Time.timeScale = 0f;
        m_resultTxt.text = isWin ? m_strWin : m_strLose;
        m_resultTxt.color = isWin ? m_colorWin: m_colorLose;
        m_timeTxt.text = m_strTime + m_timer.GetTimeSpent();
        m_helpsTxt.text = m_strHelps + m_helpUsed;
        m_audioSource.clip = isWin ? m_audioWin : m_audioLose;
        m_audioSource.Play();
    }

    private void OnHelpUsed()
    {
        ++m_helpUsed;
    }
}
