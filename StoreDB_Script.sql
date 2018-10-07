create database StoreDB
go
use StoreDB
go

create table Employee(
	ID int identity(1,1) primary key not null,
	FirstName varchar(50) not null,
	LastName varchar(50) not null,
	UserName varchar(32) not null,
	Password varchar(32) not null
)

create table Customer(
	ID int identity(1,1) primary key not null,
	FirstName varchar(50) not null,
	LastName varchar(50) not null,
	UserName varchar(32) not null,
	Password varchar(32) not null
)

insert into Customer values ('Juan', 'Perez', '123', 
CONVERT(VARCHAR(32), HashBytes('MD5', 'abc'), 2))

select * from customer

create table Product(
	ID int identity(1,1) primary key not null,
	Name varchar(255) not null,
	UnitPrice decimal(8,2) not null
)

create table OrderStatus(
	ID int identity(1,1) primary key not null,
	Name varchar(255) not null
)

insert into OrderStatus(Name) 
values ('1-Unpaid'), ('2-Paid'), ('3-Processing'), ('4-Shipped'), ('5-Delivered')

create table CustomerOrder(
	ID int identity(1,1) primary key not null,
	CustomerID int not null,
	Date smalldatetime not null,
	OrderStatusID int not null,
	Amount decimal(10,2) not null,
	constraint FK_Customer_CustomerOrder foreign key (CustomerID) references Customer(ID),
	constraint FK_OrderStatus_CustomerOrder foreign key (OrderStatusID) references OrderStatus(ID)
)

create table OrderDetail(
	CustomerOrderID int not null,
	ProductID int not null,
	Quantity int not null,
	Amount decimal(10,2) not null,
	constraint FK_CustomerOrder_OrderDetail foreign key (CustomerOrderID) references CustomerOrder(ID),
	constraint FK_Product_OrderDetail foreign key (ProductID) references Product(ID),
	constraint PK_OrderDetail primary key (CustomerOrderID, ProductID)
)
