using FluentValidation;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Validators
{
    public class RegisterUserDtoValidator: AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(ICarRentDbContext carRentDbContext) 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password).WithMessage("Password incorrect");

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = carRentDbContext.Customers.Any(c => c.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
        }

    }
}
