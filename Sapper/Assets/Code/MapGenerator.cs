﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct MapConfig
{
    public int m_width;
    public int m_height;
    public int m_mineAmount;
}

public class MapGenerator : MonoBehaviour
{
    [SerializeField] MapConfig m_initialData;
    [SerializeField] MapField m_fieldDark;
    [SerializeField] MapField m_fieldLight;
    [SerializeField] float m_widthOffset = 1.5f;
    [SerializeField] float m_heightOffset = 1.5f;

    MapField[,] m_fields;

    public float WidthOffset
    {
        get { return m_widthOffset; }
    }
    public float HeightOffset
    {
        get { return m_heightOffset; }
    }

    public Vector2 FieldSize
    {
        get { return new Vector2(m_initialData.m_width, m_initialData.m_height); }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreateMap(MapConfig a_mapConfig)
    {
        m_initialData = a_mapConfig;
        CreateMap();
    }

    public void CreateMap()
    {
        m_fields = new MapField[m_initialData.m_width, m_initialData.m_height];

        int mineAmount = m_initialData.m_mineAmount;
        int minePercent = mineAmount;

        int maxMineChance = m_initialData.m_width * m_initialData.m_height;
        for (int i = 0; i < m_initialData.m_width; ++i)
        {
            for (int j = 0; j < m_initialData.m_height; ++j)
            {
                var field = Instantiate((i + j) % 2 == 0 ? m_fieldLight : m_fieldDark);
                field.transform.position = new Vector3(i * m_widthOffset, j * m_heightOffset, 0);

                int rand = UnityEngine.Random.Range(1, maxMineChance);
                field.Ellement.IsMine = rand < minePercent;
                if (rand < minePercent)
                {
                    --mineAmount;
                    minePercent = 0;
                }

                minePercent += mineAmount;

                m_fields[i, j] = field;
            }
        }

        //Fill mines around
        int minesAround = 0;
        for (int i = 0; i < m_initialData.m_width; ++i)
        {
            for (int j = 0; j < m_initialData.m_height; ++j)
            {
                minesAround = 0;
                if ( !m_fields[i, j].Ellement.IsMine )
                {
                    for (int iIndex = Math.Max(i -1, 0), iEnd = Math.Min(i+2, m_initialData.m_width); iIndex < iEnd; ++iIndex)
                    {
                        for (int jIndex = Math.Max(j -1, 0), jEnd = Math.Min(j+2, m_initialData.m_height); jIndex < jEnd; ++jIndex)
                        {
                            if (m_fields[iIndex, jIndex].Ellement.IsMine)
                            {
                                ++minesAround;
                            }
                        }
                    }

                    m_fields[i, j].Ellement.SetMineAround(minesAround);
                }
            }
        }
    }
}
