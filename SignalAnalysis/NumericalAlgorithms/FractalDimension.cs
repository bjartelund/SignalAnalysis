using FftSharp.Windows;

namespace SignalAnalysis;

/// <summary>
/// This class implements the computation of the fractal dimension of a discrete curve according to
/// Carlos Sevcik's "A procedure to estimate the fractal dimension of waveforms"
/// <seealso cref="https://arxiv.org/abs/1003.5266"/>
/// </summary>
public static class FractalDimension
{

    /// <summary>
    /// Hausdorff-Besicovitch dimension for each cumulative segment
    /// </summary>
    public static double[] DimensionCumulative { get; private set; } = Array.Empty<double>();

    /// <summary>
    /// Hausdorff-Besicovitch dimension for the whole data set
    /// </summary>
    public static double DimensionSingle { get; private set; } = double.NaN;

    public static double VarianceH { get; private set; } = double.NaN;

    private static readonly double DimensionMinimum = 1.0;

    public static void ComputeDimension(double[] xValues, double[] yValues, CancellationToken ct, bool progress = false)
    {
        if (!progress)
            (DimensionSingle, VarianceH) = ComputeH(xValues, yValues);
        else
        {
            DimensionCumulative = new double[yValues.Length];
            // Compute all but the last point
            for (int i = 0; i < yValues.Length - 1; i++)
            {
                (DimensionCumulative[i], _) = ComputeH(xValues, yValues, i);
                if (ct.IsCancellationRequested)
                    throw new OperationCanceledException("CancelFractal", ct);
            }
            // Finally, compute the last point and get both the dimension and the variance
            (DimensionSingle, VarianceH) = ComputeH(xValues, yValues, yValues.Length - 1);
            DimensionCumulative[^1] = DimensionSingle;
        }
    }

    public static void ComputeDimension(double samplingFreq, double[] yValues, CancellationToken ct, bool progress = false)
    {
        if (!progress)
            (DimensionSingle, VarianceH) = ComputeH(samplingFreq, yValues);
        else
        {
            DimensionCumulative = new double[yValues.Length];
            DimensionCumulative = ComputeH(yValues);
            // Compute all but the last point
            for (int i = 0; i < yValues.Length - 1; i++)
            {
                (DimensionCumulative[i], _) = ComputeH(samplingFreq, yValues, i);
                if (ct.IsCancellationRequested)
                    throw new OperationCanceledException("CancelFractal", ct);
            }
            // Finally, compute the last point and get both the dimension and the variance
            (DimensionSingle, VarianceH) = ComputeH(samplingFreq, yValues, yValues.Length - 1);
            DimensionCumulative[^1] = DimensionSingle;
        }
    }

    /// <summary>
    /// Computes the fractal dimension of a discrete curve.
    /// </summary>
    /// <param name="xValues">Array containing the abscissa points</param>
    /// <param name="yValues">Array containing the ordinate points</param>
    /// /// <param name="arrayIndex">Array index cutoff. Array values above this index are ignored</param>
    /// <returns>The fractal dimension of the curve and its variance</returns>
    private static (double dimension, double variance) ComputeH(double[] xValues, double[] yValues, int? arrayIndex = null)
    {
        int _nPoints;
        double _xMax;
        double _xMin;
        double _yMax;
        double _yMin;
        double _length;
        double _varLength;
        double _LN;
        double dimensionH;
        double varianceH;

        if (arrayIndex.HasValue)
            _nPoints = arrayIndex.Value + 1;
        else
            _nPoints = yValues.Length;

        if (_nPoints < 2)
        {
            dimensionH = DimensionMinimum;
            varianceH = double.NaN;
        }
        else
        {
            (_xMax, _xMin) = GetMaxMin(xValues, arrayIndex);
            (_yMax, _yMin) = GetMaxMin(yValues, arrayIndex);

            (_length, _varLength) = NormalizedLength(xValues, yValues, _nPoints, _xMax, _xMin, _yMax, _yMin);
            _LN = System.Math.Log(2 * (_nPoints - 1));
            dimensionH = 1 + System.Math.Log(_length) / _LN;
            varianceH = (_nPoints - 1) * _varLength / (_length * _LN * _LN);
        }

        return (dimensionH, varianceH);
    }

