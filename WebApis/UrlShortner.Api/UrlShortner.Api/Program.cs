using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UrlShortner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration[@"ConnectionStrings:urlshortener"];
builder.Services.AddDbContext<LongUrlContext>(options =>
    options.UseSqlServer(connectionString,
        b => b.MigrationsAssembly("UrlShortner.Api")));
builder.Services.AddScoped<ILongUrlContext>(provider => provider.GetService<LongUrlContext>());
builder.Services.AddScoped<IUrlShortnerService, UrlShortnerService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});
builder.Services.AddControllers();

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

app.UseAuthorization();
app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run();