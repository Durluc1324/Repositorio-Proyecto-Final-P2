using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class EliminarClienteCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;
    
    public EliminarClienteCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("eliminarcliente")]
    public async Task EliminarClienteAsync([Remainder]string mensaje)
    {
        // lo que debe de recibir es: !eliminarcliente tomriddle@gmail.com || 092123
        string[] partes = mensaje.Split(" ");

        if (partes.Length == 0)
        {
            await ReplyAsync("No se ha introducido datos suficientes. Usa '!eliminarcliente emailOTelefono'.");
            return;
        }
        
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        string emailOTelefono = partes[0];
        List<Cliente> cliente = Fachada.FachadaSistema.DelegarBuscarClientes(emailOTelefono);

        if (cliente.Count == 0)
        {
            await ReplyAsync("No tienes un cliente con ese email o telefono. Usa `!verclientes` para ver tu lista.");
            return;
        }
        Cliente clienteAEliminar = cliente[0];
        
        Fachada.FachadaSistema.DelegarEliminarCliente(vendedor, clienteAEliminar);

        await ReplyAsync($"El cliente {clienteAEliminar.Nombre} {clienteAEliminar.Apellido} ha sido eliminado exitosamente");

    }

}