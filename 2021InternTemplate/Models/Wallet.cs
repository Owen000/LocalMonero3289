using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021InternTemplate.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public long Balance { get; set; }

        //Wallets have multiple transactions. We keep track of them in the transactions table, so we only need a List of transactions here that entity framework automatically populates
        public List<WalletTransactions> Transactions { get; set; }
        
    }
}
