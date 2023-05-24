using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> AllVehicle = new List<GameObject>();
    [HideInInspector] public List<GameObject> Vehicle = new List<GameObject>();
    public TerrainManager terrainManager;
    
    public GameObject currentVehicle;
    public GameObject choseVehicle;
    public Vector3 currentVehiclePosition;
    public FollowPlayer Cam;
    
    SpawnTerrains _spawnTerrains;
    private UIManager _uiManager;

    [Header("Button Text")] 
    public List<TMP_Text> Button = new List<TMP_Text>();

    void Start()
    {
        _uiManager = gameObject.GetComponent<UIManager>();
        _spawnTerrains = gameObject.GetComponent<SpawnTerrains>();
        Cam.player = currentVehicle;
        RandomVehicleUseLevel();
    }

    // Update is called once per frame
    void Update()
    {
        // currentVehiclePosition = currentVehicle.transform.localToWorldMatrix.GetPosition();
        // if(currentVehiclePosition.x > terrainManager.currentPosition.x)
        // {
        //     terrainManager.UpdateCurrentPosition();
        //     if (currentVehicle.GetComponent<HelicopterController>() != null)
        //     {
        //         currentVehicle.GetComponent<HelicopterController>().UpdateCurentHight();
        //     }
        // }
    }

    public IEnumerator ThisUpdateCurrentPosition()
    {
        while(true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            currentVehiclePosition = currentVehicle.transform.localToWorldMatrix.GetPosition();
            if(currentVehiclePosition.x > terrainManager.currentPosition.x)
            {
                terrainManager.UpdateCurrentPosition();
                if (currentVehicle.GetComponent<HelicopterController>() != null)
                {
                    currentVehicle.GetComponent<HelicopterController>().UpdateCurentHight();
                }

                if (terrainManager.CheckEnd() == true)
                {
                    _uiManager.EndGame();
                    Cam.enabled = false;
                }
            }
        }
    }

    public void RandomVehicleUseLevel()
    {
        List<TerrainsData> temptr = new List<TerrainsData>();
        for (int i = 0; i < 3; i++)
        {
            GameObject vhc = AllVehicle[Random.Range(0, AllVehicle.Count)];
            Button[i].text = vhc.name;
            Vehicle.Add(vhc);
             List<TerrainsData> cpd = vhc.GetComponent<VehicleData>().Corresponding;
             for (int j = 0; j < cpd.Count; j++)
             {
                 temptr.Add(cpd[j]);
             }
            AllVehicle.Remove(vhc);
        }
        //var temp = ;
        _spawnTerrains.terrainsData = temptr.Distinct().ToList();
        _spawnTerrains.SetUpTerrainsData();
        terrainManager.UpdateCurrentPosition();
        StartCoroutine(ThisUpdateCurrentPosition());
    }
    

    public void Vehicle0()
    {
        choseVehicle = Vehicle[0];
        SwapVehicle();
    }
    public void Vehicle1()
    {
        choseVehicle = Vehicle[1];
        SwapVehicle();
    }
    public void Vehicle2()
    {
        choseVehicle = Vehicle[2];
        SwapVehicle();
    }
    
    // public void Vehicle3()
    // {
    //     choseVehicle = Vehicle[3];
    //     SwapVehicle();
    // }
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
        Cam.player = currentVehicle;
    }
}
