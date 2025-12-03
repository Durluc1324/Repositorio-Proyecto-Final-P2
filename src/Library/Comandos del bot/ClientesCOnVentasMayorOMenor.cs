using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class ClientesCOnVentasMayorOMenor: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public ClientesCOnVentasMayorOMenor(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("clinentesconventasmayoromenor")]

    public async Task CLientesCOnVentasMayorOMenorCommandAsync(string monto1, string monto2OSign)
    {
        if (string.IsNullOrWhiteSpace(monto1) || string.IsNullOrWhiteSpace(monto2OSign))
        {
            await ReplyAsync("Formato inválido. Uso: `!agregaretiquetacliente EmailCLiente etiqueta`");
            return;
        }

        // Verificar login
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        if (monto2OSign == ">" || monto2OSign == "<")
        {
            List<Cliente> n =
                Fachada.FachadaSistema.DelegarClientesConVentasMayoresOMenoresAMonto(vendedor, monto1, monto2OSign);
            
            var panel = new StringBuilder();
            panel.AppendLine("--- Clientes Encontrados ---");
            panel.AppendLine();
            foreach (Cliente cliente in n)
            {
                panel.AppendLine("--------");
                panel.AppendLine($"Cliente: {cliente.Nombre} {cliente.Apellido}");
                panel.AppendLine($"Teléfono: {cliente.Telefono}");
                panel.AppendLine($"Email: {cliente.Email}");
                if (cliente.FechaNacimiento.HasValue)
                {
                    DateTime f = cliente.FechaNacimiento.Value;
                    panel.AppendLine($"Fecha de nacimiento: {f.Day}:{f.Month}:{f.Year}");
                }
                else
                {
                    panel.AppendLine("Fecha de nacimiento: -");
                }
                panel.AppendLine($"Género: {cliente.Genero}");
                panel.AppendLine($"Usuario asignado: {cliente.UsuarioAsignado.Nombre} {cliente.UsuarioAsignado.Apellido}");
            
            }
            panel.AppendLine("--------"); 
            await ReplyAsync(panel.ToString());
        }

        if (double.TryParse(monto2OSign))
        {
            //quedó incompleto pero intentaba que llame al comando de Administrarclientes para obtener clientes por rango
        }
    }
}