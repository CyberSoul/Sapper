using UnityEngine;
using UnityEngine.UI;

public enum FieldType
{
    Empty,
    Mine,
    Number
}

public class FieldEllement : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_spriteView;
    [SerializeField] Sprite m_mine;
    [SerializeField] Sprite[] m_numbers = new Sprite[9];

    FieldType m_type = FieldType.Empty;

    public bool IsMine
    {
        get { return m_type == FieldType.Mine; }
        set
        {
            m_type = value ? FieldType.Mine : FieldType.Empty;
            if (value)
            {
                m_spriteView.sprite = m_mine;
            }
        }
    }

    public FieldType GetFieldType()
    {
        return m_type;
    }

    public void SetMineAround(int a_mineAmount)
    {
        IsMine = false;
        if (a_mineAmount == 0)
        {
            m_type = FieldType.Empty;
            m_spriteView.enabled = false;
            //canvas.SetActive(false);
        }
        else
        {
            m_type = FieldType.Number;
            m_spriteView.enabled = true;
            m_spriteView.sprite = m_numbers[a_mineAmount-1]; // -1 because nu,bers start from 1.
        }
    }
}
