using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class CrearReunionCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public CrearReunionCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("crearreunion")]
    public async Task CrearReunionCommandAsync(string mensaje)
    {
        if (string.IsNullOrEmpty(mensaje))
        {
            await ReplyAsync(
                "Formato incorrecto. Uso: `!crearreunion correoreceptor dd:mm:yyyy tema | lugar`");
            return;
        }

        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }
        var partes = mensaje.Split("|");
        if (partes.Length != 2)
        {
            await ReplyAsync("Debes separar el tema del contenido con `|`.");
            return;
        }

        string datosPrincipales = partes[0].Trim();
        string lugar = partes[1].Trim();

        string[] datos = datosPrincipales.Split(" ");

        if (datos.Length < 4)
        {
            await ReplyAsync("Faltan datos. Uso: `!crearreunion correoreceptor dd:mm:yyyy tema | contenido`");
            return;
        }

        Persona receptor = Fachada.FachadaSistema.BuscarPersona(datos[0]);
        
        if (receptor == null)
        {
            await ReplyAsync($"No se encontró al receptor con el correo {datos[0]}");
            return;
        }
        
        string[] partesFecha = datos[2].Split(":");
        if (partesFecha.Length != 3 ||
            !int.TryParse(partesFecha[0], out int dia) ||
            !int.TryParse(partesFecha[1], out int mes) ||
            !int.TryParse(partesFecha[2], out int año))
        {
            await ReplyAsync("Fecha inválida. Usa el formato dd:mm:yyyy");
            return;
        }

        DateTime fechaReunion;
        try
        {
            fechaReunion = new DateTime(año, mes, dia);
        }
        catch
        {
            await ReplyAsync("La fecha ingresada no es válida.");
            return;
        }

        string tema = string.Join(" ", datos.Skip(3));

        Reuniones reunion = Fachada.FachadaSistema.DelegarCrearReunion(vendedor, receptor, fechaReunion, tema, lugar);

        await ReplyAsync($"La reunión con {reunion.Receptor.Nombre} {reunion.Receptor.Apellido} en {reunion.Lugar} ha sido creada con éxito");

    }


}