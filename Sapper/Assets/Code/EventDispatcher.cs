using System;

public class EventDispatcher
{
    public static event Action<bool> GameEnd;

    public static void TriggerGameEnd(bool isWin)
    {
        if (GameEnd!= null)
        {
            GameEnd.Invoke(isWin);
        }
    }
}
