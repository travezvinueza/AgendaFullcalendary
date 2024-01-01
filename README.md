[!NOTE]
INTRODUCCIÓN

Esta aplicación se desarrolló utilizando ASP.NET Core, haciendo uso del paquete Microsoft.AspNetCore.Identity. Este paquete es comúnmente empleado para implementar la autenticación de usuarios y la gestión de roles en aplicaciones web.

Al integrar la autenticación e Identity en tu aplicación, se generan automáticamente diversas tablas en la base de datos para almacenar información relacionada con usuarios y roles. Estas tablas abarcan aspectos como usuarios, roles, claims, logins externos, entre otros. Dichas estructuras son esenciales para gestionar la autenticación y autorización de usuarios en la aplicación.

Si bien es posible eliminar las tablas no utilizadas, no se recomienda hacerlo, ya que va en contra de las buenas prácticas de desarrollo.

ALGUNOS COMANDOS UTILIZADOS DURANTE EL DESARROLLO DEL PROYECTO

dotnet restore: Verifica posibles errores en el proyecto.

dotnet ef migrations add Migration: Genera automáticamente la carpeta de migraciones.

dotnet ef database update: Actualiza la base de datos en caso de cambios en el código.

dotnet publish -c Release: Publica el proyecto, listo para ser desplegado en un hosting.

URLS RELEVANTES

Registro de usuarios administradores: http://localhost:5133/UserAuthentication/RegisterAdmin

Hosting utilizado para desplegar el proyecto: FreeASPHosting.net

FreeASPHosting.net es un proveedor de servicios de alojamiento web gratuito, especializado en respaldar aplicaciones y sitios web basados en tecnologías de Microsoft, como ASP.NET.
