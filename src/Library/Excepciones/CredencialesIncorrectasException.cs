using System;

namespace ClassLibrary.Excepciones;

public class CredencialesInvalidasException: Exception
{
    public CredencialesInvalidasException(string mensaje) : base(mensaje)
    {
        
    }
}