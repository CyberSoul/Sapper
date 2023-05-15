using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class MineCounter : MonoBehaviour
{
    TMP_Text m_text;

    int m_counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_text = GetComponent<TMP_Text>();
        MapField.MineMarked += MineMarked;

        m_counter = MapConfigController.Instance.m_currentConfig.m_mineAmount;
        UpdateString();
    }
    private void OnDestroy()
    {
        MapField.MineMarked -= MineMarked;
    }

    private void MineMarked(int modif)
    {
        m_counter -= modif;
        UpdateString();
    }

    void UpdateString()
    {
        m_text.text = m_counter.ToString();
    }
}
