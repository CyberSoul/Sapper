using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class Timer : MonoBehaviour
{
    TMPro.TMP_Text m_text;

    float m_timeFromStart = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_text = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_timeFromStart += Time.deltaTime;
        if (m_text != null)
        {
            m_text.text = GetTimeSpent();
        }
    }

    public string GetTimeSpent()
    {
        return $"{((int)m_timeFromStart / 60)}:{((int)m_timeFromStart % 60):D2}";
    }

    public void ResetTime()
    {
        m_timeFromStart = 0;
    }
}