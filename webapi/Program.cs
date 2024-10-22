using webapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using YourNamespace.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DtpBdLabsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS policy setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("https://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddScoped<IAccidentRepository, AccidentRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DTP lab3-4-5",
        Description = "Це API створене з допомогою фреймворку ASP .NET.<br/>" +
        "Призначене для ведення обліку ДТП.<br/>" +
        "Дoступний функціонал:<br/>" +
        "1) CRUD для Accidents.<br/>" +
        "2) CRUD для people."
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS before authorization
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
