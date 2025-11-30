using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class AdministrarInteracciones
{
    // Singleton
    private static readonly AdministrarInteracciones _instancia = new AdministrarInteracciones();
    public static AdministrarInteracciones Instancia => _instancia;

    // Lista interna
    private readonly List<Interaccion> _interacciones = new List<Interaccion>();

    // Constructor privado
    private AdministrarInteracciones()
    {
        
    }


    // ---------- MÉTODOS PARA CREAR INTERACCIONES ----------

    public Llamada CrearLlamada(Usuario emisor, Persona receptor, DateTime fecha, string tema)
    {
        ValidarUsuario(emisor);

        var llamada = new Llamada(emisor, receptor, fecha, tema);
        _interacciones.Add(llamada);
        return llamada;
    }

    public Mensaje CrearMensaje(Usuario emisor, Persona receptor, DateTime fecha, string tema)
    {
        ValidarUsuario(emisor);

        var mensaje = new Mensaje(emisor, receptor, fecha, tema);
        _interacciones.Add(mensaje);
        return mensaje;
    }

    public Email CrearEmail(Usuario emisor, Persona receptor, DateTime fecha, string tema, string contenido)
    {
        ValidarUsuario(emisor);

        var email = new Email(emisor, receptor, fecha, tema, contenido);
        _interacciones.Add(email);
        return email;
    }

    public Reuniones CrearReunion(Usuario emisor, Persona receptor, DateTime fecha, string tema, string lugar)
    {
        ValidarUsuario(emisor);

        var reunion = new Reuniones(emisor, receptor, fecha, tema, lugar);
        _interacciones.Add(reunion);
        return reunion;
    }

    // ---------- MÉTODOS DE CONSULTA ----------

    public List<Interaccion> VerInteraccionesCliente(Cliente cliente, Type? tipo = null, DateTime? fecha = null)
    {
        var resultado = new List<Interaccion>();

        foreach (var interaccion in cliente.ListaInteracciones)
        {
            if (tipo != null && interaccion.GetType() != tipo)
                continue;

            if (fecha.HasValue && interaccion.Fecha.Date != fecha.Value.Date)
                continue;

            resultado.Add(interaccion);
        }

        return resultado;
    }

    // ---------- MÉTODOS DE MODIFICACIÓN ----------

    public void AgregarNota(Interaccion interaccion, string nota)
    {
        interaccion.AddNota(nota);
    }

    public void EliminarInteraccion(Interaccion interaccion)
    {
        interaccion.Emisor.EliminarInteraccion(interaccion);
        interaccion.Receptor.EliminarInteraccion(interaccion);
        _interacciones.Remove(interaccion);
    }

    // ---------- MÉTODOS AUXILIARES ----------

    private void ValidarUsuario(Usuario usuario)
    {
        if (usuario.Suspendido)
            throw new InvalidOperationException("Usuario suspendido.");
    }


    private void ActualizarInteraccionesNoRespondidas(Usuario usuario)
    {
        // 1. Reiniciar solo Emails y Mensajes
        foreach (Interaccion inter in usuario.ListaInteracciones)
        {
            if (inter is IRespondible r && (inter is Email || inter is Mensaje))
            {
                r.MarcarComoNoRespondido();
            }
        }

        // 2. Ver cuál responde a cuál
        foreach (Interaccion inter1 in usuario.ListaInteracciones)
        {
            foreach (Interaccion inter2 in usuario.ListaInteracciones)
            {
                if (inter1 == inter2) continue;

                if (inter1 is IRespondible resp1 && inter2 is IRespondible resp2)
                {
                    // inter1 responde a inter2
                    if (resp1.EsRespuestaDe(resp2))
                    {
                        resp2.MarcarComoRespondido();
                    }
                }
            }
        }
    }
    
    public List<IRespondible> ObtenerInteraccionesNoRespondidas(Usuario usuario)
    {
        // Primero actualizar
        ActualizarInteraccionesNoRespondidas(usuario);

        List<IRespondible> pendientes = new List<IRespondible>();

        foreach (var interaccion in usuario.ListaInteracciones)
        {
            if (interaccion is IRespondible r && !r.Respondido)
            {
                pendientes.Add(r);
            }
        }

        return pendientes;
    }
    
    public void LimpiarParaTest()
    {
        _interacciones.Clear();
    }

    public List<Interaccion> Interacciones()
    {
        return _interacciones;
    }


}
}