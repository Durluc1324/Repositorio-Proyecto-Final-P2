using System;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class CrearClienteCommand : ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public CrearClienteCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("crearcliente")]
    public async Task CrearClienteAsync(string mensaje)
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);

        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }
        //Debería de recibir algo como "Tom Riddle tomriddle@gmail.com 1234 hombre 02:04:1992
        string[] datos = mensaje.Split(" ");
        
        if (datos.Length < 6)
        {
            await ReplyAsync("Formato incorrecto: usa `!crearcliente nombre apellido email telefono genero dd:mm:yyyy`");
            return;
        }
        
        string[] fechaNacimiendo = datos[5].Split(":");
        
        if (fechaNacimiendo.Length != 3 ||
            !int.TryParse(fechaNacimiendo[0], out int dia) ||
            !int.TryParse(fechaNacimiendo[1], out int mes) ||
            !int.TryParse(fechaNacimiendo[2], out int año))
        {
            await ReplyAsync("La fecha debe tener formato dd:mm:yyyy");
            return;
        }

        DateTime fecha;

        try
        {
            fecha = new DateTime(año, mes, dia);
        }
        catch
        {
            await ReplyAsync("La fecha ingresada no es válida.");
            return;
        }


        Cliente cliente = Fachada.FachadaSistema.DelegarCrearCliente(vendedor, datos[0], datos[1], datos[2], datos[3],
            datos[4], new DateTime(año, mes, dia));
        
        await ReplyAsync($"El cliente {cliente.Nombre} {cliente.Apellido} ha sido creado correctamente");
    }

}