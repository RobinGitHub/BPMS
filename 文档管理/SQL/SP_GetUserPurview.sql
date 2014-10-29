/****** 对象:  StoredProcedure [dbo].[SP_GetOperatorPurview]    脚本日期: 08/21/2012 15:24:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--通过操作员ID获取操作员对应的权限
CREATE proc [dbo].[SP_GetUserPurview]
@SystemId int,
@UserId int
AS
begin

set nocount on;
set transaction isolation level read uncommitted;

select r.ID RoleId, s.ID SystemId, r.code as RoleCode,tp.id as Purviewid,s.code as SystemCode,tp.Name,tp.Code,tp.ParentId,tp.PurviewType,tp.Remark
from UserRole tor
inner join RolePurview trp on tor.roleid=trp.roleid
inner join PurviewInfo tp on trp.PurviewID=tp.id
inner join RoleInfo r on trp.roleid=r.id
inner join SystemInfo s on tp.systemid=s.id
where tp.isenable=1 and r.isenable=1
and tor.UserId=@UserId and s.ID =@SystemId
and tp.id not in
(
	select purviewid from UserPurview
	where UserId=@UserId and HasRight=0
)
union
select r.ID RoleId, s.ID SystemId,r.code as RoleCode,tp.id as Purviewid,s.Code as SystemCode,tp.Name,tp.Code,tp.ParentId,tp.PurviewType,tp.Remark
from UserPurview op
inner join PurviewInfo tp on op.purviewid=tp.id
inner join RoleInfo r on op.roleid=r.id
inner join SystemInfo s on tp.systemid=s.id
where op.UserId=@UserId and s.ID =@SystemId and r.isenable=1 and tp.isenable=1 and op.HasRight=1

end