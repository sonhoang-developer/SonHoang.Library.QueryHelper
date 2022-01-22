using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SonHoang.Library.Helpers
{
    public static class MailHelper
    {
        public static async Task<bool> SendMail(string _from, string _to, string _subject, string _body, string host, string _gmailsend, string _gmailpassword, bool isBodyHtml = true)
        {
            MailMessage message = new(
                    from: _from,
                    to: _to,
                    subject: _subject,
                    body: _body
                );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = isBodyHtml;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);
            try
            {
                SmtpClient client = new(host);
                client.Port = 587;
                client.Credentials = new NetworkCredential(_gmailsend, _gmailpassword);
                client.EnableSsl = true;
                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new SmtpException(ex.Message, ex.InnerException);
            }
        }

    }
}
