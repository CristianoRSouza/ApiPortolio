using FluentValidation;
using ApiEntregasMentoria.Controllers;

namespace ApiEntregasMentoria.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória");
        }
    }
}
