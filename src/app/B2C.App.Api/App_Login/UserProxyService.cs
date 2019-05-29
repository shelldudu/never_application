using Never;
using Never.Caching;

namespace B2C.App.Api
{
    public class UserProxyService : IUserProxyService
    {
        public UserProxyService(IUserService service, ICaching caching)
        {
            this.Service = service; this.Caching = caching;
        }

        public IUserService Service { get; }
        public ICaching Caching { get; }


        public ApiResult<string> ChangePassword(ChangePwdReqs reqs)
        {
            return this.Service.ChangePassword(reqs);
        }

        public ApiResult<int> GetCount(string mobile)
        {
            return this.Service.GetCount(mobile);
        }

        public ApiResult<UserModel> GetUser(long userId)
        {
            return this.Caching.Get(userId.ToString(), () =>
             {
                 return this.Service.GetUser(userId);
             });
        }

        public ApiResult<string> LockUser(long userId)
        {
            return this.Service.LockUser(userId);
        }

        public ApiResult<UserModel> Login(LoginUserReqs reqs)
        {
            return this.Service.Login(reqs);
        }

        public ApiResult<UserModel> Register(RegisterUserReqs reqs)
        {
            return this.Service.Register(reqs);
        }

        public ApiResult<string> UnlockUser(long userId)
        {
            return this.Service.UnlockUser(userId);
        }
    }
}
