using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class AgregarNotaCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public AgregarNotaCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("agregarnota")]
    public async Task AgregarNotaAsync(int index, [Remainder] string nota)
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        List<Interaccion> interacciones = Fachada.FachadaSistema.DelegarObtenerInteraccionesDe(vendedor);

        if (index < 1 || index > interacciones.Count)
        {
            await ReplyAsync("Índice inválido. Usa `!misinteracciones` para ver los números correctos.");
            return;
        }

        Interaccion interaccion = interacciones[index - 1];

        Fachada.FachadaSistema.DelegarAgregarNota(interaccion, nota);

        await ReplyAsync($"La nota fue agregada correctamente a la interacción {index}.");
    }


}