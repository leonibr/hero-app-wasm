using HeroApp.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HeroApp.Domain.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<(ApiResponse ApiResponse, string UserId)> CreateUserAsync(string userName, string password);

        Task<ApiResponse> DeleteUserAsync(string userId);
    }
}
