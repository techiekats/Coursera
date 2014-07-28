create PROCEDURE [dbo].[InitMatrix]
	@rows bigint,
	@cols bigint
AS
declare @i bigint , @j bigint
set @i=@rows - 1 
set @j = @cols - 1
drop table [dbo].[Rows];
drop table [dbo].[Cols];
drop table [dbo].[Matrix];
CREATE TABLE [dbo].[Rows] (
    [Id] BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Cols] (
    [Id] BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Matrix] (
    [RowNo]     BIGINT NOT NULL,
    [ColNo]     BIGINT NOT NULL,
    [CellValue] BIGINT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Matrices] PRIMARY KEY CLUSTERED ([RowNo] ASC, [ColNo] ASC)
);

while (@i >= 0)
begin 	
	set @i = @i-1
	insert into [dbo].[Rows] values (@i)
end

while (@j >= 0)
begin 
	insert into [dbo].[Cols] values (@j)
	set @j = @j - 1
end
insert into [dbo].[Matrix]  
select R.Id, C.Id from [dbo].[Rows] R, [dbo].[Cols] C 