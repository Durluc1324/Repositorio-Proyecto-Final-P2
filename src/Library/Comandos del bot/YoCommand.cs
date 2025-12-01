using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class YoCommand : ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public YoCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("yo")]
    public async Task YoAsync()
    {
        // Obtenemos al usuario autenticado dentro de tu SessionService
        var u = sessions.GetUsuario(Context.User.Id);

        if (u == null)
        {
            await ReplyAsync("No estás autenticado. Usa el comando `!login` para iniciar sesión.");
            return;
        }

        // 👉 Aquí va la parte que vos vas a implementar.
        // Este string lo vas a construir como quieras.
        string mensaje = ConstruirPanelUsuario(u);

        await ReplyAsync(mensaje);
    }

    // Dejé este método separado SOLO para que vos edites acá
    private string ConstruirPanelUsuario(Usuario u)
    {
        // 🌟 ACA ADENTRO VOS ARMÁS EL TEXTO DEL PANEL 🌟
        // Te dejo un esqueleto para que sea fácil:

        string panel = $@"
📌 **Información del Usuario**
Nombre: {u.Nombre}
Apellido: {u.Apellido}
Teléfono: {u.Telefono}

📊 **Clientes Totales**
Cantidad: {u.ClientesAsignados.Count}
Clientes:" + $@" 
{foreach (Cliente cliente in u.ClientesAsignados){}}

📅 **Interacciones Recientes**
(Aquí vos agregás lo que corresponda)

📆 **Próximas Reuniones**
(Aquí agregás lo que quieras mostrar)
";

        return panel;
    }
}