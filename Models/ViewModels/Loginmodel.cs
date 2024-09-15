using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace PractiseEfCoreWIthSP.Models.ViewModels
{
    public class Loginmodel
    {
        public string EmailAddress { get; set; }
        public string Password{ get; set; }
    }
}
