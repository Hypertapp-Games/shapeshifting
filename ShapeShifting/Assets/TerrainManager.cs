using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 currentPosition = new Vector3(0,0,0);
    public int currentPiece = 0;
    void Start()
    {
        //UpdateCurrentPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateCurrentPosition()
    {
        var p = gameObject.transform.GetChild(currentPiece).GetComponent<Piece>().endPoint;
        currentPosition = p.transform.localToWorldMatrix.GetPosition();
        currentPiece++;
    }

    public bool CheckEnd()
    {
        if (currentPiece >= gameObject.transform.childCount)
        {
            return true;
        }

        return false;
    }
}
