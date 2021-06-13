using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Task
    {
        public int id { get; set; }
        [StringLength(150)]
        public string description { get; set; }
        public bool pending { get; set; }
    }
}
