CREATE SCHEMA `b2c_message`;
USE `b2c_message`;

CREATE TABLE `email_code` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AggregateId` char(36) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `UsageType` tinyint(4) DEFAULT NULL,
  `UsageCode` varchar(10) DEFAULT NULL,
  `ExpireTime` datetime DEFAULT NULL,
  `UsageStatus` tinyint(4) DEFAULT NULL,
  `ClientIP` varchar(15) DEFAULT NULL,
  `Platform` tinyint(4) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `Creator` varchar(20) DEFAULT NULL,
  `EditDate` datetime DEFAULT NULL,
  `Editor` varchar(20) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_Email` (`Email`,`UsageType`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `mobile_code` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AggregateId` char(36) DEFAULT NULL,
  `Mobile` bigint(20) DEFAULT NULL,
  `UsageType` tinyint(4) DEFAULT NULL,
  `UsageCode` varchar(10) DEFAULT NULL,
  `ExpireTime` datetime DEFAULT NULL,
  `UsageStatus` tinyint(4) DEFAULT NULL,
  `ClientIP` varchar(15) DEFAULT NULL,
  `Platform` tinyint(4) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `Creator` varchar(20) DEFAULT NULL,
  `EditDate` datetime DEFAULT NULL,
  `Editor` varchar(20) DEFAULT NULL,
  `Version` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_Mobile` (`Mobile`,`UsageType`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
