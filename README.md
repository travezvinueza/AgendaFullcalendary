> [!NOTE]
> INTRODUCCIÓN
> Esta aplicacion se desarrollo con ASP.NET Core, haciendo uso del paquete paquete Microsoft.AspNetCore.Identity. Esto es   > comúnmente utilizado para implementar la autenticación de usuarios y la gestión de roles en una aplicación web.
> Al agregar la autenticación e Identity a tu aplicación, se crea automáticamente una serie de tablas en tu base de datos
> para almacenar información relacionada con los usuarios y roles. 
> Estas tablas incluyen cosas como usuarios, roles, claims, logins externos, y más. 
> Estas tablas son parte de la infraestructura necesaria para gestionar la autenticación y autorización de usuarios en tu   > aplicación.
> Si deseas puedes borrar las tablas que no se este utilizando pero no es muy recomendable por que es de malas practicas.

> [!TIP]
> ALGUNOS COMANDOS UTILIZADOS DURANTE EL DESARROLLO DEL PROYECTO

dotnet restore                            ---> es para verificar posibles errores

dotnet ef migrations add Migration        ---> creara automaticamente la carpeta migration

dotnet ef database update                 ---> actualizara tu base de datos si se ase algun cambio en el codigo

dotnet publish -c Release                 ---> es para publicar el proyecto subirlo a un hosting
> [!IMPORTANT]
http://localhost:5133/UserAuthentication/RegisterAdmin      --> con esta URL es para registrar usuarios admin

> [!IMPORTANT]
https://freeasphosting.net                 --> en esta URL se creo una cuenta para poder desplegar el proyecto

FreeASPHosting.net es un proveedor de servicios de alojamiento web gratuito que se centra
en el soporte de aplicaciones y sitios web basados en tecnologías de Microsoft, como ASP.NET. 

> [!WARNING]
 DEMO
 https://travez.bsite.net/
 Usuario: Mateo         Password: Mateo.eras@123*
