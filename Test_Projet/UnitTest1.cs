using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PSI_TD_2;
using System.IO;



namespace PSI_TD_2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Addition_Complexe()
        {
            Complexe comp = new Complexe(3, 2);
            Complexe comp1 = new Complexe(2, 3);

            Assert.AreEqual(new Complexe(5, 5), comp.SommeComplexe(comp1));
        }

        [TestMethod]
        public void Test_Multiplication_Complexe()
        {
            Complexe comp = new Complexe(3, 2);
            Complexe comp1 = new Complexe(2, 3);
            Assert.AreEqual(new Complexe(0, 13), comp.MultiplicationComplexe(comp1));
        }

        [TestMethod]
        public void TestIntToEndian()
        {
            string valuetest = Program.TestIntToEndian(1223452);
            string resultat = "28 171 18 ";
            Assert.AreEqual(valuetest, resultat);
        }

        [TestMethod]
        public void TestChoixVersion()
        {
            int valuetest = QRCode.ChoixVersion("HELLO WORLD");
            int valuetest2 = QRCode.ChoixVersion("HELLO THIS IS JADE AND THIBAULT");
            Assert.AreEqual(valuetest, 1);
            Assert.AreEqual(valuetest2, 2);

        }


        /*
        [TestMethod]

        public void Test_Fractale()
        {
            MyImage.Fractale(3500,3500).Save("testfractale.bmp");
        }

        
        [TestMethod]
        public void Test_QRCode()
        {
            QRCode QRtest = new QRCode(1);
            QRtest.Write_On_QR("HELLO WORLD");
            QRtest.Convert_QR_To_Image().Save("TestQR.bmp");
        }
        */

    }
}
