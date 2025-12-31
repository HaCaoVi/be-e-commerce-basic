using System.ComponentModel.DataAnnotations;
using e_commerce_basic.Dtos.Product;
using e_commerce_basic.Types;

namespace e_commerce_basic.Common.Validators
{
    public class DiscountValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dto = (ProductDto)validationContext.ObjectInstance;

            if (dto.TypeDiscount == EDiscount.Percent)
            {
                if (dto.Discount < 0 || dto.Discount > 100)
                    return new ValidationResult("Discount percent must be between 0 and 100");
            }

            if (dto.TypeDiscount == EDiscount.Vnd)
            {
                if (dto.Discount < 0 || dto.Discount > dto.Price)
                    return new ValidationResult("Discount VND must be between 0 and product price");
            }

            return ValidationResult.Success;
        }
    }

}