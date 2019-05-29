use b2c_admin;

CREATE TABLE department (
    Id          INTEGER       PRIMARY KEY AUTOINCREMENT,
    AggregateId CHAR (24),
    Name        VARCHAR (30),
    Descn       VARCHAR (100),
    CreateDate  DATETIME,
    Creator     VARCHAR (20),
    EditDate    DATETIME,
    Editor      VARCHAR (20),
    Version     INTEGER
);


CREATE TABLE department_resource (
    Id           INTEGER      PRIMARY KEY AUTOINCREMENT,
    AggregateId  CHAR (24),
    DepartmentId CHAR (24),
    ResourceId   CHAR (24),
    CreateDate   DATETIME,
    Creator      VARCHAR (20),
    EditDate     DATETIME,
    Editor       VARCHAR (20),
    Version      INTEGER
);


CREATE TABLE employee (
    Id           INTEGER       PRIMARY KEY AUTOINCREMENT,
    AggregateId  CHAR (24),
    UserName     VARCHAR (20),
    Password     VARCHAR (300),
    NickName     VARCHAR (20),
    DepartmentId CHAR (24),
    GroupSort    INTEGER,
    CreateDate   DATETIME,
    Creator      VARCHAR (20),
    EditDate     DATETIME,
    Editor       VARCHAR (20),
    Version      INTEGER
);


CREATE TABLE employee_group (
    Id          INTEGER      PRIMARY KEY AUTOINCREMENT,
    AggregateId CHAR (24),
    EmployeeId  CHAR (24),
    GroupId     CHAR (24),
    CreateDate  DATETIME,
    Creator     VARCHAR (20),
    EditDate    DATETIME,
    Editor      VARCHAR (20),
    Version     INTEGER
);


CREATE TABLE [group] (
    Id           INTEGER       PRIMARY KEY AUTOINCREMENT,
    AggregateId  CHAR (24),
    Name         VARCHAR (30),
    Descn        VARCHAR (100),
    DepartmentId CHAR (24),
    CreateDate   DATETIME,
    Creator      VARCHAR (20),
    EditDate     DATETIME,
    Editor       VARCHAR (24),
    Version      INTEGER
);

CREATE TABLE group_resource (
    Id          INTEGER      PRIMARY KEY AUTOINCREMENT,
    AggregateId CHAR (24),
    GroupId     CHAR (24),
    ResourceId  CHAR (24),
    GroupSort   INTEGER,
    CreateDate  DATETIME,
    Creator     VARCHAR (20),
    EditDate    DATETIME,
    Editor      VARCHAR (20),
    Version     INTEGER
);


CREATE TABLE resource (
    Id          INTEGER       PRIMARY KEY AUTOINCREMENT,
    AggregateId CHAR (24),
    GroupSort   INTEGER,
    ActionDescn VARCHAR (300),
    CreateDate  DATETIME,
    Creator     VARCHAR (20),
    EditDate    DATETIME,
    Editor      VARCHAR (20),
    Version     INTEGER
);

CREATE VIEW v_my_res AS
    SELECT a.Id AS Id,
           a.AggregateId AS AggregateId,
           gr.GroupId AS GroupId,
           eg.EmployeeId AS EmployeeId,
           a.GroupSort AS GroupSort,
           a.ActionDescn AS ActionDescn,
           a.CreateDate AS CreateDate,
           a.Creator AS Creator,
           a.EditDate AS EditDate,
           a.Editor AS Editor,
           a.Version AS Version
      FROM (
               (
                   resource a
                   JOIN
                   group_resource gr ON ( (a.AggregateId = gr.ResourceId) ) 
               )
               JOIN
               employee_group eg ON ( (eg.GroupId = gr.GroupId) ) 
           );
