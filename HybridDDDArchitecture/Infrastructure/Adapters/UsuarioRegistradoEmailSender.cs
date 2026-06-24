using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Adapters
{
    internal class UsuarioRegistradoEmailSender : IUsuarioRegistradoEmailSender
    {
        public string SengridApiKey { get; }
        public UsuarioRegistradoEmailSender(IConfiguration configuration)
        {
                SengridApiKey = configuration.GetValue<string>("SendGrid:SecretKey") ?? throw new ArgumentNullException(paramName: "SecretKey", "SecretKey is not configured.");
        }
        public Task SendEmailAsync(string to, string subject, string body)
        {
            var Client= new SendGridClient(SengridApiKey);
            var from = new EmailAddress("noreply.museoantropologia@gmail.com");
            var Emailto= new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(from, Emailto, subject, "", body);

            return Client.SendEmailAsync(msg);

        }
    }
}
