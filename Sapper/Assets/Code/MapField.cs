using System;
using UnityEngine;
using UnityEngine.Events;

public class MapField : MonoBehaviour
{
    public static event Action HelpUsed;
    public static event Action<int> MineMarked;

    [SerializeField] FieldFlag m_flag;
    [SerializeField] GameObject m_grass;
    [SerializeField] GameObject m_earth;
    [SerializeField] FieldEllement m_ellement;

    [SerializeField] bool m_isOpen = false;
    [SerializeField] float m_timeForLongTouch = 1f;

    [Space]
    [SerializeField] UnityEvent OnDig;
    [SerializeField] UnityEvent OnFlagPlace;
    [SerializeField] UnityEvent OnFlagUnplace;
    [SerializeField] UnityEvent OnFlagChecked;

    public MapGenerator m_map;

    /*float m_touchTimeStamp;
    int m_lastTouchCount = 0;
    bool m_isTouched = false;

    public void OnMouseDown()
    {
        m_isTouched = true;
        m_touchTimeStamp = Time.unscaledTime;
    }*/

    public void OnMouseUpAsButton()
    {
        if (!IsOpen)
        {
            switch (GameController.Instance.CurrentActionType)
            {
                case ActionPress.Check:
                    if (m_ellement.IsMine)
                    {
                        if (!m_flag.IsChecked)
                        {
                            Action();
                            EventDispatcher.TriggerGameEnd(false);
                        }
                    }
                    else
                    {
                        OnDig.Invoke();
                        //GameController.Instance.ActiveMap.UnlockFields(this);
                        m_map.UnlockFields(this);
                    }
                    break;
                case ActionPress.SafeCheck:
                    OnFlagChecked.Invoke();
                    SafeCHeck();
                    if (HelpUsed != null)
                    {
                        HelpUsed.Invoke();
                    }
                    break;
                case ActionPress.Flag:
                    AlternativeAction();
                    break;
            }
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
        /*if (m_isTouched)
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
        m_isTouched = false;*/
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
        m_map = GameController.Instance.ActiveMap;
    }

    public void Action()
    {
        m_grass.SetActive( false );
        m_isOpen = true;
    }

    public void AlternativeAction()
    {
        if (!m_flag.IsChecked)
        {
            bool isPlaced = m_flag.AlternativeAction();

            if (MineMarked != null)
            {
                MineMarked.Invoke(isPlaced ? 1 : -1);
            }

            if (isPlaced)
            {
                OnFlagPlace.Invoke();
            }
            else
            {
                OnFlagUnplace.Invoke();
            }
        }
    }

    public void SafeCHeck()
    {

        if (m_ellement.IsMine)
        {
            m_flag.Check();

            if (MineMarked != null)
            {
                MineMarked.Invoke(1);
            }
        }
        else
        {
            GameController.Instance.ActiveMap.UnlockFields(this);
        }
    }

}
