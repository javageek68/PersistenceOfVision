using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public SimpleVehicleController vehicleController;
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    public float lookAhead;

    public float steering;
    public float braking;
    public float throttle;

    Vector3 waypointLocation = Vector3.zero;
    public float distanceTravelled;
    float maxSteerAngle = 40;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribed to the pathUpdated event so that we're notified if the path changes during the simulation
        pathCreator.pathUpdated += OnPathChanged;
    }


    private void FixedUpdate()
    {
        Transform vehicleTransform = vehicleController.transform;
        // get the next waypoint
        Vector3 waypoint =  GetWaypointLocation(vehicleTransform);
        // calculate the steering angle toward that waypoint
        steering = GetSteeringAngle(vehicleTransform, waypoint);
        // set the vehicle props
        vehicleController.steeringAngle = steering;
        vehicleController.throttle = throttle;
        vehicleController.braking = braking;

        Debug.DrawLine(vehicleTransform.position, waypoint, Color.red);

    }

    Vector3 GetWaypointLocation(Transform goTransform)
    {
        // use the hero current position to get the distance along the path we are at
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(goTransform.position);
        // use the lookAhead distance to find the next waypoint position
        Vector3 waypointLocation = pathCreator.path.GetPointAtDistance(distanceTravelled + lookAhead, endOfPathInstruction);
        return waypointLocation;
    }

    float GetSteeringAngle(Transform goTransform, Vector3 waypoint)
    {
        // get the relative direction
        Vector3 vRelative = goTransform.InverseTransformPoint(waypoint);
        // get the normalized value
        float normalizedSteerAngle = (vRelative.x / vRelative.magnitude);
        // multiply it by the max steer angle to get the actual angle
        float steerAngle = normalizedSteerAngle * maxSteerAngle;

        return steerAngle;
    }

    private void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
