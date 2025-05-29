using ECommerce.Application.DTOs;
using FluentValidation;

namespace ECommerce.Application.Validators;
public class OrderItemValidator : AbstractValidator<OrderItemDto>
{
    public OrderItemValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage("OrderItemDto nesnesi null olamaz.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Ürün ID'si boş olamaz.")
            .NotNull().WithMessage("Ürün ID'si null olamaz.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Ürün fiyatı boş olamaz.")
            .NotNull().WithMessage("Ürün fiyatı null olamaz.")
            .GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Ürün miktarı en az 1 olmalıdır.");
    }
}