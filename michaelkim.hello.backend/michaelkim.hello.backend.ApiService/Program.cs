var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

// Add CORS service and specify allowed origins - need to directly note the address due to extension policy
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Optional but can help in dev
    });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Use CORS middleware
app.UseCors("AllowReactApp");
/*
app.UseCors(static builder => 
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
*/

app.UseHttpsRedirection();
app.MapDefaultEndpoints();

// Hello World endpoint
app.MapGet("/helloworld", () => Results.Ok("Hello World from .NET!"))
   .WithName("GetHelloWorld");

app.Run();