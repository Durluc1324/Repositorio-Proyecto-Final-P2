using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace ClassLibrary;

public class MisInteraccionesCommand: ModuleBase<SocketCommandContext>
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
            sb.AppendLine($"{i+1}) {it.GetType().Name} — {it.Fecha:dd/MM/yyyy} — {it.Emisor.Email} → {it.Receptor.Email} — \"{it.Tema}\"");
        }

        await ReplyAsync(sb.ToString());
    }

}