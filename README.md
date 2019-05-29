# 使用never框架开发的一整套web，app接口，业务接口的demo

# 项目组织架构
- admin 一个带简单的权限管理后台，
- admin.taskschd 使用webhost寄宿的netcore程序，可运行job等业务
- eqsyconfig 简单的配置中心，当app_data文件下的文件被修改时，可重新组织配置文件下发到不同的业务api，省去每个api繁琐的配置文件
- message.api 一个简单的api业务，使用cqrs开发，用于帮助理解ddd
- app.api 一个对接app的数据接口，同时对message.api等后台业务的综合调用，其中调用api会支持熔断机制
- app.host 对app.api的转发数据，同时在此支持ids身份验证

# 构架设计
<a target="_blank" rel="never" href="https://raw.githubusercontent.com/shelldudu/never_application/master/doc/b2c_app.png"><img src="https://raw.githubusercontent.com/shelldudu/never_application/master/doc/b2c_app.png" alt="alt tag" style="max-width:100%;"></a>


# 代码组织
<a target="_blank" rel="never" href="https://raw.githubusercontent.com/shelldudu/never_application/master/doc/b2c_app_start.png"><img src="https://raw.githubusercontent.com/shelldudu/never_application/master/doc/b2c_app_start.png" alt="alt tag" style="max-width:100%;"></a>