using System;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;

namespace Library.Comandos_del_bot;

public class LoginCommand : ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public LoginCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("login")]
    public async Task LoginAsync(string emailOrPhone, string password)
    {
        try
        {
            Usuario u = Fachada.FachadaSistema.DelegarLogin(emailOrPhone, password);
            sessions.SetUsuario(Context.User.Id, u);
            await ReplyAsync($"Bienvenido, {u.Nombre} {u.Apellido}");
        }
        catch (Exception ex)
        {
            await ReplyAsync(ex.Message);
        }
    }
}
