using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BudgetingApp.Hubs;

public interface ITokenExchangeClient
{
    Task SendAsync(string user, string publicToken);
}


// TODO: Investigate different ways to fetch public tokens (https://plaid.com/docs/quickstart/glossary/#public-token)
// This might be overkill, there might be another non-server related way to handle newly added plaid items.
public class TokenExchangeHub : Hub<ITokenExchangeClient>
{
    public async Task SendPublicToken(string user, string publicToken)
            => await Clients.Caller.SendAsync(user, publicToken);
}
