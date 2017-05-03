using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.Utils.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Utils.Tests
{
    [TestFixture]
    public class LogManagerTests
    {
        private class Sample
        {
        }

        [TearDown]
        public void Clean()
        {
            LogManager.LoggerFactory = null;
        }

        [Test]
        public void GetLoggerByTypeTest()
        {
            ICommonLoggerFactory commonLoggerFactory = Substitute.For<ICommonLoggerFactory>();
            LogManager.LoggerFactory = commonLoggerFactory;

            ICommonLogger commonLogger = LogManager.GetLogger(typeof(Sample));
            Assert.IsNotNull(commonLogger);
            commonLoggerFactory.Received(1).Create(typeof(Sample));
        }

        [Test]
        public void GetLoggerByGenericTypeTest()
        {
            ICommonLoggerFactory commonLoggerFactory = Substitute.For<ICommonLoggerFactory>();
            LogManager.LoggerFactory = commonLoggerFactory;

            ICommonLogger commonLogger = LogManager.GetLogger<Sample>();
            Assert.IsNotNull(commonLogger);
            commonLoggerFactory.Received(1).Create<Sample>();
        }

        [Test]
        public void GetLoggerByCorrectTypeNameTypeTest()
        {
            ICommonLoggerFactory commonLoggerFactory = Substitute.For<ICommonLoggerFactory>();
            LogManager.LoggerFactory = commonLoggerFactory;

            string typeName = typeof(Sample).ToString();
            ICommonLogger commonLogger = LogManager.GetLogger(typeName);
            Assert.IsNotNull(commonLogger);
            commonLoggerFactory.Received(1).Create(typeName);
        }

        [Test]
        public void LoggerFactoryGetDefaultFactoryTest()
        {
            ICommonLoggerFactory current = LogManager.LoggerFactory;
            Assert.AreEqual(typeof(DebugLoggerFactory), current.GetType());
        }

        [Test]
        public void LoggerFactoryGetUserFactoryTest()
        {
            ICommonLoggerFactory commonLoggerFactory = Substitute.For<ICommonLoggerFactory>();
            LogManager.LoggerFactory = commonLoggerFactory;
            ICommonLoggerFactory current = LogManager.LoggerFactory;
            Assert.AreEqual(commonLoggerFactory.GetType(), current.GetType());
        }

        [Test]
        public void LoggerFactorySetNullTest()
        {
            LogManager.LoggerFactory = null;
            ICommonLoggerFactory current = LogManager.LoggerFactory;
            Assert.AreEqual(typeof(DebugLoggerFactory), current.GetType());
        }
    }
}
