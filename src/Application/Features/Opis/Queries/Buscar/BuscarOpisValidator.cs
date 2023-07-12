using FluentValidation;

namespace Application.Features.Opis.Queries.Buscar;

public class BuscarOpisValidator : AbstractValidator<ReqBuscarOpis>
{
    public BuscarOpisValidator()
    {
        RuleFor( x => x.dtt_fecha_desde )
            .NotEmpty().WithMessage( " la fecha desde es requerida" )
            .NotNull().WithMessage( "La fecha desde es requerida" );
        RuleFor( x => x.dtt_fecha_hasta )
            .NotEmpty().WithMessage( " la fecha hasta es requerida" )
            .NotNull().WithMessage( "La fecha hasta es requerida" );
        RuleFor( x => x.str_tipo_transf )
            .NotEmpty().WithMessage( "El tipo de transferencia es requerido" )
            .NotNull().WithMessage( "El tipo de transferencia es requerido" );

        RuleFor( x => x.str_cta_ordenante )
            .NotEmpty().WithMessage( "La cuenta del ordenante es requerida " )
            .NotNull().WithMessage( "La cuenta del ordenante es requerida" );

        RuleFor( x => x.str_ident_ordenante )
            .NotEmpty().WithMessage( "La identificación del ordenante es requerida " )
            .NotNull().WithMessage( "La identificación del ordenante es requerida" );

        RuleFor( x => x.str_cta_beneficiario )
            .NotEmpty().WithMessage( "La cuenta del beneficiario es requerida " )
            .NotNull().WithMessage( "La cuenta del beneficiario es requerida" );

        RuleFor( x => x.str_ident_beneficiario )
            .NotEmpty().WithMessage( "La identificación del beneficiario es requerida " )
            .NotNull().WithMessage( "La identificación del beneficiario es requerida" );

        RuleFor( x => x.str_ruc_proveedor )
            .NotEmpty().WithMessage( "El ruc del proveedor es requerido " )
            .NotNull().WithMessage( "El ruc del proveedor es requerido" );

        RuleFor( x => x.str_comprobante_venta )
            .NotEmpty().WithMessage( "El comprobante de venta es requerido " )
            .NotNull().WithMessage( "El comprobante de venta es requerido" );

        RuleFor( x => x.str_comprobante_cont )
            .NotEmpty().WithMessage( "El comprobante contable es requerido " )
            .NotNull().WithMessage( "El comprobante contable es requerido" );

        RuleFor( x => x.int_codigo_opi )
            .NotEmpty().WithMessage( "El código opi es requerido " )
            .NotNull().WithMessage( "El código opi es requerido" );
    }
}