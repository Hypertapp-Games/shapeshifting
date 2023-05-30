using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

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
    [SerializeField]public int currentPiece = 0;
    [HideInInspector] public Vector3 currentVehiclePosition;
    
    public GameObject currentTerrain;
    public  Piece piece;
    private Vector3 positionInFrames;
    private Vector3 noMovementFrames;
    public float timeNoMovement = 0;
    public bool isFlying = false;
    

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
        StartCoroutine(CheckNoMovement());

    }

    public void Update()
    {
        if (_isStart)
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
    }

    public IEnumerator CheckNoMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!isPlayer)
            {
                positionInFrames = currentVehicle.gameObject.transform.localToWorldMatrix.GetPosition();
                double x = Math.Round(positionInFrames.x, 2);
                double y = Math.Round(positionInFrames.y, 2);
                double z = Math.Round(positionInFrames.z, 2);
                positionInFrames= new Vector3((float)x, (float)y, (float)z);
                if (noMovementFrames != positionInFrames)
                {
                    noMovementFrames = positionInFrames;
                    timeNoMovement = 0;
                }
                else
                {
                    Debug.Log(1);
                    timeNoMovement += 0.1f;
                }

                if (timeNoMovement >= 2)
                {
                    currentTerrain = terrainManager.transform.GetChild(currentPiece).gameObject;
                    AutoChangeVehicle();
                    timeNoMovement = 0;
                }
            }
        }
    }
    public void UpdateCurrentPosition()
    {
        var p = terrainManager.transform.GetChild(currentPiece).GetComponent<Piece>().endPoint;
        currentPosition = p.transform.localToWorldMatrix.GetPosition();
        currentPiece++;
        currentTerrain = terrainManager.transform.GetChild(currentPiece -1 ).gameObject;
        if (!isPlayer)
        {
            if (!isFlying)
            {
                AutoChangeVehicle();
            }
            else
            {
                for (int i = 0; i < vehicle.Count; i++)
                {
                    if (vehicle[i].name == "Glide")
                    {
                        choseVehicle = vehicle[i];
                        SwapVehicle();
                    }
                }
            }
        }
    }

    public void AutoChangeVehicle()
    {
        Debug.Log("AutoChangeVehicle");
        if (currentTerrain.GetComponentInChildren<Obstacle>() != null)
        {
            Debug.Log(currentTerrain.GetComponentInChildren<Obstacle>().name);
            piece = currentTerrain.GetComponentInChildren<Obstacle>().gameObject.GetComponent<Piece>();
        }
        else
        {
            piece = currentTerrain.GetComponent<Piece>();
        }

        CheckVehicle();
        
        
  
    }

    public void CheckVehicle()
    {
        for (int i = 0; i < piece.vehicleCanMoveIn.Count; i++)
        {
            for (int j = 0; j < vehicle.Count; j++)
            {
                if (vehicle[j].name == piece.vehicleCanMoveIn[i].name)
                {
                    Debug.Log(vehicle[j].name);
                    if (piece.name == "Fly Piece")
                    {
                        choseVehicle = vehicle[j];
                        SwapVehicle();
                        Debug.Log("StartFly");
                        isFlying = true;
                    }
                    else
                    {
                        BotSwapVehicle(j);  
                    }
                    return;
                }
            }
        }
    }
    
    public void BotSwapVehicle(int index)
    {
        StartCoroutine(_BotSwapVehicle(index));
    }

    public IEnumerator _BotSwapVehicle(int index)
    {
        float t = UnityEngine.Random.Range(0.7f, 1.3f);
        yield return new WaitForSeconds(t);
        choseVehicle = vehicle[index];
        SwapVehicle();
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
               vhc.name = prefabVehicles[i].name;
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
        if (currentVehicle != choseVehicle)
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

            if (isPlayer)
            {
                cam.player = currentVehicle;
            }
        }
        
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
