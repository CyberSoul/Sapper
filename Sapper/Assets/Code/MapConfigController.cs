using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Events;

[Serializable]
public struct MapConfig
{
    const char c_delimerField = ';';

    public string m_name;
    public int m_width;
    public int m_height;
    public int m_mineAmount;
    
    public MapConfig(string configStr)
    {
        string[] dataFileds = configStr.Split(c_delimerField);

        m_name = dataFileds[0];
        m_width = int.Parse(dataFileds[1]);
        m_height = int.Parse(dataFileds[2]);
        m_mineAmount = int.Parse(dataFileds[3]);
    }

    public MapConfig(MapConfig other)
    {
        m_name=other.m_name;
        m_width = other.m_width;
        m_height = other.m_height;
        m_mineAmount = other.m_mineAmount;
    }

    public string ToString()
    {
        return $"{m_name}{c_delimerField}{m_width}{c_delimerField}{m_height}{c_delimerField}{m_mineAmount}";
    }
}

public class MapConfigController : MonoBehaviourSingleton<MapConfigController>
{
    const char c_delimerLine = '\n';

    [SerializeField] string configPath;
    /*[SerializeField] UnityEvent OnUpdateConfigs;
    [SerializeField] UnityEvent<MapConfig> OnChangedCurrentConfig;*/

    public static Action OnUpdateConfigs;
    public static Action<MapConfig> OnChangedCurrentConfig;


    public List<MapConfig> m_loadedConfig = new List<MapConfig>();
    public MapConfig m_currentConfig;

   /* protected override void Awake()
    {
        base.Awake();

        //WriteToFile();
    }*/

    private void Start()
    {
        if (configPath != "")
        {
            LoadFromFile();
            if (m_loadedConfig.Count == 0)
            {
                m_currentConfig = new MapConfig();
                m_currentConfig.m_name = "Лёгкий";
                m_currentConfig.m_width = 9;
                m_currentConfig.m_height = 9;
                m_currentConfig.m_mineAmount = 9;
                m_loadedConfig.Add(m_currentConfig);
            }

            SelectFromLoaded(0);

            if (OnUpdateConfigs != null)
            {
                OnUpdateConfigs.Invoke();
            }
        }
    }

    public void WriteToFile()
    {
        string path = Path.Combine(Application.streamingAssetsPath, this.configPath);

        string text = "";
        for (int i = 0; i < m_loadedConfig.Count; ++i)
        {
            text += m_loadedConfig[i].ToString();
            if (i != (m_loadedConfig.Count - 1))
            {
                text += c_delimerLine;
            }
        }

        // Write the modified string back to the text file
        File.WriteAllText(path, text);
    }

    public void SelectFromLoaded(int index)
    {
        if (index < 0 || index >= m_loadedConfig.Count)
        {
            return;
        }

        m_currentConfig = m_loadedConfig[index];
        if (OnChangedCurrentConfig != null)
        {
            OnChangedCurrentConfig.Invoke(m_currentConfig);
        }
    }

    public void DeleteCurrentConfig()
    {
        RemoveConfig(m_currentConfig.m_name);
        if (m_loadedConfig.Count > 0)
        {
            m_currentConfig = m_loadedConfig[0];
        }
        else 
        {
            m_currentConfig = new MapConfig();
        }

        WriteToFile();

        if (OnUpdateConfigs != null)
        {
            OnUpdateConfigs.Invoke();
        }

        if (OnChangedCurrentConfig != null)
        {
            OnChangedCurrentConfig.Invoke(m_currentConfig);
        }
    }

    private void RemoveConfig(string name)
    {
        for (int i = 0; i < m_loadedConfig.Count; ++i)
        {
            if (m_loadedConfig[i].m_name == name)
            {
                m_loadedConfig.RemoveAt(i);
                return;
            }
        }
    }

    public void SaveCurrentConfig()
    {
        AddCurrentConfig();
        WriteToFile();

        if (OnUpdateConfigs != null)
        {
            OnUpdateConfigs.Invoke();
        }
    }

    private void AddCurrentConfig()
    {
        for (int i = 0; i < m_loadedConfig.Count; ++i)
        {
            if (m_loadedConfig[i].m_name == m_currentConfig.m_name)
            {
                m_loadedConfig[i] = m_currentConfig;
                return;
            }
        }

        m_loadedConfig.Add(m_currentConfig);
    }

    private void LoadFromFile()
    {
        string path = Path.Combine(Application.streamingAssetsPath, this.configPath);
        if (!File.Exists(path))
        {
            Debug.LogError($"Can not find config, path = {path}");
            return;
        }

        string fileData = File.ReadAllText(path);
        string[] configDatas = fileData.Split(c_delimerLine);
        foreach (string configStr in configDatas)
        {
            if (configStr != "")
            {
                MapConfig config = new MapConfig(configStr);
                m_loadedConfig.Add(config);
            }
        }
    }
}
