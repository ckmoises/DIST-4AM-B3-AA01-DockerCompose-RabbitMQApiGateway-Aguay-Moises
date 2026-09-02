# Sistema de Vehiculos (Arquitectura de Microservicios)

Este proyecto es un sistema basado en una arquitectura de microservicios construido con **.NET**. Esta compuesto por un API Gateway y dos microservicios independientes (`Categoria.Api` y `Vehiculo.Api`) que utilizan **RabbitMQ** para la comunicacion asincrona y **SQL Server** como motor de bases de datos relacional.

---

## Tecnologias y Herramientas

- **Backend:** .NET 10 / C# (Web API)
- **Base de Datos:** SQL Server
- **Mensajeria:** RabbitMQ
- **Contenedores y Orquestacion:** Docker y Docker Compose
- **Patrones de Arquitectura:** Microservicios, API Gateway

---

## Arquitectura del Sistema

```text
                  +-------------------+
                  |      Cliente      |
                  +---------+---------+
                            | HTTP
                            v
                  +-------------------+
                  |   API Gateway     |
                  |   (Puerto 9100)   |
                  +---------+---------+
                            |
           +----------------+----------------+
           |                                 |
           v                                 v
 +-------------------+             +-------------------+
 |   Categoria.Api   |             |   Vehiculo.Api    |
 |   (Puerto 5200)   |             |   (Puerto 5300)   |
 +---------+---------+             +---------+---------+
           |                                 |
           |           +---------+           |
           +---------> | RabbitMQ| <---------+
           |           +---------+           |
           v                                 v
 +-------------------+             +-------------------+
 |   DB_Categoria    |             |    DB_Vehiculo    |
 |  (SQL Server)     |             |   (SQL Server)    |
 +-------------------+             +-------------------+
```

---

## Requisitos Previos

Para poder ejecutar este proyecto en tu entorno local, asegurate de tener instalado lo siguiente:

1. **[Docker Desktop](https://www.docker.com/products/docker-desktop/)** (Debe estar ejecutandose).
2. **SQL Server** instalado de manera local en el puerto por defecto `1433`.
   * **Importante:** Tu SQL Server debe tener habilitada la **Autenticacion Mixta** (Windows & SQL Server Authentication) para permitir la conexion mediante usuario y contrasena.
3. Un cliente de SQL como **SQL Server Management Studio (SSMS)** o **Azure Data Studio** para ejecutar los scripts iniciales.
4. **Git** para clonar el repositorio.

---

## Paso 1: Configuracion de Bases de Datos

El sistema no utiliza bases de datos en memoria; requiere bases de datos reales. Se han incluido los scripts que configuran automaticamente las bases de datos, usuarios, contrasennas y permisos para que Docker se pueda comunicar con tu maquina anfitriona.

1. Abre SSMS o Azure Data Studio y conectate a tu instancia local de SQL Server.
2. Ve a la carpeta `BaseDatos` dentro de este proyecto.
3. **Ejecuta el script `CategoriaDB.sql`**: Creara la base de datos `DB_Categoria` y el login `usuario_inventariosA` (contrasena: `admin12`).
4. **Ejecuta el script `VehiculoDB.sql`**: Creara la base de datos `DB_Vehiculo` y el login `usuario_librosA` (contrasena: `admin`).

> **Nota:** Los scripts ya estan configurados para coincidir exactamente con las credenciales del entorno de Docker.

---

## Paso 2: Despliegue con Docker Compose

Una vez configurada la base de datos, levantar la aplicacion es sencillo con Docker Compose.

1. Abre una terminal y situate en la raiz del proyecto.
2. Ejecuta el siguiente comando para construir las imagenes y levantar los contenedores en segundo plano:

```bash
docker-compose up --build -d
```

### Contenedores que se levantaran:

| Contenedor | Descripcion | Puerto |
|---|---|---|
| `rabbitmqVehiculos` | Servidor de mensajeria RabbitMQ | 5674 (AMQP), 15674 (Admin UI) |
| `categoria-api` | Microservicio de Categorias | 5200 |
| `vehiculo-api` | Microservicio de Vehiculos | 5300 |
| `apigateway-vehiculos` | API Gateway (entrada principal) | 9100 |

> Panel de administracion de RabbitMQ: [http://localhost:15674](http://localhost:15674) — Usuario: `guest` / Contrasena: `guest`

---

## Pruebas y Uso

Se recomienda realizar todas las peticiones a traves del **API Gateway** y no a los microservicios individuales directamente.

La URL base de la aplicacion es:

`
http://localhost:9100
`

---

## Detener el Sistema

Para detener todos los microservicios de manera segura, ejecuta en la raiz del proyecto:

```bash
docker-compose down
```