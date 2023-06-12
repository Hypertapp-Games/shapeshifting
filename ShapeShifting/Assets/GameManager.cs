using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public List<GameObject> allVehicle = new List<GameObject>();
    public List<PlayerManager> allPlayer = new List<PlayerManager>();
    [HideInInspector]public UIManager _uiManager;
    [HideInInspector]public SpawnTerrains _spawnTerrains;
    [HideInInspector] public int ordinal = 1;
    [HideInInspector] public TestFT testFt;
    public int level = 0;
    public int coin = 0;
    public int coinGetInThisLevel = 0;
    private void Start()
    {
        _spawnTerrains = gameObject.GetComponent<SpawnTerrains>();
        testFt = gameObject.GetComponent<TestFT>();
        //RandomVehicleUseLevel();
        level = PlayerPrefs.GetInt("Level");
        testFt.Level = level;
         //PlayerPrefs.SetInt("Level",1);
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
            GameObject vhc = null;
            if (i==0 && testFt.vhcData.vehiclekUseInNextLevel != null)
            {
                for (int j = 0; j < allVehicle.Count; j++)
                {
                    if(allVehicle[j].name == testFt.vhcData.vehiclekUseInNextLevel.name)
                    {
                        vhc = allVehicle[j];
                        testFt.vhcData.vehiclekUseInNextLevel = null;
                        EditorUtility.SetDirty(testFt.vhcData);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        Debug.Log("UseUnlock");
                    }
                }
            }
            else
            {
                var vhr = Random.Range(0, allVehicle.Count);
                vhc = allVehicle[vhr];
            }
            
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

    public void EndGame()
    {
        testFt.UnlockNewShape();
    }

    public void GetBonusCoin()
    {
        _uiManager.EndGame(ordinal);
        if (ordinal == 1)
        {
            PlayerPrefs.SetInt("Level", level+1);
            coinGetInThisLevel = 500;
        }
        else
        {
            coinGetInThisLevel = 500 - (ordinal-1)*100;
        }

        _uiManager.coin.text = coinGetInThisLevel.ToString();
        StartCoroutine(_uiManager._SicleSlider());
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
