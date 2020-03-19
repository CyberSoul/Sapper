using UnityEngine;

/*enum FlagState
{
    Checked,
    Unchecked
}*/

public class FieldFlag : MonoBehaviour
{
    [SerializeField] Sprite flagChecked;
    [SerializeField] Sprite flagUnchecked;

    bool isChecked;

    public Sprite FlagView
    {
        get { return isChecked ? flagChecked : flagUnchecked; }
    }

    public void Action()
    {

    }

    public void AlternativeAction()
    {

    }
}
