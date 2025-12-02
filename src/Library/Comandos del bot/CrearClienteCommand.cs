using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
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
    public async Task CrearClienteAsync(string nombre, string apellido, string email, string telefono, string genero, string fechaNacimiento)
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);

        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }
        //Debería de recibir algo como "Tom Riddle tomriddle@gmail.com 1234 hombre 02:04:1992
        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(telefono)||string.IsNullOrEmpty(genero) || string.IsNullOrEmpty(fechaNacimiento))
        {
            await ReplyAsync("Formato incorrecto. Use '!crearcliente nombre apellido telefono genero dd:mm:yyyy'");
            return;
        }
        
        string[] fechaNacimiendo = fechaNacimiento.Split(":");
        
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

        try
        {
            Cliente cliente = Fachada.FachadaSistema.DelegarCrearCliente(vendedor, nombre, apellido, email, telefono,
                genero, fecha);

            await ReplyAsync($"El cliente {cliente.Nombre} {cliente.Apellido} ha sido creado correctamente");
        }
        catch (Exception ex)
        {
            await ReplyAsync($"Error: {ex}");

        }
        
    }

}