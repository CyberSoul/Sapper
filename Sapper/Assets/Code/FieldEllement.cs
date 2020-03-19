using UnityEngine;
using UnityEngine.UI;

public class FieldEllement : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_spriteView;
    [SerializeField] Sprite m_mine;
    [SerializeField] Sprite[] m_numbers = new Sprite[9];

    bool isMineField = false;

    public bool IsMine
    {
        get { return isMineField; }
        set
        {
            isMineField = value;
            if (isMineField)
            {
                m_spriteView.sprite = m_mine;
            }
        }
    }

    public void SetMineAround(int a_mineAmount)
    {
        IsMine = false;
        if (a_mineAmount == 0)
        {
            m_spriteView.enabled = false;
            //canvas.SetActive(false);
        }
        else
        {
            m_spriteView.enabled = true;
            m_spriteView.sprite = m_numbers[a_mineAmount-1]; // -1 because nu,bers start from 1.
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
