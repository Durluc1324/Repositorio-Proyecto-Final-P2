using System.Collections.Generic;

namespace ClassLibrary;

public class SessionService
{
    private readonly Dictionary<ulong, Usuario> sesiones = new();

    public void SetUsuario(ulong discordId, Usuario u)
    {
        sesiones[discordId] = u;
    }

    public Usuario? GetUsuario(ulong discordId)
    {
        sesiones.TryGetValue(discordId, out var u);
        return u;
    }

    public void Logout(ulong discordId)
    {
        sesiones.Remove(discordId);
    }
}
