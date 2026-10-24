using System;
using System.Collections.Generic;
using System.Text;
using DocuTrackBST.Models;

namespace DocuTrackBST.Views
{
    public class ConsolaView
    {
        public ConsolaView()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public void MostrarBannerInicial()
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine("       DOCUTRACK S.A. - GESTOR JERÁRQUICO DE DOCUMENTOS (BST - MVC)       ");
            Console.WriteLine("==========================================================================");
        }

        public void MostrarEncabezadoSeccion(string titulo)
        {
            Console.WriteLine("\n--------------------------------------------------------------------------");
            Console.WriteLine($"  {titulo}");
            Console.WriteLine("--------------------------------------------------------------------------");
        }

        public void MostrarSubtitulo(string texto)
        {
            Console.WriteLine($"\n-> {texto}");
        }

        public void MostrarResultadoInsercion(string nombre, bool esCarpeta, bool exito, int comparaciones, string detalle)
        {
            string tipo = esCarpeta ? "Carpeta" : "Archivo";
            string estado = exito ? "[OK]" : "[RECHAZADO]";
            Console.WriteLine($"  {estado} Insertar({nombre} - {tipo}) | Comparaciones: {comparaciones} | {detalle}");
        }

        public void MostrarResultadoBusqueda(string nombreBuscado, Nodo? nodoHallado, int comparaciones)
        {
            if (nodoHallado != null)
            {
                Console.WriteLine($"  [HALLADO]    Clave: '{nombreBuscado}' | Tipo: {nodoHallado.ObtenerTipoTexto()} | Comparaciones: {comparaciones}");
            }
            else
            {
                Console.WriteLine($"  [NO HALLADO] Clave: '{nombreBuscado}' | Comparaciones: {comparaciones}");
            }
        }

        public void MostrarResultadoActualizacion(string nombreAntiguo, string nombreNuevo, bool exito, string detalle)
        {
            string estado = exito ? "[ACTUALIZADO]" : "[ERROR]";
            Console.WriteLine($"  {estado} '{nombreAntiguo}' -> '{nombreNuevo}'");
            Console.WriteLine($"     Detalle: {detalle}");
        }

        public void MostrarResultadoEliminacion(string nombre, bool exito, string casoEjecutado)
        {
            string estado = exito ? "[ELIMINADO]" : "[NO ELIMINADO]";
            Console.WriteLine($"  {estado} Clave: '{nombre}' | {casoEjecutado}");
        }

        // Impresión del árbol en ASCII usando conectores ├──, └── y │
        public void ImprimirArbolAscii(Nodo? raiz, string titulo)
        {
            Console.WriteLine($"\n  [{titulo}]");

            if (raiz == null)
            {
                Console.WriteLine("  (Árbol vacío)");
                return;
            }

            Console.WriteLine($"  {raiz.Nombre} {raiz.ObtenerTipoTexto()} (Raíz)");
            ImprimirRamas(raiz, "  ");
        }

        private void ImprimirRamas(Nodo padre, string prefijo)
        {
            bool tieneIzq = padre.Izquierdo != null;
            bool tieneDer = padre.Derecho != null;

            if (tieneIzq)
            {
                bool esUltimoHijo = !tieneDer;
                DibujarNodo(padre.Izquierdo!, prefijo, esUltimoHijo, "Izq");
            }

            if (tieneDer)
            {
                DibujarNodo(padre.Derecho!, prefijo, true, "Der");
            }
        }

        private void DibujarNodo(Nodo nodo, string prefijo, bool esUltimo, string lado)
        {
            string conector = esUltimo ? "└── " : "├── ";
            Console.WriteLine($"{prefijo}{conector}({lado}) {nodo.Nombre} {nodo.ObtenerTipoTexto()}");

            string nuevoPrefijo = prefijo + (esUltimo ? "    " : "│   ");
            ImprimirRamas(nodo, nuevoPrefijo);
        }

        public void MostrarRecorrido(string nombreRecorrido, List<string> elementos)
        {
            Console.WriteLine($"\n  * {nombreRecorrido}:");
            if (elementos.Count == 0)
            {
                Console.WriteLine("    (Vacío)");
                return;
            }
            Console.WriteLine("    " + string.Join(" -> ", elementos));
        }

        public void MostrarAlturaFinal(int altura)
        {
            Console.WriteLine("\n==========================================================================");
            Console.WriteLine($"  Altura final del árbol: {altura} niveles");
            Console.WriteLine("==========================================================================");
        }
    }
}