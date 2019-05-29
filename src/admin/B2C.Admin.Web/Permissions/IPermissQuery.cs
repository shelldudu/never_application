using Never;
using B2C.Admin.Web.Permissions.EnumTypes;
using B2C.Admin.Web.Permissions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Permissions
{
    /// <summary>
    /// 权限
    /// </summary>
    public interface IPermissionQuery
    {
        int CountUsingUserName(string userName);

        int CountUsingGroupName(string groupName);

        int CountUsingDepartmentName(string departmentName);

        EmployeeModel GetEmployeeUsingAggId(string aggregateId);

        EmployeeModel GetEmployeeUsingName(string userName);

        PagedData<EmployeeModel> GetPageEmployee(PagedSearch scout);

        PagedData<EmployeeModel> GetPageEmployee(PagedSearch scout,string departmentId);

        IEnumerable<EmployeeModel> GetAllEmployee(string groupAggId);

        IEnumerable<ResourceModel> GetAllResource(GroupSort flag);

        IEnumerable<ResourceModel> GetAllResource(GroupSort[] flag);

        IEnumerable<ResourceModel> GetDepartmentResource(string departmentId);

        IEnumerable<OwnerResourceModel> GetDepartmentOwnerResource(string departmentId);

        IEnumerable<ResourceModel> GetGroupResource(string groupId);

        IEnumerable<OwnerResourceModel> GetGroupOwnerResource(string departmentId, string groupId);

        IEnumerable<ResourceModel> GetEmployeeResource(string employeeId);

        IEnumerable<OwnerGroupModel> GetEmployeeOwnerGroup(string departmentId, string employeeId);

        GroupModel GetGroupUsingAggId(string aggregateId);

        IEnumerable<GroupModel> GetAllGroup(string departAggId);

        PagedData<GroupModel> GetPageGroup(PagedSearch scout,string departAggId);

        PagedData<GroupModel> GetPageGroup(PagedSearch scout);

        DepartmentModel GetDepartmentUsingAggId(string aggregateId);

        IEnumerable<DepartmentModel> GetAllDepartment();

        PagedData<DepartmentModel> GetPageDeparement(PagedSearch scout);
    }
}