using Application.Models.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class DeliveryTypeValidator : AbstractValidator<Delivery>
    {
        public DeliveryTypeValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("El ID del ítem no puede estar vacío.");
            RuleFor(x => x.to)
                .NotEmpty().WithMessage("El número de contacto no puede estar vacío.")
                .MaximumLength(250).WithMessage("Las notas no pueden exceder los 250 caracteres.");
        }
    }
}