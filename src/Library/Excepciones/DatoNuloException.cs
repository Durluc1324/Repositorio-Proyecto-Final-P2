using System;

namespace ClassLibrary.Excepciones;

public class DatoNuloException: Exception
{
    public DatoNuloException(string mensaje): base(mensaje)
    {
        
    }
}