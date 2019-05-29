using Never;
using Never.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B2C.Calc.Banks.Repayments
{
    /// <summary>
    /// 等额本息
    /// </summary>
    public class 等额本息 : RepaymentCalc, IRepaymentCalc
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="等额本息"/> class.
        /// </summary>
        /// <param name="dateType">结算类型</param>
        /// <param name="startDate">开始结算日期</param>
        public 等额本息(RepaymentTimeUnitType dateType, DateTime startDate)
            : base(dateType, startDate)
        {
            if (dateType != RepaymentTimeUnitType.月)
                throw new DomainException("等额本息只能以月单位结算");
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

            switch (this.DateType)
            {
                case RepaymentTimeUnitType.天:
                    {
                        if (deadline / 30 != periods)
                            throw new ArgumentException(string.Format("当前以天计算，共{0}天，期数应为{1}，但参数的期数却为{2}", deadline, deadline / 30, periods));
                    }
                    break;

                case RepaymentTimeUnitType.季:
                    {
                        if (deadline * 4 != periods)
                            throw new ArgumentException(string.Format("当前以季计算，共{0}季，期数应为{1}，但参数的期数却为{2}", deadline, deadline * 4, periods));
                    }
                    break;

                case RepaymentTimeUnitType.年:
                    {
                        if (deadline * 12 != periods)
                            throw new ArgumentException(string.Format("当前以年计算，共{0}年，期数应为{1}，但参数的期数却为{2}", deadline, deadline * 12, periods));
                    }
                    break;

                case RepaymentTimeUnitType.月:
                    {
                        if (deadline != periods)
                            throw new ArgumentException(string.Format("当前以月计算，共{0}月，期数应为{1}，但参数的期数却为{2}", deadline, deadline, periods));
                    }
                    break;
            }
            switch (rateType)
            {
                case RepaymentRateType.天利率:
                    {
                        rate = rate * 30;
                    }
                    break;

                case RepaymentRateType.季利率:
                    {
                        rate = rate / 4;
                    }
                    break;

                case RepaymentRateType.年利率:
                    {
                        rate = rate / 12;
                    }
                    break;

                case RepaymentRateType.月利率:
                    break;
            }

            var list = new List<RepaymentModel>(periods);
            /*算法中row第二个参数是指期数*/
            var factor = (decimal)Math.Pow((double)(1m + rate), periods);
            var avg = principal * rate * factor / (factor - 1);

            //if (periods == 1)
            //    return new RepaymentModel[] { new RepaymentModel((avg - principal * rate * this.Strategy.GetUnitAmount()) * deadline, principal * rate * this.Strategy.GetUnitAmount() * deadline, 1, this.Strategy.GetPayTime(this.StartDate, deadline)) };

            var avgd = deadline / periods;
            if (!autoFixPrincipalToInteger || (autoFixPrincipalToInteger && principal % periods == 0))
            {
                for (var i = 1; i <= periods; i++)
                {
                    var interest = this.Strategy.GetNUnitAmount((principal - list.Sum(o => o.Principal)) * rate * deadline, RepaymentRateType.月利率) / periods;
                    var model = new RepaymentModel() { Principal = (avg - interest), Interest = interest, Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, deadline * i / periods) };
                    list.Add(model);
                }
            }
            else
            {
                for (var i = 1; i <= periods; i++)
                {
                    var interest = this.Strategy.GetNUnitAmount((principal - list.Sum(o => o.Principal)) * rate * deadline, RepaymentRateType.月利率) / periods;
                    var fixPrincipal = (avg - interest).FormatC(0);
                    var model = new RepaymentModel() { Principal = fixPrincipal, Interest = interest + (avg - interest - fixPrincipal), Period = i, PayDate = this.Strategy.GetPayTime(this.StartDate, deadline * i / periods) };
                    list.Add(model);
                }
            }

            //list.Add(new RepaymentModel((avg - principal * rate) * avgd, principal * rate * avgd, 1, this.Strategy.GetPayTime(this.StartDate, avgd * 1)));

            //for (var i = 2; i <= periods; i++)
            //{
            //    var interest = (principal - list.Sum(o => o.Principal)) * rate * avgd;
            //    var model = new RepaymentModel((avg - interest) * avgd, interest * avgd, i, this.Strategy.GetPayTime(this.StartDate, avgd * i));
            //    list.Add(model);
            //}

            return list;
        }

        #endregion calc

        #region tostring

        /// <summary>
        /// 返回string
        /// </summary>
        public override string ToString()
        {
            return "等额本息";
        }

        #endregion tostring
    }
}