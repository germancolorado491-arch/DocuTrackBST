using System;

namespace DocuTrackBST.Models
{
    public class Nodo
    {
        public string Nombre { get; set; }
        public bool EsCarpeta { get; set; }
        public Nodo? Izquierdo { get; set; }
        public Nodo? Derecho { get; set; }

        public Nodo(string nombre, bool esCarpeta)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del nodo no puede estar vacío.");
            }

            Nombre = nombre.Trim();
            EsCarpeta = esCarpeta;
            Izquierdo = null;
            Derecho = null;
        }

        // Verifica si el nodo no tiene hijos (es hoja)
        public bool EsHoja()
        {
            return Izquierdo == null && Derecho == null;
        }

        public string ObtenerTipoTexto()
        {
            if (EsCarpeta)
                return "[Carpeta]";
            else
                return "[Archivo]";
        }
    }
}