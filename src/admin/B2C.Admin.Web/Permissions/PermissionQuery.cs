using Never;
using Never.EasySql;
using Never.IoC;
using B2C.Admin.Web.Permissions.EnumTypes;
using B2C.Admin.Web.Permissions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions
{
    [SingletonAutoInjecting("admin", typeof(IPermissionQuery))]
    internal class PermissionQuery : IPermissionQuery
    {
        private readonly IDaoBuilder daoBuilder = null;
        public PermissionQuery(QueryDaoBuilder daoBuilder) { this.daoBuilder = daoBuilder; }

        public int CountUsingDepartmentName(string departmentName)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { Name = departmentName.ToNullableParameter() }).QueryForObject<int>("qryDepartmentRootCount");
        }

        public int CountUsingGroupName(string groupName)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { Name = groupName.ToNullableParameter() }).QueryForObject<int>("qryGroupRootCount");
        }

        public int CountUsingUserName(string userName)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { UserName = userName.ToNullableParameter() }).QueryForObject<int>("qryEmployeeRootCount");
        }

        public IEnumerable<DepartmentModel> GetAllDepartment()
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { }).QueryForEnumerable<DepartmentModel>("qryDepartmentRoot");
        }

        public IEnumerable<EmployeeModel> GetAllEmployee(string groupAggId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { GroupId = groupAggId.ToNullableParameter() }).QueryForEnumerable<EmployeeModel>("qryEmployeeRootUsingGroup");
        }

        public IEnumerable<GroupModel> GetAllGroup(string departAggId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { DepartmentId = departAggId.ToNullableParameter() }).QueryForEnumerable<GroupModel>("qryGroupRoot");
        }
        public PagedData<GroupModel> GetPageGroup(PagedSearch scout, string departAggId)
        {
            var para = new
            {
                StartIndex = scout.StartIndex,
                EndIndex = scout.EndIndex,
                PageSize = scout.PageSize,
                PageNow = scout.PageNow,
                DepartmentId = departAggId.ToNullableParameter()
            };

            var totalCount = this.daoBuilder.Build().ToEasyXmlDao(para).QueryForObject<int>("qryGroupRootCount");
            if (totalCount <= 0)
                return new PagedData<GroupModel>(scout.PageNow, scout.PageSize, 0, new GroupModel[0]);

            return new PagedData<GroupModel>(scout.PageNow, scout.PageSize, totalCount, this.daoBuilder.Build().ToEasyXmlDao(para).QueryForEnumerable<GroupModel>("qry_GroupRoot"));

        }
        public IEnumerable<ResourceModel> GetAllResource(GroupSort flag)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { GroupSort = (int)flag }).QueryForEnumerable<ResourceModel>("qryResourceRoot");
        }
        public IEnumerable<ResourceModel> GetAllResource(GroupSort[] flag)
        {
            if (flag.IsNullOrEmpty())
                return new ResourceModel[0];

            return this.daoBuilder.Build().ToEasyXmlDao(new { GroupSortArray = flag.Select(t => (int)t) }).QueryForEnumerable<ResourceModel>("qryResourceRoot");
        }

        public IEnumerable<ResourceModel> GetDepartmentResource(string departmentId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { DepartmentId = departmentId.ToNullableParameter() }).QueryForEnumerable<ResourceModel>("qryResourceRootOfDepartment");
        }

        public IEnumerable<OwnerResourceModel> GetDepartmentOwnerResource(string departmentId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { DepartmentId = departmentId.ToNullableParameter() }).QueryForEnumerable<OwnerResourceModel>("qryDepartmentOwnerResourceRoot");
        }
        public IEnumerable<ResourceModel> GetEmployeeResource(string employeeId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { EmployeeId = employeeId.ToNullableParameter() }).QueryForEnumerable<ResourceModel>("qryEmployeeResourceRoot");
        }

        public IEnumerable<OwnerGroupModel> GetEmployeeOwnerGroup(string departmentId, string employeeId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new
            {
                DepartmentId = departmentId.ToNullableParameter(),
                EmployeeId = employeeId.ToNullableParameter()
            }).QueryForEnumerable<OwnerGroupModel>("qryEmployeeOwnerGroupRoot");
        }
        public IEnumerable<ResourceModel> GetGroupResource(string groupId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { GroupId = groupId.ToNullableParameter() }).QueryForEnumerable<ResourceModel>("qryResourceRootOfGroup");
        }

        public IEnumerable<OwnerResourceModel> GetGroupOwnerResource(string departmentId, string groupId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new
            {
                DepartmentId = departmentId.ToNullableParameter(),
                GroupId = groupId.ToNullableParameter()
            }).QueryForEnumerable<OwnerResourceModel>("qryGroupOwnerResourceRoot");
        }

        public DepartmentModel GetDepartmentUsingAggId(string aggregateId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = aggregateId.ToNullableParameter() }).QueryForObject<DepartmentModel>("qryDepartmentRoot");
        }

        public PagedData<DepartmentModel> GetPageDeparement(PagedSearch scout)
        {
            var para = new
            {
                StartIndex = scout.StartIndex,
                EndIndex = scout.EndIndex,
                PageSize = scout.PageSize,
                PageNow = scout.PageNow,
            };

            var totalCount = this.daoBuilder.Build().ToEasyXmlDao(para).QueryForObject<int>("qryDepartmentRootCount");
            if (totalCount <= 0)
                return new PagedData<DepartmentModel>(scout.PageNow, scout.PageSize, 0, new DepartmentModel[0]);

            return new PagedData<DepartmentModel>(scout.PageNow, scout.PageSize, totalCount, this.daoBuilder.Build().ToEasyXmlDao(para).QueryForEnumerable<DepartmentModel>("qry_DepartmentRoot"));

        }

        public EmployeeModel GetEmployeeUsingAggId(string aggregateId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = aggregateId.ToNullableParameter() }).QueryForObject<EmployeeModel>("qryEmployeeRoot");
        }

        public EmployeeModel GetEmployeeUsingName(string userName)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { UserName = userName.ToNullableParameter() }).QueryForObject<EmployeeModel>("qryEmployeeRoot");
        }

        public PagedData<EmployeeModel> GetPageEmployee(PagedSearch scout)
        {
            var para = new
            {
                StartIndex = scout.StartIndex,
                EndIndex = scout.EndIndex,
                PageSize = scout.PageSize,
                PageNow = scout.PageNow,
            };

            var totalCount = this.daoBuilder.Build().ToEasyXmlDao(para).QueryForObject<int>("qryEmployeeRootCount");
            if (totalCount <= 0)
                return new PagedData<EmployeeModel>(scout.PageNow, scout.PageSize, 0, new EmployeeModel[0]);

            return new PagedData<EmployeeModel>(scout.PageNow, scout.PageSize, totalCount, this.daoBuilder.Build().ToEasyXmlDao(para).QueryForEnumerable<EmployeeModel>("qry_EmployeeRoot"));
        }

        public PagedData<EmployeeModel> GetPageEmployee(PagedSearch scout, string departmentId)
        {
            var para = new
            {
                StartIndex = scout.StartIndex,
                EndIndex = scout.EndIndex,
                PageSize = scout.PageSize,
                PageNow = scout.PageNow,
                DepartmentId = departmentId.ToNullableParameter(),
            };

            var totalCount = this.daoBuilder.Build().ToEasyXmlDao(para).QueryForObject<int>("qryEmployeeRootCount");
            if (totalCount <= 0)
                return new PagedData<EmployeeModel>(scout.PageNow, scout.PageSize, 0, new EmployeeModel[0]);

            return new PagedData<EmployeeModel>(scout.PageNow, scout.PageSize, totalCount, this.daoBuilder.Build().ToEasyXmlDao(para).QueryForEnumerable<EmployeeModel>("qry_EmployeeRoot"));

        }

        public GroupModel GetGroupUsingAggId(string aggregateId)
        {
            return this.daoBuilder.Build().ToEasyXmlDao(new { AggregateId = aggregateId.ToNullableParameter() }).QueryForObject<GroupModel>("qryGroupRoot");
        }

        public PagedData<GroupModel> GetPageGroup(PagedSearch scout)
        {
            var para = new
            {
                StartIndex = scout.StartIndex,
                EndIndex = scout.EndIndex,
                PageSize = scout.PageSize,
                PageNow = scout.PageNow,
            };

            var totalCount = this.daoBuilder.Build().ToEasyXmlDao(para).QueryForObject<int>("qryGroupRootCount");
            if (totalCount <= 0)
                return new PagedData<GroupModel>(scout.PageNow, scout.PageSize, 0, new GroupModel[0]);

            return new PagedData<GroupModel>(scout.PageNow, scout.PageSize, totalCount, this.daoBuilder.Build().ToEasyXmlDao(para).QueryForEnumerable<GroupModel>("qry_GroupRoot"));
        }
    }
}