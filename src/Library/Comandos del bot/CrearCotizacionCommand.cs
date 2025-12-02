using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class CrearCotizacionCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public CrearCotizacionCommand(SessionService session)
    {
        this.sessions = session;
    }

    [Command("crearcotizacion")]
public async Task CrearCotizacionCommandAsync(string emailOTelefonoCliente, string fechaLimite, [Remainder]string descripcion)
{
    Usuario vendedor = sessions.GetUsuario(Context.User.Id);
    if (vendedor == null)
    {
        await ReplyAsync("Debes iniciar sesión primero con `!login`.");
        return;
    }

    if (string.IsNullOrEmpty(emailOTelefonoCliente)  || string.IsNullOrEmpty(fechaLimite)
        || string.IsNullOrEmpty(descripcion))
    {
        await ReplyAsync("Formato incorrecto: use '!crearcotizacion EmailOTelefonoCliente  dd:mm:yyyy descrición' \n" +
                         "La fecha subida es la fecha de vencimiento de la cotización ");
        return;
    }

    List<Cliente> clientes = Fachada.FachadaSistema.DelegarBuscarClientes(emailOTelefonoCliente);

    if (clientes.Count == 0)
    {
        await ReplyAsync("No se encontró un cliente con ese dato.");
        return;
    }

    if (clientes.Count > 1)
    {
        await ReplyAsync("Se encontraron múltiples clientes con ese dato. Usa un dato único como correo.");
        return;
    }

    Cliente cliente = clientes[0];
    
    DateTime fechaCotizacionRealizada = DateTime.Now;

    // ---- PARSE FECHA LÍMITE ----
    string[] fechaLimitada = fechaLimite.Split(":");

    if (fechaLimitada.Length != 3 ||
        !int.TryParse(fechaLimitada[0], out int dia) ||
        !int.TryParse(fechaLimitada[1], out int mes) ||
        !int.TryParse(fechaLimitada[2], out int año))
    {
        await ReplyAsync("La fecha límite debe tener formato dd:mm:yyyy");
        return;
    }

    DateTime fechaLimiteCotizacion;
    try
    {
        fechaLimiteCotizacion = new DateTime(año, mes, dia);
    }
    catch
    {
        await ReplyAsync("La fecha límite ingresada no es válida.");
        return;
    }

    // ---- CREAR LA COTIZACIÓN ----
    Cotizacion cotizacion = Fachada.FachadaSistema.DelegarCrearCotizacion(vendedor, cliente, fechaCotizacionRealizada, fechaLimiteCotizacion, descripcion);

    await ReplyAsync($"La cotización para **{cliente.Nombre} {cliente.Apellido}** con fecha límite **{fechaLimiteCotizacion:dd/MM/yyyy}** ha sido creada con éxito.");
}

}