using NUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace InterpolationTests
{
    public class Tests
    {
        private (double[] xs, double[] ys) RandomWalks(int count)
        {
            Random rand = new(0);
            double[] xs = Enumerable.Range(0, count).Select(x => (rand.NextDouble() - .5) * 100).ToArray();
            double[] ys = Enumerable.Range(0, count).Select(x => (rand.NextDouble() - .5) * 100).ToArray();
            return (xs, ys);
        }

        [TestCase(5)]
        [TestCase(7)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        [TestCase(21)]
        [TestCase(22)]
        [TestCase(23)]
        [TestCase(24)]
        [TestCase(100)]
        public void Test_InputLengths(int count)
        {
            (double[] xs, double[] ys) = RandomWalks(count);
            (double[] interpolatedXs, double[] interpolatedYs) = Interpolation.Cubic.InterpolateXY(xs, ys, 5);
            Assert.IsNotNull(interpolatedXs);
            Assert.IsNotNull(interpolatedYs);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(23)]
        public void Test_OutputMultiples(int count)
        {
            (double[] xs, double[] ys) = RandomWalks(23);
            (double[] interpolatedXs, double[] interpolatedYs) = Interpolation.Cubic.InterpolateXY(xs, ys, count);
            Assert.IsNotNull(interpolatedXs);
            Assert.IsNotNull(interpolatedYs);
        }

        private string GetCodeToInstantiateArray(double[] values, string name, int columns = 5)
        {
            StringBuilder sb = new();
            sb.AppendLine($"{name} = new double[]");
            sb.AppendLine("{");
            for (int i = 0; i < values.Length; i++)
            {
                if (i % columns == 0 && i > 0)
                    sb.Append("\n");
                sb.Append($"{values[i]}, ");
            }
            sb.AppendLine("\n};");
            return sb.ToString();
        }

        [Test]
        public void Test_KnownValues_Match()
        {
            (double[] xs, double[] ys) = RandomWalks(20);
            (double[] interpolatedXs, double[] interpolatedYs) = Interpolation.Cubic.InterpolateXY(xs, ys, 5);
            Assert.IsNotNull(interpolatedXs);
            Assert.IsNotNull(interpolatedYs);

            Console.WriteLine(GetCodeToInstantiateArray(interpolatedXs, "xs"));
            Console.WriteLine(GetCodeToInstantiateArray(interpolatedYs, "ys"));

            double[] expectedXs = {
                22.624326996795986,32.019630850401846,29.343715252454555,23.3007244045564,19.23010618329573,
                16.787322302362085,15.491919532672515,14.863444645144094,14.421444410693885,13.685465600238945,
                12.17505498469635,9.409759334983153,4.910265216198772,-1.434405548179225,-8.84023420484501,
                -16.389881649676518,-23.166008778551667,-28.2512764873484,-30.72834567194463,-29.67987722821829,
                -24.450794309231465,-15.743692305698374,-4.69917885246317,7.541817983498482,19.77343136397489,
                30.614377765026624,38.654047865499805,42.48183234424059,40.68712188009511,32.59876285101555,
                20.653006654295233,8.102223438279335,-1.80121319409447,-5.8049296398885035,-1.542040880700494,
                9.021920230138413,22.59848910287441,35.89887361638896,45.634281649563526,48.710596479947256,
                44.835258808453716,35.81677953206771,23.514601591690816,9.788167928224674,-3.5030785174291106,
                -14.499694804368929,-21.34223799169315,-22.318153857409143,-19.09173715705384,-14.307239515801566,
                -7.949673796438698,0.10595141596835139,8.514837406410711,14.003316025273142,14.618012888242848,
                11.576735967186709,6.515810606638248,1.0715621511309972,-3.1196840548015268,-4.421602666625791,
                -1.2302076142862974,6.612408590971121,17.26842142215534,28.75414741590025,39.085903108839716,
                46.28000503760763,48.394942905840736,44.86636883108056,36.780425498238465,25.32065670461398,
                11.670606247506608,-2.9861820757841944,-17.46616446795889,-30.585797131718014,-41.16153626976204,
                -48.01243676205682,-50.47027550695317,-48.992567285438646,-44.186363358925746,-36.65871498882698,
                -27.01667343655487,-15.867289963521916,-3.8176158311406443,8.525297699176456,20.554399366016842,
                31.662637907968016,41.22499143558036,48.28050300707636,51.57036445601545,49.82544833872013,
                42.352365379232864,31.212980225761946,19.29241072263329,9.132063694497665,1.1656973254780523,
                -4.971095059719222,-9.644228257858618,-13.219617065704604,-16.063176280021644,-18.540820697574222
            };

            double[] expectedYs = {
                31.690790868220287,35.3396393662554,45.56326402539062,52.24932430599314,50.57549118581606,
                42.241349529237255,29.1024355923628,13.014285631298621,-4.167564097849176,-20.58757733897459,
                -34.3902178359716,-43.71994933273423,-46.72449292402444,-42.604213411856236,-33.1172347721597,
                -20.402689383246173,-6.599709623426989,6.152572128986532,15.715023495683056,19.94851209835124,
                17.50325552747191,11.115757177389076,4.84083804920694,2.731312273387354,7.2025505465044715,
                16.246758562590227,27.116686476585,37.06508444342917,43.34470261806313,43.94288264879679,
                39.93393041509217,33.20290204531299,25.634857099690926,19.114855138457624,15.111651787824103,
                13.057158116857538,11.761321573713525,10.033935620829455,6.684793720642718,0.6145126524871783,
                -7.969260026879537,-17.878649338305973,-27.902018609090796,-36.82773116653265,-43.44415033793019,
                -46.53963945058208,-44.90256183178695,-37.42574593121469,-25.47925440895904,-13.245178778278984,
                -5.361344328389992,-6.333796068058976,-14.660680059796185,-20.943530917210378,-18.83525970721601,
                -9.830459696987282,3.541368400709855,18.75072387310943,33.26810600744551,44.56401409095213,
                50.148521248883256,49.3012789431795,43.74413243255064,35.37741576418509,26.101462985271255,
                17.81660814299754,12.386783441853837,10.487220945364385,11.36852890341605,14.197243625754925,
                18.139901422127085,22.36303860227862,26.03319147595561,28.316896352904152,28.380689542870318,
                25.393615284901884,19.019535158656605,10.00873692666765,-0.7441771059242743,-12.344604633978442,
                -23.897943352354126,-34.50959095591061,-43.28494513950717,-49.32940359800308,-51.74836402625762,
                -49.647224119130065,-42.25561890415289,-31.125621710204086,-19.868457143966268,-12.166690639853831,
                -10.66684478746432,-13.057353111204014,-15.5452021320817,-14.80352127467525,-10.364244728369988,
                -2.841933767220736,7.1468047258037295,18.9853638677346,32.0571367756031,45.745516566440564
            };

            Assert.AreEqual(expectedXs.Length, interpolatedXs.Length);
            Assert.AreEqual(expectedYs.Length, interpolatedYs.Length);

            for (int i = 0; i < expectedXs.Length; i++)
            {
                Assert.AreEqual(expectedXs[i], interpolatedXs[i]);
                Assert.AreEqual(expectedYs[i], interpolatedYs[i]);
            }
        }
    }
}