using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebBellwether.Models.Models.Auth;
using WebBellwether.Repositories.Entities.Auth;
using WebBellwether.Services.Factories;
using WebBellwether.Services.Utility;

namespace WebBellwether.Services.Services.AuthService
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(UserModel userModel);
        Task<IdentityUser> FindUser(string userName, string password);
        Client FindClient(string clientId);
        Task<bool> AddRefreshToken(RefreshToken token);
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);
        List<RefreshToken> GetAllRefreshTokens();
        Task<IdentityResult> CreateAsync(IdentityUser user);
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
        Task<RefreshToken> FindRefreshToken(string refreshTokenId);
    }
    public class AuthService:IAuthService
    {
        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };
            var result = await RepositoryFactory.UserManager.CreateAsync(user, userModel.Password);
            return result;
        }
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await RepositoryFactory.UserManager.FindAsync(userName, password);
            return user;
        }
        public Client FindClient(string clientId)
        {
            var client = ModelMapper.Map<Client,ClientDao>(RepositoryFactory.Context.Clients.Find(clientId));
            return client;
        }
        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var existingToken = RepositoryFactory.Context.RefreshTokens.SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);
            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(ModelMapper.Map<RefreshToken,RefreshTokenDao>(existingToken));
            }
            RepositoryFactory.Context.RefreshTokens.Add(ModelMapper.Map<RefreshTokenDao,RefreshToken>(token));
            return await RepositoryFactory.Context.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await RepositoryFactory.Context.RefreshTokens.FindAsync(refreshTokenId);
            if (refreshToken != null)
            {
                RepositoryFactory.Context.RefreshTokens.Remove(refreshToken);
                return await RepositoryFactory.Context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            RepositoryFactory.Context.RefreshTokens.Remove(ModelMapper.Map<RefreshTokenDao,RefreshToken>(refreshToken));
            return await RepositoryFactory.Context.SaveChangesAsync() > 0;
        }
        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = ModelMapper.Map<RefreshToken,RefreshTokenDao>(await RepositoryFactory.Context.RefreshTokens.FindAsync(refreshTokenId));
            return refreshToken;
        }
        public List<RefreshToken> GetAllRefreshTokens()
        {
            return ModelMapper.Map<RefreshToken[],RefreshTokenDao[]>(RepositoryFactory.Context.RefreshTokens.ToArray()).ToList();
        }
        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await RepositoryFactory.UserManager.FindAsync(loginInfo);
            return user;
        }
        public async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            var result = await RepositoryFactory.UserManager.CreateAsync(user);
            return result;
        }
        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await RepositoryFactory.UserManager.AddLoginAsync(userId, login);
            return result;
        }
    }
}
