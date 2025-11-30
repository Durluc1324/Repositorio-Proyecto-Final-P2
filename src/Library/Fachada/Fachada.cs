using System;
using System.Collections.Generic;

namespace ClassLibrary;

public class Fachada
{

    private static readonly Fachada _fachada = new Fachada();
    public static Fachada FachadaSistema => _fachada;
    //Llamada a métodos de AdminsitrarCLientes.
    

    public Cliente DelegarCrearCliente(Usuario solicitante, string nombre, string apellido, string email,
        string telefono,
        string genero, DateTime fechaNacimiento)
    {
        Cliente clienteNuevo = AdministrarClientes.Instancia.CrearCliente(solicitante, nombre, apellido, email,
            telefono, genero,
            fechaNacimiento);
        return clienteNuevo;
    }

    public void DelegarModificarCliente(Usuario solicitante, Cliente cliente,
        string? nombre = null, string? apellido = null, string? email = null, string? telefono = null,
        string? genero = null, DateTime? fechaNacimiento = null)
    {
        AdministrarClientes.Instancia.ModificarCliente(solicitante, cliente, nombre, apellido, email,
            telefono, genero, fechaNacimiento);
    }

    public void DelegarEliminarCliente(Usuario solicitante, Cliente cliente)
    {
        AdministrarClientes.Instancia.EliminarCliente(solicitante, cliente);
    }

    public List<Cliente> DelegarBuscarClientes(string criterio)
    {
        return AdministrarClientes.Instancia.BuscarClientes(criterio);
    }

    public void DelegarAgregarEtiquetaCliente(Usuario solicitante, Cliente cliente, string etiqueta)
    {
        AdministrarClientes.Instancia.AgregarEtiquetaCliente(solicitante, cliente, etiqueta);
    }

    public List<Cliente> DelegarVerClientesPropios(Usuario usuario)
    {
        return AdministrarClientes.Instancia.VerClientesPropios(usuario);
    }

    public void DelegarAsignarClienteAOtroVendedor(Vendedor vendedor, Cliente cliente, Vendedor vendedorNuevo)
    {
        AdministrarClientes.Instancia.AsignarClienteAOtroVendedor(vendedor, cliente, vendedorNuevo);
    }

    //Llamada a métodos de AdministrarCotizaciones

    public void DelegarCrearCotizacion(Usuario creador, Cliente cliente,
        DateTime fecha, DateTime fechaLimite, string descripcion)
    {
        AdministrarCotizaciones.Instancia.CrearCotizacion(creador, cliente, fecha, fechaLimite, descripcion);
    }

    public List<Cotizacion> DelegarObtenerCotizaciones()
    {
        return AdministrarCotizaciones.Instancia.ObtenerCotizaciones();
    }

    public List<Cotizacion> DelegarBuscarCotizacionesComoAdmin(string criterio, DateTime? fecha,
        DateTime? fechaLimite)
    {
        return AdministrarCotizaciones.Instancia.BuscarCotizacionesComoAdmin(criterio, fecha, fechaLimite);
    }

    public List<Cotizacion> DelegarBuscarCotizacionesComoUsuario(
        Usuario usuario, string criterio, DateTime? fecha, DateTime? fechaLimite)
    {
        return AdministrarCotizaciones.Instancia.BuscarCotizacionesComoUsuario(usuario, criterio, fecha,
            fechaLimite);
    }

    //Llamada a métodos de AdminisitrarInteracciones

    public Llamada DelegarCrearLlamada(Usuario emisor, Persona receptor, DateTime fecha, string tema)
    {
        return AdministrarInteracciones.Instancia.CrearLlamada(emisor, receptor, fecha, tema);
    }

    public Mensaje DelegarCrearMensaje(Usuario emisor, Persona receptor, DateTime fecha, string tema)
    {
        return AdministrarInteracciones.Instancia.CrearMensaje(emisor, receptor, fecha, tema);
    }

    public Email DelegarCrearEmail(Usuario emisor, Persona receptor, DateTime fecha, string tema, string contenido)
    {
        return AdministrarInteracciones.Instancia.CrearEmail(emisor, receptor, fecha, tema, contenido);
    }

    public Reuniones DelegarCrearReunion(Usuario emisor, Persona receptor, DateTime fecha, string tema, string lugar)
    {
        return AdministrarInteracciones.Instancia.CrearReunion(emisor, receptor, fecha, tema, lugar);
    }

