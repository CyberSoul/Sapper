using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PresetViewController : MonoBehaviour
{
    [SerializeField] TMP_InputField inputName;
    [SerializeField] TMP_InputField inputWidth;
    [SerializeField] TMP_InputField inputHeight;
    [SerializeField] TMP_InputField inputMineCount;
    [Space]
    [SerializeField] ConfigSelector selector;

    MapConfigController configController;

    // Start is called before the first frame update
    void Start()
    {
        configController = MapConfigController.Instance;

        OnCurrentConfigChanged(configController.m_currentConfig);

        MapConfigController.OnChangedCurrentConfig += OnCurrentConfigChanged;

        inputName.onValueChanged.AddListener(OnNameChanged);
        inputWidth.onValueChanged.AddListener(OnWidthChanged);
        inputHeight.onValueChanged.AddListener(OnHeightChanged);
        inputMineCount.onValueChanged.AddListener(OnMinesChanged);
    }

    private void OnDestroy()
    {
        MapConfigController.OnChangedCurrentConfig -= OnCurrentConfigChanged;
    }

    public void OnCurrentConfigChanged(MapConfig config)
    {
        inputName.SetTextWithoutNotify(config.m_name);
        inputWidth.SetTextWithoutNotify(config.m_width.ToString());
        inputHeight.SetTextWithoutNotify(config.m_height.ToString());
        inputMineCount.SetTextWithoutNotify(config.m_mineAmount.ToString());
    }

    public void OnNameChanged(string value)
    {
        configController.m_currentConfig.m_name = value;
        selector.ChooseEmpty();
    }
    public void OnWidthChanged(string value)
    {
        configController.m_currentConfig.m_width = int.Parse(value);
        selector.ChooseEmpty();
    }
    public void OnHeightChanged(string value)
    {
        configController.m_currentConfig.m_height = int.Parse(value);
        selector.ChooseEmpty();
    }
    public void OnMinesChanged(string value)
    {
        configController.m_currentConfig.m_mineAmount = int.Parse(value);
        selector.ChooseEmpty();
    }
}
