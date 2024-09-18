using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneApp.Business.Constants;

public class Context
{
    public Guid UserId { get; init; } = default!;

    public Guid TenantId { get; init; } = default!;
}
