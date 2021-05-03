using System;
using System.ComponentModel.DataAnnotations;
using DotNetCraft.Common.CommonClasses.Tests;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions;
using DotNetCraft.Common.Utils.Logging;
using DotNetCraft.Common.Utils.Reflection;
using NUnit.Framework;

namespace DotNetCraft.Common.Utils.Tests
{
    [TestFixture]
    public class PropertyManagerTests
    {
        public class Sample
        {
            [Key]
            public string SingleProperty { get; set; }

            [Required]
            public int FirstProperty { get; set; }

            [Required]
            public int SecondProperty { get; set; }
        }

        #region SingleOrDefault...

        [Test]
        [TestCase(typeof(KeyAttribute), true)]
        [TestCase(typeof(CategoryAttribute), false)]
        public void SingleOrDefaultTest(Type attributeType, bool shouldExist)
        {
            IReflectionManager reflectionManager = new ReflectionManager(new DebugLoggerFactory());
            var propertyDefinition = reflectionManager.GetPropertyInfosByAttribute(attributeType, typeof(Sample));
            if (shouldExist)
            {
                Assert.IsNotNull(propertyDefinition);
                Assert.AreEqual(1, propertyDefinition.Count);
                Assert.AreEqual("SingleProperty", propertyDefinition[0].Name);
            }
            else
            {
                Assert.AreEqual(0, propertyDefinition.Count);
            }
        }

        [Test]
        [TestCase(null, typeof(Sample))]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrDefaultNullParametersTest(Type objectType, Type attributeType)
        {
            IReflectionManager reflectionManager = new ReflectionManager(new DebugLoggerFactory());
            reflectionManager.GetPropertyInfosByAttribute(attributeType, objectType);
            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [TestCase(null, typeof(KeyAttribute))]
        [TestCase(null, null)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DefaultNullFirstParametersTest(Type objectType, Type attributeType)
        {
            IReflectionManager reflectionManager = new ReflectionManager(new DebugLoggerFactory());
            reflectionManager.GetPropertyInfosByAttribute(attributeType, objectType);
            Assert.Fail("ArgumentException expected");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrDefaultGenericNullParametersTest()
        {
            IReflectionManager reflectionManager = new ReflectionManager(new DebugLoggerFactory());
            reflectionManager.GetPropertyInfosByAttribute(null, typeof(Sample));
            
            Assert.Fail("ArgumentNullException expected");
        }

        #endregion
    }
}
