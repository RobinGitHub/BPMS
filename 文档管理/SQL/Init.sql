/*
系统应该与组织架构是2套平行的关系，然后通过关联表进行关联
在用户表中是否应该关联组织架构的信息
*/

--数据初始化
/*
1.系统表
2.用户
*/
--系统
INSERT INTO SystemInfo(ID, Name,Code, IsEnable, Remark, SortIndex, CreateDate,CreateUserId, CreateUserName, ModifyDate, ModifyUserId, ModifyUserName)
VALUES(1, '通用权限管理系统', 'BPMS', 1, '', 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')

--集团、公司、部门、工作组
INSERT INTO dbo.Organization(ID,Name,Code,ParentId,ShortName,Category,Remark,IsEnable,SortIndex,CreateDate,CreateUserId,CreateUserName,ModifyDate,ModifyUserId,ModifyUserName)
VALUES(1, '集团','JT', 0, 'JT', '1', '', 1, 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')

INSERT INTO dbo.Organization(ID,Name,Code,ParentId,ShortName,Category,Remark,IsEnable,SortIndex,CreateDate,CreateUserId,CreateUserName,ModifyDate,ModifyUserId,ModifyUserName)
VALUES(2, '公司','GS', 1, 'GS', '2', '', 1, 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')


INSERT INTO dbo.Organization(ID,Name,Code,ParentId,ShortName,Category,Remark,IsEnable,SortIndex,CreateDate,CreateUserId,CreateUserName,ModifyDate,ModifyUserId,ModifyUserName)
VALUES(3, '部门','BM', 2, 'BM', '3', '', 1, 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')

INSERT INTO dbo.Organization(ID,Name,Code,ParentId,ShortName,Category,Remark,IsEnable,SortIndex,CreateDate,CreateUserId,CreateUserName,ModifyDate,ModifyUserId,ModifyUserName)
VALUES(4, '工作组','GZZ', 3, 'GZZ', '4', '', 1, 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')

--角色
select * from RoleInfo
INSERT INTO dbo.RoleInfo(ID,SystemId,CompanyId,Name,Code,Category,Remark,IsEnable,SortIndex,CreateDate,CreateUserId,CreateUserName,ModifyDate,ModifyUserId,ModifyUserName)
VALUES(1, 1, 1, '管理员', 'admin', 1, '', 1, 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')


--用户
INSERT INTO dbo.UserInfo(ID,Code,Account,Password,Name,Spell,RoleId,Gender,DutyId,CompanyId,DepartmentId,WorkgroupId,Remark,IsEnable,SortIndex,CreateDate,CreateUserId,CreateUserName,ModifyDate,ModifyUserId,ModifyUserName)
VALUES(1, 'admin', 'admin', '123456', '管理员','admin',1, '男', 0, 1,2,3,'', 1, 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')
--用户角色关系
INSERT INTO UserRole(ID, UserId, RoleId, CreateDate, CreateUserId, CreateUserName)
VALUES(1, 1, 1, GETDATE(), 1, 'admin')

--菜单
INSERT INTO dbo.MenuInfo(ID,SystemId,ParentId,Name,Code,Category,PurviewId,IsSplit,Remark,IsEnable,SortIndex,CreateDate,CreateUserId,CreateUserName,ModifyDate,ModifyUserId,ModifyUserName)
VALUES(1, 1, 0, '系统管理', 'SystemMng', 1, 0, 0, '', 1, 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')

INSERT INTO dbo.MenuInfo(ID,SystemId,ParentId,Name,Code,Category,PurviewId,IsSplit,Remark,IsEnable,SortIndex,CreateDate,CreateUserId,CreateUserName,ModifyDate,ModifyUserId,ModifyUserName)
VALUES(3, 1, 1, '菜单管理', 'MenuMng', 1, 0, 0, '', 1, 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')

INSERT INTO dbo.MenuInfo(ID,SystemId,ParentId,Name,Code,Category,PurviewId,IsSplit,Remark,IsEnable,SortIndex,CreateDate,CreateUserId,CreateUserName,ModifyDate,ModifyUserId,ModifyUserName)
VALUES(2, 1, 0, '组织架构', 'OrganizationMng', 1, 0, 0, '', 1, 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')

INSERT INTO dbo.MenuInfo(ID,SystemId,ParentId,Name,Code,Category,PurviewId,IsSplit,Remark,IsEnable,SortIndex,CreateDate,CreateUserId,CreateUserName,ModifyDate,ModifyUserId,ModifyUserName)
VALUES(4, 1, 0, '组织机构管理', 'OrganMng', 1, 0, 0, '', 1, 1, GETDATE(), 1, 'admin',GETDATE(), 1, 'admin')


INSERT INTO TableId(UpdateTime, TableName, CurrentId) VALUES(GETDATE(), 'SystemInfo', 1)
INSERT INTO TableId(UpdateTime, TableName, CurrentId) VALUES(GETDATE(), 'Organization', 4)
INSERT INTO TableId(UpdateTime, TableName, CurrentId) VALUES(GETDATE(), 'RoleInfo', 1)
INSERT INTO TableId(UpdateTime, TableName, CurrentId) VALUES(GETDATE(), 'UserInfo', 1)
INSERT INTO TableId(UpdateTime, TableName, CurrentId) VALUES(GETDATE(), 'UserRole', 1)
INSERT INTO TableId(UpdateTime, TableName, CurrentId) VALUES(GETDATE(), 'MenuInfo', 4)







