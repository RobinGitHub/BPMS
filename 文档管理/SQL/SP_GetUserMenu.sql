/****** 对象:  StoredProcedure [dbo].[SP_GetOperatorMenu]    脚本日期: 08/21/2012 15:24:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--通过操作员ID获取操作员对应的菜单
create proc [dbo].[SP_GetUserMenu]
@SystemId int,
@UserId int
AS
begin

set nocount on;
set transaction isolation level read uncommitted;

select tmp2.SystemCode,tmp2.RoleCode,tmp2.RoleId, MenuInfo.ID,SystemId,ParentId,Name,Code,Category,PurviewId,Icon,IconUrl,NavigateUrl,FormName,IsSplit,IsEnable,SortIndex
from MenuInfo 
inner join (
	select tmp.systemcode,tmp.rolecode,tmp.roleId,mi.id
	from MenuInfo mi 
	inner join (
		select r.code as RoleCode,tp.id as Purviewid,s.code as SystemCode,r.ID RoleId,tp.Name,tp.Code,tp.ParentId,tp.PurviewType,tp.Remark
		from UserRole tor
		inner join RolePurview trp on tor.roleid=trp.roleid
		inner join PurviewInfo tp on trp.PurviewID=tp.id
		inner join RoleInfo r on trp.roleid=r.id
		inner join SystemInfo s on tp.systemid=s.id
		where tp.isenable=1 and r.isenable=1
		and tor.UserId=@UserId and s.ID = @SystemId
		and tp.id not in
		(
			select purviewid from UserPurview
			where UserId = @UserId and HasRight=0
		)
		union
		select r.code as RoleCode,tp.id as Purviewid,s.code as SystemCode,r.ID RoleId,tp.Name,tp.Code,tp.ParentId,tp.PurviewType,tp.Remark
		from UserPurview op
		inner join PurviewInfo tp on op.purviewid=tp.id
		inner join RoleInfo r on op.roleid=r.id
		inner join SystemInfo s on tp.systemid=s.id
		where op.UserId=@UserId and s.ID = @SystemId and r.isenable=1 and tp.isenable=1 and op.HasRight=1
	) tmp on mi.purviewid=tmp.purviewid
	where mi.isenable=1
	union
	select s.code as SystemCode,'' as RoleCode, 0 RoleId,mi.id
	from MenuInfo mi
	inner join SystemInfo s on mi.systemid=s.id
	where mi.purviewid is null and mi.isenable=1 and s.ID = @SystemId
) tmp2 on MenuInfo.id=tmp2.id

end