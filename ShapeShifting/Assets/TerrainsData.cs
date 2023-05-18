using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Terrain Data", menuName = "Terain Data")]
public class TerrainsData : ScriptableObject
{
    public GameObject terrain;
    public int terrainCod;
    public int maxInSuccesion;
    public int maxCall;
    [HideInInspector]public int maxCallTemp;
    public List<GameObject> attachObject = new List<GameObject>();
}
