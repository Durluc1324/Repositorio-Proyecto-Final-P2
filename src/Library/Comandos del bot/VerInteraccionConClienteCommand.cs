using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class VerInteraccionConClienteCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public VerInteraccionConClienteCommand(SessionService session)
    {
        this.sessions = session;
    }

    [Command("verinteraccionconcliente")]
    public async Task VerInteraccionConClienteCommandAsync(string emailCliente)
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        List<Interaccion> interacciones = Fachada.FachadaSistema.DelegarObtenerInteraccionesDe(vendedor);

        if (interacciones.Count == 0)
        {
            await ReplyAsync("No tienes interacciones registradas.");
            return;
        }
        
        List<Cliente> clientes = Fachada.FachadaSistema.DelegarBuscarClientes(emailCliente);

        if (clientes.Count == 0)
        {
            await ReplyAsync("No se encontró un cliente con ese correo.");
            return;
        }

        if (clientes.Count > 1)
        {
            await ReplyAsync("Se encontraron múltiples clientes con ese dato. Usa un dato único como correo.");
            return;
        }

        Cliente cliente = clientes[0];

        List<Interaccion> interaccionesDeUsuario = Fachada.FachadaSistema.DelegarObtenerInteraccionesDe(vendedor);

        List<(int index, Interaccion it)> interaccionesFiltradas = new();
        for (int i = 0; i < interaccionesDeUsuario.Count; i++)
        {
            var inter = interaccionesDeUsuario[i];

            if (inter.Emisor.Equals(cliente) || inter.Receptor.Equals(cliente))
                interaccionesFiltradas.Add((i, inter));
        }
        
        StringBuilder sb = new StringBuilder();

        foreach (var item in interaccionesFiltradas)
        {
            int originalIndex = item.index + 1; 
            Interaccion it = item.it;

            sb.AppendLine($"{originalIndex}) {it.GetType().Name} — {it.Fecha:dd/MM/yyyy} — {it.Emisor.Email} → {it.Receptor.Email} — \"{it.Tema}\"");
        }

        await ReplyAsync(sb.ToString());
        
    }
}