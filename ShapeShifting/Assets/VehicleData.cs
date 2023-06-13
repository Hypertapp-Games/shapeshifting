using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class VehicleData : MonoBehaviour
{
    [Serializable]
    public struct SpeedOnTerrain
    {
        public String Terrain;
        public float speed;
        public float timeToChangeSpeed;
    }
    public List<SpeedOnTerrain> st;

    public List<TerrainsData> terrainCorresponding = new List<TerrainsData>();

    public bool isGroundVehicle;
    //public float normalspeed;
    public float slowspeed;
    //public float timeChangeSpeed = 0.1f;
    public GameObject cast;
    public float maxDistancecast = 1;
    int _terrainType;
    int _terrainTypeRealTime = 0;
    int _currentTerrainType = 0;
    public string terrainNameInTime;
    public string currentTerrainName;
    public float currentSpeed;
    public float terrainSpeed;

    public GameObject mesh;

    private void Start()
    {
        //_terrainType = terrainCorresponding[0].terrainType;
        if (gameObject.GetComponent<CharacterMove>() != null)
        {
            currentSpeed = gameObject.GetComponent<CharacterMove>().speed;
        }
        else if (gameObject.GetComponent<CarController>() != null)
        {
            currentSpeed = gameObject.GetComponent<CarController>().setCarSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (isGroundVehicle == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(cast.transform.position, cast.transform.forward,out hit, maxDistancecast))
            {
                //try
                //{
                //    _terrainTypeRealTime = Int32.Parse(hit.transform.gameObject.name); 
                //}
                //catch (Exception )
                //{
                //    _terrainTypeRealTime = 100;
                //}
                terrainNameInTime = hit.transform.gameObject.name;
            }
        }

        if (currentTerrainName != terrainNameInTime)
        {
            currentTerrainName = terrainNameInTime;
            float tCS = 0;
            bool Noinformation = true;
            for (int i = 0; i < st.Count; i++)
            {
                if(st[i].Terrain == currentTerrainName)
                {
                    terrainSpeed = st[i].speed;
                    tCS = st[i].timeToChangeSpeed;
                    Noinformation = false;
                    break;
                }
            }
            if(Noinformation == true)
            {
                terrainSpeed = slowspeed;
                tCS = 0.1f;
            }

            if (gameObject.GetComponent<CharacterMove>() != null)
            {
                gameObject.GetComponent<CharacterMove>().ChangeSpeed(tCS, currentSpeed, terrainSpeed);
            }
            else if (gameObject.GetComponent<CarController>() != null)
            {
                gameObject.GetComponent<CarController>().ChangeSpeed(tCS, currentSpeed, terrainSpeed);
            }
            currentSpeed = terrainSpeed;

            //if (_currentTerrainType == _terrainType)
            //{

            //    if (gameObject.GetComponent<CharacterMove>() != null)
            //    {
            //        gameObject.GetComponent<CharacterMove>().ChangeSpeed(timeChangeSpeed, slowspeed, normalspeed);
            //    }
            //    else if (gameObject.GetComponent<CarController>() != null)
            //    {
            //        gameObject.GetComponent<CarController>().ChangeSpeed(timeChangeSpeed, slowspeed, normalspeed);
            //    }
            //}
            //else
            //{
            //    if (gameObject.GetComponent<CharacterMove>() != null)
            //    {
            //        gameObject.GetComponent<CharacterMove>().ChangeSpeed(timeChangeSpeed, normalspeed,slowspeed);
            //    }
            //    else if (gameObject.GetComponent<CarController>() != null)
            //    {
            //        gameObject.GetComponent<CarController>().ChangeSpeed(timeChangeSpeed, normalspeed,slowspeed);
            //    }
            //}


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

