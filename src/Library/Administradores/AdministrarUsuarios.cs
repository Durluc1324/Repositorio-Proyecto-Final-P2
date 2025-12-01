using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary.Excepciones;

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
    public Usuario CrearUsuario(Usuario solicitante, string nombre, string apellido, string email, string telefono, string contraseña, TipoRol rol)
    {
        if (solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Solo administradores pueden crear usuarios.");
        Usuario nuevoUsuario = null;
        if (rol == TipoRol.USUARIO)
        {
            nuevoUsuario = new Usuario(nombre, apellido, email, telefono, contraseña, rol);

        }

        if (rol == TipoRol.VENDEDOR)
        {
            nuevoUsuario = new Vendedor(nombre, apellido, email, telefono, contraseña);
        }

        if (rol == TipoRol.ADMINISTRADOR)
        {
            nuevoUsuario = new Administrador(nombre, apellido, email, telefono, contraseña);
        }
        _usuarios.Add(nuevoUsuario);
        return nuevoUsuario;
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

    public Usuario Login(string emailOTelefono, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(emailOTelefono))
            throw new DatoNuloException("Debe ingresar email o teléfono.");

        if (string.IsNullOrWhiteSpace(contraseña))
            throw new DatoNuloException("Debe ingresar la contraseña.");

        Usuario usuario = _usuarios.FirstOrDefault(u =>
            (u.Email.Equals(emailOTelefono.Trim(), StringComparison.OrdinalIgnoreCase) ||
             u.Telefono.Equals(emailOTelefono.Trim(),StringComparison.OrdinalIgnoreCase))
            && u.Contraseña == contraseña);

        if (usuario == null)
            throw new CredencialesInvalidasException("Credenciales incorrectas.");

        if (usuario.Suspendido)
            throw new UsuarioSuspendidoException();

        return usuario;
    }

    public void AgregarAdministrador(string nombre, string apellido, string email, string telefono, string contraseña)
    {
        _usuarios.Add(new Administrador(nombre, apellido, email, telefono, contraseña));
    }
    
    public void LimpiarParaTest()
    {
        _usuarios.Clear();
    }

    public List<Usuario> Usuarios()
    {
        return _usuarios;
    }
}
}