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
    /// <summary>
    /// Crea una instancia de usuario
    /// </summary>
    /// <param name="solicitante"></param>
    /// <param name="nombre"></param>
    /// <param name="apellido"></param>
    /// <param name="email"></param>
    /// <param name="telefono"></param>
    /// <param name="contraseña"></param>
    /// <param name="rol"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public Usuario CrearUsuario(Usuario solicitante, string nombre, string apellido, string email, string telefono, string contraseña, string rol)
    {
        if (solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Solo administradores pueden crear usuarios.");
        Usuario nuevoUsuario = null;
        if (rol.Equals("usuario", StringComparison.OrdinalIgnoreCase))
        {
            nuevoUsuario = new Usuario(nombre, apellido, email, telefono, contraseña, TipoRol.USUARIO);

        }

        if (rol.Equals("vendedor", StringComparison.OrdinalIgnoreCase))
        {
            nuevoUsuario = new Vendedor(nombre, apellido, email, telefono, contraseña);
        }

        if (rol.Equals("administrador", StringComparison.OrdinalIgnoreCase))
        {
            nuevoUsuario = new Administrador(nombre, apellido, email, telefono, contraseña);
        }
        _usuarios.Add(nuevoUsuario);
        return nuevoUsuario;
    }
    

    /// <summary>
    /// Elimina al usuario de la lista global y no puede acceder al sistema
    /// </summary>
    /// <param name="solicitante"></param>
    /// <param name="usuario"></param>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public void EliminarUsuario(Usuario solicitante, Usuario usuario)
    {
        if (solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Solo administradores pueden eliminar usuarios.");

        _usuarios.Remove(usuario);
    }

    /// <summary>
    /// Suspende al usuario, negandole el acceso al sistema
    /// </summary>
    /// <param name="solicitante"></param>
    /// <param name="usuario"></param>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public void SuspenderUsuario(Usuario solicitante, Usuario usuario)
    {
        if (solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Solo administradores pueden suspender usuarios.");
        if (usuario.Rol == TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Un administrador no puede suspender a otro administrasdor");


        usuario.Suspendido = true;
    }

    /// <summary>
    /// Rehab
    /// </summary>
    /// <param name="solicitante"></param>
    /// <param name="usuario"></param>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public void RehabilitarUsuario(Usuario solicitante, Usuario usuario)
    {
        if (solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("Solo administradores pueden rehabilitar usuarios.");

        usuario.Suspendido = false;
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

    public Usuario BuscarUsuario(string criterio)
    {
        Usuario usuarioEncontrado = null;

        foreach (Usuario usuario in _usuarios)
        {
            if (usuario.Nombre.Equals(criterio, StringComparison.OrdinalIgnoreCase) ||
                usuario.Apellido.Equals(criterio, StringComparison.OrdinalIgnoreCase)||
                usuario.Telefono.Equals(criterio, StringComparison.OrdinalIgnoreCase)||
                usuario.Email.Equals(criterio, StringComparison.OrdinalIgnoreCase))
                usuarioEncontrado = usuario;
        }
        
        return usuarioEncontrado;
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