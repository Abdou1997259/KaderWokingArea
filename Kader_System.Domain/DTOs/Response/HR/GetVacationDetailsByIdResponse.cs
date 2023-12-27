using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kader_System.Domain.DTOs.Response.HR
{
    public class GetVacationDetailsByIdResponse
    {
        public int ApplyAfterMonth { get; set; }
        public int Balance { get; set; }
        public bool CanTransfer { get; set; }
        public  string NameAr { get; set; }
        public string NameEn { get; set; }
        public string VacationType { get; set; }
    }
}
