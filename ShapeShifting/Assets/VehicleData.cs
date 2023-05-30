using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VehicleData : MonoBehaviour
{
    public List<TerrainsData> TerrainCorresponding = new List<TerrainsData>();
    public int CorrespondingCode;
    public bool isGroundVehicle;
    public float normalspeed;
    public float slowspeed;
    public float timeChange = 0.1f;
    public GameObject cast;
    public float maxDistancecast = 1;
    public int CurrentCorrespondingCode = 0;
    public int checkChangeCCCode = 0;
    private void Start()
    {
        CorrespondingCode = TerrainCorresponding[0].VehicleCode;
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
                    CurrentCorrespondingCode = Int32.Parse(hit.transform.gameObject.name); 
                }
                catch (Exception )
                {
                    CurrentCorrespondingCode = 100;
                }
            }
        }

        if (checkChangeCCCode != CurrentCorrespondingCode)
        {
            checkChangeCCCode = CurrentCorrespondingCode;
            if (checkChangeCCCode == CorrespondingCode)
            {
                
                if (gameObject.GetComponent<CharacterMove>() != null)
                {
                    //ChangeSpeed(slowspeed,normalspeed, gameObject.GetComponent<CharacterMove>().speed);
                    gameObject.GetComponent<CharacterMove>().ChangeSpeed(timeChange, slowspeed, normalspeed);
                }
                else if (gameObject.GetComponent<CarController>() != null)
                {
                    gameObject.GetComponent<CarController>().ChangeSpeed(timeChange, slowspeed, normalspeed);
                }
            }
            else
            {
                if (gameObject.GetComponent<CharacterMove>() != null)
                {
                    //ChangeSpeed(normalspeed,slowspeed, gameObject.GetComponent<CharacterMove>().speed);
                    gameObject.GetComponent<CharacterMove>().ChangeSpeed(timeChange, normalspeed,slowspeed);
                }
                else if (gameObject.GetComponent<CarController>() != null)
                {
                    gameObject.GetComponent<CarController>().ChangeSpeed(timeChange, normalspeed,slowspeed);
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

    void ChangeSpeed(float _from, float _to , float _speed)
    {
        StartCoroutine(timeChange.Tweeng(x => { _speed = x; }, _from, _to));
    }
}

