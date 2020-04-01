using HeroApp.Domain;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace HeroApp.Infra.Services
{
    public static class IdentityResultExtensions
    {
        public static ApiResponse ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? ApiResponse.Success()
                : ApiResponse.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
