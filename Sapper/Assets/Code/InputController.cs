using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] MapGenerator m_map;
    [SerializeField] float m_timeForLongTouch = 1f;

    Vector2 m_lastTouch;
    bool m_isTouched = false;
    float m_touchTimeStamp;
    int m_lastTouchCount = 0;

    [SerializeField] bool m_isWin = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isWin)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Mouse click");
                m_lastTouch = Input.mousePosition;
                //m_isTouched = true;
                m_touchTimeStamp = Time.unscaledTime;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //OnTouchEnd(m_lastTouch, false);
                OnTouchEnd(Input.mousePosition, false);
            }

            else if (Input.GetMouseButtonUp(1))
            {
                OnTouchEnd(Input.mousePosition, true);
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (!m_isTouched)
                {
                    m_isTouched = true;
                    m_touchTimeStamp = Time.unscaledTime;
                }
                Touch touch = Input.GetTouch(0);
                m_lastTouch = touch.position;
            }
            else
            {
                if (m_isTouched)
                {
                    bool isLongTouch = (Time.unscaledTime - m_touchTimeStamp) > m_timeForLongTouch;
                    OnTouchEnd(m_lastTouch, isLongTouch);
                    m_isTouched = false;
                }

            }
            //For feature check fro multitouches.
            if (m_lastTouchCount < Input.touchCount)
            {
            }
        }

    }

    void OnTouchEnd( Vector2 touchPos, bool a_isALternative )
    {
        var testPos = Camera.main.ScreenToWorldPoint(touchPos);
        Debug.Log($"pos.x = {touchPos.x}, pos.y = {touchPos.y}, testPos.x = {testPos.x}, testPos.y = {testPos.y} ");
        //bool isLongTouch = (Time.unscaledTime - m_touchTimeStamp) > m_timeForLongTouch;
        RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero);
        if (hit2D)
        {
            MapField field = hit2D.transform.GetComponent<MapField>();
            if (field)
            {
                Debug.Log("Object found");
                if (a_isALternative)
                {
                    field.AlternativeAction();
                }
                else
                {
                    if (field.Ellement.IsMine)
                    {
                        field.Action();
                        Debug.Log("You lose");
                        //m_map.CreateMap();
                        return;
                    }
                    else
                    {
                        m_map.UnlockFields(field);
                    }
                }
            }
        }
    }
}
