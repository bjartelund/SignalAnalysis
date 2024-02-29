﻿namespace SignalAnalysis.UnitTest;

[TestClass]
public class DerivativeTest
{
    private readonly double[] sin1Hz = [0, 0.06279052, 0.125333234, 0.187381315, 0.248689887, 0.309016994, 0.368124553, 0.425779292, 0.481753674, 0.535826795, 0.587785252, 0.63742399,
        0.684547106, 0.728968627, 0.770513243, 0.809016994, 0.844327926, 0.87630668, 0.904827052, 0.929776486, 0.951056516, 0.968583161, 0.982287251, 0.992114701, 0.998026728,
        1, 0.998026728, 0.992114701, 0.982287251, 0.968583161, 0.951056516, 0.929776486, 0.904827052, 0.87630668, 0.844327926, 0.809016994, 0.770513243, 0.728968627, 0.684547106,
        0.63742399, 0.587785252, 0.535826795, 0.481753674, 0.425779292, 0.368124553, 0.309016994, 0.248689887, 0.187381315, 0.125333234, 0.06279052, 0, -0.06279052, -0.125333234,
        -0.187381315, -0.248689887, -0.309016994, -0.368124553, -0.425779292, -0.481753674, -0.535826795, -0.587785252, -0.63742399, -0.684547106, -0.728968627, -0.770513243,
        -0.809016994, -0.844327926, -0.87630668, -0.904827052, -0.929776486, -0.951056516, -0.968583161, -0.982287251, -0.992114701, -0.998026728, -1, -0.998026728, -0.992114701,
        -0.982287251, -0.968583161, -0.951056516, -0.929776486, -0.904827052, -0.87630668, -0.844327926, -0.809016994, -0.770513243, -0.728968627, -0.684547106, -0.63742399,
        -0.587785252, -0.535826795, -0.481753674, -0.425779292, -0.368124553, -0.309016994, -0.248689887, -0.187381315, -0.125333234, -0.06279052, 0];

    private readonly double[] cos1Hz = [1, 0.998026728, 0.992114701, 0.982287251, 0.968583161, 0.951056516, 0.929776486, 0.904827052, 0.87630668, 0.844327926, 0.809016994, 0.770513243,
        0.728968627, 0.684547106, 0.63742399, 0.587785252, 0.535826795, 0.481753674, 0.425779292, 0.368124553, 0.309016994, 0.248689887, 0.187381315, 0.125333234, 0.06279052,
        0, -0.06279052, -0.125333234, -0.187381315, -0.248689887, -0.309016994, -0.368124553, -0.425779292, -0.481753674, -0.535826795, -0.587785252, -0.63742399, -0.684547106,
        -0.728968627, -0.770513243, -0.809016994, -0.844327926, -0.87630668, -0.904827052, -0.929776486, -0.951056516, -0.968583161, -0.982287251, -0.992114701, -0.998026728, -1,
        -0.998026728, -0.992114701, -0.982287251, -0.968583161, -0.951056516, -0.929776486, -0.904827052, -0.87630668, -0.844327926, -0.809016994, -0.770513243, -0.728968627,
        -0.684547106, -0.63742399, -0.587785252, -0.535826795, -0.481753674, -0.425779292, -0.368124553, -0.309016994, -0.248689887, -0.187381315, -0.125333234, -0.06279052,
        0, 0.06279052, 0.125333234, 0.187381315, 0.248689887, 0.309016994, 0.368124553, 0.425779292, 0.481753674, 0.535826795, 0.587785252, 0.63742399, 0.684547106, 0.728968627,
        0.770513243, 0.809016994, 0.844327926, 0.87630668, 0.904827052, 0.929776486, 0.951056516, 0.968583161, 0.982287251, 0.992114701, 0.998026728, 1];

    private readonly double[] sin2Hz = [0, 0.062666617, 0.124344944, 0.184062276, 0.240876837, 0.293892626, 0.342273553, 0.385256621, 0.422163963, 0.452413526, 0.475528258,
        0.491143625, 0.499013364, 0.499013364, 0.491143625, 0.475528258, 0.452413526, 0.422163963, 0.385256621, 0.342273553, 0.293892626, 0.240876837, 0.184062276, 0.124344944,
        0.062666617, 0, -0.062666617, -0.124344944, -0.184062276, -0.240876837, -0.293892626, -0.342273553, -0.385256621, -0.422163963, -0.452413526, -0.475528258, -0.491143625,
        -0.499013364, -0.499013364, -0.491143625, -0.475528258, -0.452413526, -0.422163963, -0.385256621, -0.342273553, -0.293892626, -0.240876837, -0.184062276, -0.124344944,
        -0.062666617, 0, 0.062666617, 0.124344944, 0.184062276, 0.240876837, 0.293892626, 0.342273553, 0.385256621, 0.422163963, 0.452413526, 0.475528258, 0.491143625,
        0.499013364, 0.499013364, 0.491143625, 0.475528258, 0.452413526, 0.422163963, 0.385256621, 0.342273553, 0.293892626, 0.240876837, 0.184062276, 0.124344944, 0.062666617,
        0, -0.062666617, -0.124344944, -0.184062276, -0.240876837, -0.293892626, -0.342273553, -0.385256621, -0.422163963, -0.452413526, -0.475528258, -0.491143625,
        -0.499013364, -0.499013364, -0.491143625, -0.475528258, -0.452413526, -0.422163963, -0.385256621, -0.342273553, -0.293892626, -0.240876837, -0.184062276, -0.124344944,
        -0.062666617, 0];

