using System;
using UnityEngine;

public class ObjectTracker : MonoBehaviour
{
    public GameObject trackedObject; 
    public GameObject predictionObject;
    private KalmanFilter kf;
    private const int stateDimension = 6;
    private const int measurementDimension = 3;
    //private double?[,] measurements = new double?[,]
    //{
    //    { 1, 1, 1 },
    //    { 2, 2, 2 },
    //    { null, null, null }, // Occlusion
    //    { 4, 4, 4 },
    //    { 5, 5, 5 }
    //};

    private void Start()
    {
        //InitializeKalmanFilter();
        kf = KalmanFilter.InitKalmanFilter(stateDimension, measurementDimension);
    }

    private double?[] getNextMeasurement()
    {
        double?[] data = new double?[measurementDimension];
        data[0] = this.trackedObject.transform.position.x;
        data[1] = this.trackedObject.transform.position.y;
        data[2] = this.trackedObject.transform.position.z;
        return data;
    }

    private void Update()
    {
        kf.Predict();
   
        double?[] measurement = getNextMeasurement();
        kf.Update(measurement);

        double[] predictedPosition = kf.GetPrediction();

        RenderPrediction(predictedPosition);
    }

    private void RenderPrediction(double[] pos)
    {
        Debug.Log($"{pos[0]}, {pos[1]}, {pos[2]}");
        //Vector3 NewVec = new Vector3((float)pos[0], (float)pos[1], (float)pos[2]);
        
        //predictionObject.transform.position = NewVec;
    }

    private void DrawObject(double x, double y, double z)
    {
        trackedObject.transform.position = new Vector3((float)x, (float)y, (float)z);
    }
}