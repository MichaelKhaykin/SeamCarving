using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving.CoreLogic
{
    public static class MainComponent
    {
        public static List<double[,]> GetDels(double[,] image)
        {
            int[,] sY = new int[,]
            {
                { -1, -2, -1 },
                { 0, 0, 0 },
                { 1, 2, 1 },
            };
            int[,] sX = new int[,]
            {
                { -1, 0, 1 },
                { -2, 0, 2 },
                { -1, 0, 1 }
            };

            double[,] vY = new double[image.GetLength(0) - 2, image.GetLength(1) - 2];
            double[,] vX = new double[image.GetLength(0) - 2, image.GetLength(1) - 2];

            //remember we are going from 1 to length - 1 because we have a 3x3 matrix so it would
            //stick out on both sides
            for (int i = 1; i < image.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < image.GetLength(1) - 1; j++)
                {
                    #region sY
                    double ans = 0;
                    for (int m = 0; m < sY.GetLength(0); m++)
                    {
                        for (int n = 0; n < sY.GetLength(1); n++)
                        {
                            ans += (sY[m, n] * image[m + i - 1, n + j - 1]);
                        }
                    }
                    vY[i - 1, j - 1] = ans;
                    #endregion
                    #region sX
                    ans = 0;
                    for (int m = 0; m < sX.GetLength(0); m++)
                    {
                        for (int n = 0; n < sX.GetLength(1); n++)
                        {
                            ans += (sX[m, n] * image[m + i - 1, n + j - 1]);
                        }
                    }
                    vX[i - 1, j - 1] = ans;
                    #endregion
                }
            }

            return new List<double[,]>() { vX, vY };
        }
        public static double[,] ComputeEnergyLevels(double[,] delX, double[,] delY)
        {
            int width = delX.GetLength(1);
            int height = delX.GetLength(0);

            double[,] energyLevels = new double[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var calc = delX[i, j] * delX[i, j] + delY[i, j] * delY[i, j];
                    energyLevels[i, j] = Math.Sqrt(calc);
                }
            }

            return energyLevels;
        }
        public static double[,] GenerateDP(double[,] energyLevelMap)
        {
            int width = energyLevelMap.GetLength(1);
            int height = energyLevelMap.GetLength(0);


            double[,] dp = new double[energyLevelMap.GetLength(0), energyLevelMap.GetLength(1)];

            //copy initial data
            for (int i = 0; i < width; i++)
            {
                dp[height - 1, i] = energyLevelMap[height - 1, i];
            }

            //fill rest of dp
            for (int i = height - 2; i >= 0; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    var downLeft = j - 1 >= 0 ? dp[i + 1, j - 1] : int.MaxValue;
                    var down = dp[i + 1, j];
                    var downRight = j + 1 < width ? dp[i + 1, j + 1] : int.MaxValue;

                    /*
                    if(downLeft == int.MaxValue)
                    {
                        downLeft = belly;
                    }
                    if(down == int.MaxValue)
                    {
                        down = belly;
                    }
                    if(downRight == int.MaxValue)
                    {
                        downRight = belly;
                    }
                    */

                    var energy = energyLevelMap[i, j];
                    /*
                    if(energy == int.MaxValue)
                    {
                        energy = belly;
                    }
                    */
                    dp[i, j] = Math.Min(Math.Min(downLeft, down), downRight) + energy;
                }
            }

            return dp;
        }
        public static HashSet<(int, int)> FindSeam(double[,] dp)
        {
            int width = dp.GetLength(1);
            int height = dp.GetLength(0);

            //find initial min point in dp table
            HashSet<(int, int)> seamPath = new HashSet<(int, int)>();
            double min = double.PositiveInfinity;
            int ind = -1;
            for (int i = 0; i < width; i++)
            {
                if (dp[0, i] < min)
                {
                    min = dp[0, i];
                    ind = i;
                }
            }

            //add to path
            (int, int) pos = (0, ind);
            seamPath.Add(pos);

            while (pos.Item1 + 1 < height)
            {
                var downLeft = (pos.Item1 + 1, pos.Item2 - 1);
                var down = (pos.Item1 + 1, pos.Item2);
                var downRight = (pos.Item1 + 1, pos.Item2 + 1);

                var downLeftVal = pos.Item2 - 1 >= 0 ? dp[downLeft.Item1, downLeft.Item2] : int.MaxValue;
                var downVal = dp[down.Item1, down.Item2];
                var downRightVal = pos.Item2 + 1 < width ? dp[downRight.Item1, downRight.Item2] : int.MaxValue;

                var best = Math.Min(Math.Min(downLeftVal, downVal), downRightVal);

                if (best == downLeftVal)
                {
                    pos = downLeft;
                }
                else if (best == downVal)
                {
                    pos = down;
                }
                else
                {
                    pos = downRight;
                }

                seamPath.Add(pos);
            }

            return seamPath;
        }
        public static double[,] CarveXAxis(double[,] energyLevelMap, HashSet<(int, int)> path)
        {
            int width = energyLevelMap.GetLength(1);
            int height = energyLevelMap.GetLength(0);

            double[,] newImage = new double[height, width - 1];

            for (int i = 0; i < height; i++)
            {
                int xAxis = 0;
                for (int j = 0; j < width; j++)
                {
                    if (path.Contains((i, j))) continue;

                    newImage[i, xAxis] = energyLevelMap[i, j];
                    xAxis++;
                }
            }

            return newImage;
        }
        public static double[,] GaussianBlur(double[,] original)
        {
            int[,] matrix = new int[,]
            {
                { 2, 1, 2, 1, 2 },
                { 1, 1, 4, 1, 1 },
                { 2, 4, 8, 4, 2 },
                { 1, 1, 4, 1, 1 },
                { 2, 1, 2, 1, 2 },
            };

            int offset = matrix.GetLength(0) / 2;

            double matrixSum = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrixSum += matrix[i, j];
                }
            }

            double[,] blur = new double[original.GetLength(0), original.GetLength(1)];
            Array.Copy(original, blur, original.GetLength(0) * original.GetLength(1));

            for (int i = offset; i < original.GetLength(0) - offset; i++)
            {
                for (int j = offset; j < original.GetLength(1) - offset; j++)
                {
                    double blurredVal = 0;
                    for (int y = 0; y < matrix.GetLength(0); y++)
                    {
                        for (int x = 0; x < matrix.GetLength(1); x++)
                        {
                            blurredVal += original[y + i - offset, x + j - offset] * matrix[y, x];
                        }
                    }
                    blurredVal /= matrixSum;

                    blur[i, j] = blurredVal;
                }
            }

            return blur;
        }

        public static int TwoToOne(int y, int x, int width)
        {
            return y * width + x;
        }
        public static Complex[,] DFT(double[,] signal)
        {
            var n = signal.GetLength(0) * signal.GetLength(1);
            
            Complex[,] dft = new Complex[signal.GetLength(0), signal.GetLength(1)];

            for(int i = 0; i < signal.GetLength(0); i++)
            {
                for(int j = 0; j < signal.GetLength(1); j++)
                {
                    var a = TwoToOne(i, j, signal.GetLength(0));

                    Complex current = new Complex(0, 0);
                    for(int y = 0; y < signal.GetLength(0); y++)
                    {
                        for(int x = 0; x < signal.GetLength(1); x++)
                        {
                            var b = TwoToOne(y, x, signal.GetLength(0));

                            double angle = -2 * Math.PI * a * b / n;

                            current += signal[y, x] * Complex.Exp(new Complex(0, angle));
                        }
                    }

                    dft[i, j] = current;
                }
            }

            return dft;
        }


        //Testing
        public static double[,] GenerateDPHorizontally(double[,] energyLevelMap)
        {
            int width = energyLevelMap.GetLength(1);
            int height = energyLevelMap.GetLength(0);

            double[,] dp = new double[energyLevelMap.GetLength(0), energyLevelMap.GetLength(1)];

            //copy initial data
            for (int i = 0; i < height; i++)
            {
                dp[i, width - 1] = energyLevelMap[i, width - 1];
            }

            //fill rest of dp
            for (int i = width - 2; i >= 0; i--)
            {
                for (int j = 0; j < height; j++)
                {
                    var energy = energyLevelMap[j, i];
                    dp[j, i] = dp[j, i + 1] + energy;
                }
            }


            return dp;
        }

        
        public static HashSet<(int, int)> FindHorizontalSeam(double[,] dp)
        {
            int width = dp.GetLength(1);
            int height = dp.GetLength(0);

            //find initial min point in dp table
            HashSet<(int, int)> seamPath = new HashSet<(int, int)>();
            double min = double.PositiveInfinity;
            int ind = -1;
            for (int i = 0; i < height; i++)
            {
                if (dp[i, 0] < min)
                {
                    min = dp[i, 0];
                    ind = i;
                }
            }

            for (int i = 0; i < width; i++)
            {
                seamPath.Add((ind, i));
            }

            return seamPath;
        }
        public static double[,] CarveYAxis(double[,] energyLevelMap, HashSet<(int, int)> path)
        {
            int width = energyLevelMap.GetLength(1);
            int height = energyLevelMap.GetLength(0);

            double[,] newImage = new double[height - 1, width];

            int skipIndex = path.First().Item1;

            int yAxis = 0;
            for (int i = 0; i < height; i++)
            {
                if (i == skipIndex) continue;

                for (int j = 0; j < width; j++)
                {
                    newImage[yAxis, j] = energyLevelMap[i, j];
                }

                yAxis++;
            }


            return newImage;
        }
    }
}
