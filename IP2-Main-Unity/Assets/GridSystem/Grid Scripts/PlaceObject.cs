using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    //varibles
    public bool Placed { get; private set; } //has the tower been placed
    public Vector3Int size{get; private set;} //the size of the tower so we can check if its in a grid square
    private Vector3[] Vertices;
    
    
    
    //methods
    private void GetPositions()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>(); 
        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }
    
    private void CalculateSize()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = Building.current.gridSize.WorldToCell(worldPos);
            
            size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), Math.Abs((vertices[0] - vertices[3]).y), 1);
        }
        
    }

    public Vector3 getStartPos()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    private void Start()
    {
        GetPositions();
        CalculateSize();
        Debug.Log($"PlaceObject size in cells = {size}"); //debug

    }

    public virtual void Place()
    {
        
        Placed = true;
        
        //any extra events, effects timer etc...
        
        
    }
    
    
}
