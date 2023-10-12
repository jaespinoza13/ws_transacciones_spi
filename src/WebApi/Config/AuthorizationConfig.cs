using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Config;

public static class AuthorizationConfig
{
    public static void AddAuthorizationConfigApi(this IServiceCollection services, IConfiguration configuration)
    {
        //AUTHORIZATION 
        var issuer = configuration.GetValue<string>("issuer");
        var keyTokenPub = configuration.GetValue<string>("Key_token_pub")!;
        var keyEncryptToken = configuration.GetValue<string>("Key_encrypt_token")!;

        var publicKeyBytes = Convert.FromBase64String(keyTokenPub);
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
        var keyRsa = new RsaSecurityKey(rsa);

        var securityKeyDecrypt = new SymmetricSecurityKey(Encoding.Default.GetBytes(keyEncryptToken));


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                IssuerSigningKey = keyRsa,
                TokenDecryptionKey = securityKeyDecrypt,
                ClockSkew = TimeSpan.Zero
            });
    }
}