using System;
using UnityEngine;

public class ObjectTracker : MonoBehaviour
{
    public GameObject trackedObject; // Assign this in the Unity Editor
    private KalmanFilter kf;
    private int step;
    private const int stateDimension = 6;
    private const int measurementDimension = 3;
    private double?[,] measurements = new double?[,]
    {
        { 1, 1, 1 },
        { 2, 2, 2 },
        { null, null, null }, // Occlusion
        { 4, 4, 4 },
        { 5, 5, 5 }
    };

    private void Start()
    {
        //InitializeKalmanFilter();
        kf = KalmanFilter.InitKalmanFilter(stateDimension, measurementDimension);
    }

    private double?[] getNextMeasurement(int step)
    {
        double?[] data = new double?[measurementDimension];
        for (int i = 0; i < measurementDimension; i++)
        {
            data[i] = measurements[step, i];
        }
        return data;
    }

    private void Update()
    {
        if (step >= measurements.GetLength(0)) return;

        kf.Predict();

        if (measurements[step, 0].HasValue)
        {
            //var z = new Matrix(measurementDimension, 1);
            //z[0, 0] = measurements[step, 0].Value;
            //z[1, 0] = measurements[step, 1].Value;
            //z[2, 0] = measurements[step, 2].Value;

            //kf.Update(z);
            double?[] measurement = getNextMeasurement(step);
            kf.Update(measurement);
        }

        DrawObject(kf.X[0, 0], kf.X[1, 0], kf.X[2, 0]);
        step++;
    }

    //private void InitializeKalmanFilter()
    //{
    //    kf = new KalmanFilter(stateDimension, measurementDimension);
    //    kf.X[0, 0] = 0; // x
    //    kf.X[1, 0] = 0; // y
    //    kf.X[2, 0] = 0; // z
    //    kf.X[3, 0] = 1; // vx
    //    kf.X[4, 0] = 1; // vy
    //    kf.X[5, 0] = 1; // vz

    //    double dt = 1.0; // Time step
    //    kf.F[0, 0] = 1; kf.F[0, 3] = dt;
    //    kf.F[1, 1] = 1; kf.F[1, 4] = dt;
    //    kf.F[2, 2] = 1; kf.F[2, 5] = dt;
    //    kf.F[3, 3] = 1;
    //    kf.F[4, 4] = 1;
    //    kf.F[5, 5] = 1;

    //    kf.H[0, 0] = 1;
    //    kf.H[1, 1] = 1;
    //    kf.H[2, 2] = 1;
    //}

    private void DrawObject(double x, double y, double z)
    {
        trackedObject.transform.position = new Vector3((float)x, (float)y, (float)z);
    }
}