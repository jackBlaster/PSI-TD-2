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
        public void Test_Constructeur_Image()
        {
            MyImage image = new MyImage(30, 30);
            image.SetPixel(10, 10, new Pixel(9, 45, 63));
            image.Save("testuni.bmp");
        }

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

    }
}
