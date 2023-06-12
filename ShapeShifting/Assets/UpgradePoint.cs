using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

public class UpgradePoint : MonoBehaviour
{
    // Start is called before the first frame update
    public Vehicle vhcData;
    public GameObject vhc;
    public string nameVehicle = "CarUpgrade";
    public string LvVehicle = "CarUpgradeLv";
    public string CoinVehicle = "CarUpgradeCoin";
    public bool CanUpgrade = true;
    public int level ;
    public int coinUpgrade = 0;
    public TMP_Text coinText;
    public TMP_Text LvText;
    public bool isLock = true;
    void Start()
    {
        level = PlayerPrefs.GetInt(LvVehicle);
         //PlayerPrefs.SetInt(LvVehicle, 1);
         //PlayerPrefs.SetInt(CoinVehicle,1500);
        StartCoroutine(SetUp());
       

    }

    IEnumerator SetUp()
    {
        yield return new WaitForSeconds(0.01f);
        if(level == 0);
        {
            Debug.Log(1);
            PlayerPrefs.SetInt(LvVehicle, 1);
            PlayerPrefs.SetInt(CoinVehicle,1500);
        }
        level = PlayerPrefs.GetInt(LvVehicle);
        coinUpgrade = PlayerPrefs.GetInt(CoinVehicle);
        if (level >= 5)
        {
            coinText.text = "Max";
            CanUpgrade = false;
        }
        else
        {
            coinText.text = coinUpgrade.ToString();
            CanUpgrade = true;
        }
        LvText.text ="LV "+ level.ToString();
        for (int i = 0; i < vhcData.VehicleInGame.Count; i++)
        {
            if (vhcData.VehicleInGame[i].name == vhc.name)
            {
                isLock = false;
            }
        }

        if (isLock == true)
        {
            CanUpgrade = false;
            coinText.text = "Lock";
        }

    }

    public void Upgrade()
    {
        coinUpgrade -= 100;
        PlayerPrefs.SetInt(CoinVehicle,coinUpgrade);
        if(coinUpgrade == 0)
        {
            level++;
            PlayerPrefs.SetInt(CoinVehicle,1500);
            coinUpgrade = PlayerPrefs.GetInt(CoinVehicle);
            PlayerPrefs.SetInt(LvVehicle, level);
            Debug.Log(level);
        }
        coinText.text = coinUpgrade.ToString();
        LvText.text = "LV " + level.ToString();
        if (level >= 5)
        {
            coinText.text = "Max";
            CanUpgrade = false;
        }
    }

    public void Reset()
    {
        PlayerPrefs.SetInt(LvVehicle, 1);
        PlayerPrefs.SetInt(CoinVehicle,1500);
        level = PlayerPrefs.GetInt(LvVehicle);
        coinUpgrade = PlayerPrefs.GetInt(CoinVehicle);
        coinText.text = coinUpgrade.ToString();
        LvText.text = level.ToString();
        CanUpgrade = true;
    }
}
