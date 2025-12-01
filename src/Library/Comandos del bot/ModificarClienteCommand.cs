using System;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class ModificarClienteCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public ModificarClienteCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("modificarcliente")]
    public async Task ModificarClienteAsync(string mensaje)
    {
        //divide el mensaje en una array de strings para poder organizar los datos
        string[] partes = mensaje.Split(" ");
        
        //Si no tiene el formato inválido, lanza error
        if (partes.Length < 2)
        {
            await ReplyAsync("Formato incorrecto. Usa: `!modificarusuario email parametro:valor ...`");
            return;
            
        }
        
        //Si el usuario no está logueado, no se realiza la tarea.
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }
        
        //Busca entre los clientes del usuario al cliente con ese mail, si no lo tiene, lo informa
        string emailBuscado = partes[0];
        Cliente cliente = null;

        foreach (Cliente c in vendedor.ClientesAsignados)
        {
            if (c.Email == emailBuscado)
            {
                cliente = c;
                break;
            }
        }
        
        if (cliente == null)
        {
            await ReplyAsync("No tienes un cliente con ese email. Usa `!verclientes` para ver tu lista.");
            return;
        }
        
        // variables auxiliares para luego
        string nombre = null;
        string apellido = null;
        string emailNuevo = null;
        string telefono = null;
        string genero = null;
        DateTime? fechaNacimiento = null;
        
        //
        for (int i = 1; i < partes.Length; i++)
        {
            string param = partes[i];

            // Si no tiene ":", lo ignoramos
            int pos = param.IndexOf(":");
            if (pos == -1)
                continue;

            string clave = param.Substring(0, pos).ToLower();
            string valor = param.Substring(pos + 1);

            switch (clave)
            {
                case "nombre":
                    nombre = valor;
                    break;

                case "apellido":
                    apellido = valor;
                    break;

                case "email":
                    emailNuevo = valor;
                    break;

                case "telefono":
                    telefono = valor;
                    break;

                case "genero":
                    genero = valor;
                    break;

                case "fecha":
                    string[] fechaPartes = valor.Split(":");
                    if (fechaPartes.Length == 3 &&
                        int.TryParse(fechaPartes[0], out int dia) &&
                        int.TryParse(fechaPartes[1], out int mes) &&
                        int.TryParse(fechaPartes[2], out int año))
                    {
                        try
                        {
                            fechaNacimiento = new DateTime(año, mes, dia);
                        }
                        catch { }
                    }
                    break;
            }
        }
        
        Fachada.FachadaSistema.DelegarModificarCliente(vendedor, cliente, nombre, apellido, 
            emailNuevo, telefono, genero, fechaNacimiento);
        
        await ReplyAsync($"El cliente {cliente.Nombre} {cliente.Apellido} fue modificado correctamente.");


    }
}