namespace TestDataUSBasicLibrary.Distributions.Gaussian;
public static class ExtensionsForRandomizer
{

    // Coefficients used in Acklam's Inverse Normal Cumulative Distribution function.
    private static readonly double[] _acklamsCoefficientA =
       [-39.696830d, 220.946098d, -275.928510d, 138.357751d, -30.664798d, 2.506628d];

    private static readonly double[] _acklamsCoefficientB =
       [-54.476098d, 161.585836d, -155.698979d, 66.801311d, -13.280681d];

    private static readonly double[] _acklamsCoefficientC =
       [-0.007784894002d, -0.32239645d, -2.400758d, -2.549732d, 4.374664d, 2.938163d];

    private static readonly double[] _acklamsCoefficientD = [0.007784695709d, 0.32246712d, 2.445134d, 3.754408d];

    // Break-Points used in Acklam's Inverse Normal Cumulative Distribution function.
    private const double _acklamsLowBreakPoint = 0.02425d;
    private const double _acklamsHighBreakPoint = 1.0d - _acklamsLowBreakPoint;


    /// <summary>
    /// This algorithm follows Peter J Acklam's Inverse Normal Cumulative Distribution function.
    /// Reference: P.J. Acklam, "An algorithm for computing the inverse normal cumulative distribution function," 2010
    /// </summary>
    /// <returns>
    /// A double between 0.0 and 1.0
    /// </returns>
    private static double InverseNCD(double probability)
    {
        // Rational approximation for lower region of distribution
        if (probability < _acklamsLowBreakPoint)
        {
            double q = Math.Sqrt(-2 * Math.Log(probability));
            return (((((_acklamsCoefficientC[0] * q + _acklamsCoefficientC[1]) * q + _acklamsCoefficientC[2]) * q + _acklamsCoefficientC[3]) * q +
                     _acklamsCoefficientC[4]) * q + _acklamsCoefficientC[5]) /
                   ((((_acklamsCoefficientD[0] * q + _acklamsCoefficientD[1]) * q + _acklamsCoefficientD[2]) * q + _acklamsCoefficientD[3]) * q + 1);
        }

        // Rational approximation for upper region of distribution
        if (_acklamsHighBreakPoint < probability)
        {
            double q = Math.Sqrt(-2 * Math.Log(1 - probability));
            return -(((((_acklamsCoefficientC[0] * q + _acklamsCoefficientC[1]) * q + _acklamsCoefficientC[2]) * q + _acklamsCoefficientC[3]) * q +
                      _acklamsCoefficientC[4]) * q + _acklamsCoefficientC[5]) /
                   ((((_acklamsCoefficientD[0] * q + _acklamsCoefficientD[1]) * q + _acklamsCoefficientD[2]) * q + _acklamsCoefficientD[3]) * q + 1);
        }

        // Rational approximation for central region of distribution
        {
            double q = probability - 0.5d;
            double r = q * q;
            return (((((_acklamsCoefficientA[0] * r + _acklamsCoefficientA[1]) * r + _acklamsCoefficientA[2]) * r + _acklamsCoefficientA[3]) * r +
                     _acklamsCoefficientA[4]) * r + _acklamsCoefficientA[5]) * q /
                   (((((_acklamsCoefficientB[0] * r + _acklamsCoefficientB[1]) * r + _acklamsCoefficientB[2]) * r + _acklamsCoefficientB[3]) * r +
                     _acklamsCoefficientB[4]) * r + 1);
        }
    }

    extension (Randomizer rnd)
    {
        /// <summary>
        /// Generate a random double, based on the specified normal distribution.
        /// <example>
        /// To create random values around an average height of 69.1
        /// inches with a standard deviation of 2.9 inches away from the mean
        /// <code>
        /// GaussianDouble(69.1, 2.9)
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="mean">Mean value of the normal distribution</param>
        /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
        public double GaussianDouble(double mean, double standardDeviation)
        {
            double p = InverseNCD(rnd.Double(0D, 1D));
            return p * standardDeviation + mean;
        }

        /// <summary>
        /// Generate a random int, based on the specified normal distribution.
        /// <example>
        /// To create random int values around an average age of 35 years, with
        /// a standard deviation of 4 years away from the mean.
        /// </example>
        /// <code>
        /// call GaussianInt(35, 4)
        /// </code>
        /// </summary>
        /// <param name="mean">Mean average of the normal distribution</param>
        /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
        public int GaussianInt(double mean, double standardDeviation)
        {
            return Convert.ToInt32(rnd.GaussianDouble(mean, standardDeviation));
        }

        /// <summary>
        /// Generate a float decimal, based on the specified normal distribution.
        /// <example>
        /// To create random float values around an average height of 69.1
        /// inches with a standard deviation of 2.9 inches away from the mean
        /// <code>
        /// GaussianFloat(69.1, 2.9)
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="mean">Mean average of the normal distribution</param>
        /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
        public float GaussianFloat(double mean, double standardDeviation)
        {
            return Convert.ToSingle(rnd.GaussianDouble(mean, standardDeviation));
        }

        /// <summary>
        /// Generate a random decimal, based on the specified normal distribution.
        /// <example>
        /// To create random values around an average height of 69.1
        /// inches with a standard deviation of 2.9 inches away from the mean
        /// <code>
        /// GaussianDecimal(69.1, 2.9)
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="mean">Mean average of the normal distribution</param>
        /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
        public decimal GaussianDecimal(double mean, double standardDeviation)
        {
            return Convert.ToDecimal(rnd.GaussianDouble(mean, standardDeviation));
        }
    }

    

}
