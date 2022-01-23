using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class Task
    {
        [Key]
        public int id { get; set; }

        [StringLength(150, ErrorMessage = "Description max length is 150")]
        [Required(ErrorMessage = "Description is required")]
        public string description { get; set; }

        [Required(ErrorMessage = "Pending is required")]
        [DefaultValue(true)]
        public bool pending { get; set; }

        public DateTime? date { get; set; }
    }
}
