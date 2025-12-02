using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class CrearLlamadaCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;
    
    public CrearLlamadaCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("crearllamada")]
    public async Task CrearLlamadaCommandAsync(string numeroCliente, string fechaCreacion, [Remainder]string tema)
    {
        if (string.IsNullOrEmpty(numeroCliente) || string.IsNullOrEmpty(fechaCreacion) || string.IsNullOrEmpty(tema))   
        {
            await ReplyAsync("No se han introducido datos suficientes. Use '!crearllamada NumeroDelCliente dd:mm:yyyy Tema'");
            return;
        }
        
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        
        List<Cliente> cliente = Fachada.FachadaSistema.DelegarBuscarClientes(vendedor,numeroCliente);

        if (cliente.Count > 1)
        {
            await ReplyAsync("Se han encontrado múltiples clientes. Por favor revise usar un correo único o pruebe usando un dato único del cliente");
            return;
        }
        
        string[] partesFecha = fechaCreacion.Split(":");
        
        if (partesFecha.Length != 3 ||
            !int.TryParse(partesFecha[0], out int dia) ||
            !int.TryParse(partesFecha[1], out int mes) ||
            !int.TryParse(partesFecha[2], out int año))
        {
            await ReplyAsync("Fecha inválida. Usa el formato dd:mm:yyyy");
            return;
        }

        DateTime fechadeLlamada;

        try
        {
            fechadeLlamada = new DateTime(año, mes, dia);
        }
        catch
        {
            await ReplyAsync("La fecha ingresada no es válida.");
            return;
        }

        Llamada llamada = Fachada.FachadaSistema.DelegarCrearLlamada(vendedor, cliente[0], fechadeLlamada, tema);

        await ReplyAsync($"La llamada con {llamada.Receptor.Nombre} {llamada.Receptor.Apellido} ha sido añadida");
    }
}