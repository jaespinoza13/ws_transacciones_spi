using FluentValidation;

namespace Application.Features.Opis.Queries.Imprimir.Transferencias;

public class ImprimirTransferenciaValidator : AbstractValidator<ReqImprimirTransferencia>
{
    public ImprimirTransferenciaValidator()
    {
        RuleFor( x => x.int_codigo_opi )
            .NotEmpty().WithMessage( "El código opi es requerido " )
            .NotNull().WithMessage( "El código opi es requerido" );
        RuleFor( x => x.str_tipo_ordenante )
            .NotEmpty().WithMessage( "El tipo de ordenante es requerido " )
            .NotNull().WithMessage( "El tipo de ordenante es requerido" );
        RuleFor( x => x.str_tipo_persona )
            .NotEmpty().WithMessage( "El tipo de persona es requerido " )
            .NotNull().WithMessage( "El tipo de persona es requerido" );
    }
}