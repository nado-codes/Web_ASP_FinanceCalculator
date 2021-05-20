create table Tests
(
	Id int primary key identity not null,
	[Name] varchar(64) not null,
	LastModified timestamp not null,
	DateAdded datetime not null default(getutcdate())
)
go
create procedure GetTests
as
begin
	select * from Tests
end
go
create procedure GetTestById
	@id int
as
begin
	set nocount on;
	select Id,LastModified from Tests where Id=@id
end
go
create procedure AddTest
	@name varchar(64)
as
begin
	set nocount off;
	insert into Tests ([Name]) Values(@name)
end
go
create procedure UpdateTest
	@id int,
	@name varchar(64),
	@lastModified binary(8)
as
begin
	set nocount off;
	update Tests SET
		[Name]=@name
	where Id=@id and LastModified=@lastModified
end
go
create procedure DeleteTest
	@id int,
	@lastModified binary(8)
as
begin
	set nocount off;
	delete from Tests where Id=@id and LastModified=@lastModified
end

/*
	drop procedure GetTests
	drop procedure GetTestById
	drop procedure AddTest
	drop procedure UpdateTest
	drop procedure DeleteTest
	drop table Tests
*/
