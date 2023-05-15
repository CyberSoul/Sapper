using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    protected void Awake()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(Exit);
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}