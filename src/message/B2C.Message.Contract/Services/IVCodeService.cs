using Never;
using Never.Attributes;
using B2C.Message.Contract.Request;

namespace B2C.Message.Contract.Services
{
    /// <summary>
    /// 验证码接口
    /// </summary>
    public interface IVCodeService
    {
        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="reqs">The request.</param>
        /// <returns></returns>
        [ApiActionRemark("a9a900aedbb5", "HttpPost")]
        ApiResult<string> CreateMobileValidateCode(CreateMobileValidateCodeReqs reqs);

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="reqs">The request.</param>
        /// <returns></returns>
        [ApiActionRemark("a9a900aee071", "HttpPost")]
        ApiResult<string> CheckMobileValidateCode(CheckMobileValidateCodeReqs reqs);

        /// <summary>
        /// 创建邮箱验证码
        /// </summary>
        /// <param name="reqs"></param>
        /// <returns></returns>
        [ApiActionRemark("a9a900aee49a", "HttpPost")]
        ApiResult<string> CreateEmailValidateCode(CreateEmailValidateCodeReqs reqs);

        /// <summary>
        /// 校验邮箱验证码
        /// </summary>
        /// <param name="reqs"></param>
        /// <returns></returns>
        [ApiActionRemark("a9a900aee8c6", "HttpPost")]
        ApiResult<string> CheckEmailValidateCode(CheckEmailValidateCodeReqs reqs);
    }
}