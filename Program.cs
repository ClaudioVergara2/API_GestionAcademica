using API_Notas.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

var accesoWEB = "_accesoWEB";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: accesoWEB,
    policy =>
    {
        policy.WithOrigins("https://localhost:44335/") //cambiar con la ip que tengan
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EvaluacionContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("conex")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
    app.UseDeveloperExceptionPage(); // Habilita la página de errores detallados en desarrollo
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(accesoWEB);

app.Run();
