using api.Controllers;
using api.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestNunit.TaskControllerTest
{
    class ValidateTask
    {
        public static bool Validate(Task task, ref TasksController controller)
        {
            // This function is because the unit test always pass ModelState.IsValid

            if (task == null)
            {
                controller.ModelState.AddModelError("Error", "Error task not valid");
                return false;
            }

            // Function to test Model.IsValid
            var context = new ValidationContext(task, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                                task, context, results,
                                validateAllProperties: true
                            );
            if (!isValid)
            {
                controller.ModelState.AddModelError("Error", "Error task not valid");
            }

            return isValid;
        }
    }
}
