using System;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks;
using NUnit.Framework;

namespace DataAccessLayer.Tests.UnitOfWorks
{
    [TestFixture]
    public class DataBaseParameterTests
    {
        [Test]
        [TestCase(null, null, false)]
        [TestCase("", null, false)]
        [TestCase("           ", null, false)]
        [TestCase("Test", null, true)]
        [TestCase("Test", "fasdfds", true)]
        [TestCase("Test", 5.0f, true)]
        [TestCase("Test", 5, true)]
        public void ConstructorTest(string parameterName, object parameterValue, bool isCorrect)
        {
            try
            {
                IDataBaseParameter dataBaseParameter = new DataBaseParameter(parameterName, parameterValue);
                if (isCorrect == false)
                    Assert.Fail("Incorrect behavior of the constructor.");

                Assert.AreEqual(parameterName, dataBaseParameter.ParameterName);
                Assert.AreEqual(parameterValue, dataBaseParameter.ParameterValue);
            }
            catch (Exception)
            {
                if (isCorrect)
                    Assert.Fail("Incorrect behavior of the constructor.");
            }            
        }
    }
}
