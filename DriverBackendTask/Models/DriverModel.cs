using System.ComponentModel.DataAnnotations;

namespace DriverBackendTask.Models
{
    /// <summary>
    ///  Model to represent the Driver model
    /// </summary>
    public class DriverModel
    {
        public int Id { set; get; }
        [Required(ErrorMessage ="First Name is required.")]
        [StringLength(20, ErrorMessage ="First Name lenghth can't be more than 20 characters")]
        public string FirstName { set; get; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(20, ErrorMessage = "Last Name lenghth can't be more than 20 characters")]
        public string LastName { set; get; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage ="Email is invalid")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Phone Number is Invalid")]
        public string PhoneNumber { set; get; }
    }
}
