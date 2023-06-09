using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Terrain Data", menuName = "Terain Data")]
public class TerrainsData : ScriptableObject
{
    public GameObject terrain;
    public int terrainCode;
    public int maxInSuccesion;
    public int maxCall;
    [HideInInspector]public int maxCallTemp;
    public List<GameObject> attachObject = new List<GameObject>();
    public int terrainType;
    public bool IsLock;
}
