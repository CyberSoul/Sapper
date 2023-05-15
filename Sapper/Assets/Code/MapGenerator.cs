using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] MapField m_fieldDark;
    [SerializeField] MapField m_fieldLight;
    [SerializeField] float m_widthOffset = 1.5f;
    [SerializeField] float m_heightOffset = 1.5f;
    [SerializeField] Transform m_mapRoot;
    [SerializeField] CinemachineTargetGroup m_targetGroup;

    //[SerializeField] CameraController m_camController;
    [SerializeField]
    MapConfig m_initialData;
    MapField[,] m_fields;
    
    public float WidthOffset
    {
        get { return m_widthOffset; }
    }
    public float HeightOffset
    {
        get { return m_heightOffset; }
    }

    public Vector2Int FieldSize
    {
        get { return new Vector2Int(m_initialData.m_width, m_initialData.m_height); }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (MapConfigController.Instance != null)
        {
            m_initialData = MapConfigController.Instance.m_currentConfig;
        }

        CreateMap();
    }

    private void AddCornerToTargetGroup()
    {
        if (m_targetGroup != null)
        {
            CinemachineTargetGroup.Target[] corners = new CinemachineTargetGroup.Target[4];
            int i = 0;
            Vector2Int fieldSize = FieldSize;
            corners[i++] = CinTargetFromFiled(0,0);
            corners[i++] = CinTargetFromFiled(fieldSize.x - 1, 0);
            corners[i++] = CinTargetFromFiled(0, fieldSize.y - 1);
            corners[i++] = CinTargetFromFiled(fieldSize.x - 1, fieldSize.y - 1);

            m_targetGroup.m_Targets = corners;
        }
    }

    private CinemachineTargetGroup.Target CinTargetFromFiled(int x, int y)
    {
        CinemachineTargetGroup.Target cinTarget = new CinemachineTargetGroup.Target();
        cinTarget.target = m_fields[x, y].transform;
        cinTarget.weight = 2;
        cinTarget.radius = 1.28f/2f;
        return cinTarget;
    }

    public void CreateMap(MapConfig a_mapConfig)
    {
        m_initialData = a_mapConfig;
        CreateMap();
    }

    public void RecreateMap()
    {
        ClearMap();
        CreateMap();
    }

    public void ClearMap()
    {
        for (int i = 0; i < m_initialData.m_width; ++i)
        {
            for (int j = 0; j < m_initialData.m_height; ++j)
            {
                Destroy(m_fields[i, j].gameObject);
            }
        }
        //Array.Clear(m_fields,0, m_fields.Length);
    }

    public void CreateMap()
    {
        m_fields = new MapField[m_initialData.m_width, m_initialData.m_height];

        int mineAmount = m_initialData.m_mineAmount;
        int minePercent = mineAmount;

        int fieldsAmount = m_initialData.m_width * m_initialData.m_height;

        int[] minePositions = new int[mineAmount];
        //TO_DO: change this generation at some day.
        {
            List<int> uniqueNumbers = new List<int>(fieldsAmount);
            for (int i = 0; i < fieldsAmount; ++i)
            {
                uniqueNumbers.Add(i);
            }

            for (int i = 0; i < mineAmount; ++i)
            {
                int ranPos = UnityEngine.Random.Range(0, uniqueNumbers.Count);
                minePositions[i] = uniqueNumbers[ranPos];
                uniqueNumbers.RemoveAt(ranPos);
            }
        }

        for (int i = 0; i < m_initialData.m_width; ++i)
        {
            for (int j = 0; j < m_initialData.m_height; ++j)
            {
                var field = Instantiate((i + j) % 2 == 0 ? m_fieldLight : m_fieldDark, m_mapRoot);
                field.transform.position = new Vector3(i * m_widthOffset, j * m_heightOffset, 0);
                int minesAround = 0;
                int currentPosIndex = i * m_initialData.m_width + j;
                Predicate<int> searchPredicate = (value) => value == currentPosIndex;
                if (Array.Exists(minePositions, searchPredicate))
                {
                    field.Ellement.IsMine = true;
                }
                else
                {
                    minesAround = 0;
                    for (int iIndex = Math.Max(i - 1, 0), iEnd = Math.Min(i + 2, m_initialData.m_width); iIndex < iEnd; ++iIndex)
                    {
                        for (int jIndex = Math.Max(j - 1, 0), jEnd = Math.Min(j + 2, m_initialData.m_height); jIndex < jEnd; ++jIndex)
                        {
                            currentPosIndex = iIndex * m_initialData.m_width + jIndex;
                            searchPredicate = (value) => value == currentPosIndex;
                            if (Array.Exists(minePositions, searchPredicate))
                            {
                                ++minesAround;
                            }
                        }
                    }
                    field.Ellement.SetMineAround(minesAround);
                }
                field.m_map = this;
                m_fields[i, j] = field;
            }
        }

        AddCornerToTargetGroup();
    }

    public void UnlockFields(MapField a_field)
    {
        int x = Mathf.RoundToInt(a_field.transform.position.x / m_widthOffset);
        int y = Mathf.RoundToInt(a_field.transform.position.y / m_heightOffset);

        UnlockFields(x, y);
        if (CheckForMinesEnd())
        {
            EventDispatcher.TriggerGameEnd(true);
        }
    }

    public void UnlockFields( int a_startedX, int a_startedY )
    {
        UnlockFieldsRecurs(a_startedX, a_startedY);
    }

    void UnlockFieldsRecurs(int a_x, int a_y )
    {
        if ( a_x < 0 || a_x >= m_initialData.m_width || a_y < 0 || a_y >= m_initialData.m_height )
        {
            return;
        }

        MapField checkedField = m_fields[a_x, a_y];
        if (checkedField.IsOpen || checkedField.Ellement.IsMine)
        {
            return;
        }

        checkedField.Action();

        if (checkedField.IsEmpty)
        {
            for ( int i = -1; i < 2; ++i )
            {
                for (int j = -1; j< 2;++j )
                {
                    UnlockFieldsRecurs(a_x +i, a_y+j);
                }
            }
        }

    }

    bool CheckForMinesEnd()
    {
        for (int i = 0; i < m_initialData.m_width; ++i)
        {
            for (int j = 0; j < m_initialData.m_height; ++j)
            {
                if (!m_fields[i, j].IsOpen && !m_fields[i, j].Ellement.IsMine )
                {
                    return false;
                }
            }
        }

        return true;
    }
}
