using Microsoft.AspNetCore.Authorization;
using Task2.Models;

namespace Task2.Policies
{
    public class OwnTodoAuthorizationHandler : AuthorizationHandler<OwnTodoRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnTodoRequirement requirement)
        {
            var roles = Enum.GetNames(typeof(UserRoles));
            var r = roles.Any(role => context.User.IsInRole(role));

            if (context.User.Identity != null && context.User.IsInRole(UserRoles.Admin.ToString()))
                context.Succeed(requirement);            
            else if (context.User.Identity != null && roles.Any(role => context.User.IsInRole(role)))
            {
                // Access the HttpContext to retrieve the route data
                var httpContext = (context.Resource as DefaultHttpContext)?.HttpContext;
                if (httpContext != null)
                {
                    // Access route data to get the ID from the URL
                    var routeData = httpContext.Request.RouteValues;
                    if (routeData.TryGetValue("id", out var idValue))
                    {
                        if (int.TryParse(idValue?.ToString(), out var id))
                        {
                            var dbContext = httpContext.RequestServices.GetService<ToDoDbContext>();
                            var item = dbContext.ToDo.Find(id);
                            if (item != null && item.CreatedBy != null && item.CreatedBy.Equals(context.User.Identity.Name))
                            {
                                context.Succeed(requirement);
                            }
                        }
                    }
                    else
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}

