﻿using Never.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B2C.Calc.Banks.Repayments
{
    /// <summary>
    /// 等额本金
    /// </summary>
    public class 等额本金 : RepaymentCalc, IRepaymentCalc
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="等额本金"/> class.
        /// </summary>
        /// <param name="dateType">结算类型</param>
        /// <param name="startDate">开始结算日期</param>
        public 等额本金(RepaymentTimeUnitType dateType, DateTime startDate)
            : base(dateType, startDate)
        {
        }

        #endregion ctor

        #region calc

        /// <summary>
        /// 还款列表
        /// </summary>
        /// <param name="principal">本金</param>
        /// <param name="rateType">利率计算方式</param>
        /// <param name="rate">利率</param>
        /// <param name="deadline">期限</param>
        /// <param name="periods">期数</param>
        /// <param name="autoFixPrincipalToInteger">是否为自动修正本金整数</param>
        /// <returns></returns>
        public override IEnumerable<RepaymentModel> GetRepayments(decimal principal, RepaymentRateType rateType, decimal rate, decimal deadline, int periods = 1, bool autoFixPrincipalToInteger = true)
        {
            if (periods <= 0)
                throw new DomainException("还款期数不能小于0");

            var list = new List<RepaymentModel>(periods);
            var avg = principal / periods;
            /*不用修正本金或进要修正本金但能整除*/
            if (!autoFixPrincipalToInteger || (autoFixPrincipalToInteger && principal % periods == 0))
            {
                for (var i = 1; i <= periods; i++)
                {
                    var interest = this.Strategy.GetNUnitAmount((principal - list.Sum(o => o.Principal)) * rate * deadline, rateType) / periods;
                    var model = new RepaymentModel() { Principal = avg, Interest = interest, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, deadline * i / periods) };
                    list.Add(model);
                }
            }
            else
            {
                var fixPrincipal = principal - principal % periods;
                var firstPrinciapl = principal - (periods - 1) * fixPrincipal / periods;
                var firstModel = new RepaymentModel() { Principal = firstPrinciapl, Interest = this.Strategy.GetNUnitAmount(principal * rate * deadline, rateType) / periods, Period = 1, PayDate = this.Strategy.GetPayTime(this.StartDate, deadline * 1 / periods) };
                list.Add(firstModel);

                /*只有大于1期后才可能重新计算本金与利息*/
                if (periods > 1)
                {
                    var changeAvg = (principal - firstPrinciapl) / (periods - 1);
                    for (var i = 2; i <= periods; i++)
                    {
                        var interest = this.Strategy.GetNUnitAmount((principal - list.Sum(o => o.Principal)) * rate * deadline, rateType) / periods;
                        var model = new RepaymentModel() { Principal = changeAvg, Interest = interest, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, deadline * i / periods) };
                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion calc

        #region tostring

        /// <summary>
        /// 返回string
        /// </summary>
        public override string ToString()
        {
            return "等额本金";
        }

        #endregion tostring
    }
}