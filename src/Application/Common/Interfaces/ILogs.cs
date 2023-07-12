using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface ILogs
{
    Task SaveHeaderLogs(dynamic transaction, string strOperacion, string strMetodo, string strClase);

    Task SaveResponseLogs(dynamic transaction, string strOperacion, string strMetodo, string strClase);

    Task SaveExceptionLogs(dynamic transaction, string strOperacion, string strMetodo, string strClase, object objError);

    Task SaveHttpErrorLogs(dynamic transaction, string strMetodo, string strClase, dynamic objError,string strIdTransaccion);

    Task SaveAmenazasLogs(ValidacionInyeccion validacion, string strOperacion, string strMetodo, string strClase);

    Task SaveExcepcionDataBaseSybase(dynamic transaction, string strMetodo, Exception excepcion, string strClase);
}