using System;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;

namespace Library.Comandos_del_bot;

public class LogoutCommand : ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public LogoutCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("logout")]
    public async Task LogoutAsync()
    {
        // Verificar si estaba logueado
        var user = sessions.GetUsuario(Context.User.Id);
        
        if (user == null)
        {
            await ReplyAsync("No tenías ninguna sesión iniciada.");
            return;
        }

        // Cerrar sesión
        sessions.Logout(Context.User.Id);

        await ReplyAsync($"Hasta luego, {user.Nombre}. Sesión cerrada.");
    }
}