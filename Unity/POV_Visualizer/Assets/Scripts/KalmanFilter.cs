using System;

public class KalmanFilter
{
    private int n; // Number of states
    private int m; // Number of measurements

    public Matrix X { get; private set; } // State vector
    public Matrix P { get; private set; } // Error covariance matrix
    public Matrix F { get; private set; } // State transition matrix
    public Matrix H { get; private set; } // Measurement matrix
    public Matrix R { get; private set; } // Measurement noise covariance matrix
    public Matrix Q { get; private set; } // Process noise covariance matrix

    public static KalmanFilter InitKalmanFilter(int stateDimension, int measurementDimension)
    {
        KalmanFilter kf;
        kf = new KalmanFilter(stateDimension, measurementDimension);
        kf.X[0, 0] = 0; // x
        kf.X[1, 0] = 0; // y
        kf.X[2, 0] = 0; // z
        kf.X[3, 0] = 1; // vx
        kf.X[4, 0] = 1; // vy
        kf.X[5, 0] = 1; // vz

        double dt = 1.0; // Time step
        kf.F[0, 0] = 1; kf.F[0, 3] = dt;
        kf.F[1, 1] = 1; kf.F[1, 4] = dt;
        kf.F[2, 2] = 1; kf.F[2, 5] = dt;
        kf.F[3, 3] = 1;
        kf.F[4, 4] = 1;
        kf.F[5, 5] = 1;

        kf.H[0, 0] = 1;
        kf.H[1, 1] = 1;
        kf.H[2, 2] = 1;

        return kf;
    }

    public KalmanFilter(int stateDimension, int measurementDimension)
    {
        n = stateDimension;
        m = measurementDimension;

        X = new Matrix(n, 1);
        P = Matrix.Identity(n);
        F = Matrix.Identity(n);
        H = new Matrix(m, n);
        R = Matrix.Identity(m);
        Q = Matrix.Identity(n);
    }

    public void Predict()
    {
        X = F * X;
        P = F * P * F.Transpose() + Q;
    }

    public void Update(double?[] measurements)
    {
        var z = new Matrix(this.m, 1);
        for (int d = 0; d < this.m; d++)
        {
            z[d, 0] = measurements[d].Value;
        }

        this.Update(z);
    }

    public void Update(Matrix z)
    {
        var y = z - (H * X);
        var S = H * P * H.Transpose() + R;
        var K = P * H.Transpose() * S.Inverse();

        X = X + (K * y);
        P = (Matrix.Identity(n) - (K * H)) * P;
    }
}
