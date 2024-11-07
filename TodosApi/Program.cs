using FluentValidation;
using TodosApi.ApiEndpoints;
using TodosApi.Database;
using TodosApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.AddMediatR(configuration =>
{
    configuration.Lifetime = ServiceLifetime.Scoped;
    configuration.RegisterServicesFromAssemblyContaining<Program>();
});

builder.Services.AddSingleton<MigrationsRunner>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapTodos();

var migrationsRunner = app.Services.GetRequiredService<MigrationsRunner>();
migrationsRunner.Run();

app.Run();
