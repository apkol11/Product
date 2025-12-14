using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Business;
using Infrastructure;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBusinessServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add controllers with automatic model validation
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Customize model validation error response
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new
                {
                    Field = e.Key,
                    Errors = e.Value.Errors.Select(x => x.ErrorMessage).ToArray()
                })
                .ToArray();

            var result = new
            {
                Message = "Validation failed",
                Errors = errors
            };
                
            return new BadRequestObjectResult(result);
        };
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API v1");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
