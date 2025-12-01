using System;

namespace ClassLibrary.Excepciones;

public class UsuarioSuspendidoException: Exception
{
    public UsuarioSuspendidoException():
        base("El usuario ha sido suspendido y no puede realizar ciertas acciones"){}
}