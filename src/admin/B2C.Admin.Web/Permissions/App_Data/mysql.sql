CREATE SCHEMA `b2c_admin` ;
use `b2c_admin`;

CREATE TABLE `department` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `AggregateId` CHAR(24) NULL,
  `Name` VARCHAR(30) NULL,
  `Descn` VARCHAR(100) NULL,
  `CreateDate` DATETIME NULL,
  `Creator` VARCHAR(20) NULL,
  `EditDate` DATETIME NULL,
  `Editor` VARCHAR(20) NULL,
  `Version` INT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `UNQ_AggregateId` (`AggregateId` ASC),
  UNIQUE INDEX `UNQ_Name` (`Name` ASC));

CREATE TABLE `department_resource` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `AggregateId` CHAR(24) NULL,
  `DepartmentId` CHAR(24) NULL,
  `ResourceId` CHAR(24) NULL,
  `CreateDate` DATETIME NULL,
  `Creator` VARCHAR(20) NULL,
  `EditDate` DATETIME NULL,
  `Editor` VARCHAR(20) NULL,
  `Version` INT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IDX_DepartmentId` (`DepartmentId` ASC));

CREATE TABLE `employee` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AggregateId` char(24) DEFAULT NULL,
  `UserName` varchar(20) DEFAULT NULL,
  `Password` varchar(300) DEFAULT NULL,
  `NickName` varchar(20) DEFAULT NULL,
  `DepartmentId` char(24) DEFAULT NULL,
  `GroupSort` int(11) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `Creator` varchar(24) DEFAULT NULL,
  `EditDate` datetime DEFAULT NULL,
  `Editor` varchar(20) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UNQ_AggregateId` (`AggregateId`),
  UNIQUE KEY `UNQ_UserName` (`UserName`),
  KEY `IDX_DepartmentId` (`DepartmentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `employee_group` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AggregateId` char(24) DEFAULT NULL,
  `EmployeeId` char(24) DEFAULT NULL,
  `GroupId` char(24) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `Creator` varchar(20) DEFAULT NULL,
  `EditDate` datetime DEFAULT NULL,
  `Editor` varchar(20) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_EmployeeId` (`EmployeeId`),
  KEY `IDX_GroupId` (`GroupId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `group` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AggregateId` char(24) DEFAULT NULL,
  `Name` varchar(30) DEFAULT NULL,
  `Descn` varchar(100) DEFAULT NULL,
  `DepartmentId` char(24) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `Creator` varchar(20) DEFAULT NULL,
  `EditDate` datetime DEFAULT NULL,
  `Editor` varchar(20) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UNQ_AggregateId` (`AggregateId`),
  KEY `IDX_Department` (`DepartmentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `group_resource` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AggregateId` char(24) DEFAULT NULL,
  `GroupId` char(24) DEFAULT NULL,
  `ResourceId` char(24) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `Creator` varchar(20) DEFAULT NULL,
  `EditDate` datetime DEFAULT NULL,
  `Editor` varchar(20) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_GroupId` (`GroupId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `resource` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AggregateId` char(24) DEFAULT NULL,
  `GroupSort` int(11) DEFAULT NULL,
  `ActionDescn` varchar(300) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `Creator` varchar(20) DEFAULT NULL,
  `EditDate` datetime DEFAULT NULL,
  `Editor` varchar(20) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UNQ_AggregateId` (`AggregateId`),
  KEY `IDX_Group` (`GroupSort`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE SQL SECURITY DEFINER VIEW `v_my_res` AS 
select `a`.`Id` AS `Id`,`a`.`AggregateId` AS `AggregateId`,`gr`.`GroupId` AS `GroupId`,`eg`.`EmployeeId` AS `EmployeeId`,`a`.`GroupSort` AS `GroupSort`,`a`.`ActionDescn` AS `ActionDescn`,`a`.`CreateDate` AS `CreateDate`,`a`.`Creator` AS `Creator`,`a`.`EditDate` AS `EditDate`,`a`.`Editor` AS `Editor`,`a`.`Version` AS `Version` 
from ((`resource` `a` join `group_resource` `gr` on ((`a`.`AggregateId` = `gr`.`ResourceId`))) join `employee_group` `eg` on((`eg`.`GroupId` = `gr`.`GroupId`)));
