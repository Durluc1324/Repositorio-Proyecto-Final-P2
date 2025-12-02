using System;
using System.Collections.Generic;

namespace ClassLibrary;

public class Fachada
{

    private static readonly Fachada _fachada = new Fachada();
    public static Fachada FachadaSistema => _fachada;
    //Llamada a métodos de AdminsitrarCLientes.
    

    public Cliente DelegarCrearCliente(Usuario solicitante, string nombre, string apellido, string email,
        string telefono, string genero, DateTime fechaNacimiento)
    {
        return AdministrarClientes.Instancia.CrearCliente(solicitante, nombre, apellido, email,
            telefono, genero, fechaNacimiento);
        
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

    public Cotizacion DelegarCrearCotizacion(Usuario creador, Cliente cliente,
        DateTime fecha, DateTime fechaLimite, string descripcion)
    {
        return AdministrarCotizaciones.Instancia.CrearCotizacion(creador, cliente, fecha, fechaLimite, descripcion);
    }
    

    public List<Cliente> DelegarBuscarClientesSinInteraccionDesde(Usuario usuario, DateTime fecha)
    {
        return AdministrarClientes.Instancia.BuscarClientesSinInteraccionDesde(usuario, fecha);
    }

    //Llamada a métodos de AdminisitrarInteracciones

    public List<Interaccion> DelegarObtenerInteraccionesDe(Usuario usuario)
    {
        return AdministrarInteracciones.Instancia.ObtenerInteraccionesDe(usuario);
    }


    public Llamada DelegarCrearLlamada(Usuario emisor, Persona receptor, DateTime fecha, string tema)
    {
        return AdministrarInteracciones.Instancia.CrearLlamada(emisor, receptor, fecha, tema);
    }

    public Mensaje DelegarCrearMensaje(Persona emisor, Persona receptor, DateTime fecha, string tema)
    {
        return AdministrarInteracciones.Instancia.CrearMensaje(emisor, receptor, fecha, tema);
    }

    public Email DelegarCrearEmail(Persona emisor, Persona receptor, DateTime fecha, string tema, string contenido)
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
    

    //Llamada a métodos de AdministrarUsuarios

    public Usuario DelegarCrearUsuario(Usuario solicitante, string nombre, string apellido, string email, string telefono, string contraseña, string rol)

    {
        return AdministrarUsuarios.Instancia.CrearUsuario(solicitante, nombre, apellido, email, telefono, contraseña, rol);
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
    

    public Usuario DelegarLogin(string emailOTelefono, string contraseña)
    {
       return AdministrarUsuarios.Instancia.Login(emailOTelefono, contraseña);
    }

    public Usuario DelegarBuscarUsuario(string criterio)
    {
        return AdministrarUsuarios.Instancia.BuscarUsuario(criterio);
    }

    public Persona BuscarPersona(string criterio)
    {
        var clientes = AdministrarClientes.Instancia.BuscarClientes(criterio);
    
        if (clientes.Count == 1)
            return clientes[0];
    
        var usuario = AdministrarUsuarios.Instancia.BuscarUsuario(criterio);
        if (usuario != null)
            return usuario;

        return null;
    }


        
        
    //Llamada a AdministrarVentas

    public Venta DelegarCrearVenta(Usuario usuario, Cliente cliente, DateTime fecha)
    {
        return AdministrarVentas.Instancia.CrearVenta(usuario, cliente, fecha);
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
    

    public List<Venta> DelegarObtenerVentasPeriodo(Usuario usuario, DateTime inicio, DateTime fin)
    {
        return AdministrarVentas.Instancia.ObtenerVentasPeriodo(usuario, inicio, fin);
    }


}