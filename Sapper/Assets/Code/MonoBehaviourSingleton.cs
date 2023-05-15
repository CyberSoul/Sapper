using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance = null;

    public static T Instance
    {
        get { return instance; }
    }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            GameObject.DontDestroyOnLoad(instance);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}