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
    }
}
