using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Requests.Transfer
{
    public class UpdateTransferRequest:CreateTransferRequest
    {
        public int TransferId { get; set; } 
    }
}
