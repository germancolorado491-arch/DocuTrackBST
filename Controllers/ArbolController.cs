using System;
using DocuTrackBST.Models;
using DocuTrackBST.Views;

namespace DocuTrackBST.Controllers
{
    public class ArbolController
    {
        private ArbolBinario arbol;
        private ConsolaView vista;

        public ArbolController()
        {
            arbol = new ArbolBinario();
            vista = new ConsolaView();
        }

        public void IniciarSimulacion()
        {
            vista.MostrarBannerInicial();

            ConstruirArbolInicial();
            RealizarBusquedasRapidas();
            RealizarActualizacionesSelectivas();
            RealizarEliminacionesSelectivas();
            MostrarRecorridosYAltura();
        }

        private void RegistrarNodo(string nombre, bool esCarpeta)
        {
            bool exito = arbol.Insertar(nombre, esCarpeta, out int comparaciones, out string detalle);
            vista.MostrarResultadoInsercion(nombre, esCarpeta, exito, comparaciones, detalle);
        }

        private void ConstruirArbolInicial()
        {
            vista.MostrarEncabezadoSeccion("1. CONSTRUCCIÓN DEL ÁRBOL (Mínimo 14 nodos + Prueba de duplicado)");

            // Raíz principal
            RegistrarNodo("M_Repositorio", true);

            // Nodos del subárbol izquierdo
            RegistrarNodo("F_Contabilidad", true);
            RegistrarNodo("C_Actas", true);
            RegistrarNodo("J_Impuestos", true);
            RegistrarNodo("A_Balance.pdf", false);
            RegistrarNodo("D_Contratos", true);
            RegistrarNodo("E_Anexos", true);
            RegistrarNodo("H_Declaracion.xlsx", false);

            // Nodos del subárbol derecho
            RegistrarNodo("T_Proyectos", true);
            RegistrarNodo("P_Legal", true);
            RegistrarNodo("W_Sistemas", true);
            RegistrarNodo("N_Demandas", true);
            RegistrarNodo("O_Fallos", true);
            RegistrarNodo("R_Poderes.pdf", false);
            RegistrarNodo("U_Manual.txt", false);
            RegistrarNodo("Y_Backups", true);
            RegistrarNodo("Z_Semanal.zip", false);

            // Prueba de rechazo de duplicado (case-insensitive)
            RegistrarNodo("f_contabilidad", true);

            vista.ImprimirArbolAscii(arbol.Raiz, "ÁRBOL INICIAL");
        }

        private void RealizarBusquedasRapidas()
        {
            vista.MostrarEncabezadoSeccion("2. BÚSQUEDAS RÁPIDAS (2 Izquierda, 2 Derecha, 2 Inexistentes)");

            vista.MostrarSubtitulo("2.1. Búsquedas en el subárbol izquierdo:");
            BuscarElemento("A_Balance.pdf");
            BuscarElemento("H_Declaracion.xlsx");

            vista.MostrarSubtitulo("2.2. Búsquedas en el subárbol derecho:");
            BuscarElemento("R_Poderes.pdf");
            BuscarElemento("Z_Semanal.zip");

            vista.MostrarSubtitulo("2.3. Búsquedas de elementos inexistentes:");
            BuscarElemento("B_Facturas.pdf");
            BuscarElemento("X_Auditoria.doc");
        }

        private void BuscarElemento(string nombre)
        {
            Nodo? encontrado = arbol.Buscar(nombre, out int comparaciones);
            vista.MostrarResultadoBusqueda(nombre, encontrado, comparaciones);
        }

        private void RealizarActualizacionesSelectivas()
        {
            vista.MostrarEncabezadoSeccion("3. ACTUALIZACIONES SELECTIVAS (Eliminar + Insertar)");

            vista.MostrarSubtitulo("3.1. Actualizar nodo hoja ('A_Balance.pdf' -> 'B_BalanceFinal.pdf'):");
            bool act1 = arbol.Actualizar("A_Balance.pdf", "B_BalanceFinal.pdf", out string det1);
            vista.MostrarResultadoActualizacion("A_Balance.pdf", "B_BalanceFinal.pdf", act1, det1);
            vista.ImprimirArbolAscii(arbol.Raiz, "ÁRBOL TRAS ACTUALIZAR HOJA");

            vista.MostrarSubtitulo("3.2. Actualizar nodo con un solo hijo ('D_Contratos' -> 'D_Convenios'):");
            bool act2 = arbol.Actualizar("D_Contratos", "D_Convenios", out string det2);
            vista.MostrarResultadoActualizacion("D_Contratos", "D_Convenios", act2, det2);
            vista.ImprimirArbolAscii(arbol.Raiz, "ÁRBOL TRAS ACTUALIZAR NODO CON UN HIJO");

            vista.MostrarSubtitulo("3.3. Actualizar la raíz ('M_Repositorio' -> 'M_RepositorioCentral'):");
            string nombreRaiz = arbol.Raiz != null ? arbol.Raiz.Nombre : "M_Repositorio";
            bool act3 = arbol.Actualizar(nombreRaiz, "M_RepositorioCentral", out string det3);
            vista.MostrarResultadoActualizacion(nombreRaiz, "M_RepositorioCentral", act3, det3);
            vista.ImprimirArbolAscii(arbol.Raiz, "ÁRBOL TRAS ACTUALIZAR RAÍZ");
        }

        private void RealizarEliminacionesSelectivas()
        {
            vista.MostrarEncabezadoSeccion("4. ELIMINACIONES SELECTIVAS (Hoja, Un Hijo y Raíz)");

            vista.MostrarSubtitulo("4.1. Eliminar nodo hoja ('R_Poderes.pdf'):");
            bool elim1 = arbol.Eliminar("R_Poderes.pdf", out string caso1);
            vista.MostrarResultadoEliminacion("R_Poderes.pdf", elim1, caso1);
            vista.ImprimirArbolAscii(arbol.Raiz, "ÁRBOL TRAS ELIMINAR HOJA");

            vista.MostrarSubtitulo("4.2. Eliminar nodo con un solo hijo ('Y_Backups'):");
            bool elim2 = arbol.Eliminar("Y_Backups", out string caso2);
            vista.MostrarResultadoEliminacion("Y_Backups", elim2, caso2);
            vista.ImprimirArbolAscii(arbol.Raiz, "ÁRBOL TRAS ELIMINAR NODO CON UN HIJO");

            string raizActual = arbol.Raiz != null ? arbol.Raiz.Nombre : "";
            vista.MostrarSubtitulo($"4.3. Eliminar raíz actual ('{raizActual}'):");
            bool elim3 = arbol.Eliminar(raizActual, out string caso3);
            vista.MostrarResultadoEliminacion(raizActual, elim3, caso3);
            vista.ImprimirArbolAscii(arbol.Raiz, "ÁRBOL FINAL TRAS ELIMINAR RAÍZ");
        }

        private void MostrarRecorridosYAltura()
        {
            vista.MostrarEncabezadoSeccion("5. RECORRIDOS DE VERIFICACIÓN Y ALTURA DEL ÁRBOL");

            vista.MostrarRecorrido("Preorden (Raíz - Izq - Der)", arbol.RecorridoPreorden());
            vista.MostrarRecorrido("Inorden (Izq - Raíz - Der)", arbol.RecorridoInorden());
            vista.MostrarRecorrido("Postorden (Izq - Der - Raíz)", arbol.RecorridoPostorden());
            vista.MostrarRecorrido("Por Niveles (Amplitud con Queue)", arbol.RecorridoPorNiveles());

            int altura = arbol.CalcularAltura();
            vista.MostrarAlturaFinal(altura);
        }
    }
}