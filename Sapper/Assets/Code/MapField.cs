using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapField : MonoBehaviour
{
    [SerializeField] FieldFlag m_flag;
    [SerializeField] GameObject m_grass;
    [SerializeField] GameObject m_earth;
    [SerializeField] FieldEllement m_ellement;

    [SerializeField] bool m_isOpen = false;

    [SerializeField] float m_timeForLongTouch = 1f;

    public MapGenerator m_map;

    float m_touchTimeStamp;
    int m_lastTouchCount = 0;
    bool m_isTouched = false;

    public void OnMouseDown()
    {
        m_isTouched = true;
           m_touchTimeStamp = Time.unscaledTime;
    }

    public void OnMouseUpAsButton()
    {
        switch (GameController.Instance.CurrentActionType)
        {
            case ActionPress.Check:
                if (m_ellement.IsMine)
                {
                    if (!m_flag.IsChecked)
                    {
                        Action();
                        Debug.Log("You lose");
                        //m_map.CreateMap();
                    }
                }
                else
                {
                    //gameObject.SendMessageUpwards("UnlockFields", this);
                    GameController.Instance.ActiveMap.UnlockFields(this);
                // m_map.UnlockFields(this); //TO_DO: rework to global event system
                }
                break;
            case ActionPress.SafeCheck:
                SafeCHeck();
                break;
            case ActionPress.Flag:
                AlternativeAction();
                break;
        }

       /* bool isLongTouch = (Time.unscaledTime - m_touchTimeStamp) > m_timeForLongTouch;
        Debug.Log($"Press on {transform.position.x}, {transform.position.y}");
        if (isLongTouch)
        {
            AlternativeAction();
        }
        else
        {
            if (Ellement.IsMine)
            {
                Action();
                Debug.Log("You lose");
                //m_map.CreateMap();
                return;
            }
            else
            {
                //gameObject.SendMessageUpwards("UnlockFields", this);
                m_map.UnlockFields(this); //TO_DO: rework to global event system
            }
        }*/
    }

    public void OnMouseUp()
    {
        return;
        if (m_isTouched)
        {
            bool isLongTouch = (Time.unscaledTime - m_touchTimeStamp) > m_timeForLongTouch;
            Debug.Log($"Press on {transform.position.x}, {transform.position.y}");
            if (isLongTouch)
            {
                AlternativeAction();
            }
            else
            {
                if (Ellement.IsMine)
                {
                    Action();
                    Debug.Log("You lose");
                    //m_map.CreateMap();
                    return;
                }
                else
                {
                    m_map.UnlockFields(this); //TO_DO: rework to global event system
                }
            }
        }
        m_isTouched = false;
    }

    public bool IsOpen
    {
        get { return m_isOpen; }
    }
    public bool IsEmpty
    {
        get { return m_ellement.GetFieldType() == FieldType.Empty; }
    }

    public FieldEllement Ellement
    {
        get { return m_ellement; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action()
    {
        m_grass.SetActive( false );
        m_isOpen = true;
    }

    public void AlternativeAction()
    {
        m_flag.AlternativeAction();
        //  m_flag
        Debug.Log("AlternativeAction");
    }

    public void SafeCHeck()
    {

        if (m_ellement.IsMine)
        {
            m_flag.Check();
        }
        else
        {
            GameController.Instance.ActiveMap.UnlockFields(this);
        }
    }

}
