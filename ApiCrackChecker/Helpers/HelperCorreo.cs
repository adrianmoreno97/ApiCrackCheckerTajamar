using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ApiCrackChecker.Helpers
{
    public class HelperCorreo
    {
        IConfiguration configuration;
        public HelperCorreo(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<String> EnviarEmail(String email, int id, String accion)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            String subject = "";
            String content = "";
            String textoplano = "";
            if (accion == "activar")
            {
                subject = "Activación de cuenta";
                textoplano = "Activación de cuenta";
                String url = "https://clientewebcrackchecker.azurewebsites.net/Manage/ActivarCuenta/" + id;
                content = "<body><h2>Crackchecker ES</h2><p>Para activar tu cuenta accede a: <a href=" + url + ">Activar Cuenta</a></p></body>";
            }
            else if (accion == "recuperar")
            {
                subject = "Restablecer contraseña";
                textoplano = "Restablecer contraseña";
                String url = "https://clientewebcrackchecker.azurewebsites.net/Manage/RestablecerPass?email=" + email;
                content = "<body><h2>Crackchecker ES</h2><p>Para restablecer tu contraseña accede a <a href=" + url + ">este enlace</a></p></body>";
            }
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no-reply@crackchecker.es", "CrackChecker Es"),
                Subject = subject,
                PlainTextContent = textoplano,
                HtmlContent = content
            };
            msg.AddTo(new EmailAddress(email));
            var response = await client.SendEmailAsync(msg);
            String msj = "Correo de activación enviado con éxito a " + email + ". Revise su correo";
            return msj;
        }
    }

}
