using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021InternTemplate.Models
{
    public class MoneroUser : IdentityUser
    {        
        public string Code { get; set; }


        //Users have one wallet. We keep track of which wallet belongs to the user in this WalletId parameter. Entity Framework automatically gives us access to the Wallet object
        public Wallet Wallet { get; set; }
        public int WalletId { get; set; }        

    }
}
