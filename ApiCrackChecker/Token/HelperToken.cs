using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCrackChecker.Token
{
    public class HelperToken
    {
        public String Issuer { get; set; }
        public String Audience { get; set; }
        public String SecretKey { get; set; }
        public HelperToken(IConfiguration configuration)
        {
            this.Issuer = configuration["ApiAuth:Issuer"];
            this.Audience = configuration["ApiAuth:Audience"];
            this.SecretKey = configuration["ApiAuth:SecretKey"];
        }

        // Creamos un metodo privado para generar una clave simétrica a partir de nuestro SecretKey
        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }

        // Método para configurar las opciones de seguridad del token
        // Los métodos de configuración son Action
        public Action<JwtBearerOptions> GetJwtOptions()
        {
            Action<JwtBearerOptions> jwtoptions = new Action<JwtBearerOptions>(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = this.Issuer,
                    ValidAudience = this.Audience,
                    IssuerSigningKey = this.GetKeyToken()
                };
            });
            return jwtoptions;
        }

        public Action<AuthenticationOptions> GetAuthOptions()
        {
            Action<AuthenticationOptions> authoptions = new Action<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            return authoptions;
        }
    }
}
