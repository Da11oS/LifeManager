using LM.Data;
using Microsoft.AspNetCore.Mvc;

namespace LM.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private DbContext _ctx;

        [TestInitialize]
        public void Init()
        {
            _ctx = (DbContext)(TestsInit.CreateService().GetService(typeof(DbContext)));
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}