    /// <summary>
    /// Computes the fractal dimension of a discrete curve with a constant sampling rate starting at time 0 seconds.
    /// </summary>
    /// <param name="samplingFreq">The sampling frequency</param>
    /// <param name="yValues">Array containing the ordinate points</param>
    /// <param name="arrayIndex">Array index cutoff. Array values above this index are ignored</param>
    /// <returns>The fractal dimension of the curve and its variance</returns>
    private static (double dimension, double variance) ComputeH(double samplingFreq, double[] yValues, int? arrayIndex = null)
    {
        int _nPoints;
        double _yMax;
        double _yMin;
        double _length;
        double _LN;
        double _varLength;
        double dimensionH;
        double varianceH;

        if (arrayIndex.HasValue)
            _nPoints = arrayIndex.Value + 1;
        else
            _nPoints = yValues.Length;

        if (_nPoints < 2)
        {
            dimensionH = DimensionMinimum;
            varianceH = double.NaN;
        }
        else
        {
            (_yMax, _yMin) = GetMaxMin(yValues, arrayIndex);

            (_length, _varLength) = NormalizedLength(yValues, _nPoints, _yMax, _yMin);
            _LN = System.Math.Log10(2 * (_nPoints - 1));
            dimensionH = 1 + System.Math.Log10(_length) / _LN;
            varianceH = (_nPoints - 1) * _varLength / (_length * _LN * _LN);
        }

        return (dimensionH, varianceH);
    }

    /// <summary>
    /// Computes the fractal dimension of a discrete curve with a constant sampling rate starting at time 0 seconds.
    /// </summary>
    /// <param name="samplingFreq">The sampling frequency</param>
    /// <param name="yValues">Array containing the ordinate points</param>
    /// <param name="arrayIndex">Array index cutoff. Array values above this index are ignored</param>
    /// <returns>The fractal dimension of the curve and its variance</returns>
    private static double [] ComputeH(double[] yValues)
    {
        double[] _yMax = new double[yValues.Length];
        double[] _yMin = new double[yValues.Length];
        double[] partialSeg = new double[yValues.Length];
        bool _recompute = false;

        _yMax[0] = yValues[0];
        _yMin[0] = yValues[0];
        partialSeg[0] = 0;
        DimensionCumulative[0] = DimensionMinimum;
        for (int i = 1; i < yValues.Length - 1; i++)
        {
            if (yValues[i] > _yMax[i - 1])
            {
                _yMax[i] = yValues[i];
                _recompute = true;
            }
            if (yValues[i] < _yMin[i - 1])
            {
                _yMin[i] = yValues[i];
                _recompute = true;
            }

            if (!_recompute)
                partialSeg[i] = partialSeg[i - 1] + Math.Pow((yValues[i] - yValues[i - 1]) / (_yMax[i] - _yMin[i]), 2);
            else
            {
                double sumSeg = 0;
                for (int j = i; j <= i; j++)
                    sumSeg += Math.Pow((yValues[j] - yValues[j - 1]) / (_yMax[i] - _yMin[i]), 2);
                partialSeg[i] = sumSeg;
            }
            partialSeg[i] += Math.Pow(1 / (i + 1 - 1), 2);

            DimensionCumulative[i] = 1 + System.Math.Log10(partialSeg[i]) / System.Math.Log10(2 * (i + 1 - 1));
        }

        return DimensionCumulative;
    }

