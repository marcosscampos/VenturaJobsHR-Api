using FluentValidation;
using VenturaJobsHR.Users.Domain.Abstractions.Validations;
using VenturaJobsHR.Users.Domain.Models;

namespace VenturaJobsHR.Users.Validation;

public class UserValidation : AbstractValidator<User>, IUserValidation
{
    public UserValidation()
    {
        ValidateName();
        ValidatePhone();
        ValidateEmail();
        ValidateAddress();
        ValidateFirebaseId();
    }

    private void ValidateFirebaseId()
    {
        RuleFor(x => x.FirebaseId).NotEmpty().WithMessage("O preenchimento do FirebaseId(UID) é obrigatório.");
    }

    private void ValidateAddress()
    {
        RuleFor(x => x.Address).NotNull().WithMessage("Objeto endereço inválido.");
        RuleFor(x => x.Address.City).NotEmpty().WithMessage("Campo Cidade é obrigatório");
        RuleFor(x => x.Address.State).NotEmpty().WithMessage("Campo Estado é obrigatório");
        RuleFor(x => x.Address.PostalCode).NotEmpty().WithMessage("Campo CEP é obrigatório");
        RuleFor(x => x.Address.Complement).NotEmpty().WithMessage("Campo Complemento é obrigatório");
        RuleFor(x => x.Address.CompleteAddress).NotEmpty().WithMessage("Campo Endereço é obrigatório");
    }

    private void ValidateEmail()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email inválido.")
            .NotEmpty().WithMessage("Campo Email obrigatório.");
    }

    private void ValidatePhone()
    {
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Campo Telefone é obrigatório.");
    }

    private void ValidateName()
    {
        RuleFor(x => x.Name)
            .Length(3, 200).WithMessage("Campo Nome não pode ser nulo e deve ter entre 3 a 200 caracteres.");
    }
}
