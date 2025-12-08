# The Last Hope

**The Last Hope** es un proyecto desarrollado por **Yunquian Ao**, **Bernat Cifuentes**, **Pau Hernandez** y **Luis Fernandez**.

El juego se basa en **proteger la última cura** de oleadas de zombies junto a tus amigos conectados online.

---

## Contenido actual del proyecto

- Servidor para conexión online  
- Sistema de registro con usuario y contraseña  
- Inicio de sesión  
- Sala de espera  
- Sistema de emparejamiento  
- Entrada al juego  

---

## Instrucciones

1. Ejecute el archivo `.sln` en el servidor para iniciarlo.  
2. El **Jugador 1** inicia el archivo `.exe`.  
3. En la interfaz de la cuenta, el jugador puede elegir entre **registrarse** o **iniciar sesión**.  
   - Las cuentas existentes pueden iniciar sesión directamente.  
4. Tras iniciar sesión, los jugadores acceden a la sala y hacen clic en **“Iniciar”** para comenzar el emparejamiento.  
5. El **Jugador 2** sigue los mismos pasos.  
6. Cuando ambos jugadores estén listos, se emparejarán y entrarán juntos al juego.

Guía de Despliegue — Proyecto Last Hope

(Cliente Unity + Servidor MobaServer)

1. Requisitos
Cliente (Unity / Ejecutable)

Windows 10/11

Si se quiere abrir el proyecto: Unity 2022.3.x o superior

Red local o misma máquina que el servidor

Servidor (MobaServer)

Windows 10/11

Visual Studio 2022

.NET Framework/Runtime requerido por el proyecto

MySQL Server 5.7+

MySQL Workbench o phpMyAdmin (opcional)

2. Configuración del Servidor
2.1. Importar la base de datos

Crear una base de datos nueva (por ejemplo: game_db).

Importar el archivo SQL proporcionado (userinfo.sql, rolesinfo.sql, etc.).

Verificar que las tablas contienen datos básicos de usuario.

2.2. Configurar la conexión MySQL

En el proyecto MobaServer, revisar en los archivos relacionados con MySQL:

MySqlConnection conn = new MySqlConnection(
    "server=127.0.0.1;user id=root;password=XXXX;database=game_db;");

Modificar:

server → IP del servidor (normalmente 127.0.0.1)

user id / password → credenciales correctas

database → nombre de la BD importada

2.3. Iniciar el Servidor

Abrir MobaServer.sln en Visual Studio.

Restaurar paquetes NuGet si lo solicita.

Ejecutar el proyecto (F5).

El servidor iniciará escuchando en el puerto 8899.

3. Configuración del Cliente
3.1. Dirección IP del Servidor

En el cliente, revisar en:

Scripts/Net/UClient.cs

Buscar la IP configurada:

socket.Connect("127.0.0.1", 8899);

Modificar si es necesario:

Si el cliente y servidor están en la misma máquina → 127.0.0.1

Si están en equipos distintos → usar la IP local del servidor (ej. 192.168.1.35)

3.2. Ejecutar el Cliente

Si se usa Unity → Abrir la escena Login o Level01 y pulsar Play

Si se usa el ejecutable → Abrir LastHope.exe

Una vez conectado, el cliente recibirá los datos del servidor y cargará el escenario de batalla.

4. Estructura Recomendada del Proyecto
Last-Hope/
│
├── Cliente_Unity/
│   ├── LastHope.exe
│   ├── Assets/
│   └── ProjectSettings/
│
├── Servidor/
│   ├── MobaServer.sln
│   ├── MySql/
│   ├── Net/
│   ├── Player/
│   ├── Room/
│   ├── Match/
│   └── GameModule/
│
└── BaseDeDatos/
    ├── userinfo.sql
    ├── rolesinfo.sql
    └── otros.sql

5. Notas Importantes

El servidor siempre debe iniciarse antes que el cliente.

El firewall de Windows debe permitir el puerto 8899.

Si el cliente no muestra nada en la escena, normalmente significa:

No ha podido conectarse al servidor

IP incorrecta

Servidor detenido

Para pruebas en dos ordenadores del mismo WiFi, usar la IP local del servidor.