using FluentValidation;
using PractiseEfCoreWIthSP.Models.ViewModels;

namespace PractiseEfCoreWIthSP.Validations
{
    public class UserValidation:AbstractValidator<UserAddModel>
    {
        public UserValidation()
        {
            RuleFor(u => u.Name)
             .NotEmpty().WithMessage("{PropertyName} should not be empty")
             .Matches("^[a-zA-Z ]+$").WithMessage("{PropertyName} Has Alphabet Only")
             .Length(2, 20).WithMessage("{PropertyName} Length is between 2 to 20 characters");

            RuleFor(u => u.EmailAddress)
                .NotEmpty().WithMessage("EmailAddress should not be empty")
               .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("{PropertyName} is not valid");

            RuleFor(u => u.Password)
                 .NotEmpty().WithMessage("{PropertyName} should not be empty")
                 .MinimumLength(3).WithMessage("{PropertyName} Length is minium 3 characters");
        }
    }
}
