# Taller Integrador - Árboles Binarios de Búsqueda (MVC en C#)
## Caso de Estudio: DocuTrack S.A.

Proyecto de consola en C# (.NET 8.0) implementado bajo el patrón Modelo-Vista-Controlador (MVC). Permite gestionar jerárquicamente carpetas y archivos utilizando un Árbol Binario de Búsqueda (BST) ordenado por nombre, incluyendo operaciones CRUD, conteo de comparaciones, impresión en consola en formato ASCII y recorridos del árbol.

## Integrantes y Responsabilidades

| Integrante | Responsabilidades |
| :--- | :--- |
| Germán Alberto Colorado Moscoso | Implementación de la capa Model (`Nodo.cs`, `ArbolBinario.cs`), métodos de inserción, búsqueda, eliminación (3 casos), actualización y recorridos. |
| Leimar Soto Olaya | Implementación de la capa View (`ConsolaView.cs`), capa Controller (`ArbolController.cs`, `Program.cs`), impresión del árbol en ASCII y redacción del `README.md`. |

## Estructura del Proyecto

- **Models/**: Contiene `Nodo.cs` y `ArbolBinario.cs` con la estructura y lógica del BST sin referencias a consola.
- **Views/**: Contiene `ConsolaView.cs` encargada de mostrar los resultados, métricas y el diagrama ASCII con los conectores `├──`, `└──` y `│`.
- **Controllers/**: Contiene `ArbolController.cs` que ejecuta en orden los casos de prueba solicitados.
- **Program.cs**: Punto de entrada que instancia e invoca únicamente al controlador.

## Requisitos para Ejecutar

1. Tener instalado el SDK de .NET 8.0 (o superior).
2. Clonar el repositorio y abrir una terminal en la carpeta del proyecto:
   ```bash
   git clone <URL_DEL_REPOSITORIO>
   cd DocuTrackBST