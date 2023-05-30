using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    [HideInInspector] public List<GameObject> prefabVehicles = new List<GameObject>();
    public List<GameObject> vehicle = new List<GameObject>();

    public GameObject currentVehicle;
    public GameObject choseVehicle;
    public FollowPlayer cam;
    
    private bool _isStart = false;
    public bool isPlayer;
    [Header("InMap")]
    public GameObject terrainManager;
    [HideInInspector]public Vector3 currentPosition = new Vector3(0,0,0);
    [HideInInspector]public int currentPiece = 0;
    [HideInInspector] public Vector3 currentVehiclePosition;

    void Start()
    {
        if (isPlayer)
        {
            cam.player = currentVehicle;
        }
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2.3f);
        _isStart = true;
        currentVehicle.GetComponent<CharacterMove>().enabled = true;

    }

    public void Update()
    {
        currentVehiclePosition = currentVehicle.transform.localToWorldMatrix.GetPosition();
        if (currentVehiclePosition.x > currentPosition.x)
        {
            UpdateCurrentPosition();
            if (currentVehicle.GetComponent<HelicopterController>() != null)
            {
                currentVehicle.GetComponent<HelicopterController>().UpdateCurentHight();
            }
            if (CheckEnd() && isPlayer)
            {
                gameManager._uiManager.EndGame();
                cam.enabled = false;
            }
        }
    }
    public void UpdateCurrentPosition()
    {
        var p = terrainManager.transform.GetChild(currentPiece).GetComponent<Piece>().endPoint;
        currentPosition = p.transform.localToWorldMatrix.GetPosition();
        currentPiece++;
    }
    public bool CheckEnd()
    {
        if (currentPiece >= terrainManager.transform.childCount)
        {
            return true;
        }
        return false;
    }

    public void SpawnVehicle()
    {
        for (int i = 0; i < prefabVehicles.Count; i++)
        {
            if (prefabVehicles[i].name == currentVehicle.name)
            {
                vehicle.Add(currentVehicle);
            }
            else
            {
               var vhc= Instantiate(prefabVehicles[i], currentVehicle.transform.localToWorldMatrix.GetPosition(),
                    currentVehicle.transform.rotation);
               vhc.gameObject.SetActive(false);
               vhc.transform.SetParent(gameObject.transform);
               vehicle.Add(vhc);
               if (vhc.GetComponent<HelicopterController>() != null)
               {
                   vhc.GetComponent<HelicopterController>().playerManager = gameObject.GetComponent<PlayerManager>();
               }
            }
        }
    }
    public void Vehicle0()
    {
        if (_isStart)
        {
            choseVehicle = vehicle[0];
            SwapVehicle();
        }
       
    }
    public void Vehicle1()
    {
        if (_isStart)
        {
            choseVehicle = vehicle[1];
            SwapVehicle();
        }
    }
    public void Vehicle2()
    {
        if (_isStart)
        {
            choseVehicle = vehicle[2];
            SwapVehicle();
        }
    }
    
    public void SwapVehicle()
    {
        choseVehicle.transform.position = currentVehicle.transform.localToWorldMatrix.GetPosition();
        currentVehicle.gameObject.SetActive(false);
        currentVehicle = choseVehicle;
        if (currentVehicle.GetComponent<CarController>() != null)
        {
            currentVehicle.GetComponent<CarController>().AddForce();
        }
        currentVehicle.transform.eulerAngles = new Vector3(0,90,0);
        currentVehicle.gameObject.SetActive(true);

        if (currentVehicle.GetComponent<HelicopterController>()!= null)
        {
            currentVehicle.GetComponent<HelicopterController>().UpdateCurentHight();
        }
        cam.player = currentVehicle;
    }
    
    // public IEnumerator ThisUpdateCurrentPosition()
    // {
    //     while(true)
    //     {
    //         yield return new WaitForSeconds(Time.deltaTime);
    //         currentVehiclePosition = currentVehicle.transform.localToWorldMatrix.GetPosition();
    //         if(currentVehiclePosition.x > terrainManager.currentPosition.x)
    //         {
    //             terrainManager.UpdateCurrentPosition();
    //             if (currentVehicle.GetComponent<HelicopterController>() != null)
    //             {
    //                 currentVehicle.GetComponent<HelicopterController>().UpdateCurentHight();
    //             }
    //
    //             if (terrainManager.CheckEnd() == true)
    //             {
    //                 _uiManager.EndGame();
    //                 Cam.enabled = false;
    //             }
    //         }
    //     }
    // }

    // public void RandomVehicleUseLevel()
    // {
    //     List<TerrainsData> temptr = new List<TerrainsData>();
    //     for (int i = 0; i < 3; i++)
    //     {
    //         var vhr = Random.Range(0, AllVehicle.Count);
    //         GameObject vhc = AllVehicle[vhr];
    //         _uiManager.AutoScroll(vhc.name,i);
    //         Vehicle.Add(vhc);
    //         botController.UpdateVehicleInUse(vhr);
    //          List<TerrainsData> cpd = vhc.GetComponent<VehicleData>().Corresponding;
    //          for (int j = 0; j < cpd.Count; j++)
    //          {
    //              temptr.Add(cpd[j]);
    //          }
    //         AllVehicle.Remove(vhc);
    //     }
    //     _spawnTerrains.terrainsData = temptr.Distinct().ToList();
    //     _spawnTerrains.SetUpTerrainsData();
    //     terrainManager.UpdateCurrentPosition();
    //     StartCoroutine(ThisUpdateCurrentPosition());
    // }
    
}
