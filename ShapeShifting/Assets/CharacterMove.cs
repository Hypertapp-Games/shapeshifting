using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10.0f;
    public bool bootSpeed = false;
    public float gravity =-9.8f;
    public CharacterController cc;
    void Start()
    {
        cc = this.GetComponent<CharacterController>();
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveCharacter();
    }

    void moveCharacter()
    {
         var objectdirection = transform.forward;
         objectdirection =  objectdirection* speed + new Vector3(0, objectdirection.y + gravity * Time.deltaTime, 0);
         cc.Move(objectdirection);
    }
    public void ChangeSpeed(float timeChange ,float _from, float _to)
    {
        if (bootSpeed)
        {
            _to += _to * 0.25f;
        }
        StartCoroutine(timeChange.Tweeng(x => { speed = x; }, _from, _to));
    }
}
