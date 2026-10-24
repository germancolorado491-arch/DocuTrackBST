using System;
using System.Collections.Generic;

namespace DocuTrackBST.Models
{
    public class ArbolBinario
    {
        public Nodo? Raiz { get; private set; }

        public ArbolBinario()
        {
            Raiz = null;
        }

        // Comparación case-insensitive ordinal invariante según requerimiento
        private int CompararClaves(string claveA, string claveB)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(claveA.Trim(), claveB.Trim());
        }

        public bool Insertar(string nombre, bool esCarpeta, out int comparaciones, out string mensajeDetalle)
        {
            comparaciones = 0;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                mensajeDetalle = "Rechazado: El nombre no puede estar vacío.";
                return false;
            }

            string nombreLimpio = nombre.Trim();

            if (Raiz == null)
            {
                Raiz = new Nodo(nombreLimpio, esCarpeta);
                mensajeDetalle = $"Insertado como raíz: '{nombreLimpio}' ({Raiz.ObtenerTipoTexto()}).";
                return true;
            }

            Nodo actual = Raiz;
            while (true)
            {
                comparaciones++;
                int cmp = CompararClaves(nombreLimpio, actual.Nombre);

                // Política de duplicados: no se permiten nombres repetidos
                if (cmp == 0)
                {
                    mensajeDetalle = $"Duplicado rechazado: Ya existe '{nombreLimpio}' en el árbol.";
                    return false;
                }

                // Regla de negocio: un archivo siempre es hoja, no puede tener subnodos
                if (!actual.EsCarpeta)
                {
                    mensajeDetalle = $"Rechazado: '{actual.Nombre}' es un archivo y no puede contener subnodos.";
                    return false;
                }

                if (cmp < 0)
                {
                    if (actual.Izquierdo == null)
                    {
                        actual.Izquierdo = new Nodo(nombreLimpio, esCarpeta);
                        mensajeDetalle = $"Insertado '{nombreLimpio}' a la izquierda de '{actual.Nombre}'.";
                        return true;
                    }
                    actual = actual.Izquierdo;
                }
                else
                {
                    if (actual.Derecho == null)
                    {
                        actual.Derecho = new Nodo(nombreLimpio, esCarpeta);
                        mensajeDetalle = $"Insertado '{nombreLimpio}' a la derecha de '{actual.Nombre}'.";
                        return true;
                    }
                    actual = actual.Derecho;
                }
            }
        }

        public Nodo? Buscar(string nombre, out int comparaciones)
        {
            comparaciones = 0;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                return null;
            }

            string nombreLimpio = nombre.Trim();
            Nodo? actual = Raiz;

            while (actual != null)
            {
                comparaciones++;
                int cmp = CompararClaves(nombreLimpio, actual.Nombre);

                if (cmp == 0)
                {
                    return actual;
                }

                if (cmp < 0)
                    actual = actual.Izquierdo;
                else
                    actual = actual.Derecho;
            }

            return null;
        }

        public bool Eliminar(string nombre, out string casoEjecutado)
        {
            casoEjecutado = "";

            if (string.IsNullOrWhiteSpace(nombre) || Raiz == null)
            {
                casoEjecutado = "Árbol vacío o nombre inválido.";
                return false;
            }

            bool eliminado = false;
            string casoDetectado = "";

            Raiz = EliminarRecursivo(Raiz, nombre.Trim(), ref eliminado, ref casoDetectado);

            if (!eliminado)
            {
                casoEjecutado = $"No se encontró el nodo '{nombre.Trim()}'.";
                return false;
            }

            casoEjecutado = casoDetectado;
            return true;
        }

        private Nodo? EliminarRecursivo(Nodo? actual, string nombre, ref bool eliminado, ref string casoDetectado)
        {
            if (actual == null)
            {
                return null;
            }

            int cmp = CompararClaves(nombre, actual.Nombre);

            if (cmp < 0)
            {
                actual.Izquierdo = EliminarRecursivo(actual.Izquierdo, nombre, ref eliminado, ref casoDetectado);
            }
            else if (cmp > 0)
            {
                actual.Derecho = EliminarRecursivo(actual.Derecho, nombre, ref eliminado, ref casoDetectado);
            }
            else
            {
                eliminado = true;

                // Caso 1: Es un nodo hoja (sin hijos)
                if (actual.EsHoja())
                {
                    if (casoDetectado == "")
                        casoDetectado = "Caso 1: Nodo Hoja (desenlazado directamente).";
                    return null;
                }

                // Caso 2: Tiene un solo hijo (derecho o izquierdo)
                if (actual.Izquierdo == null)
                {
                    if (casoDetectado == "")
                        casoDetectado = "Caso 2: Nodo con un solo hijo (reconectado hijo derecho).";
                    return actual.Derecho;
                }

                if (actual.Derecho == null)
                {
                    if (casoDetectado == "")
                        casoDetectado = "Caso 2: Nodo con un solo hijo (reconectado hijo izquierdo).";
                    return actual.Izquierdo;
                }

                // Caso 3: Tiene dos hijos -> Reemplazar por el sucesor inorden (menor del subárbol derecho)
                Nodo sucesor = ObtenerMinimo(actual.Derecho);
                if (casoDetectado == "")
                {
                    casoDetectado = $"Caso 3: Nodo con dos hijos (reemplazado por sucesor inorden '{sucesor.Nombre}').";
                }

                actual.Nombre = sucesor.Nombre;
                actual.EsCarpeta = sucesor.EsCarpeta;

                // Eliminar el nodo sucesor que acabamos de subir
                bool auxEliminado = false;
                string auxCaso = "YaRegistrado";
                actual.Derecho = EliminarRecursivo(actual.Derecho, sucesor.Nombre, ref auxEliminado, ref auxCaso);
            }

            return actual;
        }

        private Nodo ObtenerMinimo(Nodo nodo)
        {
            Nodo actual = nodo;
            while (actual.Izquierdo != null)
            {
                actual = actual.Izquierdo;
            }
            return actual;
        }

        // Estrategia obligatoria: Eliminar el nodo antiguo e insertar el nuevo
        public bool Actualizar(string nombreAntiguo, string nombreNuevo, out string detalleOperacion)
        {
            if (string.IsNullOrWhiteSpace(nombreAntiguo) || string.IsNullOrWhiteSpace(nombreNuevo))
            {
                detalleOperacion = "Los nombres no pueden estar vacíos.";
                return false;
            }

            int compBusqueda;
            Nodo? nodoExistente = Buscar(nombreAntiguo, out compBusqueda);
            if (nodoExistente == null)
            {
                detalleOperacion = $"El nodo '{nombreAntiguo}' no existe.";
                return false;
            }

            Nodo? duplicado = Buscar(nombreNuevo, out compBusqueda);
            if (duplicado != null)
            {
                detalleOperacion = $"El nuevo nombre '{nombreNuevo}' ya está en uso.";
                return false;
            }

            bool eraCarpeta = nodoExistente.EsCarpeta;

            bool seElimino = Eliminar(nombreAntiguo, out string casoEliminacion);
            if (!seElimino)
            {
                detalleOperacion = $"No se pudo eliminar '{nombreAntiguo}'.";
                return false;
            }

            bool seInserto = Insertar(nombreNuevo, eraCarpeta, out int compInsertar, out string msgInsertar);
            if (!seInserto)
            {
                detalleOperacion = $"Error al insertar '{nombreNuevo}': {msgInsertar}";
                return false;
            }

            detalleOperacion = $"Eliminado '{nombreAntiguo}' [{casoEliminacion}] -> Insertado '{nombreNuevo}' ({compInsertar} comparaciones).";
            return true;
        }

        public List<string> RecorridoPreorden()
        {
            List<string> lista = new List<string>();
            Preorden(Raiz, lista);
            return lista;
        }

        private void Preorden(Nodo? nodo, List<string> lista)
        {
            if (nodo == null) return;
            lista.Add($"{nodo.Nombre} {(nodo.EsCarpeta ? "[C]" : "[A]")}");
            Preorden(nodo.Izquierdo, lista);
            Preorden(nodo.Derecho, lista);
        }

        public List<string> RecorridoInorden()
        {
            List<string> lista = new List<string>();
            Inorden(Raiz, lista);
            return lista;
        }

        private void Inorden(Nodo? nodo, List<string> lista)
        {
            if (nodo == null) return;
            Inorden(nodo.Izquierdo, lista);
            lista.Add($"{nodo.Nombre} {(nodo.EsCarpeta ? "[C]" : "[A]")}");
            Inorden(nodo.Derecho, lista);
        }

        public List<string> RecorridoPostorden()
        {
            List<string> lista = new List<string>();
            Postorden(Raiz, lista);
            return lista;
        }

        private void Postorden(Nodo? nodo, List<string> lista)
        {
            if (nodo == null) return;
            Postorden(nodo.Izquierdo, lista);
            Postorden(nodo.Derecho, lista);
            lista.Add($"{nodo.Nombre} {(nodo.EsCarpeta ? "[C]" : "[A]")}");
        }

        public List<string> RecorridoPorNiveles()
        {
            List<string> lista = new List<string>();
            if (Raiz == null) return lista;

            Queue<Nodo> cola = new Queue<Nodo>();
            cola.Enqueue(Raiz);

            while (cola.Count > 0)
            {
                Nodo actual = cola.Dequeue();
                lista.Add($"{actual.Nombre} {(actual.EsCarpeta ? "[C]" : "[A]")}");

                if (actual.Izquierdo != null)
                    cola.Enqueue(actual.Izquierdo);

                if (actual.Derecho != null)
                    cola.Enqueue(actual.Derecho);
            }

            return lista;
        }

        public int CalcularAltura()
        {
            return AlturaRecursiva(Raiz);
        }

        private int AlturaRecursiva(Nodo? nodo)
        {
            if (nodo == null) return 0;
            int izq = AlturaRecursiva(nodo.Izquierdo);
            int der = AlturaRecursiva(nodo.Derecho);
            return Math.Max(izq, der) + 1;
        }
    }
}