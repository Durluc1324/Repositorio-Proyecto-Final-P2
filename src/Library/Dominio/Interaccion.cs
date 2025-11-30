using System;

namespace ClassLibrary
{
    public abstract class Interaccion
    {
        public Persona Emisor { get; set; }
        public Persona Receptor { get; set; }
        public DateTime Fecha { get; set; }
        public string Tema { get; set; }
        public string Nota { get; set; }

        public Interaccion(Persona emisor, Persona receptor, DateTime fecha, string tema)
        {
            Emisor = emisor;
            Receptor = receptor;
            Fecha = fecha;
            Tema = tema;
        
            emisor.AgregarInteraccion(this);
            receptor.AgregarInteraccion(this);
        }

        public void AddNota(string nota)
        {
            Nota = nota;
        }

        public string GetReceptor()
        {
            return Receptor.Nombre;
        }
    }    
}