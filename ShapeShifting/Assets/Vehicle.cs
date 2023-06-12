using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vehicle", menuName = "Vehicle")]
public class Vehicle : ScriptableObject
{
    [System.Serializable]
    public struct VehicleLock
    {
        public GameObject vhc;
        public TerrainsData terrain;

    }
    public List<VehicleLock> vehicleLock;

    public List<GameObject> VehicleInGame;
    public List<TerrainsData> TerrainInGame;
    public int CurrentUnlock;
    public int process;
    public List<int> LevelUnlock;
    public bool InProcessUnlock;
    public GameObject vehiclekUseInNextLevel;

}
