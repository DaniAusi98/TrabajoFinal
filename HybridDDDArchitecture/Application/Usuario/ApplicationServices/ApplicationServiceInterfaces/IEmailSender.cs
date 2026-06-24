using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usuario.ApplicationServices.ApplicationServiceInterfaces
{
    public interface IUsuarioRegistradoEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);//usuario registrado handler
    }
}