    private readonly double[] cos2Hz = [0.5, 0.496057351, 0.484291581, 0.464888243, 0.43815334, 0.404508497, 0.364484314, 0.318711995, 0.267913397, 0.212889646, 0.154508497,
        0.093690657, 0.03139526, -0.03139526, -0.093690657, -0.154508497, -0.212889646, -0.267913397, -0.318711995, -0.364484314, -0.404508497, -0.43815334, -0.464888243,
        -0.484291581, -0.496057351, -0.5, -0.496057351, -0.484291581, -0.464888243, -0.43815334, -0.404508497, -0.364484314, -0.318711995, -0.267913397, -0.212889646, -0.154508497,
        -0.093690657, -0.03139526, 0.03139526, 0.093690657, 0.154508497, 0.212889646, 0.267913397, 0.318711995, 0.364484314, 0.404508497, 0.43815334, 0.464888243, 0.484291581,
        0.496057351, 0.5, 0.496057351, 0.484291581, 0.464888243, 0.43815334, 0.404508497, 0.364484314, 0.318711995, 0.267913397, 0.212889646, 0.154508497, 0.093690657, 0.03139526,
        -0.03139526, -0.093690657, -0.154508497, -0.212889646, -0.267913397, -0.318711995, -0.364484314, -0.404508497, -0.43815334, -0.464888243, -0.484291581, -0.496057351, -0.5,
        -0.496057351, -0.484291581, -0.464888243, -0.43815334, -0.404508497, -0.364484314, -0.318711995, -0.267913397, -0.212889646, -0.154508497, -0.093690657, -0.03139526, 0.03139526,
        0.093690657, 0.154508497, 0.212889646, 0.267913397, 0.318711995, 0.364484314, 0.404508497, 0.43815334, 0.464888243, 0.484291581, 0.496057351, 0.5];

    private readonly double[] sinSum = [0, 0.125457136, 0.249678177, 0.371443591, 0.489566724, 0.602909621, 0.710398106, 0.811035913, 0.903917637, 0.988240321, 1.06331351,
        1.128567615, 1.18356047, 1.227981992, 1.261656868, 1.284545253, 1.296741452, 1.298470643, 1.290083674, 1.272050039, 1.244949142, 1.209459998, 1.166349527, 1.116459645,
        1.060693345, 1, 0.935360112, 0.867769758, 0.798224974, 0.727706324, 0.65716389, 0.587502933, 0.519570431, 0.454142717, 0.391914399, 0.333488736, 0.279369617, 0.229955263,
        0.185533742, 0.146280364, 0.112256994, 0.083413269, 0.059589711, 0.04052267, 0.025851, 0.015124368, 0.00781305, 0.003319038, 0.00098829, 0.000123903, 0, -0.000123903,
        -0.00098829, -0.003319038, -0.00781305, -0.015124368, -0.025851, -0.04052267, -0.059589711, -0.083413269, -0.112256994, -0.146280364, -0.185533742, -0.229955263, -0.279369617,
        -0.333488736, -0.391914399, -0.454142717, -0.519570431, -0.587502933, -0.65716389, -0.727706324, -0.798224974, -0.867769758, -0.935360112, -1, -1.060693345, -1.116459645,
        -1.166349527, -1.209459998, -1.244949142, -1.272050039, -1.290083674, -1.298470643, -1.296741452, -1.284545253, -1.261656868, -1.227981992, -1.18356047, -1.128567615,
        -1.06331351, -0.988240321, -0.903917637, -0.811035913, -0.710398106, -0.602909621, -0.489566724, -0.371443591, -0.249678177, -0.125457136, 0];

    private readonly double[] cosSum = [1.5, 1.494084079, 1.476406282, 1.447175494, 1.406736501, 1.355565013, 1.2942608, 1.223539047, 1.144220078, 1.057217571, 0.963525492,
        0.8642039, 0.760363887, 0.653151846, 0.543733332, 0.433276755, 0.322937149, 0.213840277, 0.107067297, 0.003640239, -0.095491503, -0.189463453, -0.277506928, -0.358958347,
        -0.433266831, -0.5, -0.55884787, -0.609624814, -0.652269558, -0.686843227, -0.713525492, -0.732608866, -0.744491286, -0.749667072, -0.748716441, -0.742293749, -0.731114647,
        -0.715942366, -0.697573368, -0.676822585, -0.654508497, -0.63143828, -0.608393283, -0.586115058, -0.565292172, -0.546548019, -0.530429821, -0.517399008, -0.507823121,
        -0.501969378, -0.5, -0.501969378, -0.507823121, -0.517399008, -0.530429821, -0.546548019, -0.565292172, -0.586115058, -0.608393283, -0.63143828, -0.654508497, -0.676822585,
        -0.697573368, -0.715942366, -0.731114647, -0.742293749, -0.748716441, -0.749667072, -0.744491286, -0.732608866, -0.713525492, -0.686843227, -0.652269558, -0.609624814,
        -0.55884787, -0.5, -0.433266831, -0.358958347, -0.277506928, -0.189463453, -0.095491503, 0.003640239, 0.107067297, 0.213840277, 0.322937149, 0.433276755, 0.543733332,
        0.653151846, 0.760363887, 0.8642039, 0.963525492, 1.057217571, 1.144220078, 1.223539047, 1.2942608, 1.355565013, 1.406736501, 1.447175494, 1.476406282, 1.494084079, 1.5];


    [TestMethod]
    public void Test_Derivative_BackwardOnePoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_ForwardOnePoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_CenteredThreePoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_CenteredFivePoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_CenteredSevenPoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_CenteredNinePoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_SGLinearThreePoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_SGLinearFivePoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_SGLinearSevenPoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_SGLinearNinePoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_SGCubicFivePoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_SGCubicSevenPoint()
    {

    }

    [TestMethod]
    public void Test_Derivative_SGCubicNinePoint()
    {

    }
}
