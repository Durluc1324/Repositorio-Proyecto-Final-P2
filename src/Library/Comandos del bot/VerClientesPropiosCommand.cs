using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class VerClientesPropiosCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;
    
    public VerClientesPropiosCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("verclientespropios")]
    public async Task VerClientesPropiosCommandAsync()
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        List<Cliente> clientesDeUsuario = Fachada.FachadaSistema.DelegarVerClientesPropios(vendedor);
        string listaClientesPropios = ClientesPropios(clientesDeUsuario) ;
        await ReplyAsync(listaClientesPropios);
    }

    private string ClientesPropios(List<Cliente> clientes)
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