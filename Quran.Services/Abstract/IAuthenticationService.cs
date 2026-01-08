using Azure;
using Quran.Data.Entities.Identity;
using Quran.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Services.Abstract
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<string>> CreatRefreshToken(string OldAccessToken, string RefreshToekn);
        Task<ApiResponse<string>> CreateAccessTokenAsync(User user);
        Task<ApiResponse<string>> ConfirmEmail(string userId, string token);
        Task<ApiResponse<string>> ResetPassword(string userId, string token, string NewPassword);
        Task<ApiResponse<string>> CreateOtpAsync(string userId, string email);
        Task<ApiResponse<string>> VerifyOtpAsync(string userId, string code);
    }
}
