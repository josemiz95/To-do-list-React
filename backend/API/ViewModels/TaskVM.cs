namespace API.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class TaskVM
    {
        public int id { get; set; }

        [StringLength(150, ErrorMessage = "Description max length is 150")]
        [Required(ErrorMessage = "Description is required")]
        public string description { get; set; }

        [Required(ErrorMessage = "Pending is required")]
        public bool pending { get; set; }
    }
}
