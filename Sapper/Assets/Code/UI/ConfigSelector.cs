using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(TMP_Dropdown))]
public class ConfigSelector : MonoBehaviour
{
    [SerializeField] string emptyName = "-/-/-/-";
    MapConfigController configController;
    TMP_Dropdown m_dropdown;

    void Start()
    {
        m_dropdown = GetComponent<TMP_Dropdown>();
        m_dropdown.onValueChanged.AddListener(OnSelectOption);

        configController = MapConfigController.Instance;
        MapConfigController.OnUpdateConfigs += FillOptions;

        FillOptions();
    }

    private void OnDestroy()
    {
        MapConfigController.OnUpdateConfigs -= FillOptions;
    }

    public void FillOptions()
    {
        if (m_dropdown != null)
        {
            List<MapConfig> configs = configController.m_loadedConfig;
            m_dropdown.ClearOptions();

            m_dropdown.options.Add(new TMP_Dropdown.OptionData(emptyName));//add empty

            foreach (MapConfig config in configs)
            {
                m_dropdown.options.Add(new TMP_Dropdown.OptionData(config.m_name));
            }

            int selectedIndex = 0;
            string activeConfigName = configController.m_currentConfig.m_name;
            for (; selectedIndex < configs.Count; ++selectedIndex)
            {
                if (configs[selectedIndex].m_name == activeConfigName)
                {
                    ++selectedIndex;//Because o is empty;
                    break;
                }
            }

            m_dropdown.SetValueWithoutNotify(selectedIndex);
        }
    }

    private void OnSelectOption(int index)
    {
        if (index != 0)
        {
            configController.SelectFromLoaded(index - 1);
        }
    }

    public void ChooseEmpty()
    {
        m_dropdown.SetValueWithoutNotify(0);
    }
}
