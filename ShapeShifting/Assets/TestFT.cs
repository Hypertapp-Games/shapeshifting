using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFT : MonoBehaviour
{
    // Start is called before the first frame update
    public Vehicle vhcData;
    public int currentUnlock; // so thu tu cua shape dang duoc unlock;
    public int currentProcess;
    public int Level;
    public int LevelCantUnlock;
    public GameObject fillArea;
    public UnityEngine.UI.Image fill;
    float fillAmount;
    public GameManager gameManager;
    public void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        var c = vhcData.VehicleInGame.Count;
        for (int i = 0; i < c; i++)
        {
            gameManager.allVehicle.Add(vhcData.VehicleInGame[i]);
        }
        gameManager.RandomVehicleUseLevel();
        currentUnlock = vhcData.CurrentUnlock;
        currentProcess = vhcData.process;
        LevelCantUnlock = vhcData.LevelUnlock[currentUnlock];
        var sprites = Resources.LoadAll<Sprite>("PNG");
        for (int k = 0; k < sprites.Length; k++)
        {

            if (sprites[k].name == vhcData.vehicleLock[currentUnlock].vhc.name)
            {
                fill.sprite = sprites[k];
            }
        }
    }
  
    public void UnlockNewShape()
    {
        if((Level == LevelCantUnlock || vhcData.InProcessUnlock) && gameManager.ordinal == 1)
        {
            currentProcess += 25;
            vhcData.process = currentProcess;
            fillArea.gameObject.SetActive(true);
            vhcData.InProcessUnlock = true;
            StartCoroutine(1f.Tweeng(x => { fillAmount = x; }, 0, (float)currentProcess / 100));
            StartCoroutine(_FillOnImage());
            StartCoroutine(HideProcessUnlock());
            if (currentProcess >= 100)
            {
                vhcData.VehicleInGame.Add(vhcData.vehicleLock[currentUnlock].vhc);
                vhcData.TerrainInGame.Add(vhcData.vehicleLock[currentUnlock].terrain);
                vhcData.vehicleLock[currentUnlock].terrain.IsLock = false;
                vhcData.InProcessUnlock = false;
                vhcData.CurrentUnlock++;
                vhcData.process = 0;

            }

            
        }
        else
        {
            gameManager.GetBonusCoin();
        }

        
    }
    public IEnumerator _FillOnImage()
    {
        while(fillAmount < 100)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            fill.fillAmount = fillAmount;
        }
    }
    public IEnumerator HideProcessUnlock()
    {
        yield return new WaitForSeconds(2f);
        fillArea.gameObject.SetActive(false);
        gameManager.GetBonusCoin();
    }
}
