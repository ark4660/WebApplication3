using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder.Extensions;
using Scalar.AspNetCore;
using static FirebaseAdmin.FirebaseApp;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public sealed class FirebaseA
{
    private static readonly FirebaseA _Firebase = new FirebaseA();
    private FirebaseA()
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(@"C:\stwbdkw-firebase-adminsdk-fbsvc-b5e637503a.json"),
            ProjectId = "stwbdkw",
        });
    }

    public static FirebaseA FirebaseInstance()
    {
        return _Firebase;
    }
}
