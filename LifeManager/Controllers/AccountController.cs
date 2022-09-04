﻿using LM.Api.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = LM.Api.Admin.IAuthorizationService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        
        public AccountController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        
        // GET: api/<AccountController>
        [HttpPost]
        public Task<RegisterResult> RegisterAsync(string mail, string name, string password, CancellationToken cancellationToken = default)
        {
            return _authorizationService.RegisterAsync(mail, name, password, cancellationToken);
        }
        
        [HttpPost]
        public Task<LogInResult> LoginAsync(string mail, string password, CancellationToken cancellationToken = default)
        {
            return _authorizationService.LogInAsync(mail, password, cancellationToken);
        }
        
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}