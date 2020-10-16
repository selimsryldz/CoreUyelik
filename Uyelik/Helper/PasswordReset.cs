using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Uyelik.Helper
{
    public class PasswordReset
    {
        public static void PasswordResetSendEmail(string link, string email)
        {
            MailMessage mail = new MailMessage();

            SmtpClient smtpClient = new SmtpClient();
            
            mail.From = new MailAddress("mehmetyilmazofficial11@gmail.com");
            mail.To.Add(email);

            mail.Subject = $"dafs@hotmail.com::Şifre sıfırlama";
            mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            mail.Body += $"<a href='{link}'>Şifre yenileme linki</a>";
            mail.IsBodyHtml = true;
           
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential("mehmetyilmazofficial11@gmail.com", "13579630g");
            smtpClient.EnableSsl = true;
            smtpClient.Send(mail);


        }
        

        



    }
}
