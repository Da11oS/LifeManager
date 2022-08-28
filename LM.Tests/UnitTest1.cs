using Database;
using Microsoft.AspNetCore.Mvc;

namespace LM.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public void TestMethod1([FromServices ] DbContext ctx)
        {
        }
    }
}