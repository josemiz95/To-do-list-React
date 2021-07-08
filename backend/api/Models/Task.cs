using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Models
{
    public class Task
    {
        [Key]
        public int id { get; set; }
        [StringLength(150, ErrorMessage = "Description max length is 150")]
        [Required(ErrorMessage = "Description is required")]
        public string description { get; set; }
        [Required(ErrorMessage = "Pending is required")]
        public bool pending { get; set; }
        [JsonIgnore]
        public DateTime? date { get; set; }
    }
}