    public List<Interaccion> DelegarVerInteraccionesCliente(Cliente cliente, Type? tipo = null, DateTime? fecha = null)
    {
        return AdministrarInteracciones.Instancia.VerInteraccionesCliente(cliente, tipo, fecha);
    }

    public void DelegarAgregarNota(Interaccion interaccion, string nota)
    {
        AdministrarInteracciones.Instancia.AgregarNota(interaccion, nota);
    }

    public void DelegarEliminarInteraccion(Interaccion interaccion)
    {
        AdministrarInteracciones.Instancia.EliminarInteraccion(interaccion);
    }

    public List<IRespondible> ObtenerInteraccionesNoRespondidas(Usuario usuario)
    {
        return AdministrarInteracciones.Instancia.ObtenerInteraccionesNoRespondidas(usuario);
    }

    //Llamada a métodos de AdministrarUsuarios

    public void DelegarCrearUsuario(Usuario solicitante, string nombre, string apellido,
        string email, string telefono, TipoRol rol)
    {
        AdministrarUsuarios.Instancia.CrearUsuario(solicitante, nombre, apellido, email, telefono, rol);
    }

    public void DelegarEliminarUsuario(Usuario solicitante, Usuario usuario)
    {
        AdministrarUsuarios.Instancia.EliminarUsuario(solicitante, usuario);
    }

    public void DelegarSuspenderUsuario(Usuario solicitante, Usuario usuario)
    {
        AdministrarUsuarios.Instancia.SuspenderUsuario(solicitante, usuario);
    }

    public void DelegarRehabilitarUsuario(Usuario solicitante, Usuario usuario)
    {
        AdministrarUsuarios.Instancia.RehabilitarUsuario(solicitante, usuario);
    }

    public List<Usuario> DelegarVerTodos(Usuario solicitante)
    {
        return AdministrarUsuarios.Instancia.VerTodos(solicitante);
    }

    //Llamada a AdministrarVentas

    public Venta DelegarCrearVenta(Usuario usuario, Cliente cliente, DateTime fecha)
    {
        return AdministrarVentas.Instancia.CrearVenta(usuario, cliente, fecha);
    }

    public List<Venta> DelegarObtenerTodasLasVentas()
    {
        return AdministrarVentas.Instancia.ObtenerVentas();
    }

    public List<Venta> DelegarBuscarTodasPorCliente(Cliente cliente)
    {
        return AdministrarVentas.Instancia.BuscarTodasPorCliente(cliente);
    }

    public List<Venta> DelegarBuscarTodasPorUsuario(Usuario vendedor)
    {
        return AdministrarVentas.Instancia.BuscarTodasPorUsuario(vendedor);
    }

    public List<Venta> DelegarBuscarTodasPorFecha(DateTime fecha)
    {
        return AdministrarVentas.Instancia.BuscarTodasPorFecha(fecha);
    }

    public List<Venta> DelegarBuscarTodasPorRango(Usuario usuario, DateTime inicio, DateTime fin)
    {
        return AdministrarVentas.Instancia.BuscarTodasPorRango(usuario, inicio, fin);
    }

    public List<Venta> DelegarBuscarVentasComoAdmin(string criterio, DateTime? fecha, Cliente? cliente, Usuario? vendedor)
    {
        return AdministrarVentas.Instancia.BuscarVentasComoAdmin(criterio, fecha, cliente, vendedor);
    }

    public List<Venta> DelegarBuscarVentaComoUsuario(Usuario usuario, string criterio, DateTime? fecha)
    {
        return AdministrarVentas.Instancia.BuscarVentaComoUsuario(usuario, criterio, fecha);
    }

    public void DelegarAgregarProducto(Venta venta, string nombre, double precio, int cantidad)
    {
        AdministrarVentas.Instancia.AgregarProducto(venta, nombre, precio, cantidad);
    }

    public void DelegarCerrarVenta(Venta venta)
    {
        AdministrarVentas.Instancia.CerrarVenta(venta);
    }

    public double DelegarObtenerTotalVentasPeriodo(Usuario usuario, DateTime inicio, DateTime fin)
    {
        return AdministrarVentas.Instancia.ObtenerTotalVentasPeriodo(usuario, inicio, fin);
    }


}