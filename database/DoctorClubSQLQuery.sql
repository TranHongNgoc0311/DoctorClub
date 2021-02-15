create drop database DoctorClub
go
use DoctorClub
go
--Thong tin nguoi dung
create table Users(
	Id int primary key identity,
	Name nvarchar(200) not null,
	Username nvarchar(50) unique not null,
	Password nvarchar(100) not null default 12345678,
	AccStatus int default 1,
	Birthday date,
	Image text,
	Phone nvarchar(20) not null,
	Email nvarchar(200) not null,
	Address nvarchar(250) not null,
	Awards text,
	online bit,
	Introdution text,
	ActivePoints int default 0,
	Created date default getDate()
)
go 
--Thong tin ky nang chuyen mon
create table Specializations(
	Id int primary key identity,
	Name nvarchar(100) not null unique,
	Status bit default 1,
	Created date default getDate()
)
go 
--Thong tin ky nang chuyen mon cua nguoi dung
create table UserSpecializations(
	UserID int foreign key references Users(id),
	SpeID int foreign key references Specializations(id),
	Position nvarchar(100),
	Place nvarchar(200),
	FromDate date,
	ToDate date
)
--Thong tin truong hoc
create table Academy(
	Id int primary key identity,
	Name nvarchar(200) not null unique,
	Status bit default 1,
	Created date default getDate()
)
go 
--Thong tin hoc van cua nguoi dung
create table UserEducations(
	UserID int foreign key references Users(id),
	SId int foreign key references Academy(id),
	Image text not null,
	FromDate date,
	ToDate date
)
go
--Thong tin phan loai bai dang
create table Categories(
	Id int primary key identity,
	Name nvarchar(100) unique,
	Slug nvarchar(200),
	description text,
	parentId int,
	Status bit default 1
)
go
--Thong tin bai dang
create table Posts(
	Id nvarchar(11) primary key,
	UserId int foreign key references Users(id),
	Title nvarchar(200),
	Slug nvarchar(250),
	CatId int foreign key references Categories(id),
	Content text,
	Views int,
	Created date default getdate(),
	Updated date,
	Status bit default 0
)
go 
--Thong tin Binh luan/Tra loi
create table Comments(
	Id nvarchar(20) primary key,
	Content text,
	Created date default getdate(),
	Updated date,
	UserId int foreign key references Users(Id),
	PostId nvarchar(11) foreign key references Posts(Id),
	ParrentId nvarchar(20)
)
go
create table CmtLikes(
	UserId int foreign key references Users(Id),
	LikeStatus bit,
	CmtId nvarchar(20) foreign key references Comments(id)
)
go
--Thong tin cac tag cua bai viet
create table Tags(
	Name nvarchar(100) unique,
	Slug nvarchar(200),
	PostId nvarchar(11) foreign key references Posts(Id)
)
go
create table Notifications(
	id nvarchar(11) primary key,
	title nvarchar(250),
	url text,
	created date default getdate()
)
go
create table UserNotification(
	NoId nvarchar(11) foreign key references Notifications(id),
	userId int foreign key references Users(id),
	status bit default 1
)