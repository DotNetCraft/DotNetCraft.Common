using System;
using System.ComponentModel.DataAnnotations;
using DotNetCraft.Common.Core.Utils.ReflectionExtensions;
using DotNetCraft.Common.Utils.ReflectionExtensions;
using NUnit.Framework;

namespace Utils.Tests
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
            IReflectionManager reflectionManager = new ReflectionManager();
            var propertyDefinition = reflectionManager.SingleOrDefault(typeof(Sample), attributeType);
            if (shouldExist)
            {
                Assert.IsNotNull(propertyDefinition);
                Assert.AreEqual("SingleProperty", propertyDefinition.Name);
            }
            else
            {
                Assert.IsNull(propertyDefinition);
            }
        }

        [Test]
        [TestCase(typeof(Sample), null)]
        [TestCase(null, typeof(KeyAttribute))]
        [TestCase(null, null)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrDefaultNullParametersTest(Type objectType, Type attributeType)
        {
            IReflectionManager reflectionManager = new ReflectionManager();
            reflectionManager.SingleOrDefault(objectType, attributeType);
            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [TestCase(typeof(KeyAttribute), true)]
        [TestCase(typeof(CategoryAttribute), false)]
        public void SingleOrDefaultGenericTest(Type attributeType, bool shouldExist)
        {
            IReflectionManager reflectionManager = new ReflectionManager();
            var propertyDefinition = reflectionManager.SingleOrDefault<Sample>(attributeType);
            if (shouldExist)
            {
                Assert.IsNotNull(propertyDefinition);
                Assert.AreEqual("SingleProperty", propertyDefinition.Name);
            }
            else
            {
                Assert.IsNull(propertyDefinition);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrDefaultGenericNullParametersTest()
        {
            IReflectionManager reflectionManager = new ReflectionManager();
            reflectionManager.SingleOrDefault<Sample>(null);
            
            Assert.Fail("ArgumentNullException expected");
        }

        #endregion

        #region Single...

        [Test]
        public void SingleTest()
        {
            IReflectionManager reflectionManager = new ReflectionManager();
            var propertyInfo = reflectionManager.Single(typeof(Sample), typeof(KeyAttribute));            
            Assert.IsNotNull(propertyInfo);
        }

        [Test]
        public void SingleGenericTest()
        {
            IReflectionManager reflectionManager = new ReflectionManager();
            var propertyInfo = reflectionManager.Single<Sample>(typeof(KeyAttribute));
            Assert.IsNotNull(propertyInfo);
        }

        [Test]
        [ExpectedException(typeof(ReflectionManagerException))]
        public void SingleWithoutPropetyTest()
        {
            IReflectionManager reflectionManager = new ReflectionManager();
            reflectionManager.Single(typeof(Sample), typeof(CategoryAttribute));
            Assert.Fail("ReflectionManagerException expected");
        }

        [Test]
        [TestCase(typeof(Sample), null)]
        [TestCase(null, typeof(KeyAttribute))]
        [TestCase(null, null)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleTest(Type objectType, Type attributeType)
        {
            IReflectionManager reflectionManager = new ReflectionManager();
            reflectionManager.Single(objectType, attributeType);
            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [ExpectedException(typeof(ReflectionManagerException))]
        public void SingleWithDublicateTest()
        {
            IReflectionManager reflectionManager = new ReflectionManager();
            reflectionManager.Single(typeof(Sample), typeof(RequiredAttribute));
            Assert.Fail("ReflectionManagerException expected");
        }

        #endregion
    }
}
