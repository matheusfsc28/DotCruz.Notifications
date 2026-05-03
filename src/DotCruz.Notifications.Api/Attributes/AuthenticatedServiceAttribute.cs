using DotCruz.Notifications.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotCruz.Notifications.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthenticatedServiceAttribute : TypeFilterAttribute
{
    public AuthenticatedServiceAttribute() : base(typeof(AuthenticatedServiceFilter)) { } 
}
