using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class SceneLoader : MonoBehaviour
{
    [SerializeField] int sceneIndex;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(LoadScene);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
