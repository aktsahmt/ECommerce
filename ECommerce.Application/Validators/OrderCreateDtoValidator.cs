using ECommerce.Application.DTOs;
using FluentValidation;
namespace ECommerce.Application.Validators;

public class OrderCreateDtoValidator : AbstractValidator<CreateDto>
{
    public OrderCreateDtoValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage("Body nesnesi null olamaz.");

        RuleFor(x => x.UserId)
            .Must(BeAValidGuid).WithMessage("Kullanıcı ID'si geçerli bir GUID olmalıdır.")
            .NotEmpty().WithMessage("Kullanıcı ID'si boş olamaz.")
            .NotNull().WithMessage("Kullanıcı ID'si null olamaz.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Ürün listesi boş olamaz.");

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemValidator());
    }
    private bool BeAValidGuid(Guid orderId)
    {
        return orderId != Guid.Empty; // Ensure the GUID is not empty
    }
}
