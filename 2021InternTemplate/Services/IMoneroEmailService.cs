using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace _2021InternTemplate.Services
{
    public interface IMoneroEmailService : IEmailSender
    {
        public void SendLoginCode(string email, string code);
        public void SendResetLink(string email, string passwordResetLink);
    }
}
