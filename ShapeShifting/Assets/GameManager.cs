using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public List<GameObject> allVehicle = new List<GameObject>();
    public List<PlayerManager> allPlayer = new List<PlayerManager>();
    [HideInInspector]public UIManager _uiManager;
    [HideInInspector]public SpawnTerrains _spawnTerrains;
    [HideInInspector] public int ordinal = 1;
    public int level = 0;
    public int coin = 0;
    public int coinGetInThisLevel = 0;
    private void Start()
    {
        _spawnTerrains = gameObject.GetComponent<SpawnTerrains>();
        //RandomVehicleUseLevel();
        level = PlayerPrefs.GetInt("Level");
        coin = PlayerPrefs.GetInt("Coin");
        _uiManager.CoinNumberText.text = coin.ToString();
    }
    private void OnEnable()
    {
        Application.targetFrameRate = 60;
        _uiManager = gameObject.GetComponent<UIManager>();
    }
    public void RandomVehicleUseLevel()
    {
        List<TerrainsData> temptr = new List<TerrainsData>();
        for (int i = 0; i < 3; i++)
        {
            var vhr = Random.Range(0, allVehicle.Count);
            GameObject vhc = allVehicle[vhr];
            _uiManager.AutoScroll(vhc.name,i);
            _uiManager.GetBtnBottSpeedImage(i, vhc);
            for (int j = 0; j < allPlayer.Count; j++)
            {
                allPlayer[j].prefabVehicles.Add(vhc);
            }
            
            List<TerrainsData> cpd = vhc.GetComponent<VehicleData>().terrainCorresponding;
            
            for (int j = 0; j < cpd.Count; j++)
            {
                if(!cpd[j].IsLock)
                temptr.Add(cpd[j]);
            }
            allVehicle.Remove(vhc);
        }
        _spawnTerrains.terrainsData = temptr.Distinct().ToList();
        _spawnTerrains.SetUpTerrainsData();
        for (int i = 0; i < allPlayer.Count; i++)
        {
            allPlayer[i].SpawnVehicle();
        }
    }
    public void StartRace()
    {
        for (int i = 0; i < allPlayer.Count; i++)
        {
            allPlayer[i].StartGame();
        }
        gameObject.GetComponent<UIManager>().HideBootSpeedButton();
    }
}
