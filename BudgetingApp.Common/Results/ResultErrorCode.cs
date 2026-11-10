using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Common.Results;

public enum ResultErrorCode
{
    None,
    NotFound,
    DuplicateUser,
    ValidationFailed,
    Unauthorized,
    Unknown
}
