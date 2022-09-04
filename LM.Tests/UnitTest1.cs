using LM.Api.Admin;
using LM.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LM.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private DbContext _ctx;
        private IUserService _user;

        [TestInitialize]
        public void Init()
        {
            _ctx = (DbContext)(TestsInit.CreateService().GetService(typeof(DbContext)));
            _user = (IUserService)(TestsInit.CreateService().GetService(typeof(IUserService)));
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var newUser = new UserView()
            {
                Id = Guid.NewGuid(),
                Email = "test_user1@mail.ru ",
                PasswordHash = "test_pass1_",
                UserName = "test_admin1"
            };
            await _user.CreateAsync(newUser, default);
        }
    }
}