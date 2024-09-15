using System.ComponentModel.DataAnnotations;

namespace PractiseEfCoreWIthSP.Models.Domains
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }

    }
}
