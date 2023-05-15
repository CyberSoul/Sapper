using UnityEngine;
using UnityEngine.UI;

public class BtnTypeSelector : MonoBehaviour
{
    static int s_typesAmount = System.Enum.GetValues(typeof(ActionPress)).Length;
    [SerializeField] Sprite[] m_sprites = new Sprite[s_typesAmount];
    [SerializeField] string[] m_texts = new string[s_typesAmount];
    [SerializeField] Image m_view;
    [SerializeField] TMPro.TMP_Text m_text;

    // Start is called before the first frame update
    void Start()
    {
        ActionPress currentType = GameController.Instance.CurrentActionType;
        m_view.sprite = m_sprites[(int)currentType];
        m_text.text = m_texts[(int)currentType];
    }

    public void ChangeType()
    {
        ActionPress newType = GameController.Instance.CurrentActionType;
        if ((int)newType == s_typesAmount - 1)
        {
            newType = 0;
        }
        else
        {
            ++newType;
        }

        GameController.Instance.CurrentActionType = newType;
        m_view.sprite = m_sprites[(int)newType];
        m_text.text = m_texts[(int)newType];
    }

}
