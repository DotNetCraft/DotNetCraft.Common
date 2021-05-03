using System;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Utils.Logging;
using NUnit.Framework;

namespace DotNetCraft.Common.Utils.Tests
{
    [TestFixture]
    public class DebugLoggerFactoryTests
    {
        [Test]
        public void GetLoggerByTypeTest()
        {
            DebugLoggerFactory commonLoggerFactory = new DebugLoggerFactory();
            ICommonLogger commonLogger = commonLoggerFactory.Create(typeof(Type));
            Assert.IsNotNull(commonLogger);
            Assert.AreEqual(typeof(DebugLogger), commonLogger.GetType());
        }

        [Test]
        public void GetLoggerByGenericTypeTest()
        {
            DebugLoggerFactory commonLoggerFactory = new DebugLoggerFactory();
            ICommonLogger commonLogger = commonLoggerFactory.Create<Type>();
            Assert.IsNotNull(commonLogger);
            Assert.AreEqual(typeof(DebugLogger), commonLogger.GetType());
        }

        [Test]
        public void GetLoggerByCorrectTypeNameTypeTest()
        {
            string typeName = typeof(Type).ToString();

            DebugLoggerFactory commonLoggerFactory = new DebugLoggerFactory();
            ICommonLogger commonLogger = commonLoggerFactory.Create(typeName);
            Assert.IsNotNull(commonLogger);
            Assert.AreEqual(typeof(DebugLogger), commonLogger.GetType());
        }
    }
}
