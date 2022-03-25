using MessageWebServer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var pathJson = "messagesData.json";

app.MapGet("/", () => "server is running");

app.MapGet("/GetAllMessages", () =>
{
    var data = File.ReadAllText(pathJson);
    List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(data);

    return messages;
});

app.MapPost("/SaveMessage", ([FromBody] Message newMessage) =>
{
    var data = File.ReadAllText(pathJson);
    List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(data);
    if (messages != null)
    {
        messages.Add(newMessage);
        File.WriteAllText(pathJson, JsonConvert.SerializeObject(messages));
    }
    else
    {
        List<Message> newmessages = new List<Message>() { newMessage };
        File.WriteAllText(pathJson, JsonConvert.SerializeObject(newmessages));
    }

    return Results.Ok();
});

app.Run();


