using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CarController : MonoBehaviour
{
    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
    }
    public float setCarSpeed = 10;

    //public float maxAcceleration = 0f;
    public float torqueSpeed = 600;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;
    
    
    private Rigidbody carRb;
    public GameObject CastGound;
    public GameObject CastFont;
    public float maxDistancecast = 1;
    private bool isStar = false;
    private bool haveObstacleAhead = false;
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        AddForce();
    }
    

    public void AddForce()
    {
        // var vel = transform.InverseTransformDirection(this.GetComponent<Rigidbody>().velocity);
        // vel.z = setCarSpeed;
        // this.GetComponent<Rigidbody>().velocity = transform.TransformDirection(vel);
        isStar = false;
        StartCoroutine(_AddForce());
    }

    public IEnumerator _AddForce()
    {
        while (!isStar)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            RaycastHit hit;
            if (Physics.Raycast(CastGound.transform.position, CastGound.transform.forward, out hit, maxDistancecast,_target))
            {
                isStar = true;
            }
        }
        
        
    }

    void Update()
    {
        AnimateWheels();
    }
    
    void LateUpdate()
    {
        Move();
    }

    public LayerMask _target;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(CastFont.transform.position, CastFont.transform.position + CastFont.transform.forward * maxDistancecast);
        //Debug.DrawLine(CastGound.transform.position, CastGound.transform.position + CastGound.transform.forward * maxDistancecast);
    }
    private void FixedUpdate()
    {
        RaycastHit hitFront;
        if (Physics.Raycast(CastFont.transform.position, CastFont.transform.forward, out hitFront, maxDistancecast))
        {
            if (gameObject.GetComponent<DrillController>() != null)
            {
                if (hitFront.transform.gameObject.name != "Ob")
                {
                    haveObstacleAhead = true;
                }
            }
            else
            {
                haveObstacleAhead = true;
            }
            
        }
        else
        {
            haveObstacleAhead = false;
        }
        RaycastHit hitGround;
        if ((Physics.Raycast(CastGound.transform.position, CastGound.transform.forward, out hitGround, 1,_target) || !isStar)&& !haveObstacleAhead)
        {
            var vel = transform.InverseTransformDirection(carRb.velocity);
            vel.z = setCarSpeed;
            carRb.velocity = transform.TransformDirection(vel);
            Debug.Log(gameObject);
        }
        
        // var vel = transform.InverseTransformDirection(this.GetComponent<Rigidbody>().velocity);
        // if (vel.z > setCarSpeed) vel.z = setCarSpeed;
        // else if (vel.z < -setCarSpeed) vel.z = -setCarSpeed;
        // this.GetComponent<Rigidbody>().velocity = transform.TransformDirection(vel);
        
        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x,90,0);

    }


    void Move()
    {
        
        foreach(var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque =   torqueSpeed * Time.deltaTime; //* maxAcceleration 
        }
    }

    void AnimateWheels()
    {
        foreach(var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            if (wheel.wheelModel != null)
            {
                wheel.wheelModel.transform.position = pos;
                wheel.wheelModel.transform.rotation = rot;
            }
         
        }
    }
    public void ChangeSpeed(float timeChange ,float _from, float _to)
    {
        StartCoroutine(timeChange.Tweeng(x => { setCarSpeed = x; }, _from, _to));
    }

}