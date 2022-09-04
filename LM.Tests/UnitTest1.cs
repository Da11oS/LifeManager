using LM.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LM.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private DbContext _ctx;
        private IUserStore<IdentityUser<Guid>> _user;

        [TestInitialize]
        public void Init()
        {
            _ctx = (DbContext)(TestsInit.CreateService().GetService(typeof(DbContext)));
            _user = (IUserStore<IdentityUser<Guid>>)(TestsInit.CreateService().GetService(typeof(IUserStore<IdentityUser<Guid>>)));
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var newUser = new IdentityUser<Guid>()
            {
                Id = Guid.NewGuid(),
                Email = "test_user@mail.ru",
                PasswordHash = "test_pass",
                UserName = "test_admin"
            };
            await _user.CreateAsync(newUser, default);
        }
    }
}