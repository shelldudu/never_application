using Never.EasySql;
using B2C.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2C.Message.SqlData.Query
{
    using B2C.Message.SqlData.Query.Xml;

    public class QueryDaoBuilder : IDaoBuilder
    {
        public Func<IDao> Build
        {
            get
            {
                if (this.daoBuilder != null)
                    return this.daoBuilder.Build;

                lock (this)
                {
                    if (this.daoBuilder != null)
                        return this.daoBuilder.Build;

                    this.Start();
                }

                return this.daoBuilder.Build;
            }
        }

        private readonly SqldbType sqldbType = SqldbType.sqlserver;

        private readonly Func<string> connection = null;

        private IDaoBuilder daoBuilder = null;

        public QueryDaoBuilder(SqldbType sqldbType, Func<string> connection)
        {
            this.sqldbType = sqldbType;
            this.connection = connection;
        }

        public QueryDaoBuilder Start()
        {
            switch (this.sqldbType)
            {
                case SqldbType.mysql:
                    {
                        var builder = new MySqlQueryDaoBuilder(this.connection);
                        builder.Startup();
                        this.daoBuilder = builder;
                    }
                    break;
                case SqldbType.sqlserver:
                    {
                        var builder = new SqlServiceQueryDaoBuilder(this.connection);
                        builder.Startup();
                        this.daoBuilder = builder;
                    }
                    break;
            }

            return this;
        }
    }
}

namespace B2C.Message.SqlData.Repository
{
    using B2C.Message.SqlData.Repository.Xml;

    public class RepositoryDaoBuilder : IDaoBuilder
    {
        public Func<IDao> Build
        {
            get
            {
                if (this.daoBuilder != null)
                    return this.daoBuilder.Build;

                lock (this)
                {
                    if (this.daoBuilder != null)
                        return this.daoBuilder.Build;

                    this.Start();
                }

                return this.daoBuilder.Build;
            }
        }

        private readonly SqldbType sqldbType = SqldbType.sqlserver;

        private readonly Func<string> connection = null;

        private IDaoBuilder daoBuilder = null;

        public RepositoryDaoBuilder(SqldbType sqldbType, Func<string> connection)
        {
            this.sqldbType = sqldbType;
            this.connection = connection;
        }

        public RepositoryDaoBuilder Start()
        {
            switch (this.sqldbType)
            {
                case SqldbType.mysql:
                    {
                        var builder = new MySqlRepositoryDaoBuilder(this.connection);
                        builder.Startup();
                        this.daoBuilder = builder;
                    }
                    break;
                case SqldbType.sqlserver:
                    {
                        var builder = new SqlServiceRepositoryDaoBuilder(this.connection);
                        builder.Startup();
                        this.daoBuilder = builder;
                    }
                    break;
            }

            return this;
        }
    }
}