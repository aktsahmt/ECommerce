using ECommerce.Application.DTOs;
using FluentValidation;
namespace ECommerce.Application.Validators;

public class OrderCancelDtoValidator : AbstractValidator<CancelDto>
{
    public OrderCancelDtoValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage("Body nesnesi null olamaz.");

        RuleFor(x => x.OrderId)
            .Must(BeAValidGuid).WithMessage("Kullanıcı ID'si geçerli bir GUID olmalıdır.")
            .NotEmpty().WithMessage("Kullanıcı ID'si boş olamaz.")
            .NotNull().WithMessage("Kullanıcı ID'si null olamaz.");
    }

    private bool BeAValidGuid(Guid orderId)
    {
        return Guid.TryParse(orderId.ToString(), out _);
    }
}
