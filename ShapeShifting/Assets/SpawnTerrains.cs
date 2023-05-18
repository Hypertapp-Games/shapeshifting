using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;
using Random = UnityEngine.Random;

public class SpawnTerrains : MonoBehaviour
{
    // Start is called before the first frame update
    public List<TerrainsData> terrainsData = new List<TerrainsData>();
    public GameObject startPiece;
    public GameObject finishPiece;
    public Transform Terrains;
    public int NumberPiece = 5;
    Vector3 startPoint = new Vector3(0, 0, 0);
    
    int currentTerrainCod = 100;
    private void Start()
    {
        SetUpTerrainsData();
    }

    public void SetUpTerrainsData()
    {
        for (int i = 0; i < terrainsData.Count; i++)
        {
            terrainsData[i].maxCallTemp = terrainsData[i].maxCall;
        }

        TerraninsGeneration();
    }
    public void TerraninsGeneration()
    {
        int i = 0;
        while ( i <= NumberPiece)
        {
            GameObject terran;
            if (i == 0)
            {
                terran = Instantiate(startPiece);
                AnchorTerrain(terran,startPiece.name);
                i++;
            }
            else if(i==NumberPiece)
            {
                terran = Instantiate(finishPiece);
                AnchorTerrain(terran,finishPiece.name);
                i++;
            }
            else
            {
                int whichTerrrain = GetCurrrentTerrain();//Random.Range(0, terrainsData.Count);
                terrainsData[whichTerrrain].maxCallTemp -= 1;
                int terrainInSuccession = Random.Range(1, terrainsData[whichTerrrain].maxInSuccesion);
                for (int j = 0; j < terrainInSuccession ; j++)
                {
                    terran = Instantiate(terrainsData[whichTerrrain].terrain);
                    AnchorTerrain(terran,terrainsData[whichTerrrain].terrain.name);
                    i++;
                    if (i > NumberPiece-1) // Neu spam du thi se spawm finish
                    {
                        break;
                    }
                }

                if (terrainsData[whichTerrrain].name == "Fly")
                {
                    SpawnAttachTerrain(terrainsData[whichTerrrain].attachObject[0]);
                }
                if (terrainsData[whichTerrrain].maxCallTemp == 0) //Neu da spawm du so trong 1 lan thi khong spawm nua
                {
                    terrainsData.Remove(terrainsData[whichTerrrain]);
                }
                
            }
        }
    }

    public void AnchorTerrain( GameObject terran, string name)
    {
        terran.name = name;
        terran.GetComponent<Piece>().AnchorWithStartPoint(startPoint);
        startPoint = terran.GetComponent<Piece>().endPoint.transform.localToWorldMatrix.GetPosition();
        terran.transform.SetParent(Terrains);
    }
    public int GetCurrrentTerrain()
    {
        if (terrainsData.Count == 1)
        {
            return 0;
        }
        else
        {
            int whichTerrrain = Random.Range(0, terrainsData.Count);
            int terrainCod = terrainsData[whichTerrrain].terrainCod;
            if (terrainCod  != currentTerrainCod)
            {
                currentTerrainCod = terrainCod;
                return whichTerrrain;
            }
            else
            {
                return GetCurrrentTerrain();
            }
        }
       
    }

    public void SpawnAttachTerrain(GameObject attach)
    {
        for (int i = 1; i <= Random.Range(2,4); i++)
        {
            GameObject _attach = Instantiate(attach);
            AnchorTerrain(_attach, "attach");
        }
    }
}