begin tran
if not exists( select top 1 1 from auction.States where code like 'CLOSED')
begin
	INSERT INTO auction.States (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
	values ('Closed','CLOSED',1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from auction.States where code like 'OPEN')
begin
	INSERT INTO auction.States (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
	values ('Open','OPEN',1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from auction.States where code like 'PaymentWait')
begin
	INSERT INTO auction.States (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
	values ('Awaiting for payment',upper('PaymentWait'),1,0,GetDate(),GetDate())
end
commit tran