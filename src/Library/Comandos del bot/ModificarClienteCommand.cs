using System;
using System.Collections.Generic;
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
    public async Task ModificarClienteAsync(string email, [Remainder] string mensaje)
    {
        // Validación básica
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(mensaje))
        {
            await ReplyAsync("Formato incorrecto. Usa: `!modificarcliente email parametro:valor ...`");
            return;
        }

        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        // Buscar cliente por email
        List<Cliente> listaClientes = Fachada.FachadaSistema.DelegarBuscarClientes(vendedor, email);

        if (listaClientes.Count == 0)
        {
            await ReplyAsync("No existe un cliente con ese email dentro de tus clientes");
            return;
        }
        if (listaClientes.Count > 1)
        {
            await ReplyAsync("Multiples clientes encontrados. Use un dato único.");
            return;
        }

        Cliente cliente = listaClientes[0]; // ← ahora sí

        // PARTES: ahora divido el mensaje por espacios
        // Ejemplo: "nombre:Tom apellido:Riddle telefono:123"
        string[] partes = mensaje.Split(" ");

        string nombre = null;
        string apellido = null;
        string emailNuevo = null;
        string telefono = null;
        string genero = null;
        DateTime? fechaNacimiento = null;

        foreach (string param in partes)
        {
            int idx = param.IndexOf(":");
            if (idx == -1)
                continue;

            string clave = param.Substring(0, idx).ToLower();
            string valor = param.Substring(idx + 1);

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
                    string[] f = valor.Split(":");
                    if (f.Length == 3 &&
                        int.TryParse(f[0], out int dia) &&
                        int.TryParse(f[1], out int mes) &&
                        int.TryParse(f[2], out int año))
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

        Fachada.FachadaSistema.DelegarModificarCliente(
            vendedor, cliente, nombre, apellido,
            emailNuevo, telefono, genero, fechaNacimiento
        );

        await ReplyAsync(
            $"El cliente {cliente.Nombre} {cliente.Apellido} fue modificado correctamente."
        );
    }
}