using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Never;
using Never.Caching;
using Never.IoC;
using Never.Security;
using Never.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace B2C.Admin.Web
{
    /// <summary>
    ///
    /// </summary>
    public class FormsAuthenticationService : IAuthenticationService
    {
        #region field

        /// <summary>
        /// 一个用户只能登陆一个账号
        /// </summary>
        private static readonly CounterDictCache<string, string> single = new CounterDictCache<string, string>(30);

        /// <summary>
        /// scheme
        /// </summary>
        public const string FormsScheme = "B2CAdmin";

        #endregion field

        #region ctor

        /// <summary>
        ///
        /// </summary>
        static FormsAuthenticationService()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="httpContext"></param>
        public FormsAuthenticationService()
        {
        }

        #endregion ctor

        #region IAuthenticationService成员

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        public virtual Task SignIn(HttpContext context, IUser user)
        {
            ////获取票证
            var encryptedTicket = string.Concat(user.UserName, ";", string.Join("", Randomizer.PokerArray(10, 6))).ToRC2();

            /*只允许一个人单次登陆*/
            ChangeFormTicket(user.UserName, encryptedTicket);
            var items = new Dictionary<string, string>()
            {
                { "UserId",user.UserId.ToString()},
                { "UserName",user.UserName},
                { "UserTicket",encryptedTicket},
            };

            var task = context.SignInAsync(FormsAuthenticationService.FormsScheme, new UserPrincipal(new UserIdentity(user)), new AuthenticationProperties(items)
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                IsPersistent = true,
                AllowRefresh = false,
                RedirectUri = string.Empty,
            });

            return task;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Task SignOut(HttpContext context, IUser customer)
        {
            if (customer != null)
                RemoveFormTicket(customer.UserName);

            return context.SignOutAsync(FormsAuthenticationService.FormsScheme);
        }

        /// <summary>
        /// 获取会员
        /// </summary>
        /// <returns></returns>
        public UserPrincipal GetUserPrincipal(HttpContext context)
        {
            if (context.User is UserPrincipal)
                return context.User as UserPrincipal;

            var token = context.AuthenticateAsync(FormsAuthenticationService.FormsScheme);
            var result = token.GetAwaiter().GetResult();
            return result.Principal as UserPrincipal;
        }

        #endregion IAuthenticationService成员

        #region match

        /// <summary>
        /// 更新凭票
        /// </summary>
        public static void ChangeFormTicket(string userName, string ticket)
        {
            single.SetValue(userName, ticket);
        }

        /// <summary>
        /// 更新凭票
        /// </summary>
        public static void RemoveFormTicket(string userName)
        {
            single.RemoveValue(userName);
        }

        /// <summary>
        /// 匹配凭票
        /// </summary>
        /// <returns></returns>
        public static bool MatchFormTicket(string userName, string ticket)
        {
            if (ticket.IsNullOrEmpty())
                return true;

            var cookie = single.GetValue(userName);
            if (cookie == null)
            {
                single.SetValue(userName, ticket);
                return true;
            }

            return ticket.Equals(cookie, StringComparison.OrdinalIgnoreCase);
        }

        #endregion match
    }
}