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
    
    
    /// <summary>
    /// Se encarga de crear una instancia de una cotización
    /// </summary>
    /// <param name="creador"></param>
    /// <param name="cliente"></param>
    /// <param name="fecha"></param>
    /// <param name="fechaLimite"></param>
    /// <param name="descripcion"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public Cotizacion CrearCotizacion(Usuario creador, Cliente cliente,
        DateTime fecha, DateTime fechaLimite, string descripcion)
    {
        if (creador == null) throw new ArgumentNullException(nameof(creador));
        if (cliente == null) throw new ArgumentNullException(nameof(cliente));

        Cotizacion c = new Cotizacion(creador, cliente, fecha, fechaLimite, descripcion);
        _cotizaciones.Add(c);
        return c;
    }
    
    // solo para los Tests
    
    public void LimpiarParaTest()
    {
        _cotizaciones.Clear();
    }

    public List<Cotizacion> Cotizaciones()
    {
        return _cotizaciones;
    }
}
}