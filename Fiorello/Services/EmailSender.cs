using System;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Fiorello.Services

{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsycn(MailMessage msg)
        {
            try
            {
                //From Address    
                string FromAddress = "abidzadeq2002@gmail.com";
                string FromAdressTitle = "Welcome To Fiorello!";
                //To Address    
                string ToAddress = msg.To.ToString();
                string ToAdressTitle = "User";
                var Subject = msg.Subject;
                var BodyContent = msg.Body;

                //Smtp Server    
                string SmtpServer = "smtp.gmail.com";
                //Smtp Port Number    
                int SmtpPortNumber = 587;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress
                                        (FromAdressTitle,
                                         FromAddress
                                         ));
                mimeMessage.To.Add(new MailboxAddress
                                         (ToAdressTitle,
                                         ToAddress
                                         ));
                mimeMessage.Subject = Subject; //Subject  
                mimeMessage.Body = new TextPart()
                {
                    Text = BodyContent
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
           
                    client.Connect(SmtpServer, SmtpPortNumber, false);
                    client.Authenticate(
                        "abidzadeq2002@gmail.com",
                        "qudret2002"
                        );
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            return Task.FromResult(0);
        }


    }
}
