using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Never;
using Never.Commands;
using Never.Exceptions;
using Never.Logging;
using Never.Security;
using Never.Utils;
using Never.Web.Mvc;
using Never.Web.Mvc.Attributes;
using B2C.Admin.Web.Models;
using B2C.Admin.Web.Permissions;
using B2C.Admin.Web.Permissions.Attributes;
using B2C.Admin.Web.Permissions.Commands;
using B2C.Admin.Web.Permissions.EnumTypes;
using B2C.Admin.Web.Permissions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2C.Admin.Web.Controllers
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [UserOperationAuthorize]
    public partial class PermissionController : AppController
    {
        #region field

        private readonly ICommandBus commandBus = null;
        private readonly ILoggerBuilder loggerBuilder = null;
        private readonly IPermissionQuery permissionQuery = null;

        public PermissionController(ICommandBus commandBus,
            ILoggerBuilder loggerBuilder,
            IPermissionQuery permissionQuery)
        {
            this.commandBus = commandBus;
            this.loggerBuilder = loggerBuilder;
            this.permissionQuery = permissionQuery;
        }

        #endregion ctor

        #region 检查当前用户的操作权限

        /// <summary>
        /// 检查当前用户的操作权限
        /// </summary>
        private string CheckCurrentOperator(GroupSort targetSort)
        {
            return CheckCurrentOperator(targetSort, this.Me);
        }

        /// <summary>
        /// 检查当前用户的操作权限
        /// </summary>
        private string CheckCurrentOperator(GroupSort targetSort, AppUser current)
        {
            if (current == null)
                return "你当前登陆信息为找到";

            if (current.GroupSort >= targetSort)
                return null;

            return "你当前登陆权限无法操作该资源";
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public AppUser Me
        {
            get
            {
                return (AppUser)this.CurrentUser;
            }
        }
        #endregion

        #region 部门与列表

        /// <summary>
        /// 新增部门
        /// </summary>
        /// <returns></returns>
        [SuperActionResource("c60b463b7f3d17c8c60000d7", "【权限管理】【读】打开添加部门"), HttpGet]
        public ActionResult AddDepartment()
        {
            return this.View(new DepartmentModel());
        }

        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SuperActionResource("c60b463b7f3d17c8c60000d8", "【权限管理】【写】添加新的部门"), HttpPost]
        public ActionResult AddDepartment(DepartmentModel model)
        {
            if (model.Name.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Name", "部门名字为空");
                return this.View(model);
            }

            if (model.Descn.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Descn", "部门描述为空");
                return this.View(model);
            }

            if (this.permissionQuery.CountUsingDepartmentName(model.Name) > 0)
            {
                this.ModelState.AddModelError("Descn", "部门已经存在");
                return this.View(model);
            }

            var check = this.CheckCurrentOperator(GroupSort.Super);
            if (check.IsNotNullOrEmpty())
            {
                this.ModelState.AddModelError("Name", check);
                return this.View(model);
            }

            try
            {
                var command = new CreateDepartmentCommand(NewId.GenerateString(NewId.StringLength.L24))
                {
                    Name = model.Name,
                    Descn = model.Descn
                };

                var handler = this.commandBus.Send(command);
                if (handler.Ok())
                    return this.RedirectToAction("EditDepartment", new { aggregateId = command.AggregateId });
            }
            catch (Exception dex)
            {
                this.ModelState.AddModelError("Name", dex.GetMessage());
            }

            return this.View(model);
        }

        /// <summary>
        /// 编辑部门
        /// </summary>
        /// <returns></returns>
        [SuperActionResource("c60b463b7f3d17c8c60000d9", "【权限管理】【读】打开编辑部门"), HttpGet]
        public ActionResult EditDepartment(string aggregateId)
        {
            if (aggregateId.IsNullOrWhiteSpace())
                return this.RedirectToAction("DepartmentList");

            var department = this.permissionQuery.GetDepartmentUsingAggId(aggregateId);
            if (department == null)
                return this.RedirectToAction("DepartmentList");

            return this.View(department);
        }

        /// <summary>
        /// 编辑部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SuperActionResource("c60b463b7f3d17c8c60000da", "【权限管理】【写】更新部门信息"), HttpPost]
        public ActionResult EditDepartment(DepartmentModel model)
        {
            if (model.Name.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Name", "部门名字为空");
                return this.View(model);
            }

            if (model.Descn.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Descn", "部门描述为空");
                return this.View(model);
            }

            var check = this.CheckCurrentOperator(GroupSort.Super);
            if (check.IsNotNullOrEmpty())
            {
                this.ModelState.AddModelError("Name", check);
                return this.View(model);
            }

            try
            {
                var handler = this.commandBus.Send(new ChangeDepartmentInfoCommand(model.AggregateId)
                {
                    Name = model.Name,
                    Descn = model.Descn,
                    Version = model.Version,
                });

                if (handler.Ok())
                    return this.RedirectToAction("EditDepartment", new { aggregateId = model.AggregateId });
            }
            catch (DomainException dex)
            {
                this.ModelState.AddModelError("Name", dex.GetMessage());
            }
            catch (SystemBusyException sysex)
            {
                this.ModelState.AddModelError("Name", sysex.GetMessage());
            }

            return this.View(model);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <returns></returns>
        [SuperActionResource("c60b463b7f3d17c8c60000dd", "【权限管理】【重要】删除部门"), HttpPost]
        public ActionResult DeleteDepartment(string aggregateId)
        {
            var check = this.CheckCurrentOperator(GroupSort.Super);
            if (check.IsNotNullOrEmpty())
            {
                return this.Json(ApiStatus.Fail, string.Empty, check);
            }

            try
            {
                var handler = this.commandBus.Send(new RemoveDepartmentCommand(aggregateId)
                {
                });

                if (handler.Ok())
                    return this.RedirectToAction("DepartmentList");
            }
            catch (Exception dex)
            {
                this.ModelState.AddModelError("Name", dex.GetMessage());
            }

            return this.Json(ApiStatus.Fail, string.Empty, this.ModelErrorMessage);
        }

        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        [SuperActionResource("c60b463b7f3d17c8c60000db", "【权限管理】【读】打开部门列表"), HttpGet]
        public ActionResult DepartmentList()
        {
            var model = new PagedSearch();
            return this.View(model);
        }

        /// <summary>
        /// 部门列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SuperActionResource("c60b463b7f3d17c8c60000dc", "【权限管理】【刷新】部门列表"), HttpPost]
        public ActionResult DepartmentList(PagedSearch model)
        {
            var departments = this.permissionQuery.GetPageDeparement(model);
            if (departments == null)
                departments = new PagedData<DepartmentModel>();

            var grid = new JsonGridModel()
            {
                TotalCount = departments.TotalCount,
                PageNow = departments.PageNow,
                PageSize = departments.PageSize,
                Records = departments.Records
            };

            return this.Json(ApiStatus.Success, grid);
        }

        /// <summary>
        /// 编辑小组
        /// </summary>
        /// <returns></returns>
        [SuperActionResource("c60b463b7f3d17c8c60000f3", "【权限管理】【重要】打开部门权限资源"), HttpGet]
        public ActionResult EditDepartmentResource(string aggregateId)
        {
            if (aggregateId.IsNullOrWhiteSpace())
                return this.RedirectToAction("DepartmentList");

            var department = this.permissionQuery.GetDepartmentUsingAggId(aggregateId);
            if (department == null)
            {
                this.ModelState.AddModelError("DepartmentId", "找不到该部门");
                return this.View(department);
            }

            var resources = this.permissionQuery.GetDepartmentOwnerResource(department.AggregateId);
            ViewBag.Resources = resources;

            return this.View(department);
        }

        /// <summary>
        /// 编辑员工所属小组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SuperActionResource("c60b463b7f3d17c8c60000f4", "【权限管理】【重要】对部门分配资源"), HttpPost]
        public ActionResult EditDepartmentResource(DepartmentModel model)
        {
            var department = this.permissionQuery.GetDepartmentUsingAggId(model.AggregateId);
            if (department == null)
                return this.Json(ApiStatus.Fail, string.Empty, "找不到该部门");

            var splits = (model.Descn ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (splits.IsNullOrEmpty())
            {
                //删除所关联的所有资源
                var handler = this.commandBus.Send(new DistributeDepartmentResourceCommand()
                {
                    DepartmentId = model.AggregateId,
                    ResourceId = new string[0]
                });

                if (!handler.Ok())
                {
                    return this.Json(ApiStatus.Fail, string.Empty, this.HandlerMerssage(handler, "删除部门关联资源的操作失败"));
                }

                return this.Json(ApiStatus.Success, string.Empty);
            }

            if (splits.Any())
            {
                var handler = this.commandBus.Send(new DistributeDepartmentResourceCommand()
                {
                    DepartmentId = model.AggregateId,
                    ResourceId = splits
                });

                if (!handler.Ok())
                {
                    return this.Json(ApiStatus.Fail, string.Empty, this.HandlerMerssage(handler, "删除部门关联资源的操作失败"));
                }

                return this.Json(ApiStatus.Success, string.Empty);
            }

            return this.Json(ApiStatus.Fail, string.Empty, "更新部门关联资源的操作成功");
        }

        #endregion 部门与列表

        #region 小组与列表

        /// <summary>
        /// 新增小组
        /// </summary>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000de", "【权限管理】【读】打开添加小组"), HttpGet]
        public ActionResult AddGroup()
        {
            if (this.Me.GroupSort != GroupSort.Super)
            {
                return this.View(new GroupModel() { DepartmentId = this.Me.DepartmentId });
            }

            return this.View(new GroupModel() { });
        }

        /// <summary>
        /// 新增小组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000df", "【权限管理】【写】添加新的小组"), HttpPost]
        public ActionResult AddGroup(GroupModel model)
        {
            if (model.Name.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Name", "小组名字为空");
                return this.View(model);
            }

            if (model.Descn.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Descn", "小组描述为空");
                return this.View(model);
            }

            if (this.permissionQuery.CountUsingGroupName(model.Name) > 0)
            {
                this.ModelState.AddModelError("Descn", "小组已经存在");
                return this.View(model);
            }

            var check = this.CheckCurrentOperator(GroupSort.Leader);
            if (check.IsNotNullOrEmpty())
            {
                this.ModelState.AddModelError("Name", check);
                return this.View(model);
            }

            if (this.Me.GroupSort != GroupSort.Super && model.DepartmentId != this.Me.DepartmentId)
            {
                this.ModelState.AddModelError("DepartmentId", "是猴子叫你改部门信息吗");
                return this.View(model);
            }

            try
            {
                var command = new CreateGroupCommand(NewId.GenerateString(NewId.StringLength.L24))
                {
                    Name = model.Name,
                    Descn = model.Descn,
                    DepartmentId = model.DepartmentId,
                };

                var handler = this.commandBus.Send(command);
                if (handler.Ok())
                    return this.RedirectToAction("EditGroup", new { aggregateId = command.AggregateId });
            }
            catch (Exception dex)
            {
                this.ModelState.AddModelError("Name", dex.GetMessage());
            }

            return this.View(model);
        }

        /// <summary>
        /// 编辑小组
        /// </summary>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000e0", "【权限管理】【读】打开编辑小组"), HttpGet]
        public ActionResult EditGroup(string aggregateId)
        {
            if (aggregateId.IsNullOrWhiteSpace())
                return this.RedirectToAction("GroupList");

            var group = this.permissionQuery.GetGroupUsingAggId(aggregateId);
            if (group == null)
                return this.RedirectToAction("GroupList");

            if (this.Me.GroupSort != GroupSort.Super && group.DepartmentId != this.Me.DepartmentId)
                return this.RedirectToAction("GroupList");

            var department = this.permissionQuery.GetDepartmentUsingAggId(group.DepartmentId);
            group.DepartmentId = department == null ? group.DepartmentId : department.Name;

            return this.View(group);
        }

        /// <summary>
        /// 编辑小组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000e1", "【权限管理】【写】更新小组信息"), HttpPost]
        public ActionResult EditGroup(GroupModel model)
        {
            if (model.Name.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Name", "小组名字为空");
                return this.View(model);
            }

            if (model.Descn.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Descn", "小组描述为空");
                return this.View(model);
            }

            if (this.Me.GroupSort != GroupSort.Super && model.DepartmentId != this.Me.DepartmentId)
            {
                this.ModelState.AddModelError("DepartmentId", "你不能修改别人部门的小组信息");
                return this.View(model);
            }

            var check = this.CheckCurrentOperator(GroupSort.Leader);
            if (check.IsNotNullOrEmpty())
            {
                this.ModelState.AddModelError("Name", check);
                return this.View(model);
            }

            try
            {
                var handler = this.commandBus.Send(new ChangeGroupInfoCommand(model.AggregateId)
                {
                    Name = model.Name,
                    Descn = model.Descn,
                    Version = model.Version,
                });

                if (handler.Ok())
                    return this.RedirectToAction("EditGroup", new { aggregateId = model.AggregateId });
            }
            catch (DomainException dex)
            {
                this.ModelState.AddModelError("Name", dex.GetMessage());
            }
            catch (SystemBusyException sysex)
            {
                this.ModelState.AddModelError("Name", sysex.GetMessage());
            }

            return this.View(model);
        }

        /// <summary>
        /// 删除小组
        /// </summary>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000e2", "【权限管理】【重要】删除小组"), HttpPost]
        public ActionResult DeleteGroup(string aggregateId)
        {
            var check = this.CheckCurrentOperator(GroupSort.Leader);
            if (check.IsNotNullOrEmpty())
            {
                return this.Json(ApiStatus.Fail, string.Empty, check);
            }

            var group = this.permissionQuery.GetGroupUsingAggId(aggregateId);
            if (group == null)
            {
                return this.Json(ApiStatus.Fail, string.Empty, "删除成功");
            }

            if (this.Me.GroupSort != GroupSort.Super && group.DepartmentId != this.Me.DepartmentId)
            {
                return this.Json(ApiStatus.Fail, string.Empty, "你不能修改别人部门的小组信息");
            }

            try
            {
                var handler = this.commandBus.Send(new RemoveGroupCommand(aggregateId)
                {
                });

                if (handler.Ok())
                    return this.RedirectToAction("GroupList");
            }
            catch (Exception dex)
            {
                this.ModelState.AddModelError("Name", dex.GetMessage());
            }

            return this.Json(ApiStatus.Fail, string.Empty, this.ModelErrorMessage);
        }

        /// <summary>
        /// 小组列表
        /// </summary>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000e6", "【权限管理】【读】打开小组列表"), HttpGet]
        public ActionResult GroupList()
        {
            var model = new PagedSearch();
            return this.View(model);
        }

        /// <summary>
        /// 小组列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000e7", "【权限管理】【刷新】小组列表"), HttpPost]
        public ActionResult GroupList(PagedSearch model)
        {
            PagedData<GroupModel> groups = null;
            if (this.Me.GroupSort != GroupSort.Super)
            {
                groups = this.permissionQuery.GetPageGroup(model, this.Me.DepartmentId);
            }
            else
            {
                groups = this.permissionQuery.GetPageGroup(model);
            }

            if (groups == null)
                groups = new PagedData<GroupModel>();

            var grid = new JsonGridModel()
            {
                TotalCount = groups.TotalCount,
                PageNow = groups.PageNow,
                PageSize = groups.PageSize,
                Records = groups.Records
            };

            return this.Json(ApiStatus.Success, grid);
        }

        /// <summary>
        /// 编辑小组
        /// </summary>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000f2", "【权限管理】【重要】打开小组权限资源"), HttpGet]
        public ActionResult EditGroupResource(string aggregateId)
        {
            if (aggregateId.IsNullOrWhiteSpace())
                return this.RedirectToAction("GroupList");

            var group = this.permissionQuery.GetGroupUsingAggId(aggregateId);
            if (group == null)
                return this.RedirectToAction("GroupList");

            if (this.Me.GroupSort != GroupSort.Super && group.DepartmentId != this.Me.DepartmentId)
                return this.RedirectToAction("GroupList");

            var department = this.permissionQuery.GetDepartmentUsingAggId(group.DepartmentId);
            if (department == null)
            {
                this.ModelState.AddModelError("DepartmentId", "找不到该小组所属的部门");
                return this.View(group);
            }

            var resources = this.permissionQuery.GetGroupOwnerResource(department.AggregateId, group.AggregateId);
            ViewBag.Resources = resources;

            return this.View(group);
        }

        /// <summary>
        /// 编辑员工所属小组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000f5", "【权限管理】【重要】对小组分配资源"), HttpPost]
        public ActionResult EditGroupResource(GroupModel model)
        {
            var group = this.permissionQuery.GetGroupUsingAggId(model.AggregateId);
            if (group == null)
                return this.Json(ApiStatus.Fail, string.Empty, "不存在该小组");

            if (this.Me.GroupSort != GroupSort.Super && group.DepartmentId != this.Me.DepartmentId)
                return this.Json(ApiStatus.Fail, string.Empty, "你不可修改该小组资源");

            var splits = (model.Descn ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (splits.IsNullOrEmpty())
            {
                //删除所关联的所有资源
                var handler = this.commandBus.Send(new DistributeGroupResourceCommand()
                {
                    GroupId = model.AggregateId,
                    ResourceId = new string[0]
                });

                if (!handler.Ok())
                {
                    return this.Json(ApiStatus.Fail, string.Empty, this.HandlerMerssage(handler, "删除小组关联资源的操作失败"));
                }

                return this.Json(ApiStatus.Success, string.Empty);
            }

            var resources = this.permissionQuery.GetDepartmentResource(group.DepartmentId);
            if (resources.IsNullOrEmpty())
                return this.Json(ApiStatus.Fail, string.Empty, "小组的部门资源为空");

            foreach (var split in splits)
            {
                if (resources.Any(t => t.AggregateId == split))
                    continue;

                return this.Json(ApiStatus.Fail, string.Empty, string.Concat(split, "权限不是该部门下的权限资源，分配失败"));
            }

            if (splits.Any())
            {
                var handler = this.commandBus.Send(new DistributeGroupResourceCommand()
                {
                    GroupId = model.AggregateId,
                    ResourceId = splits
                });

                if (!handler.Ok())
                {
                    return this.Json(ApiStatus.Fail, string.Empty, this.HandlerMerssage(handler, "删除小组关联资源的操作失败"));
                }

                return this.Json(ApiStatus.Success, string.Empty);
            }

            return this.Json(ApiStatus.Fail, string.Empty, "更新小组关联资源的操作成功");
        }

        #endregion 小组与列表

        #region 员工与列表

        /// <summary>
        /// 新增员工
        /// </summary>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000e8", "【权限管理】【读】打开添加员工"), HttpGet]
        public ActionResult AddEmployee()
        {
            if (this.Me.GroupSort != GroupSort.Super)
            {
                return this.View(new EmployeeModel() { DepartmentId = this.Me.DepartmentId });
            }

            return this.View(new EmployeeModel());
        }

        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000e9", "【权限管理】【写】添加新的员工"), HttpPost]
        public ActionResult AddEmployee(EmployeeModel model)
        {
            if (model.UserName.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("UserName", "员工名字为空");
                return this.View(model);
            }

            if (model.Password.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Password", "员工密码为空");
                return this.View(model);
            }

            if (model.DepartmentId.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("DepartmentId", "员工部门为空");
                return this.View(model);
            }

            if (this.permissionQuery.CountUsingUserName(model.UserName) > 0)
            {
                this.ModelState.AddModelError("UserName", "员工登陆名字已经存在");
                return this.View(model);
            }

            var check = this.CheckCurrentOperator(model.GroupSort);
            if (check.IsNotNullOrEmpty())
            {
                this.ModelState.AddModelError("GroupSort", check);
                return this.View(model);
            }

            if (this.Me.GroupSort != GroupSort.Super && model.DepartmentId != this.Me.DepartmentId)
            {
                this.ModelState.AddModelError("DepartmentId", "是猴子叫你改部门信息吗");
                return this.View(model);
            }

            try
            {
                var command = new CreateEmployeeCommand(NewId.GenerateString(NewId.StringLength.L24))
                {
                    UserName = model.UserName,
                    Password = model.Password.Trim().ToSHA256(),
                    DepartmentId = model.DepartmentId,
                    GroupSort = model.GroupSort,
                    NickName = model.NickName,
                };

                var handler = this.commandBus.Send(command);
                if (handler.Ok())
                    return this.RedirectToAction("EditEmployee", new { aggregateId = command.AggregateId });
            }
            catch (Exception dex)
            {
                this.ModelState.AddModelError("UserName", dex.GetMessage());
            }

            return this.View(model);
        }

        /// <summary>
        /// 编辑员工
        /// </summary>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000ea", "【权限管理】【读】打开编辑员工"), HttpGet]
        public ActionResult EditEmployee(string aggregateId)
        {
            if (aggregateId.IsNullOrWhiteSpace())
                return this.RedirectToAction("EmployeeList");

            var employee = this.permissionQuery.GetEmployeeUsingAggId(aggregateId);
            if (employee == null)
                return this.RedirectToAction("EmployeeList");

            switch (this.Me.GroupSort)
            {
                case GroupSort.Leader:
                    {
                        if (employee.DepartmentId != this.Me.DepartmentId)
                            return this.RedirectToAction("EmployeeList");
                    }
                    break;
                case GroupSort.Muggle:
                    {
                        if (this.Me.AggregateId != employee.AggregateId)
                            return this.RedirectToAction("EmployeeList");
                    }
                    break;
            }

            return this.View(employee);
        }

        /// <summary>
        /// 编辑员工
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000eb", "【权限管理】【写】更新员工信息"), HttpPost]
        public ActionResult EditEmployee(EmployeeModel model)
        {
            var employee = this.permissionQuery.GetEmployeeUsingAggId(model.AggregateId);
            if (employee == null)
            {
                this.ModelState.AddModelError("UserName", "不存在该员工");
                return this.View(model);
            }

            if (this.Me.GroupSort != GroupSort.Super && model.DepartmentId != this.Me.DepartmentId)
            {
                this.ModelState.AddModelError("DepartmentId", "你不能修改别人部门的员工信息");
                return this.View(model);
            }

            ICommandHandlerResult handler = null;
            if (employee.NickName.IsNotEquals(model.NickName))
            {
                handler = this.commandBus.Send(new ChangeEmployeeNickNameCommand(model.AggregateId)
                {
                    NickName = model.NickName,
                });

                if (!handler.Ok())
                {
                    this.ModelState.AddModelError("NickName", this.HandlerMerssage(handler, "更新昵称失败"));
                    return this.View(model);
                }
            }

            if (employee.GroupSort != model.GroupSort)
            {
                if (this.Me.AggregateId == model.AggregateId)
                {
                    this.ModelState.AddModelError("GroupSort", "你不可以修改自己的身份");
                    return this.View(model);
                }

                var check = this.CheckCurrentOperator(model.GroupSort);
                if (check.IsNotNullOrEmpty())
                {
                    this.ModelState.AddModelError("GroupSort", check);
                    return this.View(model);
                }

                handler = this.commandBus.Send(new ChangeEmployeeOwnerCommand(model.AggregateId)
                {
                    DepartmentId = employee.DepartmentId,
                    GroupSort = model.GroupSort
                });

                if (!handler.Ok())
                {
                    this.ModelState.AddModelError("GroupSort", this.HandlerMerssage(handler, "更新员工身份失败"));
                    return this.View(model);
                }
            }

            return this.View(model);
        }

        /// <summary>
        /// 编辑员工密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000ef", "【权限管理】【重要】更新员工密码"), HttpPost]
        public ActionResult EditEmployeePassword(EmployeeModel model)
        {
            if (model.Password.IsNullOrWhiteSpace())
            {
                return this.Json(ApiStatus.Fail, string.Empty, "密码为空");
            }

            var employee = this.permissionQuery.GetEmployeeUsingAggId(model.AggregateId);
            if (employee == null)
            {
                return this.Json(ApiStatus.Fail, string.Empty, "不存在该员工");
            }

            if (this.Me.GroupSort != GroupSort.Super && model.DepartmentId != this.Me.DepartmentId)
            {
                this.ModelState.AddModelError("DepartmentId", "你不能修改别人部门的员工信息");
                return this.View(model);
            }

            if (employee.Password.IsNotEquals(model.Password.Trim().ToSHA256()))
            {
                var handler = this.commandBus.Send(new ChangeEmployeePasswordCommand(model.AggregateId)
                {
                    Password = model.Password.Trim().ToSHA256(),
                });

                if (!handler.Ok())
                {
                    return this.Json(ApiStatus.Fail, string.Empty, this.HandlerMerssage(handler, "更新密码失败"));
                }
            }

            return this.Json(ApiStatus.Success, string.Empty, "更新密码成功");
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000ec", "【权限管理】【重要】删除员工"), HttpPost]
        public ActionResult DeleteEmployee(string aggregateId)
        {
            var employee = this.permissionQuery.GetEmployeeUsingAggId(aggregateId);
            if (employee == null)
            {
                return this.Json(ApiStatus.Fail, string.Empty, "不存在该员工");
            }

            if (this.Me.GroupSort != GroupSort.Super && employee.DepartmentId != this.Me.DepartmentId)
            {
                return this.Json(ApiStatus.Fail, string.Empty, "你不能修改别人部门的小组信息");
            }

            var check = this.CheckCurrentOperator(employee.GroupSort);
            if (check.IsNotNullOrEmpty())
            {
                return this.Json(ApiStatus.Fail, string.Empty, check);
            }

            try
            {
                var handler = this.commandBus.Send(new RemoveEmployeeCommand(aggregateId)
                {
                });

                if (handler.Ok())
                    return this.RedirectToAction("EmployeeList");
            }
            catch (Exception dex)
            {
                this.ModelState.AddModelError("Name", dex.GetMessage());
            }

            return this.Json(ApiStatus.Fail, string.Empty, this.ModelErrorMessage);
        }

        /// <summary>
        /// 员工列表
        /// </summary>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000ed", "【权限管理】【读】打开员工列表"), HttpGet]
        public ActionResult EmployeeList()
        {
            var model = new PagedSearch();
            return this.View(model);
        }

        /// <summary>
        /// 员工列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000ee", "【权限管理】【刷新】员工列表"), HttpPost]
        public ActionResult EmployeeList(PagedSearch model)
        {
            PagedData<EmployeeModel> employees = null;
            if (this.Me.GroupSort != GroupSort.Super)
            {
                employees = this.permissionQuery.GetPageEmployee(model, this.Me.DepartmentId);
            }
            else
            {
                employees = this.permissionQuery.GetPageEmployee(model);
            }

            if (employees == null)
                employees = new PagedData<EmployeeModel>();

            var alldepartments = this.permissionQuery.GetAllDepartment();
            if (alldepartments != null)
            {
                if (employees.Records != null)
                {
                    foreach (var i in employees.Records)
                    {
                        if (i.GroupSort == GroupSort.Super)
                        {
                            if (i.AggregateId != this.Me.AggregateId)
                            {
                                i.Id = -1;
                                i.AggregateId = "";
                                i.UserName = "********";
                                i.NickName = "地球很危险";
                                i.DepartmentId = "神秘部门";
                            }
                        }
                        else
                        {
                            i.DepartmentId = alldepartments.Any(t => t.AggregateId == i.DepartmentId) ? alldepartments.First(t => t.AggregateId == i.DepartmentId).Name : "未知部门";
                        }
                    }
                }
            }
            else if (employees.Records != null)
            {
                foreach (var i in employees.Records)
                {

                    if (i.GroupSort == GroupSort.Super)
                    {
                        if (i.AggregateId != this.Me.AggregateId)
                        {
                            i.Id = -1;
                            i.AggregateId = "";
                            i.UserName = "********";
                            i.NickName = "地球很危险";
                            i.DepartmentId = "神秘部门";
                        }
                    }
                    else
                    {
                        i.DepartmentId = alldepartments.Any(t => t.AggregateId == i.DepartmentId) ? alldepartments.First(t => t.AggregateId == i.DepartmentId).Name : "未知部门";
                    }
                }
            }

            var grid = new JsonGridModel()
            {
                TotalCount = employees.TotalCount,
                PageNow = employees.PageNow,
                PageSize = employees.PageSize,
                Records = employees.Records
            };

            return this.Json(ApiStatus.Success, grid);
        }

        /// <summary>
        /// 编辑员工所属小组
        /// </summary>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000f0", "【权限管理】【重要】打开员工小组"), HttpGet]
        public ActionResult EditEmployeeGroup(string aggregateId)
        {
            if (aggregateId.IsNullOrWhiteSpace())
                return this.RedirectToAction("EmployeeList");

            var employee = this.permissionQuery.GetEmployeeUsingAggId(aggregateId);
            if (employee == null)
                return this.RedirectToAction("EmployeeList");

            if (this.Me.GroupSort != GroupSort.Super && employee.DepartmentId != this.Me.DepartmentId)
                return this.RedirectToAction("EmployeeList");

            var department = this.permissionQuery.GetDepartmentUsingAggId(employee.DepartmentId);
            if (department == null)
            {
                this.ModelState.AddModelError("DepartmentId", "找不到该员工所属的部门");
                return this.View(employee);
            }

            var groups = this.permissionQuery.GetEmployeeOwnerGroup(department.AggregateId, employee.AggregateId);
            if (groups != null)
            {
                foreach (var i in groups)
                    i.DepartmentId = department.Name;
            }

            this.ViewBag.Group = groups;

            return this.View(employee);
        }

        /// <summary>
        /// 编辑员工所属小组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LeaderActionResource("c60b463b7f3d17c8c60000f1", "【权限管理】【重要】对员工分配小组"), HttpPost]
        public ActionResult EditEmployeeGroup(EmployeeModel model)
        {
            var employee = this.permissionQuery.GetEmployeeUsingAggId(model.AggregateId);
            if (employee == null)
                return this.Json(ApiStatus.Fail, string.Empty, "不存在该员工");

            if (this.Me.GroupSort != GroupSort.Super && employee.DepartmentId != this.Me.DepartmentId)
                return this.Json(ApiStatus.Fail, string.Empty, "你不可以修改该小组资源");

            var splits = (model.Password ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (splits.IsNullOrEmpty())
            {
                //删除所关联的所有小组
                var handler = this.commandBus.Send(new DistributeEmployeeGroupCommand()
                {
                    EmployeeId = model.AggregateId,
                    GroupId = new string[0]
                });

                if (!handler.Ok())
                {
                    return this.Json(ApiStatus.Fail, string.Empty, this.HandlerMerssage(handler, "删除员工关联小组的操作失败"));
                }

                return this.Json(ApiStatus.Success, string.Empty);
            }

            var groups = this.permissionQuery.GetAllGroup(employee.DepartmentId);
            if (groups.IsNullOrEmpty())
                return this.Json(ApiStatus.Fail, string.Empty, "部门的小组信息为空");

            foreach (var split in splits)
            {
                if (groups.Any(t => t.AggregateId == split))
                    continue;

                return this.Json(ApiStatus.Fail, string.Empty, string.Concat(split, "小组不是该部门下的小组资源，分配失败"));
            }

            if (splits.Any())
            {
                var handler = this.commandBus.Send(new DistributeEmployeeGroupCommand()
                {
                    EmployeeId = model.AggregateId,
                    GroupId = splits
                });

                if (!handler.Ok())
                {
                    return this.Json(ApiStatus.Fail, string.Empty, this.HandlerMerssage(handler, "删除员工关联小组的操作失败"));
                }

                return this.Json(ApiStatus.Success, string.Empty);
            }

            return this.Json(ApiStatus.Fail, string.Empty, "更新员工关联小组的操作成功");
        }

        #endregion 员工与列表
    }
}