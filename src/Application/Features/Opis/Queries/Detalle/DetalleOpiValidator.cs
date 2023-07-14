using FluentValidation;

namespace Application.Features.Opis.Queries.Detalle;

public class DetalleOpiValidator : AbstractValidator<ReqDetalleOpi>
{
    public DetalleOpiValidator()
    {
        RuleFor( x => x.int_codigo_opi )
            .NotEmpty().WithMessage( "El código opi es requerido " )
            .NotNull().WithMessage( "El código opi es requerido" );
        RuleFor( x => x.str_tipo_ordenante )
            .NotEmpty().WithMessage( "El tipo de ordenante es requerido " )
            .NotNull().WithMessage( "El tipo de ordenante es requerido" );
    }
}