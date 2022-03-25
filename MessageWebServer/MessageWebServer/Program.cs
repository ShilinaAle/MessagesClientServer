using MessageWebServer;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "server is running");

app.MapGet("/GetAllMessages", () =>
{
    using (ApplicationContext db = new ApplicationContext())
    {
        return db.Messages.ToList();
    }
    
});

app.MapPost("/SaveMessage", ([FromBody] Message newMessage) =>
{
    using (ApplicationContext db = new ApplicationContext())
    {
        db.Messages.Add(newMessage);
        db.SaveChanges();
    }

    return Results.Ok();
});

app.Run();


