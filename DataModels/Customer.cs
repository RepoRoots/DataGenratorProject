using System.ComponentModel.DataAnnotations;

namespace DbGenratorWithBogus.DbModels
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public string FirstName { get; set; } = string.Empty;   

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string StateProvince { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
    }
}
