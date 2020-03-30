using UnityEngine;
using UnityEngine.UI;

public class BtnTypeSelector : MonoBehaviour
{
    static int s_typesAmount = System.Enum.GetValues(typeof(ActionPress)).Length;
    [SerializeField] Sprite[] m_sprites = new Sprite[s_typesAmount];
    [SerializeField] Image m_view;
    // Start is called before the first frame update
    void Start()
    {
        ActionPress currentType = GameController.Instance.CurrentActionType;
        m_view.sprite = m_sprites[(int)currentType];
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

}
