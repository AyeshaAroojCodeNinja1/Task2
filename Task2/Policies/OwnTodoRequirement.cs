using Microsoft.AspNetCore.Authorization;

namespace Task2.Policies
{
    public class OwnTodoRequirement : IAuthorizationRequirement
    {
    }
}
