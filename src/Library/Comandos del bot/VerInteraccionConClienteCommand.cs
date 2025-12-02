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

        if (string.IsNullOrEmpty(emailCliente))
        {
            await ReplyAsync("Formato incorrecto: use '!verinteraccionconcliente EmailCLiente'");
            return;
        }

        // Buscar al cliente
        List<Cliente> clientes = Fachada.FachadaSistema.DelegarBuscarClientes(emailCliente);

        if (clientes.Count == 0)
        {
            await ReplyAsync("No se encontró un cliente con ese correo o teléfono.");
            return;
        }

        if (clientes.Count > 1)
        {
            await ReplyAsync("Se encontraron múltiples clientes con ese dato. Usa uno único como correo.");
            return;
        }

        Cliente cliente = clientes[0];

        // Obtener interacciones del cliente desde la fachada
        List<Interaccion> interaccionesDelCliente =
            Fachada.FachadaSistema.DelegarVerInteraccionesCliente(cliente);

        if (interaccionesDelCliente.Count == 0)
        {
            await ReplyAsync("Este cliente no tiene interacciones registradas.");
            return;
        }

        // Filtrar solo las interacciones donde participa este vendedor
        List<(int index, Interaccion it)> interaccionesFiltradas = new();

        for (int i = 0; i < interaccionesDelCliente.Count; i++)
        {
            var inter = interaccionesDelCliente[i];

            if (inter.Emisor.Equals(vendedor) || inter.Receptor.Equals(vendedor))
                interaccionesFiltradas.Add((i, inter));
        }

        if (interaccionesFiltradas.Count == 0)
        {
            await ReplyAsync("No tienes interacciones con este cliente.");
            return;
        }

        // Armar mensaje de salida
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Interacciones entre **{vendedor.Nombre}** y **{cliente.Nombre} {cliente.Apellido}**:");
        sb.AppendLine("--------------------------------------------------");

        foreach (var item in interaccionesFiltradas)
        {
            int originalIndex = item.index + 1;
            Interaccion it = item.it;

            sb.AppendLine($"{originalIndex}) {it.GetType().Name} — {it.Fecha:dd/MM/yyyy} — " + 
                          $"{it.Emisor.Email} → {it.Receptor.Email} — \"{it.Tema}\"");
        }

        await ReplyAsync(sb.ToString());
    }

}