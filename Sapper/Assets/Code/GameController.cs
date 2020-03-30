using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionPress
{
    Check,
    SafeCheck,
    Flag
}

public class GameController : MonoBehaviour
{
    static public GameController Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new GameController();
            }

            return s_instance;
        }
    }
    static GameController s_instance = null;

    public ActionPress CurrentActionType = ActionPress.Check;
    public MapGenerator ActiveMap;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(s_instance== null, "Game Controller Instance already exist");
        s_instance = this;
    }

}
