using Microsoft.AspNetCore.Mvc;
using UniGate.Common.HMAC;

namespace UniGate.Common.Filters;

public class RequireHmacAttribute : TypeFilterAttribute
{
    public RequireHmacAttribute() : base(typeof(HmacAuthFilter))
    {
    }
}