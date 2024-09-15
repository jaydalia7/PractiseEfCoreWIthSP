using FluentValidation;
using PractiseEfCoreWIthSP.Models.ViewModels;
using System.Runtime.InteropServices.ObjectiveC;

namespace PractiseEfCoreWIthSP.Validations
{
    public class ProductValidation:AbstractValidator<AddProductModel>
    {
        public ProductValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product Name is Required")
                .Matches("^[A-Za-z0-9 ]+$").WithMessage("Invalid Product Name")
                .Length(2, 20).WithMessage("Product Name Length is Between 2 to 20 Charaters");

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("Product Price Should Not Be 0");

        }
    }
}
