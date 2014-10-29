/****** 对象:  StoredProcedure [dbo].[GetNewID]    脚本日期: 08/21/2012 15:12:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[GetNewID] @TableName varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL serializable
	DECLARE @ReturnID BIGINT
	
	BEGIN TRAN

	IF(EXISTS(SELECT currentid FROM TableId WHERE tablename=@TableName))
	BEGIN
		-- 数据库中存在当天记录，流水号加1再获取流水号
		UPDATE dbo.TableId WITH(ROWLOCK) 
		SET currentid=currentid+1,updatetime=getdate()
		WHERE tablename=@TableName

		SELECT @ReturnID=currentid 
		FROM dbo.TableId WITH(ROWLOCK)
		WHERE tablename=@TableName
	END
	ELSE
	BEGIN
		-- 如果不存在当前表名记录，创建记录再获取流水号
		INSERT INTO TableId(tablename,currentid,updatetime) 
		VALUES(@TableName,1,getdate())

		SET @ReturnID=1
	END
	
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRAN	
	END
	ELSE 
	BEGIN
		COMMIT TRAN	
	END
	
	Select @ReturnID
	Return @ReturnID
END
