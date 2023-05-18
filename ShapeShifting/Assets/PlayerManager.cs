using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Vehicle = new List<GameObject>();
    public TerrainManager terrainManager;
    public GameObject currentVehicle;
    public GameObject choseVehicle;
    public Vector3 currentVehiclePosition;
    public FollowPlayer Cam;
    void Start()
    {
        Cam.player = currentVehicle;
    }

    // Update is called once per frame
    void Update()
    {
        currentVehiclePosition = currentVehicle.transform.localToWorldMatrix.GetPosition();
        if(currentVehiclePosition.x > terrainManager.currentPosition.x)
        {
            terrainManager.UpdateCurrentPosition();
        }
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
    public void SwapVehicle()
    {
        choseVehicle.transform.position = currentVehicle.transform.localToWorldMatrix.GetPosition();
        currentVehicle.gameObject.SetActive(false);
        currentVehicle = choseVehicle;
        currentVehicle.gameObject.SetActive(true);
        if (currentVehicle.GetComponent<HelicopterController>()!= null)
        {
            currentVehicle.GetComponent<HelicopterController>().UpdateCurentHight();
        }
        Cam.player = currentVehicle;
    }
}
