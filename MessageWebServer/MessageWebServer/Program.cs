using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var dict = new Dictionary<Guid, List<MessageDTO>>();
var app = builder.Build();

app.MapGet("/", () => "salam maleykum ");

app.MapGet("/GetAllMessages/{Id}", (Guid id) =>
{
    if (dict.ContainsKey(id))
    {
        return dict[id];
    }
    return new List<MessageDTO>();
});


app.MapPost("/SaveMessage", ([FromBody] MessageDTO newMessage) =>
{
    if (dict.TryGetValue(newMessage.UserId, out List<MessageDTO> userMessages))
    {
        userMessages.Add(newMessage);
    }
    else
    {
        dict[newMessage.UserId] = new List<MessageDTO>() { newMessage };
    }

    return Results.Ok();
});

app.Run();

public class MessageDTO
{
    public string Text { get; set; }
    public Guid UserId { get; set; }
    public string SentDateTime { get; set; }
}

