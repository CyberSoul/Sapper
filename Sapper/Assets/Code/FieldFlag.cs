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

    bool isChecked = false;

    public Sprite FlagView
    {
        get { return isChecked ? flagChecked : flagUnchecked; }
    }

    public void Action()
    {

    }

    public void OnEnable()
    {
        m_renderer.enabled = false;
    }

    public void AlternativeAction()
    {
        //if (!isChecked)
        {
            m_renderer.sprite = flagUnchecked;
            m_renderer.enabled = !m_renderer.enabled;
        }
    }
    public void Check()
    {
        isChecked = true;
        m_renderer.sprite = flagChecked;
        m_renderer.enabled = true;

    }
}
