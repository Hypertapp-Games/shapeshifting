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
    [Header("GetCurrentPosition")]
    public GameObject currentTerrain;
    public  Piece piece;
    private Vector3 positionInFrames;
    private Vector3 noMovementFrames;
    public float timeNoMovement = 0;
    public bool isFlying = false;

    [HideInInspector] public float startPositionX;
    [HideInInspector] public float endPositionX;
    [HideInInspector] public float distanceX;
    [HideInInspector] public float currentPositionX;
    [HideInInspector] public float distanceTraveled;
    public List<GameObject> _click = new List<GameObject>();
    public List<GameObject> bootSpeedBtn = new List<GameObject>();
    public GameObject changVehicleEffect;


    void Start()
    {
        if (isPlayer)
        {
            cam.player = currentVehicle;
        }
        //StartCoroutine(StartGame());
        
    }

    public void StartGame()
    {
        //yield return new WaitForSeconds(2.3f);
        startPositionX = currentVehicle.transform.localToWorldMatrix.GetPosition().x;
        endPositionX = terrainManager.transform.GetChild(terrainManager.transform.childCount - 1)
            .GetComponent<Piece>().startPoint.gameObject.transform.localToWorldMatrix.GetPosition().x;
        distanceX = endPositionX - startPositionX;

        _isStart = true;
        currentVehicle.GetComponent<CharacterMove>().enabled = true;
        StartCoroutine(CheckNoMovement());
        

    }

    public void Update()
    {
        currentVehiclePosition = currentVehicle.transform.localToWorldMatrix.GetPosition();
        currentPositionX = currentVehiclePosition.x;
        distanceTraveled = currentPositionX - startPositionX;
        if (_isStart)
        {
            if (currentVehiclePosition.x > currentPosition.x)
            {
                UpdateCurrentPosition();
                if (currentVehicle.GetComponent<HelicopterController>() != null)
                {
                    currentVehicle.GetComponent<HelicopterController>().UpdateCurentHight();
                }
                if (CheckEnd() && isPlayer)
                {
                   
                    cam.enabled = false;
                    gameManager.EndGame();
                }
                else if(CheckEnd())
                {
                    gameManager.ordinal++;
                }
            }
        }

        try
        {
            if (piece.name == "Fly Piece")
            {
                isFlying = true;
            }
        }
        catch
        {
            
        }
        
    }

    public IEnumerator CheckNoMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            
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
                //Debug.Log(1);
                timeNoMovement += 0.1f;
            }

            if (timeNoMovement >= 2)
            {
                var temp1 = terrainManager.transform.GetChild(currentPiece - 1).gameObject;
                var temp2 = terrainManager.transform.GetChild(currentPiece).gameObject;
                if (temp1.name == "River Piece" || temp2.name == "Up Piece" || temp2.name == "Wood Piece" || temp2.name == "Wall Piece")
                {
                    currentTerrain = temp2;
                }
                else
                {
                    currentTerrain = temp1;
                }
       
                //currentTerrain = terrainManager.transform.GetChild(currentPiece).gameObject;
                   
                AutoChangeVehicle();
                timeNoMovement = 0;
               // Debug.Log(gameObject.name);

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
        //Debug.Log("AutoChangeVehicle");
        if (currentTerrain.GetComponentInChildren<Obstacle>() != null)
        {
            //Debug.Log(currentTerrain.GetComponentInChildren<Obstacle>().name);
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
                    
                    if (!isPlayer)
                    {
                        //Debug.Log(vehicle[j].name);
                        if (piece.name == "Fly Piece")
                        {
                            choseVehicle = vehicle[j];
                            SwapVehicle();
                            //Debug.Log("StartFly");
                            isFlying = true;
                        }
                        else
                        {
                            BotSwapVehicle(j);  
                        }
                    }
                    else if(isPlayer)
                    {
                        _click[j].gameObject.SetActive(true);
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
            _isStart = false;
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
            _click[0].gameObject.SetActive(false);
            _click[1].gameObject.SetActive(false);
            _click[2].gameObject.SetActive(false);
            choseVehicle = vehicle[0];
            SwapVehicle();
        }
       
    }
    public void Vehicle1()
    {
        if (_isStart)
        {
            _click[0].gameObject.SetActive(false);
            _click[1].gameObject.SetActive(false);
            _click[2].gameObject.SetActive(false);
            choseVehicle = vehicle[1];
            SwapVehicle();
        }
    }
    public void Vehicle2()
    {
        if (_isStart)
        {
            _click[0].gameObject.SetActive(false);
            _click[1].gameObject.SetActive(false);
            _click[2].gameObject.SetActive(false);
            choseVehicle = vehicle[2];
            SwapVehicle();
        }
    }
    public void BootSpeedVehicle0()
    {
        bootSpeedBtn[0].gameObject.SetActive(false);
        BootSpeed(vehicle[0]);
    }
    public void BootSpeedVehicle1()
    {
        bootSpeedBtn[1].gameObject.SetActive(false);
        BootSpeed(vehicle[1]);
    }
    public void BootSpeedVehicle2()
    {
        bootSpeedBtn[2].gameObject.SetActive(false);
        BootSpeed(vehicle[2]);
    }
    public void BootSpeed(GameObject vhc)
    {
        if(gameManager.coin >= 500)
        {
            if (vhc.GetComponent<CharacterMove>() != null)
            {
                vhc.GetComponent<CharacterMove>().speed += vhc.GetComponent<CharacterMove>().speed * 0.25f;
                vhc.GetComponent<CharacterMove>().bootSpeed = true;
            }
            else if (vhc.GetComponent<CarController>() != null)
            {
                vhc.GetComponent<CarController>().setCarSpeed += vhc.GetComponent<CarController>().setCarSpeed * 0.25f;
                vhc.GetComponent<CarController>().bootSpeed = true;
            }
            gameManager.coin -= 500;
            PlayerPrefs.SetInt("Coin", gameManager.coin);
            gameManager._uiManager.CoinNumberText.text = gameManager.coin.ToString();
        }
       
    }

    public void SwapVehicle()
    {
       
         if (currentVehicle != choseVehicle)
         {
             StartCoroutine(currentVehicleScaletoZero(0.2f));
        //     choseVehicle.transform.position = currentVehicle.transform.localToWorldMatrix.GetPosition();
        //     currentVehicle.gameObject.SetActive(false);
        //     currentVehicle = choseVehicle;
        //     
        //     currentVehicle.transform.eulerAngles = new Vector3(0,90,0);
        //     currentVehicle.gameObject.SetActive(true);
        //     if (currentVehicle.GetComponent<CarController>() != null)
        //     {
        //         currentVehicle.GetComponent<CarController>().AddForce();
        //     }
        //     
        //
        //     if (currentVehicle.GetComponent<HelicopterController>()!= null)
        //     {
        //         currentVehicle.GetComponent<HelicopterController>().UpdateCurentHight();
        //     }
        //
        //     if (isPlayer)
        //     {
        //         cam.player = currentVehicle;
        //     }
         }

    }
    public IEnumerator currentVehicleScaletoZero(float t)
    {
        changVehicleEffect.gameObject.SetActive(false);
        changVehicleEffect.gameObject.SetActive(true);
        changVehicleEffect.transform.position = currentVehicle.transform.localToWorldMatrix.GetPosition();
        //changVehicleEffect.transform.parent = cam.transform;
        
        //StartCoroutine(ShowEffect(t * 2.0f));
        var mesh = currentVehicle.GetComponent<VehicleData>().mesh;
        StartCoroutine(t.Tweeng((p) => mesh.gameObject.transform.localScale = p , mesh.gameObject.transform.localScale,  new Vector3(0, 0, 0)));
        yield return new WaitForSeconds(t);
        choseVehicle.transform.position = currentVehicle.transform.localToWorldMatrix.GetPosition();
        currentVehicle.gameObject.SetActive(false);
        
        currentVehicle = choseVehicle;
        
        currentVehicle.transform.eulerAngles = new Vector3(0,90,0);
        mesh = currentVehicle.GetComponent<VehicleData>().mesh;
        
        StartCoroutine(t.Tweeng((p) => mesh.gameObject.transform.localScale = p , mesh.gameObject.transform.localScale,  new Vector3(1, 1, 1)));
        
        currentVehicle.gameObject.SetActive(true);
     
        
        
        if (currentVehicle.GetComponent<CarController>() != null)
        {
            currentVehicle.GetComponent<CarController>().AddForce();
        }
            

        if (currentVehicle.GetComponent<HelicopterController>()!= null)
        {
            currentVehicle.GetComponent<HelicopterController>().UpdateCurentHight();
        }

        if (isPlayer)
        {
            cam.player = currentVehicle;
        }
        //StartCoroutine(0.5f.Tweeng())
    }

    public IEnumerator ShowEffect(float t)
    {
        float time = 0;
        while (time < t)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
            //changVehicleEffect.transform.position = Vector3.Lerp(startPosition, currentVehicle.transform.localToWorldMatrix.GetPosition(), Mathf.SmoothStep(0,1,percent) );
            changVehicleEffect.transform.position = currentVehicle.transform.localToWorldMatrix.GetPosition();
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
