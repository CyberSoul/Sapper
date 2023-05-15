using UnityEngine;

/*enum FlagState
{
    Checked,
    Unchecked
}*/

public class FieldFlag : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_renderer;
    [SerializeField] Sprite flagChecked;
    [SerializeField] Sprite flagUnchecked;

    bool m_isChecked = false;

    public Sprite FlagView
    {
        get { return m_isChecked ? flagChecked : flagUnchecked; }
    }

    public bool IsChecked
    {
        get { return m_isChecked; }
}

    public void Action()
    {

    }

    public void OnEnable()
    {
        m_renderer.enabled = false;
    }

    public bool AlternativeAction()
    {
        //if (!isChecked)
        {
            m_renderer.sprite = flagUnchecked;
            m_renderer.enabled = !m_renderer.enabled;
            return m_renderer.enabled;
        }
    }
    public void Check()
    {
        m_isChecked = true;
        m_renderer.sprite = flagChecked;
        m_renderer.enabled = true;

    }
}
