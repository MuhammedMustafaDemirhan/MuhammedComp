using MuhammedCo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuhammedCo.Core.DTO_s
{
    public class PaymentDto:BaseDto
    {
        public int CustomerId { get; set; }
        public double Amount { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
