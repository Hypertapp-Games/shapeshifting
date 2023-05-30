using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]public float curentHight = 0;
    [HideInInspector] public CharacterMove characterMove;
    [HideInInspector] public float temp;
    public PlayerManager playerManager;


    void Start()
    {
        characterMove = GetComponent<CharacterMove>();
        temp = characterMove.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < curentHight + 1)
        {
            characterMove.gravity = 5;
        }
        else
        {
            characterMove.gravity = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Road Up")
        {
            Debug.Log(1);
            var piece = other.transform.parent.gameObject;
            characterMove.speed = 0;
            var a = piece.GetComponent<Piece>().endPoint.transform.localToWorldMatrix.GetPosition();
            curentHight = a.y;
        }

        if (other.GetComponent<Obstacle>()!= null)
        {
            curentHight = other.transform.parent.GetComponent<Piece>().endPoint.transform.localToWorldMatrix
                .GetPosition().y;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Road Up")
        {
            characterMove.speed = temp;
        }
    }
    public void UpdateCurentHight()
    {
        characterMove.speed = 0.05f;
        curentHight = playerManager.currentPosition.y;
    }
}