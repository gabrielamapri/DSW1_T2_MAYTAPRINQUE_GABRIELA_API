# Backend - Library API (instrucciones de ejecución)

Este README explica cómo preparar el entorno, aplicar migraciones y levantar el backend (API ASP.NET Core) en Windows (PowerShell). Sirve para que puedas clonar y ejecutar el proyecto rápidamente.

## Requisitos
- .NET 8 SDK (ver `dotnet --version`)
- MySQL (o MariaDB) accesible (host/puerto/usuario/contraseña)
- Opcional: `dotnet-ef` para comandos EF Core
- Proveedor MySQL: `Pomelo.EntityFrameworkCore.MySql` v8.0.2

## Quickstart (pasos mínimos)
```
1. Crear un archivo .env en la raíz del repositorio. Copiar `.env.example` a `.env` y actualizar credenciales.

  Variables esperadas en `.env`: `BD_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER`, `DB_PASSWORD`.
```

2. Crear la base de datos en MySQL (si no existe):

```sql
CREATE DATABASE library_db;
```

3. Reconstruir y aplicar migraciones con los comandos:
   
dotnet build src\Library.API\Library.API.csproj

dotnet ef database update --project src\Library.Infrastructure\Library.Infrastructure.csproj --startup-project src\Library.API\Library.API.csproj
```

4. Iniciar la API (desarrollo) con el comando:

dotnet run --project "src\Library.API\Library.API.csproj"
```

5. Abrir Swagger UI en el navegador (http://localhost:5185/swagger).

## Puerto / URL por defecto
Por defecto la API suele escuchar en `http://localhost:5185`. Revisa la línea `Now listening on:` en la salida de `dotnet run` para conocer la URL exacta.


## Ejemplos rápidos de uso (Swagger / HTTP)

- Crear libro (POST `/api/books`):

```json
{
  "title": "Libro Prueba",
  "author": "Autor",
  "isbn": "ISBN-PRUEBA-001",
  "stock": 2
}
```

- Crear préstamo (POST `/api/loans`):

```json
{
  "bookId": 1,
  "studentName": "Alumno Ejemplo"
}
```

- Dar de baja un libro (POST `/api/books/dar-baja/{id}`):

```json
{
  "reason": "motivo opcional"
}
```

## Comprobaciones importantes
- El endpoint `GET /api/books` devuelve solo libros activos (`IsActive = true`).
- Comportamiento implementado:
  - Al crear un préstamo, `Stock` se decrementa en 1. No se puede prestar si `Stock == 0`.
  - Al devolver un préstamo, `Stock` se incrementa en 1.
  - `DarBaja` crea siempre un registro en `tb_articulos_baja`; si `Stock > 0` crea además un registro en `tb_articulos_liquidacion` con la cantidad liquidada y deja `Stock = 0`.

---
