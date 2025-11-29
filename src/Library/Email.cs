using System;

namespace ClassLibrary
{
    public class Email: Interaccion, IRespondible
    {
        public string DireccionEmisor { get; private set; }
        public string DireccionReceptor { get; private set; }
        public string Contenido { get; private set; }
        public bool Respondido { get; set; }
    
        public Email(Persona emisor, Persona receptor, DateTime fecha, string tema, string contenido) : base(emisor, receptor, fecha, tema)
        {
            DireccionEmisor = emisor.Email;
            DireccionReceptor = receptor.Email;
            Contenido = contenido;
        
        }

        public void MarcarComoRespondido()
        {
            Respondido = true;
        }

        public void MarcarComoNoRespondido()
        {
            Respondido = false;
        }

        public bool EsRespuestaDe(IRespondible otra)
        {
            if (otra is Email otroEmail)
            {
                return otroEmail.Emisor == this.Receptor &&
                       otroEmail.Receptor == this.Emisor &&
                       otroEmail.Fecha > this.Fecha;
            }

            return false;
        }
    }
}