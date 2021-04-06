using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace _2021InternTemplate.Services
{
    public class MoneroEmailService : IMoneroEmailService
    {

        //Email:From
        //Email:Host
        //Email:Port
        //Email:Password
        //Email:Username

        private IConfiguration _configuration;
        private SmtpClient _client;
        public MoneroEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]));
            _client.Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]);
            _client.EnableSsl = true;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (MailMessage mailMessage = new MailMessage(_configuration["Email:From"],
                    email,
                    subject,
                    htmlMessage))
            {

                try
                {
                    _client.Send(mailMessage);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send email: {0}", ex.ToString());
                }
            }
            return null;
        }

        public void SendLoginCode(string email, string code)
        {
            SendEmailAsync(email, "Local Monero: Here's you Login Code", $"Enter the following code on the login page {code}");
        }
        public void SendResetLink(string email, string passwordResetLink){
            SendEmailAsync(email, "Local Monero: Here's you reset link", $"click on it to reset your password {passwordResetLink}");
        }
    }
}
