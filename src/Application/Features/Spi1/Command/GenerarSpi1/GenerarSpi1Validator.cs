using FluentValidation;

namespace Application.Features.Spi1.Command.GenerarSpi1;

public class GenerarSpi1Validator : AbstractValidator<ReqGenerarSpi1>
{
    public GenerarSpi1Validator()
    {
        RuleFor( x => x.dtt_fecha_corte )
            .NotEmpty().WithMessage( " la fecha del corte es requerida" )
            .NotNull().WithMessage( "La fecha del corte es requerida" );
    }
}