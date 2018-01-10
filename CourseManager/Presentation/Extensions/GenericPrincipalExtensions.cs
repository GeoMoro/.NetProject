using System.Security.Claims;
using System.Security.Principal;

namespace Presentation.Extensions
{
    public static class GenericPrincipalExtensions
    {
        public static string FullName(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated || !(user.Identity is ClaimsIdentity claimsIdentity))
            {
                return "";
            }

            foreach (var claim in claimsIdentity.Claims)
            {
                if (claim.Type == "FullName")
                {
                    return claim.Value;
                }
            }

            return "";
        }
    }
}