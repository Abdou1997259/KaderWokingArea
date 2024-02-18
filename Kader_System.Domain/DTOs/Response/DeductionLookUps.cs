using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kader_System.Domain.DTOs.Response
{
    public class DeductionLookUps
    {
        public object[] employees { get; set; }
        public object[] deductions { get; set; }
        public object[] salary_effects { get; set; }
        public object[] trans_amount_types { get; set; }
    }
}
