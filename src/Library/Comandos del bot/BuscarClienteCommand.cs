using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class BuscarClientesCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;
    
    public BuscarClientesCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("buscarcliente")]
    public async Task BuscarClientesCommandAsync(string mensaje)
    {
        if (string.IsNullOrEmpty(mensaje))
        {
            await ReplyAsync("No se han introducido datos suficientes. Use '!buscarcliente criterio (solo uno)'");
            return;
        }
        
        
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        List<Cliente> clientes = Fachada.FachadaSistema.DelegarBuscarClientes(mensaje);

        string clientesEncontrados = PanelClientesEncontrados(clientes);
        await ReplyAsync(clientesEncontrados);
    }

    private string PanelClientesEncontrados(List<Cliente> clientes)
    {
        var panel = new StringBuilder();
        panel.AppendLine("--- Clientes Encontrados ---");
        panel.AppendLine();
        foreach (Cliente cliente in clientes)
        {
            panel.AppendLine("--------");
            panel.AppendLine($"Cliente: {cliente.Nombre} {cliente.Apellido}");
            panel.AppendLine($"Teléfono: {cliente.Telefono}");
            panel.AppendLine($"Email: {cliente.Email}");
            if (cliente.FechaNacimiento.HasValue)
            {
                DateTime f = cliente.FechaNacimiento.Value;
                panel.AppendLine($"Fecha de nacimiento: {f.Day}:{f.Month}:{f.Year}");
            }
            else
            {
                panel.AppendLine("Fecha de nacimiento: -");
            }
            panel.AppendLine($"Género: {cliente.Genero}");
            panel.AppendLine($"Usuario asignado: {cliente.UsuarioAsignado.Nombre} {cliente.UsuarioAsignado.Apellido}");
            
        }
        panel.AppendLine("--------");


        return panel.ToString();
    }
}