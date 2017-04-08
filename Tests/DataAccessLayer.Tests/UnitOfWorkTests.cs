using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetCraft.Common.Core;
using DotNetCraft.Common.Core.DataAccessLayer;
using DotNetCraft.Common.Core.DataAccessLayer.UnitOfWorks;
using DotNetCraft.Common.Core.Utils.Logging;
using DotNetCraft.Common.DataAccessLayer;
using DotNetCraft.Common.DataAccessLayer.UnitOfWorks.SimpleUnitOfWorks;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessLayer.Tests
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        [Test]
        public void ConstructorTest()
        {
            IDataContext dataContext = Substitute.For<IDataContext>();
            ICommonLogger logger = Substitute.For<ICommonLogger>();
            IUnitOfWork unitOfWork = new UnitOfWork(dataContext, logger);
            Assert.IsNotNull(unitOfWork);
        }

        [Test]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullParameterTest(bool correctContext, bool correctLogger)
        {
            IDataContext dataContext = correctContext ? Substitute.For<IDataContext>() : null;
            ICommonLogger logger = correctLogger ? Substitute.For<ICommonLogger>(): null;
            new UnitOfWork(dataContext, logger);
            
            Assert.Fail("ArgumentNullException expected");
        }
    }
}
