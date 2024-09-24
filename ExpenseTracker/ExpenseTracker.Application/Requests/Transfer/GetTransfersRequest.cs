using ExpenseTracker.Application.Requests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Requests.Transfer
{
    public class GetTransfersRequest:UserRequest
    {
        public string? Search {  get; set; }
    }
}
