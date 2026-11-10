using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Adapter.Data;

public record PlaidLink(
    string Id,
    string LinkToken,
    string HostedLinkUrl);
