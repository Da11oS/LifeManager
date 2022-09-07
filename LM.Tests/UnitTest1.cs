using LM.Api.Admin;
using LM.Base.Models;
using LM.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LM.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private DbContext _ctx;
        private IUserStore<UserModel> _user;

        [TestInitialize]
        public void Init()
        {
            _ctx = (DbContext)(TestsInit.CreateService().GetService(typeof(DbContext)));
            _user = (IUserStore<UserModel>)(TestsInit.CreateService().GetService(typeof(IUserStore<UserModel>)));
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var newUser = new UserModel()
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