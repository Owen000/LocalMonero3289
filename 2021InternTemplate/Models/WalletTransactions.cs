using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021InternTemplate.Models
{
    public class WalletTransactions
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        
        public int FromWalletId { get; set;}

        public long Amount { get; set; }

        //Each transaction belongs to a wallet, so we need a WalletId to keep track of its owner. Entity Framework automatically populates the Wallet property
        public Wallet Wallet { get; set; }
        public int WalletId { get; set; }
    }
}
