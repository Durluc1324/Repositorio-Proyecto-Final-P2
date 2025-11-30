using System;

namespace ClassLibrary.Excepciones;

public class UsuarioNuloException: Exception
{
public UsuarioNuloException() 
    : base("No se puede ejecutar el método sin introducir un usuario") {}
}