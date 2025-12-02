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
public async Task CrearCotizacionCommandAsync(string emailOTelefonoCliente, string fecha, string fechaLimite, string descripcion)
{
    Usuario vendedor = sessions.GetUsuario(Context.User.Id);
    if (vendedor == null)
    {
        await ReplyAsync("Debes iniciar sesión primero con `!login`.");
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

    // ---- PARSE FECHA REALIZADA ----
    string[] fechaRealizado = fecha.Split(":");

    if (fechaRealizado.Length != 3 ||
        !int.TryParse(fechaRealizado[0], out int dia1) ||
        !int.TryParse(fechaRealizado[1], out int mes1) ||
        !int.TryParse(fechaRealizado[2], out int año1))
    {
        await ReplyAsync("La fecha debe tener formato dd:mm:yyyy");
        return;
    }

    DateTime fechaCotizacionRealizada;
    try
    {
        fechaCotizacionRealizada = new DateTime(año1, mes1, dia1);
    }
    catch
    {
        await ReplyAsync("La fecha ingresada no es válida.");
        return;
    }

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