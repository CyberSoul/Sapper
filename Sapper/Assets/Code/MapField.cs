﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapField : MonoBehaviour
{
    [SerializeField] FieldFlag m_flag;
    [SerializeField] GameObject m_grass;
    [SerializeField] GameObject m_earth;
    [SerializeField] FieldEllement m_ellement;

    public FieldEllement Ellement
    {
        get { return m_ellement; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action()
    {
        m_grass.SetActive( false );

    }

    public void AlternativeAction()
    {
        //  m_flag
        Debug.Log("AlternativeAction");
    }

}