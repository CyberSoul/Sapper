using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem
{
    private static MessageSystem m_instance;

    public MessageSystem Instance
    {
        get { return m_instance; }
    }


    public delegate void MinePressedHandler();
    public delegate void UnlockFieldHandler(MapField a_field);

    public static event MinePressedHandler MinePressedEvent;
    public static event UnlockFieldHandler UnlockFieldEvent;
}
