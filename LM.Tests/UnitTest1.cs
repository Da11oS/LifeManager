using DataModel;
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
        private LifeManagerDb _ctx;
        private IUserStore<UserModel> _user;
        private IJwtService _jwtService;

        [TestInitialize]
        public void Init()
        {
            _ctx = (LifeManagerDb)(TestsInit.CreateService().GetService(typeof(LifeManagerDb)));
            _user = (IUserStore<UserModel>)(TestsInit.CreateService().GetService(typeof(IUserStore<UserModel>)));
            _jwtService = (IJwtService)(TestsInit.CreateService().GetService(typeof(IJwtService)));
        }

        [TestMethod]
        public async Task TestCreateUser()
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
        
        [TestMethod]
        public void TestCreateJwtToken()
        {
            var newUser = new UserModel()
            {
                Id = new Guid("53f44057-dacc-478b-aa04-f68e644b8875"),
                Email = "jwttest@mail.ru",
                PasswordHash = "$2a$11$HnhuZimK0Do8QYTIZxUyT.oZf2UVSg9YCOaxwWrImoA97KI5wGpaW",
                UserName = "jwt_user"
            };
            // _jwtService.CreateToken(newUser);
        }
    }
}