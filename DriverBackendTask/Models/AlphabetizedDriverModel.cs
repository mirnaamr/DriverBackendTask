using System.ComponentModel.DataAnnotations;

namespace DriverBackendTask.Models
{
    /// <summary>
    ///  Model to represent the Alphabetized Driver Model response
    /// </summary>
    public class AlphabetizedDriverModel
    {
        public int Id { set; get; }
        public string AlphabetizedFullName { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
    }
}
