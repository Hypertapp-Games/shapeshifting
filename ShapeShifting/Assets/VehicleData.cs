using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class VehicleData : MonoBehaviour
{
    public List<TerrainsData> terrainCorresponding = new List<TerrainsData>();
    public bool isGroundVehicle;
    public float normalspeed;
    public float slowspeed;
    public float timeChangeSpeed = 0.1f;
    public GameObject cast;
    public float maxDistancecast = 1;
    int _terrainType;
    int _terrainTypeRealTime = 0;
    int _currentTerrainType = 0;
    private void Start()
    {
        _terrainType = terrainCorresponding[0].terrainType;
    }

    private void FixedUpdate()
    {
        if (isGroundVehicle == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(cast.transform.position, cast.transform.forward,out hit, maxDistancecast))
            {
                try
                {
                    _terrainTypeRealTime = Int32.Parse(hit.transform.gameObject.name); 
                }
                catch (Exception )
                {
                    _terrainTypeRealTime = 100;
                }
            }
        }

        if (_currentTerrainType != _terrainTypeRealTime)
        {
            _currentTerrainType = _terrainTypeRealTime;
            if (_currentTerrainType == _terrainType)
            {
                
                if (gameObject.GetComponent<CharacterMove>() != null)
                {
                    gameObject.GetComponent<CharacterMove>().ChangeSpeed(timeChangeSpeed, slowspeed, normalspeed);
                }
                else if (gameObject.GetComponent<CarController>() != null)
                {
                    gameObject.GetComponent<CarController>().ChangeSpeed(timeChangeSpeed, slowspeed, normalspeed);
                }
            }
            else
            {
                if (gameObject.GetComponent<CharacterMove>() != null)
                {
                    gameObject.GetComponent<CharacterMove>().ChangeSpeed(timeChangeSpeed, normalspeed,slowspeed);
                }
                else if (gameObject.GetComponent<CarController>() != null)
                {
                    gameObject.GetComponent<CarController>().ChangeSpeed(timeChangeSpeed, normalspeed,slowspeed);
                }
            }

            
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (isGroundVehicle == true)
        {
            Gizmos.color = Color.red;
            Debug.DrawLine(cast.transform.position, cast.transform.position + cast.transform.forward * maxDistancecast);
        }
    }
}

