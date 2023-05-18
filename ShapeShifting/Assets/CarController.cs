using UnityEngine;
using System;
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
    

    public float maxAcceleration = 0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;
    

    private Rigidbody carRb;
    public float torqueSpeed = 600;
    public float setCarSpeed = 10;
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        
    }

    void Update()
    {
        AnimateWheels();
       
       
    }
    
    void LateUpdate()
    {
        Move();
    }

    private void FixedUpdate()
    {
        var vel = transform.InverseTransformDirection(this.GetComponent<Rigidbody>().velocity);
        if (vel.z > setCarSpeed) vel.z = setCarSpeed;
        else if (vel.z < -setCarSpeed) vel.z = -setCarSpeed;
        this.GetComponent<Rigidbody>().velocity = transform.TransformDirection(vel);
    }


    void Move()
    {
        
        foreach(var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque =   torqueSpeed * maxAcceleration * Time.deltaTime;
        }
    }

    void AnimateWheels()
    {
        foreach(var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

}