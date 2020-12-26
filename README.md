## Registr.ar, a dotnet project
This project was my last work for the ComIT Fullstack .NET course.

This project can be run locally, but u need to configure the database path. If you clone now you will have a path made with an enviromental variable, you have to configure yours or hardcode one. Also, you have to setup some things for making it build and run, you need the run this commands to install the packages neededs:
*dotnet tool install --global dotnet-ef
*dotnet tool install --global dotnet-aspnet-codegenerator
*dotnet add package Microsoft.EntityFrameworkCore.SQLite ``If you will connect to a sqlite database``
*dotnet add package Microsoft.EntityFrameworkCore.SqlServer ``Same but for SqlServer``
*dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL ``Another one if you use PostgreSQL, currently using this on my project``
*dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
*dotnet add package Microsoft.EntityFrameworkCore.Design
*dotnet add package Microsoft.Extensions.Logging.Debug
*dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson

After having that done, you have to build the project with the command dotnet build on the console at the root path of the project. Once build you can run it with the dotnet run command and you will have your server running at your :80 port.

Thanks for visiting my repos!
You can test it in my online demo on Heroku: https://registrar-tupac.herokuapp.com/
