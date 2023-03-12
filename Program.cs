using DirectorAPI.Data;
using DirectorAPI.Models;
using DirectorAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(x=>x.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<Sistem21PrimariaContext>(x => x.UseMySql("server=sistemas19.com;database=sistem21_primaria;user=sistem21_primaria;password=sistemas19_", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.17-mariadb")));
var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run();
