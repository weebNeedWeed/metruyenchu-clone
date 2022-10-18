using System.Security.Claims;

namespace TheStory.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.Email)!.Value;
        }

        public static string GetName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.Name)!.Value;
        }

        public static string Test(this ClaimsPrincipal claimsPrincipal) => "haha";
    }
}
