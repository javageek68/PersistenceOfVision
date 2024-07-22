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

    public void Update(Matrix z)
    {
        var y = z - (H * X);
        var S = H * P * H.Transpose() + R;
        var K = P * H.Transpose() * S.Inverse();

        X = X + (K * y);
        P = (Matrix.Identity(n) - (K * H)) * P;
    }
}
