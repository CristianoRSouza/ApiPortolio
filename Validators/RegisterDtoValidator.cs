using FluentValidation;
using ApiEntregasMentoria.Data.Dto;

namespace ApiEntregasMentoria.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres")
                .MaximumLength(100).WithMessage("Senha deve ter no máximo 100 caracteres");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Senhas não conferem");

            RuleFor(x => x.Nickname)
                .NotEmpty().WithMessage("Nickname é obrigatório");
        }
    }
}
