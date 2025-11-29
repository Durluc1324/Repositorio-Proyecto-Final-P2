using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class AdministrarCotizaciones
{
    // Singleton (nombre consistente con los demás)
    private static readonly AdministrarCotizaciones _instancia = new AdministrarCotizaciones();
    public static AdministrarCotizaciones Instancia => _instancia;
    private readonly List<Cotizacion> _cotizaciones = new List<Cotizacion>();


    // Constructor privado
    private AdministrarCotizaciones()
    {
    }
    
    /*
     * CrearCotizacion(): Crea una cotización a partir de los parámetros que recibe: un usuario, un cliente, una fecha
     * (se asume que es la fecha de creación de la cotización), una fecha límite para la cotización y la descripción de
     * esta.
     */
    public Cotizacion CrearCotizacion(Usuario creador, Cliente cliente,
        DateTime fecha, DateTime fechaLimite, string descripcion)
    {
        if (creador == null) throw new ArgumentNullException(nameof(creador));
        if (cliente == null) throw new ArgumentNullException(nameof(cliente));

        Cotizacion c = new Cotizacion(creador, cliente, fecha, fechaLimite, descripcion);
        _cotizaciones.Add(c);
        return c;
    }
    
    /*
     * ObtenerCotizaciones(): utilizada por los Administradores, pueden acceder a todas las cotizaciones del sistema para
     * su revisión.
     */

    public List<Cotizacion> ObtenerCotizaciones()
    {
        return new List<Cotizacion>(_cotizaciones);
    }
    
    /*
     * BuscarCotizacionComoAdmin(): Como dice su nombre, es usada por los administradores para buscar una 
     */
    public List<Cotizacion> BuscarCotizacionesComoAdmin(string criterio, DateTime? fecha, DateTime? fechaLimite)
    {
        if (string.IsNullOrWhiteSpace(criterio) &&
            !fecha.HasValue &&
            fecha == null &&
            fechaLimite == null)
        {
            throw new InvalidOperationException("No se introdujo ningún criterio de búsqueda.");
        }

        List<Cotizacion> resultados = new List<Cotizacion>();
        criterio = criterio?.ToLower().Trim();

        foreach (Cotizacion cotizacion in _cotizaciones)
        {
            if (cotizacion.ClienteDestino.Nombre == criterio || cotizacion.ClienteDestino.Apellido == criterio ||
                cotizacion.ClienteDestino.Telefono == criterio || cotizacion.ClienteDestino.Email == criterio ||
                cotizacion.UsuarioCreador.Nombre == criterio || cotizacion.UsuarioCreador.Apellido == criterio ||
                cotizacion.UsuarioCreador.Email == criterio || cotizacion.UsuarioCreador.Telefono == criterio || 
                cotizacion.Fecha == fecha || cotizacion.FechaLimite == fechaLimite)
            {
                resultados.Add(cotizacion);
            }
        }

        return resultados;
    }
    
    public List<Cotizacion> BuscarCotizacionesComoUsuario(
        Usuario usuario, string criterio, DateTime? fecha, DateTime? fechaLimite)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");

        if (string.IsNullOrWhiteSpace(criterio) && !fecha.HasValue && !fechaLimite.HasValue)
            throw new InvalidOperationException("Debe ingresar al menos un criterio de búsqueda.");

        List<Cotizacion> resultados = new List<Cotizacion>();

        foreach (Cotizacion cotizacion in usuario.ListaCotizaciones)
        {
            bool coincide = true;

            if (!string.IsNullOrWhiteSpace(criterio))
            {
                string crit = criterio.ToLower();

                coincide &= 
                    cotizacion.ClienteDestino.Nombre.ToLower().Contains(crit) ||
                    cotizacion.ClienteDestino.Apellido.ToLower().Contains(crit) ||
                    cotizacion.ClienteDestino.Genero.ToLower().Contains(crit) ||
                    cotizacion.ClienteDestino.Telefono.Contains(crit) ||
                    cotizacion.ClienteDestino.Email.Contains(crit) ||
                    cotizacion.UsuarioCreador.Nombre.ToLower().Contains(crit) ||
                    cotizacion.UsuarioCreador.Apellido.ToLower().Contains(crit) ||
                    cotizacion.UsuarioCreador.Telefono.Contains(crit) ||
                    cotizacion.UsuarioCreador.Email.Contains(crit);
            }

            if (fecha.HasValue)
                coincide &= cotizacion.Fecha.Date == fecha.Value.Date;

            if (fechaLimite.HasValue)
                coincide &= cotizacion.FechaLimite.Date == fechaLimite.Value.Date;

            if (coincide)
                resultados.Add(cotizacion);
        }

        return resultados;
    }

    public List<Cotizacion> BuscarCotizacionComoUsuario(Usuario usuario, string criterio, DateTime? fecha,
        DateTime? fechaLimite)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");

        if (string.IsNullOrWhiteSpace(criterio) && !fecha.HasValue && !fechaLimite.HasValue)
            throw new InvalidOperationException("Debe ingresar al menos un criterio de búsqueda.");

        List<Cotizacion> resultados = new List<Cotizacion>();
        criterio = criterio.ToLower().Trim();
        bool coincide;
    
        foreach (Cotizacion cotizacion in usuario.ListaCotizaciones)
        {
            coincide = cotizacion.ClienteDestino.Nombre.ToLower().Contains(criterio) ||
                       cotizacion.ClienteDestino.Apellido.ToLower().Contains(criterio) ||
                       cotizacion.ClienteDestino.Genero.ToLower().Contains(criterio) ||
                       cotizacion.ClienteDestino.Telefono.Contains(criterio) ||
                       cotizacion.ClienteDestino.Email.Contains(criterio) ||
                       cotizacion.UsuarioCreador.Nombre.ToLower().Contains(criterio) ||
                       cotizacion.UsuarioCreador.Apellido.ToLower().Contains(criterio) ||
                       cotizacion.UsuarioCreador.Telefono.Contains(criterio) ||
                       cotizacion.UsuarioCreador.Email.Contains(criterio);
            if (fecha.HasValue)
                coincide &= cotizacion.Fecha.Date == fecha.Value.Date;

            if (fechaLimite.HasValue)
                coincide &= cotizacion.FechaLimite.Date == fechaLimite.Value.Date;

            if (coincide)
                resultados.Add(cotizacion);
        }
        
        return resultados;
    }
    
}
}