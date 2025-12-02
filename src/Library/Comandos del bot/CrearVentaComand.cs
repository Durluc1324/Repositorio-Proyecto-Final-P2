using System;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class CrearVentaComand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public CrearVentaComand(SessionService sessions)
    {
        this.sessions = sessions;
    }
    
   [Command("crearventa")]
public async Task CrearVentaAsync([Remainder]string mensaje)
{
    try
    {
        string[] partes = mensaje.Split(" ");

        if (partes.Length < 4)
        {
            await ReplyAsync("Formato incorrecto. Usa: '!crearventa correo producto precio cantidad ...'");
            return;
        }

        if ((partes.Length - 1) % 3 != 0)
        {
            await ReplyAsync("Formato incorrecto. Después del correo, cada producto debe tener: nombre precio cantidad.");
            return;
        }

        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        Cliente cliente = vendedor.ClientesAsignados
            .FirstOrDefault(c => c.Email == partes[0]);

        if (cliente == null)
        {
            await ReplyAsync("No se encontró un cliente asignado con ese correo electrónico.");
            return;
        }

        // 3. Crear la venta
        Venta venta = Fachada.FachadaSistema.DelegarCrearVenta(vendedor, cliente, DateTime.Now);

        for (int i = 1; i < partes.Length; i += 3)
        {
            string nombre = partes[i];

            if (!double.TryParse(partes[i + 1], out double precio))
            {
                await ReplyAsync($"El precio `{partes[i + 1]}` no es un número válido.");
                return;
            }

            if (!int.TryParse(partes[i + 2], out int cantidad))
            {
                await ReplyAsync($"La cantidad `{partes[i + 2]}` no es un número válido.");
                return;
            }

            venta.AgregarProducto(new Producto(nombre, precio), cantidad);
        }

        await ReplyAsync($"Venta creada correctamente para {cliente.Nombre} {cliente.Apellido}. Fecha: {venta.Fecha}");
    }
    catch (Exception ex)
    {
        await ReplyAsync(ex.Message);
    }
}

}