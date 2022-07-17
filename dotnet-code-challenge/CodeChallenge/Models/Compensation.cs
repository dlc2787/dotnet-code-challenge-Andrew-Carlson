using Microsoft.EntityFrameworkCore;
using System;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        //key this object to help find it
        public String CompensationId { get; set; }
        //public Employee employee { get; set; }
        public String employee { get; set; }
        public int salary { get; set; }
        public DateTime effectiveDate { get; set; }

    }
}
