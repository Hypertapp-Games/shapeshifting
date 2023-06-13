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
    public List<GameObject> attach = new List<GameObject>();
    public Transform Attach;
    private void Start()
    {
        //SetUpTerrainsData();
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
                Spawmscenical(terran);
                i++;
            }
            else if(i==NumberPiece)
            {
                terran = Instantiate(finishPiece);
                AnchorTerrain(terran,finishPiece.name);
                Spawmscenical(terran);
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
                    Spawmscenical(terran);
                    //if (terrainsData[whichTerrrain].terrainType != 0)
                    //{
                    //    for (int k = 0; k < terran.transform.childCount; k++)
                    //    {
                    //        terran.transform.GetChild(k).name = terrainsData[whichTerrrain].terrainType.ToString();
                    //    }
                    //}
                   
                    i++;
                    if (terrainsData[whichTerrrain].name == "Road Ob")
                    {
                        SpawnAttachObstacle(terran, terrainsData[whichTerrrain].attachObject[0]);
                    }
                    
                    if (terrainsData[whichTerrrain].name == "Fly")
                    {
                        SpawnAttachTerrain(terran,terrainsData[whichTerrrain].attachObject[0]);
                    }
                    
                    if (i > NumberPiece-1) // Neu spam du thi se spawm finish
                    {
                        break;
                    }
                }
                
                if (terrainsData[whichTerrrain].maxCallTemp == 0) //Neu da spawm du so trong 1 lan thi khong spawm nua
                {
                    terrainsData.Remove(terrainsData[whichTerrrain]);
                }
                
            }
        }
        //Terrains.gameObject.GetComponent<CombiningMeshes>().Combining();
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
            int i = 0;
            int j = 0;
            while (i == 0)
            {
                j++;
                int whichTerrrain = Random.Range(0, terrainsData.Count);
                int terrainCod = terrainsData[whichTerrrain].terrainCode;
                if (terrainCod != currentTerrainCod)
                {
                    currentTerrainCod = terrainCod;
                    i++;
                    return whichTerrrain;
                }

                if (j > 50)
                {
                    return 0;
                }
            }
            return 0;
            // else
            // {
            //     return GetCurrrentTerrain();
            // }
        }

    }

    public void SpawnAttachTerrain(GameObject terrain,GameObject attach)
    {
        for (int i = 1; i <= Random.Range(2,4); i++)
        {
            GameObject _attach = Instantiate(attach);
            AnchorTerrain(_attach, "attach");
            _attach.transform.parent = terrain.transform;
        }
    }
    
    public void SpawnAttachObstacle(GameObject terrain,GameObject obstacle)
    {
        Piece piece = terrain.GetComponent<Piece>();
        float pos_x = Random.Range(piece.startPoint.transform.localToWorldMatrix.GetPosition().x+2, piece.endPoint.transform.localToWorldMatrix.GetPosition().x-2);
        Vector3 spawmPos = new Vector3(pos_x, piece.startPoint.transform.localToWorldMatrix.GetPosition().y, piece.startPoint.transform.localToWorldMatrix.GetPosition().z);
        GameObject _attach = Instantiate(obstacle,spawmPos,Quaternion.identity);
        _attach.transform.parent = terrain.transform;
    }

    public void Spawmscenical(GameObject terrain)
    {
        var num = Random.Range(3, 7);
        Piece piece = terrain.GetComponent<Piece>();
        int xPos = 0;
        float attachPosX = 0;
        float attachPosStart = 0;
        float attachPosEnd = 0;
        while (attachPosStart <= attachPosEnd )
        {
            for (int i = 0; i < num; i++)
            {
                attachPosStart = piece.startPoint.transform.localToWorldMatrix.GetPosition().x + 2 + xPos;
                attachPosEnd = piece.endPoint.transform.localToWorldMatrix.GetPosition().x - 2;
                attachPosX = Random.Range(attachPosStart , attachPosEnd);
            
                Vector3 spawmPos = new Vector3(attachPosX, piece.startPoint.transform.localToWorldMatrix.GetPosition().y+0.5f, 7);
                GameObject _attach = Instantiate(attach[Random.Range(0,attach.Count-1)],spawmPos,Quaternion.identity);
                xPos += 8;

            }
        }
        
    }
}
