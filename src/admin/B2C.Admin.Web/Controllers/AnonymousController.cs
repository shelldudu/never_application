using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Never;
using B2C.Calc.Banks;
using B2C.Calc;
using Never.Commands;
using Never.DataAnnotations;
using Never.Web.Mvc;
using B2C.Infrastructure;
using B2C.Admin.Web.Models;
using B2C.Admin.Web.Permissions.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using B2C.Admin.Web.Infrastructures;

namespace B2C.Admin.Web.Controllers
{
    [AllowAnonymous]
    public class AnonymousController : AppController
    {
        #region ctor

        public AnonymousController()
        {
        }

        #endregion ctor

        #region calc

        [Validator(typeof(ModelValidator))]
        public class CalcSearchModel
        {
            #region prop

            /// <summary>
            /// 期限
            /// </summary>
            public int Duration { get; set; }

            /// <summary>
            /// 期数
            /// </summary>
            public int Period { get; set; }

            /// <summary>
            /// 计算日
            /// </summary>
            public DateTime StartTime { get; set; }

            /// <summary>
            /// 还款方式
            /// </summary>
            public Infrastructures.RepayType RepayType { get; set; }

            /// <summary>
            /// 时间单位
            /// </summary>
            public Infrastructures.TimeUnitType TimeUnitType { get; set; }

            /// <summary>
            /// 利率单位
            /// </summary>
            public Infrastructures.RateType RateType { get; set; }

            /// <summary>
            /// 利率
            /// </summary>
            public decimal Rate { get; set; }

            /// <summary>
            /// 本金
            /// </summary>
            public decimal Principal { get; set; }

            /// <summary>
            /// 修复本金小数点
            /// </summary>
            public bool FixPrincipalPoint { get; set; }

            /// <summary>
            /// 平均还款本金次数
            /// </summary>
            public int AvgPrincipalPeriod { get; set; }

            #endregion prop

            #region ctor

            public CalcSearchModel()
            {
                this.StartTime = DateTime.Now;
                this.Period = 1;
                this.Principal = 10000m;
                this.Rate = 10;
                this.RepayType = Infrastructures.RepayType.一次性还本付息;
                this.TimeUnitType = Infrastructures.TimeUnitType.天;
                this.RateType = Infrastructures.RateType.年;
                this.Duration = 30;
                this.AvgPrincipalPeriod = 1;
            }

            #endregion ctor

            #region validator

            private class ModelValidator : Validator<CalcSearchModel>
            {
                public override IEnumerable<KeyValuePair<Expression<Func<CalcSearchModel, object>>, string>> RuleFor(CalcSearchModel target)
                {
                    if (target.Period <= 0)
                        yield return new KeyValuePair<Expression<Func<CalcSearchModel, object>>, string>(m => m.Period, "期数小于0");

                    if (target.Duration <= 0)
                        yield return new KeyValuePair<Expression<Func<CalcSearchModel, object>>, string>(m => m.Duration, "期限小于0");

                    if (target.Rate <= 0)
                        yield return new KeyValuePair<Expression<Func<CalcSearchModel, object>>, string>(m => m.Rate, "利率小于0");

                    if (target.Principal <= 0)
                        yield return new KeyValuePair<Expression<Func<CalcSearchModel, object>>, string>(m => m.Rate, "本金小于0");

                    if (!Enum.IsDefined(typeof(Infrastructures.RepayType), (byte)target.RepayType))
                        yield return new KeyValuePair<Expression<Func<CalcSearchModel, object>>, string>(m => m.Rate, "还款方式不在预定义中");

                    if (!Enum.IsDefined(typeof(Infrastructures.TimeUnitType), (byte)target.TimeUnitType))
                        yield return new KeyValuePair<Expression<Func<CalcSearchModel, object>>, string>(m => m.Rate, "时间单位不在预定义中");

                    if (!Enum.IsDefined(typeof(Infrastructures.RateType), (byte)target.RateType))
                        yield return new KeyValuePair<Expression<Func<CalcSearchModel, object>>, string>(m => m.Rate, "利率单位不在预定义中");
                }
            }

            #endregion validator
        }

        [HttpGet, AllowAnonymous]
        public ActionResult CalcList()
        {
            return this.View(new CalcSearchModel() { });
        }

        [HttpPost, AllowAnonymous]
        public ActionResult CalcList(CalcSearchModel model)
        {
            if (!this.TryValidateModel(model))
                return this.Json(ApiStatus.Error, string.Empty, this.ModelErrorMessage);
   
            IEnumerable<RepaymentModel> list = null;
            try
            {
                var calc = model.RepayType.GetCalc(model.TimeUnitType, model.StartTime, model.Period);
                list = calc.GetRepayments(model.Principal,(RepaymentRateType)model.RateType, model.Rate / 100m, model.Duration, model.Period, model.FixPrincipalPoint);
            }
            catch (Exception ex)
            {
                return this.Json(ApiStatus.Error, ex.GetInnerException().Message);
            }

            var page = new PagedData<RepaymentModel>(1, model.Period, list);
            var grid = new JsonGridModel()
            {
                TotalCount = page.TotalCount,
                PageNow = page.PageNow,
                PageSize = page.TotalCount,
                Records = page.Records.Select(r =>
                {
                    r.Interest = r.Interest.FormatC(2);
                    r.Principal = r.Principal.FormatC(2);
                    return r;
                }),
            };

            return this.Json(ApiStatus.Success, grid);
        }

        #endregion calc

    }
}