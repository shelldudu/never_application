using Never;
using Never.EasySql;
using Never.EasySql.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2C.Message.SqlData.Query.Xml
{
    /// <summary>
    /// mysql
    /// </summary>
    /// <seealso cref="Never.EasySql.EmbeddedDaoBuilder" />
    public class MySqlQueryDaoBuilder : EmbeddedDaoBuilder
    {
        private readonly Func<string> connstring = null;

        public MySqlQueryDaoBuilder(Func<string> connstring)
        {
            this.connstring = connstring;
        }

        /// <summary>
        /// 返回数据库名字
        /// </summary>
        public override string ConnectionString { get { return this.connstring(); } }

        /// <summary>
        /// 开启连接
        /// </summary>
        /// <returns></returns>
        protected override IEasySqlExecuter CreateSqlExecuter()
        {
            return new MySqlExecuter(this.ConnectionString);
        }

        /// <summary>
        /// 获取所有的SqlMap文件
        /// </summary>
        public override string[] EmbeddedSqlMaps
        {
            get
            {
                var maps = base.GetXmlContentFromAssembly(System.Reflection.Assembly.GetExecutingAssembly(), (x, f) =>
                {
                    var id = x["namespace"].GetAttribute("id");
                    if (id.IsEquals("mysql.query", StringComparison.CurrentCultureIgnoreCase))
                        return true;

                    return false;
                });

                return maps;
            }
        }
    }
}
