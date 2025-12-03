using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class BuscarClientesSinInteraccionCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;
    
    public BuscarClientesSinInteraccionCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("buscarclientesininteracciondesde")]
    public async Task BuscarClientesSinInteraccionDesdeCommandAsync(string mensaje)
    {
        
        //Recibe algo como !buscarclientesininteracciondesde 30:11:2025
        if (string.IsNullOrEmpty(mensaje))
        {
            await ReplyAsync("No se ha introducido datos suficientes. Usa '!buscarclientesininteracciondesde dd:mm:yyyy'");
        }
        
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        string[] partesFecha = mensaje.Split(":");
        
        if (partesFecha.Length != 3 ||
            !int.TryParse(partesFecha[0], out int dia) ||
            !int.TryParse(partesFecha[1], out int mes) ||
            !int.TryParse(partesFecha[2], out int año))
        {
            await ReplyAsync("Fecha inválida. Usa el formato dd:mm:yyyy");
            return;
        }

        DateTime fechaBusqueda;

        try
        {
            fechaBusqueda = new DateTime(año, mes, dia);
        }
        catch
        {
            await ReplyAsync("La fecha ingresada no es válida.");
            return;
        }
        
        List<Cliente> resultado = Fachada.FachadaSistema.DelegarBuscarClientesSinInteraccionDesde(vendedor, fechaBusqueda);
        

        string clientesNoInteraccion = ConstruirPanelClientesSinInteraccion(resultado);
        
        await ReplyAsync(clientesNoInteraccion);

    }

    private string ConstruirPanelClientesSinInteraccion(List<Cliente> clientes)
    {
        var panel = new StringBuilder();

        panel.AppendLine("--Clientes con poca interaccion--");

        foreach (Cliente cliente in clientes)
        {
            panel.AppendLine("--------");
            panel.AppendLine($"Cliente: {cliente.Nombre} {cliente.Apellido}");
            panel.AppendLine("--------");

        }
        return panel.ToString();
    }
}