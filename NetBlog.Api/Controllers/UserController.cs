﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetBlog.Api.Config;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        public UserController(IUserService userService, UserManager<ApplicationUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userService = userService;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) { return BadRequest("Invalid Payload"); }
            var existingUser = await _userManager.FindByNameAsync(vm.UserName);
            if (existingUser == null){ return BadRequest("Invalid Username"); }
            var checkPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);
            if (!checkPassword) { return BadRequest("Invalid Username or Password"); }
            if (!existingUser.Status){ return BadRequest("This Username is Currently Inactive"); }
            var jwtToken = GenerateToken(existingUser);
            return Ok(new AuthResult
            {
                Token = jwtToken,
                Success = true
            });
        }

        private string GenerateToken(ApplicationUser newUser)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret!);
            var jwtDescreptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", newUser.Id),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };
            var token = jwtHandler.CreateToken(jwtDescreptor);
            var jwtToken = jwtHandler.WriteToken(token);
            return jwtToken;
        }
    }
}