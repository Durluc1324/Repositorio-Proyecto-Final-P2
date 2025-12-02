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