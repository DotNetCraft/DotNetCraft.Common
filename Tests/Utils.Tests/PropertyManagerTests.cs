using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DotNetCraft.Common.Core.Utils;
using DotNetCraft.Common.Utils;
using NSubstitute;
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

        [TearDown]
        public void Clean()
        {
            PropertyManager.Manager = null;
        }

        #region SingleOrDefault...

        [Test]
        [TestCase(typeof(KeyAttribute), true)]
        [TestCase(typeof(CategoryAttribute), false)]
        public void SingleOrDefaultTest(Type attributeType, bool shouldExist)
        {
            IPropertyManager propertyManager = PropertyManager.Manager;
            var property = propertyManager.SingleOrDefault(typeof(Sample), attributeType);
            if (shouldExist)
            {
                Assert.IsNotNull(property);
                Assert.AreEqual("SingleProperty", property.Name);
            }
            else
            {
                Assert.IsNull(property);
            }
        }

        [Test]
        [TestCase(typeof(Sample), null)]
        [TestCase(null, typeof(KeyAttribute))]
        [TestCase(null, null)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrDefaultNullParametersTest(Type objectType, Type attributeType)
        {
            IPropertyManager propertyManager = PropertyManager.Manager;
            propertyManager.SingleOrDefault(objectType, attributeType);
            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [TestCase(typeof(KeyAttribute), true)]
        [TestCase(typeof(CategoryAttribute), false)]
        public void SingleOrDefaultGenericTest(Type attributeType, bool shouldExist)
        {
            IPropertyManager propertyManager = PropertyManager.Manager;
            var property = propertyManager.SingleOrDefault<Sample>(attributeType);
            if (shouldExist)
            {
                Assert.IsNotNull(property);
                Assert.AreEqual("SingleProperty", property.Name);
            }
            else
            {
                Assert.IsNull(property);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrDefaultGenericNullParametersTest()
        {
            IPropertyManager propertyManager = PropertyManager.Manager;
            propertyManager.SingleOrDefault<Sample>(typeof(KeyAttribute));
            
            Assert.Fail("ArgumentNullException expected");
        }

        #endregion

        #region Single...

        [Test]
        public void SingleTest()
        {
            IPropertyManager propertyManager = PropertyManager.Manager;
            PropertyInfo propertyInfo = propertyManager.Single(typeof(Sample), typeof(KeyAttribute));            
            Assert.IsNotNull(propertyInfo);
        }

        [Test]
        public void SingleGenericTest()
        {
            IPropertyManager propertyManager = PropertyManager.Manager;
            PropertyInfo propertyInfo = propertyManager.Single<Sample>(typeof(KeyAttribute));
            Assert.IsNotNull(propertyInfo);
        }

        [Test]
        [ExpectedException(typeof(PropertyManagerException))]
        public void SingleWithoutPropetyTest()
        {
            IPropertyManager propertyManager = PropertyManager.Manager;
            propertyManager.Single(typeof(Sample), typeof(CategoryAttribute));
            Assert.Fail("PropertyManagerException expected");
        }

        [Test]
        [TestCase(typeof(Sample), null)]
        [TestCase(null, typeof(KeyAttribute))]
        [TestCase(null, null)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleTest(Type objectType, Type attributeType)
        {
            IPropertyManager propertyManager = PropertyManager.Manager;
            propertyManager.Single(objectType, attributeType);
            Assert.Fail("ArgumentNullException expected");
        }

        [Test]
        [ExpectedException(typeof(PropertyManagerException))]
        public void SingleWithDublicateTest()
        {
            IPropertyManager propertyManager = PropertyManager.Manager;
            propertyManager.Single(typeof(Sample), typeof(RequiredAttribute));
            Assert.Fail("PropertyManagerException expected");
        }

        #endregion

        #region Manager...

        [Test]
        public void PropertyManagerGetDefaultManagerTest()
        {
            IPropertyManager current = PropertyManager.Manager;
            Assert.AreEqual(typeof(PropertyManager), current.GetType());
        }

        [Test]
        public void PropertyManagerGetUserManagerTest()
        {
            IPropertyManager propertyManager = Substitute.For<IPropertyManager>();
            PropertyManager.Manager = propertyManager;
            IPropertyManager current = PropertyManager.Manager;
            Assert.AreEqual(propertyManager.GetType(), current.GetType());
        }

        [Test]
        public void PropertyManagerSetNullTest()
        {
            PropertyManager.Manager = null;
            IPropertyManager current = PropertyManager.Manager;
            Assert.AreEqual(typeof(PropertyManager), current.GetType());
        }

        #endregion
    }
}
