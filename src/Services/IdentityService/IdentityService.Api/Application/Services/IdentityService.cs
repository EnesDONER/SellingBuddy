using IdentityService.Api.Application.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Api.Application.Services
{
    public class IdentityService : IIdentityService
    {

        public Task<LoginResponseModel> LoginAsync(LoginRequestModel loginRequest)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, loginRequest.UserName),
                new Claim(ClaimTypes.Name, "EnesDoner")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SellingBudySecretKeyShouldBeLong"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: creds, notBefore: DateTime.Now);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            LoginResponseModel loginResponseModel = new()
            {
                UserName = loginRequest.UserName,
                UserToken = encodedJwt
            };

            return Task.FromResult(loginResponseModel);
        }
    }
}
