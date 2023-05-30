using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startPoint;
    public GameObject endPoint;

    public Vector3 startPosition;

    public int PieceCode;
    public List<GameObject> vehicleCanMoveIn = new List<GameObject>();
    void Awake()
    {
        startPosition = startPoint.transform.localToWorldMatrix.GetPosition();
    }
    public void AnchorWithStartPoint(Vector3 currentStartPosition)
    {
        Vector3 trans = currentStartPosition - startPosition;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == startPoint.name)
            {
                transform.GetChild(i).position = currentStartPosition;
            }
            else
            {
                transform.GetChild(i).position += trans;
            }
           
        }
    }
    
}
