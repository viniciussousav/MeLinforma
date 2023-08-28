// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Bem vindo! Digite o seu código de usuário:");
var userId = Guid.Empty;
while (userId == Guid.Empty)
{
    var isValid = Guid.TryParse(Console.ReadLine(), out userId);

    var message = isValid
        ? $"Usuário {userId} conectado"
        : "Usuário incorreto, tente novamente";

    Console.WriteLine(message);
}

await using var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5065/notifications")
    .Build();

try
{
    await connection.StartAsync();}
catch (Exception)
{
    Console.WriteLine("Falha ao se conectar");
    throw;
}

connection.On<string>(userId.ToString(), (message) => { Console.WriteLine($"[NOVA NOTIFICACAO]: {message}"); });
Console.WriteLine("[INFO] Você está recebendo notificações!!");

var client = new HttpClient { BaseAddress = new Uri("https://localhost:7051") };

while (true)
{
    Console.WriteLine("A qualquer momento, escolha uma opção");

    Console.WriteLine("1 - Listar suas notificações");
    Console.WriteLine("2 - Habilitar notificações");
    Console.WriteLine("3 - Desabilitar notificações");

    var input = Console.ReadLine();
    switch (input)
    {
        case "1":
            var notifications = await client.GetFromJsonAsync<List<Notifications>>($"/api/v1/customers/{userId}/notifications");
            PrintNotifications(notifications ?? new List<Notifications>());
            break;
        case "2":
            var enableResponse = await client.PutAsync($"/api/v1/customers/{userId}/notifications/subscribe", new StringContent(string.Empty));
            Console.WriteLine(enableResponse.IsSuccessStatusCode ? "[INFO] Notificações habilitadas!!" : "Falha");
            break;
        case "3":
            var disableResponse = await client.PutAsync($"/api/v1/customers/{userId}/notifications/unsubscribe", new StringContent(string.Empty));
            Console.WriteLine(disableResponse.IsSuccessStatusCode ? "[INFO] Notificações desabilitadas!!" : "Falha");
            break;
        default:
            Console.WriteLine("[INFO] Opção inválida!");
            break;
    }
}

void PrintNotifications(List<Notifications> notifications)
{
    var i = 1;
    notifications.ForEach(n =>
    {
        Console.WriteLine($"{i}: {n.Title} - {n.Description}");
        i++;
    });
}

public class Notifications
{
    public string Title { get; init; }
    public string Description { get; init; }
}