using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace SimpleBinaryClassification
{
    public partial class Form1 : Form
    {
        private PictureBox pictureBox;
        private bool pictureBoxCreated = false;

        public Form1()
        {
            InitializeComponent();
            CreatePictureBox();
        }

        private void CreatePictureBox()
        {
            pictureBox = new PictureBox();
            pictureBox.Parent = this;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Size = new Size(200, 200);
            pictureBox.Paint += new PaintEventHandler(pictureBox2_Paint);
            pictureBoxCreated = true;
        }



        private void btnClassify_Click(object sender, EventArgs e)
        {
            if (!pictureBoxCreated)
            {
                CreatePictureBox();
            }

            double[] weights = new double[3];
            Random rand = new Random();

            // Initialize weights with random values between -1 and 1
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = rand.NextDouble() * 2 - 1;
            }

            double learningRate = 0.1;
            int iterations = 100;
            int[][] trainingData = new int[][] {
                new int[] {0, 0, 1},
                new int[] {0, 1, 0},
                new int[] {1, 0, 0},
                new int[] {1, 1, 1}
            };

            // Train the Perceptron algorithm
            for (int i = 0; i < iterations; i++)
            {
                foreach (int[] data in trainingData)
                {
                    int input1 = data[0];
                    int input2 = data[1];
                    int target = data[2];

                    double output = weights[0] * input1 + weights[1] * input2 + weights[2];

                    // Apply hard-limiting function
                    if (output > 0) output = 1;
                    else output = 0;

                    double error = target - output;

                    // Adjust weights
                    weights[0] += learningRate * error * input1;
                    weights[1] += learningRate * error * input2;
                    weights[2] += learningRate * error;
                }
            }

            // Classify new data
            double testInput1 = double.Parse(txtX1.Text);
            double testInput2 = double.Parse(txtX2.Text);

            double testOutput = weights[0] * testInput1 + weights[1] * testInput2 + weights[2];

            // Apply hard-limiting function
            if (testOutput > 0) testOutput = 1;
            else testOutput = 0;

            lblResult.Text = " " + testOutput.ToString();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            // Draw a scatter plot of the training data
            SolidBrush brush = new SolidBrush(Color.Black);          
            Pen pen = new Pen(brush);
            pen.Width = 1;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, pictureBox.Width - 1, pictureBox.Height - 1));

            brush.Color = Color.Red;
            e.Graphics.FillEllipse(brush, 50, 50, 10, 10); // Target = 1
            brush.Color = Color.Blue;
            e.Graphics.FillEllipse(brush, 50, 150, 10, 10); // Target = 0
            brush.Color = Color.Blue;
            e.Graphics.FillEllipse(brush, 150, 50, 10, 10); // Target = 0
            brush.Color = Color.Red;
            e.Graphics.FillEllipse(brush, 150, 150, 10, 10); // Target = 1
        }
    }
}
