using System;

namespace ClassLibrary.Excepciones;

public class VentaCerradaException : Exception
{
    public VentaCerradaException() 
        : base("La venta está cerrada y no se pueden agregar productos.") {}
}