using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class AdministrarUsuarios
{
    // Singleton
    private static readonly AdministrarUsuarios _instancia = new AdministrarUsuarios();
    public static AdministrarUsuarios Instancia => _instancia;

    // Lista interna
    private readonly List<Usuario> _usuarios = new List<Usuario>();

    // Constructor privado
    private AdministrarUsuarios()
    {
    }


    // Crear usuario (solo admin)
    public void CrearUsuario(Usuario solicitante, string nombre, string apellido, string email, string telefono, TipoRol rol)
    {
        if (solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Solo administradores pueden crear usuarios.");

        Usuario nuevoUsuario = rol == TipoRol.ADMINISTRADOR
            ? new Administrador(nombre, apellido, email, telefono)
            : new Vendedor(nombre, apellido, email, telefono);

        _usuarios.Add(nuevoUsuario);
    }

    public void EliminarUsuario(Usuario solicitante, Usuario usuario)
    {
        if (solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Solo administradores pueden eliminar usuarios.");

        _usuarios.Remove(usuario);
    }

    public void SuspenderUsuario(Usuario solicitante, Usuario usuario)
    {
        if (solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Solo administradores pueden suspender usuarios.");
        if (usuario.Rol == TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Un administrador no puede suspender a otro administrasdor");


        usuario.Suspendido = true;
    }

    public void RehabilitarUsuario(Usuario solicitante, Usuario usuario)
    {
        if (solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Solo administradores pueden rehabilitar usuarios.");

        usuario.Suspendido = false;
    }

    public List<Usuario> VerTodos(Usuario solicitante)
    {
        if (solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Solo administradores pueden ver todos los usuarios.");

        return _usuarios;
    }
}
}