using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JsonWebToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly IOptions<JwtConfig> _options;

        public TokenController(IOptions<JwtConfig> options)
        {
            _options = options;
        }

        [Route("oauth")]
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetToken(TokenViewModel model)
        {
            if ("admin" == model.Code && "123qwe" == model.Password)
            {
                Claim[] claims = new Claim[] { 
                    new Claim(ClaimTypes.NameIdentifier, "admin"), new Claim(ClaimTypes.Name, "admin"), new Claim("Password", "123qwe") 
                };

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _options.Value.Issuer,
                    audience: _options.Value.Audience,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(_options.Value.ExpiresMinutes),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.IssuerSigningKey)), SecurityAlgorithms.HmacSha256));

                string tokenContent = new JwtSecurityTokenHandler().WriteToken(token);

                return new JsonResult(new { Code = 0, Token = tokenContent });
            }
            else
            {
                return new JsonResult(new { Code = 1 });
            }
        }

        [Route("getUser")]
        [HttpGet]
        public JsonResult GetUser()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            string userCode = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return new JsonResult(new { Code = userCode });
        }
    }
}