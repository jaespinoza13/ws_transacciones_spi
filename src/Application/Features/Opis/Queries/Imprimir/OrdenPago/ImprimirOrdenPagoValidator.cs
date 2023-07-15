using FluentValidation;

namespace Application.Features.Opis.Queries.Imprimir.OrdenPago;

public class ImprimirOrdenPagoValidator : AbstractValidator<ReqImprimirOrdenPago>
{
    public ImprimirOrdenPagoValidator()
    {
        RuleFor( x => x.int_codigo_opi )
            .NotEmpty().WithMessage( "El código opi es requerido " )
            .NotNull().WithMessage( "El código opi es requerido" );
        RuleFor( x => x.str_tipo_ordenante )
            .NotEmpty().WithMessage( "El tipo de ordenante es requerido " )
            .NotNull().WithMessage( "El tipo de ordenante es requerido" );
    }
}