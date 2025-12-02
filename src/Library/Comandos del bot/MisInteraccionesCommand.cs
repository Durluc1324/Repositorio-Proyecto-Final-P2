using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace ClassLibrary;

public class MisInteraccionesCommand : ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public MisInteraccionesCommand(SessionService session)
    {
        this.sessions = session;
    }

    [Command("misinteracciones")]
    public async Task MisInteraccionesAsync()
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

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < interacciones.Count; i++)
        {
            Interaccion it = interacciones[i];
            if (it is Email email)
            {
                sb.AppendLine($"{i + 1}) {email.GetType().Name}");
                sb.AppendLine($"Fecha: {email.Fecha: dd/MM/yyyy}");
                sb.AppendLine($"Emisor: {it.Emisor.Email}");
                sb.AppendLine($"Receptor: {it.Receptor.Email}");
                sb.AppendLine($"Tema: {email.Tema}");
                sb.AppendLine($"Contenido: {email.Contenido}");
                sb.AppendLine($"Nota: {it.Nota}");
            }
            else if (it is Mensaje mensaje)
            {
                sb.AppendLine($"{i + 1}) {mensaje.GetType().Name}");
                sb.AppendLine($"Fecha: {mensaje.Fecha: dd/MM/yyyy}");
                sb.AppendLine($"Emisor: {it.Emisor.Telefono} ({it.Emisor.Nombre} {it.Emisor.Apellido})");
                sb.AppendLine($"Receptor: {it.Receptor.Telefono} ({it.Receptor.Nombre} {it.Receptor.Apellido})");
                sb.AppendLine($"Contenido: {mensaje.Tema}");
                sb.AppendLine($"Nota: {it.Nota}");

            }
            else if (it is Llamada llamada)
            {
                sb.AppendLine($"{i + 1}) {llamada.GetType().Name}");
                sb.AppendLine($"Fecha: {llamada.Fecha: dd/MM/yyyy}");
                sb.AppendLine(
                    $"Numero emisor: {llamada.NumeroEmisor} ({llamada.Emisor.Nombre} {llamada.Emisor.Apellido})");
                sb.AppendLine(
                    $"Numero emisor: {llamada.NumeroReceptor} ({llamada.Receptor.Nombre} {llamada.Receptor.Apellido})");
                sb.AppendLine($"Nota: {it.Nota}");
            }
            else if (it is Reuniones reunion)
            {
                sb.AppendLine($"{i + 1} {reunion.GetType().Name}");
                sb.AppendLine($"Fecha de reunión: {reunion.Fecha: dd/MM/yyyy}");
                sb.AppendLine($"Reunión con: {reunion.Receptor.Nombre} {reunion.Receptor.Apellido}");
                sb.AppendLine($"Lugar de reunion: {reunion.Lugar}");
                sb.AppendLine($"Tema: {reunion.Tema}");
                sb.AppendLine($"Nota: {it.Nota}");
            }

        }

        await ReplyAsync(sb.ToString());
    }
}
