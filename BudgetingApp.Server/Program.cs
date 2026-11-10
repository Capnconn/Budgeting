using BudgetingApp.Adapter;
using BudgetingApp.Data.Data;
using BudgetingApp.Server.Messages;
using BudgetingApp.Server.Ngrok;
using BudgetingApp.Services;
using Going.Plaid.Entity;
using Going.Plaid.Webhook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

// Git-ignored overrides: Plaid keys and machine-specific paths live here, not in appsettings.json.
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Host.UseWolverine();

builder.Services.Configure<NgrokOptions>(builder.Configuration.GetSection(NgrokOptions.SectionName));
builder.Services.AddHostedService<NgrokTunnelHostedService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddAdapterServices(builder.Configuration);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Plaid delivers every Link-category webhook code to this one URL (configured on the Link token in
// PlaidAdapter.CreateLinkAsync), not a separate URL per code. ITEM_ADD_RESULT fires per bank,
// immediately after that bank finishes linking - unlike SESSION_FINISHED, which only fires once the
// whole (potentially multi-bank) session ends. Log every raw payload for visibility, and hand
// ITEM_ADD_RESULT's public token off to Wolverine and ack immediately - the actual token
// exchange/account/transaction sync happens in PlaidLinkPublicTokenReceivedHandler.
app.MapPost("/plaid-webhook", async (HttpRequest request, IMessageBus bus, ILogger<Program> logger) =>
{
    using var reader = new StreamReader(request.Body);
    var rawJson = await reader.ReadToEndAsync();

    using var document = JsonDocument.Parse(rawJson);
    var prettyJson = JsonSerializer.Serialize(document.RootElement, new JsonSerializerOptions { WriteIndented = true });
    logger.LogInformation("Received Plaid webhook:\n{Json}", prettyJson);

    var webhookCode = document.RootElement.TryGetProperty("webhook_code", out var codeProp) ? codeProp.GetString() : null;

    if (webhookCode != "ITEM_ADD_RESULT")
    {
        return Results.Ok();
    }

    var webhook = JsonSerializer.Deserialize<ItemAddResultWebhook>(rawJson, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

    if (webhook?.PublicToken is not null)
    {
        await bus.PublishAsync(new PlaidLinkPublicTokenReceived(webhook.PublicToken));
    }

    return Results.Ok();
});

app.MapGet("/", () => Results.Content(
    """
    <!DOCTYPE html>
    <html>
    <head><title>BudgetingApp</title></head>
    <body style="font-family: sans-serif; text-align: center; margin-top: 4rem;">
        <h1>Bank linked successfully</h1>
        <p>You can close this window and return to the apppp.</p>
    </body>
    </html>
    """,
    "text/html"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
