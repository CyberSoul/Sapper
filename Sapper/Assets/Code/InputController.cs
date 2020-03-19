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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
                OnTouchEnd();
                m_isTouched = false;
            }

        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse click");
            m_lastTouch = Input.mousePosition;
            //m_isTouched = true;
            m_touchTimeStamp = Time.unscaledTime;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnTouchEnd();
        }

        //For feature check fro multitouches.
        if (m_lastTouchCount < Input.touchCount)
        {
        }

    }

    void OnTouchEnd()
    {
        Debug.Log($"timeDiff = {Time.unscaledTime - m_touchTimeStamp}");
        bool isLongTouch = (Time.unscaledTime - m_touchTimeStamp) > m_timeForLongTouch;
        Debug.Log($"isLongTouch = {isLongTouch}");
        RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(m_lastTouch), Vector2.zero);
        if (hit2D)
        {
            MapField field = hit2D.transform.GetComponent<MapField>();
            if (isLongTouch)
            {
                field.Action();
            }
            else
            {
                field.AlternativeAction();
            }
        }
    }
}
