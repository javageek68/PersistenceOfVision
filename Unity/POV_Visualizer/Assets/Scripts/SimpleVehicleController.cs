using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVehicleController : MonoBehaviour
{
    // Settings
    [SerializeField] private float steeringAngle;
    [SerializeField] private float throttle;
    [SerializeField] private float braking;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void FixedUpdate()
    {
        Debug.Log("simple vehicle running");
        //GetInput();

        HandleMotor(throttle, braking);
        HandleSteering(steeringAngle);
        UpdateWheels();
    }

    private void HandleMotor(float throttle, float braking)
    {
        frontLeftWheelCollider.motorTorque =  throttle;
        frontRightWheelCollider.motorTorque = throttle;
        ApplyBraking(this.braking);
    }

    private void ApplyBraking(float braking)
    {
        frontRightWheelCollider.brakeTorque = braking;
        frontLeftWheelCollider.brakeTorque = braking;
        rearLeftWheelCollider.brakeTorque = braking;
        rearRightWheelCollider.brakeTorque = braking;
    }

    private void HandleSteering(float steering)
    {
        frontLeftWheelCollider.steerAngle = steering;
        frontRightWheelCollider.steerAngle = steering;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}