using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Projeto.Utilitario
{
    public class Email
    {
        public void EnviarEmail(string email, string assunto, string corpo, List<Attachment> anexos)
        {
            try
            {
                MailMessage message = new MailMessage("cotiexemplo@gmail.com", email);

                message.Subject = assunto;
                message.Body    = corpo;

                if (anexos != null)
                {
                    foreach (Attachment anexo in anexos)
                    {
                        message.Attachments.Add(anexo);
                    }
                }                

                SmtpClient smtp  = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl   = true;
                smtp.Credentials = new NetworkCredential("cotiexemplo@gmail.com", "@coticoti@");
                smtp.Send(message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
