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

    /// <summary>
    /// Crea un registro de una llamada realizada de un cliente a otro y permite anotar el tema de la llamada
    /// </summary>
    /// <param name="emisor"></param>
    /// <param name="receptor"></param>
    /// <param name="fecha"></param>
    /// <param name="tema"></param>
    /// <returns></returns>
    public Llamada CrearLlamada(Usuario emisor, Persona receptor, DateTime fecha, string tema)
    {
        ValidarUsuario(emisor);

        var llamada = new Llamada(emisor, receptor, fecha, tema);
        _interacciones.Add(llamada);
        return llamada;
    }

    /// <summary>
    /// Registra los mensajes que se comunica entre otra persona, ya sea un cliente, otro vendedor u otro administrador
    /// </summary>
    /// <param name="emisor"></param>
    /// <param name="receptor"></param>
    /// <param name="fecha"></param>
    /// <param name="tema"></param>
    /// <returns></returns>
    public Mensaje CrearMensaje(Persona emisor, Persona receptor, DateTime fecha, string tema)
    {
        if (emisor is Usuario u)
        {
            ValidarUsuario(u);
        }


        var mensaje = new Mensaje(emisor, receptor, fecha, tema);
        _interacciones.Add(mensaje);
        return mensaje;
    }

    /// <summary>
    /// Crea y devuelve una instancia del tipo Email para tener registro de los mails que el usuario quiere guardar
    /// </summary>
    /// <param name="emisor"></param>
    /// <param name="receptor"></param>
    /// <param name="fecha"></param>
    /// <param name="tema"></param>
    /// <param name="contenido"></param>
    /// <returns></returns>
    public Email CrearEmail(Persona emisor, Persona receptor, DateTime fecha, string tema, string contenido)
    {
        if (emisor is Usuario u)
        {
            ValidarUsuario(u);
        }
        
        var email = new Email(emisor, receptor, fecha, tema, contenido);
        _interacciones.Add(email);
        return email;
    }

    /// <summary>
    /// Crea y devuelve una instancia de un objeto tipo Reuniones
    /// </summary>
    /// <param name="emisor"></param>
    /// <param name="receptor"></param>
    /// <param name="fecha"></param>
    /// <param name="tema"></param>
    /// <param name="lugar"></param>
    /// <returns></returns>
    public Reuniones CrearReunion(Usuario emisor, Persona receptor, DateTime fecha, string tema, string lugar)
    {
        ValidarUsuario(emisor);

        var reunion = new Reuniones(emisor, receptor, fecha, tema, lugar);
        _interacciones.Add(reunion);
        return reunion;
    }

    // ---------- MÉTODOS DE CONSULTA ----------

    /// <summary>
    /// Método no implementado
    /// Permite ver las interacciones del cliente según el tipo, la fecha o ninguno
    /// </summary>
    /// <param name="cliente"></param>
    /// <param name="tipo"></param>
    /// <param name="fecha"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Método que se encarga de devolver la lista de interacciones del usuario
    /// </summary>
    /// <param name="usuario"></param>
    /// <returns></returns>
    public List<Interaccion> ObtenerInteraccionesDe(Usuario usuario)
    {
        return usuario.ListaInteracciones;
    }

    // ---------- MÉTODOS DE MODIFICACIÓN ----------

    /// <summary>
    /// Agrega una nota a la interacción vacía
    /// </summary>
    /// <param name="interaccion"></param>
    /// <param name="nota"></param>
    public void AgregarNota(Interaccion interaccion, string nota)
    {
        interaccion.AddNota(nota);
    }
    

    // ---------- MÉTODOS AUXILIARES ----------
    //Es un metodo que valida que el usuario no esté suspendido. Por falta de tiempo no pudo ser implementado en los métodos que debería
    private void ValidarUsuario(Usuario usuario)
    {
        if (usuario.Suspendido)
            throw new InvalidOperationException("Usuario suspendido.");
    }

    ///Hecho para actualizar las notas, revisa cuales interacciones no fueron respondidas cada que se llama. Esto le
    /// Sería útil al usuario porque 
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