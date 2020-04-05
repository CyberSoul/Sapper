using System.Collections;
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
    float m_FieldOfView;

    [SerializeField] MapConfig m_initialData;
    [SerializeField] MapField m_fieldDark;
    [SerializeField] MapField m_fieldLight;
    [SerializeField] float m_widthOffset = 1.5f;
    [SerializeField] float m_heightOffset = 1.5f;

    [SerializeField] CameraController m_camController;

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
                var field = Instantiate((i + j) % 2 == 0 ? m_fieldLight : m_fieldDark);
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
    }

    public void PositionateCamera()
    {
        Camera cam = Camera.main;
        var pixelWidth = cam.pixelWidth;
        var pixelHeight = cam.pixelHeight;

        m_camController.SetRequiredZoom(m_initialData.m_width* m_widthOffset, m_initialData.m_height*m_heightOffset);

        m_camController.m_activeZoom = 1;

        //cam.transform.position = new Vector3(pixelWidth/2, pixelHeight/2,-10);

        /*var pixlRect = cam.pixelRect;
        pixlRect.width /= 2;
        pixlRect.height /= 2;
        cam.pixelRect = pixlRect;*/

        // cam.pixelRect = new Rect(0,0, m_widthOffset*m_initialData.m_width, m_heightOffset*m_initialData.m_height);

        cam.orthographicSize +=1;
        cam.fieldOfView += 1;

        Debug.Log($"pixelHeight = {cam.pixelHeight}, pixelWidth = {cam.pixelWidth}, scaledHeight = {cam.scaledPixelHeight}, scaledWidth = {cam.scaledPixelWidth}");
        Debug.Log($"cam.orthographicSize = {cam.orthographicSize}");

    }
    void OnGUI()
    {
        //Set up the maximum and minimum values the Slider can return (you can change these)
        float max, min;
        max = 150.0f;
        min = 20.0f;
        //This Slider changes the field of view of the Camera between the minimum and maximum values
        m_FieldOfView = GUI.HorizontalSlider(new Rect(20, 20, 100, 40), m_FieldOfView, min, max);
    }

    public void UnlockFields(MapField a_field)
    {
        int x = Mathf.RoundToInt(a_field.transform.position.x / m_widthOffset);
        int y = Mathf.RoundToInt(a_field.transform.position.y / m_heightOffset);

        UnlockFields(x, y);
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
}
