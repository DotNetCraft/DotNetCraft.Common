using System.Collections.Generic;
using DotNetCraft.Common.Core.Utils.Mapping;
using DotNetCraft.Common.Utils.Mapping;
using NUnit.Framework;

namespace DotNetCraft.Common.Utils.Tests
{
    [TestFixture]
    public class MappingTests
    {
        class SimpleTestMapper: BaseMapperInstance<string, int>
        {
            #region Overrides of BaseMapperInstance<string,int>

            protected override int OnMap(string source)
            {
                return int.Parse(source);
            }

            #endregion
        }
        [Test]
        public void ListMappingTest()
        {
            IMapperManager mapperManager = new MapperManager();
            SimpleTestMapper mapperInstance = new SimpleTestMapper();
            mapperManager.RegisterMapping(mapperInstance);

            List<string> strList = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                strList.Add(i.ToString());
            }

            List<int> intList = mapperManager.Map<List<string>, List<int>>(strList);
            Assert.AreEqual(strList.Count, intList.Count);
        }
    }
}
