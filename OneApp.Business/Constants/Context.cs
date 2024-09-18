using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneApp.Business.Constants;

public class Context
{
    public string UserId { get; init; } = default!;

    public string TenantId { get; init; } = default!;
}
