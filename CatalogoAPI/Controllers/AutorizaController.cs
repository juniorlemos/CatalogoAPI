using CatalogoAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        public AutorizaController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager , IConfiguration iconfiguration)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = iconfiguration;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _signInManager.SignInAsync(user, false);
            return Ok(GeraToken(model));
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {


            var result = await _signInManager.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(GeraToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Invalido ....");
                return BadRequest(ModelState);
            }
          
        }
        private UsuarioToken GeraToken(UsuarioDTO userInfo)
        {
            var Claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.UniqueName,userInfo.Email),
                    new Claim("Anime","DragonBall"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256) ;

            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: Claims,
                expires: expiration,
                signingCredentials: credenciais);

               return new UsuarioToken()
               {
                   Authenticated = true,
                   Token = new JwtSecurityTokenHandler().WriteToken(token),
                   Expiration = expiration,
                   Message = "Token JWT OK"


               };
                
              
              
        }
    }
}
