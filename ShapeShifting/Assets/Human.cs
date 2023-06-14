using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public float curentHight = 0;
    [HideInInspector] public float curentHightPosition = 0;
    [HideInInspector] public CharacterMove characterMove;
    void Start()
    {
        characterMove = GetComponent<CharacterMove>();
    }

    private void OnEnable()
    {
        curentHight = 0;
        curentHightPosition = 0;
        //Debug.Log("OnEnable");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < curentHight +0.5f && transform.position.x < curentHightPosition)
        {
            characterMove.gravity = 3.5f;
        }
        else
        {
            characterMove.gravity = -10;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            var piece = other.transform.parent.gameObject;
            var a = piece.GetComponent<Piece>().endPoint.transform.localToWorldMatrix.GetPosition();
            curentHight = a.y;
            curentHightPosition = a.x;
        }

    }
}
