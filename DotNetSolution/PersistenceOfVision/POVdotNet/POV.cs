using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POVdotNet
{
    public partial class POV : Form
    {
        public POV()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            TestPov();
        }

        private void TestPov()
        {
            // Define state and measurement dimensions
            int stateDimension = 6; // [x, y, z, vx, vy, vz]
            int measurementDimension = 3; // [x, y, z]

            // Initialize Kalman filter
            var kf = new KalmanFilter(stateDimension, measurementDimension);

            // Set initial state (position and velocity)
            kf.X[0, 0] = 0; // x
            kf.X[1, 0] = 0; // y
            kf.X[2, 0] = 0; // z
            kf.X[3, 0] = 1; // vx
            kf.X[4, 0] = 1; // vy
            kf.X[5, 0] = 1; // vz

            // Set state transition matrix
            double dt = 1.0; // Time step
            kf.F[0, 0] = 1; kf.F[0, 3] = dt;
            kf.F[1, 1] = 1; kf.F[1, 4] = dt;
            kf.F[2, 2] = 1; kf.F[2, 5] = dt;
            kf.F[3, 3] = 1;
            kf.F[4, 4] = 1;
            kf.F[5, 5] = 1;

            // Set measurement matrix
            kf.H[0, 0] = 1;
            kf.H[1, 1] = 1;
            kf.H[2, 2] = 1;

            // Simulate measurements (some measurements are occluded)
            var measurements = new double?[,]
            {
            { 1, 1, 1 },
            { 2, 2, 2 },
            { null, null, null }, // Occlusion
            { 4, 4, 4 },
            { 5, 5, 5 }
            };

            for (int i = 0; i < measurements.GetLength(0); i++)
            {
                // Predict step
                kf.Predict();

                // Update step if measurement is available
                if (measurements[i, 0].HasValue)
                {
                    var z = new Matrix(measurementDimension, 1);
                    z[0, 0] = measurements[i, 0].Value;
                    z[1, 0] = measurements[i, 1].Value;
                    z[2, 0] = measurements[i, 2].Value;

                    kf.Update(z);
                }

                // Output the predicted state
                Console.WriteLine($"Predicted state at step {i}:");
                Console.WriteLine($"Position: ({kf.X[0, 0]}, {kf.X[1, 0]}, {kf.X[2, 0]})");
                Console.WriteLine($"Velocity: ({kf.X[3, 0]}, {kf.X[4, 0]}, {kf.X[5, 0]})");
                Console.WriteLine();
            }
        }
    }
}