    /// <summary>
    /// Computes the normalized length of a discrete curve.
    /// To do so, the passed values are normalized to be in the range [0, 1] both in the abscissa and the ordinate axes.
    /// </summary>
    /// <param name="xValues">Array containing the abscissa points</param>
    /// <param name="yValues">Array containing the ordinate points</param>
    /// <param name="nPoints">Number of points</param>
    /// <param name="xMax">The maximum x value</param>
    /// <param name="xMin">The minimum x value</param>
    /// <param name="yMax">The maximum y value</param>
    /// <param name="yMin">The minimum y value</param>
    /// <returns>The normalized length of the curve and its variance</returns>
    private static (double normLength, double variance) NormalizedLength(double[] xValues, double[] yValues, int nPoints, double xMax, double xMin, double yMax, double yMin)
    {
        double lengthTotal = 0.0;
        double lengthPartial;
        double mean = 0;
        double oldMean;
        double variance = 0;
        double xNorm1;
        double xNorm2 = 0.0;
        double yNorm1;
        double yNorm2 = 0.0;

        if (nPoints <= 1) return (0.0, 0.0);

        for (int i = 0; i < nPoints; i++)
        {
            xNorm1 = (xValues[i] - xMin) / (xMax - xMin);
            yNorm1 = (yValues[i] - yMin) / (yMax - yMin);
            if (i > 0)
            {
                lengthPartial = System.Math.Sqrt(System.Math.Pow(yNorm1 - yNorm2, 2) + System.Math.Pow(xNorm1 - xNorm2, 2));
                lengthTotal += lengthPartial;

                // Compute variance incrementally
                oldMean = mean;
                mean += (lengthPartial - mean) / i;
                variance += (lengthPartial - mean) * (lengthPartial - oldMean);
            }
            xNorm2 = xNorm1;
            yNorm2 = yNorm1;
        }

        variance /= (nPoints - 1);
        return (lengthTotal, variance);
    }

    /// <summary>
    /// Computes the normalized length of a discrete curve with a constant sampling rate starting at time 0 seconds.
    /// The passed values are normalized to be in the range [0, 1] both in the abscissa and the ordinate axes.
    /// </summary>
    /// <param name="yValues">Array containing the ordinate points</param>
    /// <param name="nPoints">Number of points</param>
    /// <param name="yMax">The maximum y value</param>
    /// <param name="yMin">The minimum y value</param>
    /// <returns>The normalized length of the curve and its variance</returns>
    private static (double normLength, double variance) NormalizedLength(double[] yValues, int nPoints, double yMax, double yMin)
    {
        double lengthTotal = 0;
        double lengthPartial;
        double mean = 0;
        double oldMean;
        double variance = 0;
        double yNorm1;
        double yNorm2 = 0;

        if (nPoints <= 1) return (0.0, 0.0);

        for (int i = 0; i < nPoints; i++)
        {
            if (yMax == yMin)
                yNorm1 = 0.0;
            else
                yNorm1 = (yValues[i] - yMin) / (yMax - yMin);
            
            if (i > 0)
            {
                lengthPartial = System.Math.Sqrt(System.Math.Pow(yNorm1 - yNorm2, 2) + System.Math.Pow(1 / (nPoints - 1), 2));
                lengthTotal += lengthPartial;

                // Compute variance incrementally
                oldMean = mean;
                mean += (lengthPartial - mean) / i;
                variance += (lengthPartial - mean) * (lengthPartial - oldMean);
            }
            yNorm2 = yNorm1;
        }

        variance /= (nPoints - 1);
        return (lengthTotal, variance);
    }

    /// <summary>
    /// Get the maximum and minimum values whithin an array.
    /// </summary>
    /// <param name="values">Array of values</param>
    /// <param name="arrayIndex">Array index cutoff. Array values above this index are ignored</param>
    /// <returns>Maximum and minimum values</returns>
    private static (double max, double min) GetMaxMin(double[] values, int? arrayIndex = null)
    {
        if (values.Length == 0) return (0.0, 0.0);

        double highest = values[0];
        double lowest = values[0];
        int nPoints;

        if (arrayIndex.HasValue)
            nPoints = arrayIndex.Value + 1;
        else
            nPoints = values.Length;

        for (int i = 1; i < nPoints; i++)
        {
            if (values[i] > highest)
                highest = values[i];
            if (values[i] < lowest)
                lowest = values[i];
        }

        return (highest, lowest);
    }
}