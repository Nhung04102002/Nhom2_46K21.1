--Nhà cung cấp
--Mã NCC mới
create function fNewMaNCC()
returns varchar(10)
as
begin
	declare @NewMaNCC varchar(10), @maxMaNCC varchar(10), @num int
	set @maxMaNCC = (select MAX(MaNCC) from NCC)
	set @num = right(@maxMaNCC,len(@maxMaNCC)-3) + 1
	set @NewMaNCC = CONCAT('NCC',REPLICATE('0',len(@maxMaNCC) - 3 - len(@num)),@num)
	return @NewMaNCC
end
go

--Khách hàng
--Mã KH mới
create function fNewMaKH()
returns varchar(10)
as
begin
	declare @NewMaKH varchar(10), @maxMaKH varchar(10), @num int
	set @maxMaKH = (select MAX(MaKH) from KHACHHANG)
	set @num = right(@maxMaKH,len(@maxMaKH)-2) + 1
	set @NewMaKH = CONCAT('KH',REPLICATE('0',len(@maxMaKH) - 2 - len(@num)),@num)
	return @NewMaKH
end
go
select dbo.fNewMaKH()
go

--Hàng
--Mã hàng mới
create function fNewMaHang()
returns varchar(8)
as
begin
	declare @NewMaHang varchar(8), @maxMaHang varchar(8), @num varchar(8)
	set @maxMaHang = (select MAX(MaHang) from HANG)
	set @num = right(@maxMaHang,len(@maxMaHang)-2)+1
	set @NewMaHang = CONCAT('SP',REPLICATE('0',len(@maxMaHang) - 2 - len(@num)),@num)
	return @NewMaHang
end
go

--Hoá đơn
--Mã hoá đơn mới
create function fNewMaHD()
returns varchar(8)
as
begin
	declare @NewMaHD varchar(8), @maxMaHD varchar(8), @num varchar(8)
	set @maxMaHD = (select MAX(MaHD) from HOADON)
	if @maxMaHD is null
	begin
		set @NewMaHD = 'HD000001'
	end
	else
	begin
		set @num = right(@maxMaHD,len(@maxMaHD)-2)+1
		set @NewMaHD = CONCAT('HD',REPLICATE('0',len(@maxMaHD) - 2 - len(@num)),@num)
	end
	return @NewMaHD
end
go
--Tạo mã nhập kho mới
create function fNewMaNK()
returns varchar(10)
as
begin
	declare @NewMaNK varchar(10), @maxMaNK varchar(10), @num int
	set @maxMaNK = (select MAX(MaNK) from PhieuNK)
	if @maxMaNK is null
	begin
		set @NewMaNK = 'NK000001'
	end
	else
	begin
		set @num = right(@maxMaNK,len(@maxMaNK)-2)+1
		set @NewMaNK = CONCAT('NK',REPLICATE('0',len(@maxMaNK) - 2 - len(@num)),@num)
	end
	return @NewMaNK
end
go

select HANG.MaHang, TenHang, CT_PhieuNK.SoLuong, GiaNhap, ThanhTien from CT_PhieuNK join HANG on CT_PhieuNK.MaHang = HANG.MaHang 
--Xoá NCC
create trigger tgDelNCC
on NCC instead of delete
as
begin
	declare @MaNCC varchar(10)
	select @MaNCC = MaNCC from deleted
	update NCC set DiaChi = 1 where MaNCC = @MaNCC
end
go
--Xoá hàng hoá
create trigger tgDelHang
on HANG instead of delete
as
begin
	declare @MaHang varchar(10)
	select @MaHang = MaHang from deleted
	update HANG set SoLuong = -1 where MaHang=@MaHang
end
go
--Kiểm tra có MaHD trong CT_HOADON hay không
create function fCheckHD_CTHD (@MaHD varchar(10))
returns int
as
begin
	declare @ret int
	if exists (select * from CT_HOADON where MaHD = @MaHD)
	begin
		set @ret = 1
	end
	else
	begin
		set @ret = 0
	end
	return @ret
end
go
--Xoá hoá đơn
create trigger tgDelCTHD
on CT_HOADON after delete
as
begin
	declare @MaHD varchar(10), @MaHang varchar(10), @SoLuong int
	select @MaHD=MaHD, @MaHang=MaHang, @SoLuong=SoLuong from deleted
	update HANG set SoLuong = SoLuong + @SoLuong where MaHang = @MaHang
	delete HOADON where MaHD = @MaHD
end
go
--Kiểm tra có MaNK trong CT_PhieuNK hay không
create function fCheckNK_CTNK (@MaNK varchar(10))
returns int
as
begin
	declare @ret int
	if exists (select * from CT_PhieuNK where MaNK = @MaNK)
	begin
		set @ret = 1
	end
	else
	begin
		set @ret = 0
	end
	return @ret
end
go
--Xoá phiếu nhập kho
create trigger tgDelCTNK
on CT_PhieuNK after delete
as
begin
	declare @MaNK varchar(10), @MaHang varchar(10), @SoLuong int
	select @MaNK=MaNK, @MaHang=MaHang, @SoLuong=SoLuong from deleted
	update HANG set SoLuong = SoLuong - @SoLuong where MaHang = @MaHang
	delete PhieuNK where MaNK = @MaNK
end
go

--Xoá khách hàng nếu có hoá đơn của KH, tự động đổi MaKH của hoá đơn thành Mã khách lẻ
create trigger tgDelKH
on HOADON instead of delete
as
begin
	declare @MaKH varchar(10)
	select @MaKH=MaKH from deleted
	update HOADON set MaKH='KH000000' where MaKH = @MaKH
	delete KHACHHANG where MaKH = @MaKH
end
go

--Kiểm tra có MaKH trong HOADON hay không
create function fCheckKH_HD (@MaKH varchar(10))
returns int
as
begin
	declare @ret int
	if exists (select * from HOADON where MaKH = @MaKH)
	begin
		set @ret = 1
	end
	else
	begin
		set @ret = 0
	end
	return @ret
end
go


--Kiểm tra đăng nhập 
create function fCheckLogin (@UserName varchar(100), @Pass varchar(100))
returns int
as
begin
	declare @result varchar(100)
	set @Pass = CONVERT(varbinary(32), HashBytes('MD5', @Pass), 2)

	if exists (select * from TAIKHOAN where UserName=@UserName and HASHPASSWORD=@Pass)
	begin
		--Đăng nhập đúng trả về kết quả = 1
		set @result = 1
	end
	else
	begin
		--Đăng nhập sai trả về kết quả = 0
		set @result = 0
	end
	return @result
end
go

--Sau khi thêm mới bản ghi chi tiết hoá đơn thì tính tổng tiền và thanh toán, cập nhật số lượng hàng đã có
create trigger tgInsCTHD
on CT_HOADON after insert
as
begin
	declare @MaHD varchar(10), @MaHang varchar(10), @SoLuong int, @TongTien numeric(12,0), @ThanhToan numeric(12,0)
	select @MaHD = MaHD, @MaHang = MaHang, @SoLuong = SoLuong from inserted
	set @TongTien = (select sum(ThanhTien) from CT_HOADON where MaHD = @MaHD)
	set @ThanhToan = @TongTien - (@TongTien * (select ChietKhau from HOADON where MaHD = @MaHD))

	update HANG set SoLuong = SoLuong - @SoLuong where MaHang = @MaHang
	update HOADON set TongTien = @TongTien, ThanhToan = @ThanhToan where MaHD = @MaHD
end
go
--Sau khi thêm mới bản ghi chi tiết phiếu nhập kho thì tính tổng tiền và thanh toán, cập nhật số lượng hàng đã có
create trigger tgInsCTpNK
on CT_PhieuNK after insert
as
begin
	declare @MaNK varchar(10), @MaHang varchar(10), @SoLuong int, @TongTien money, @ThanhToan money
	select @MaNK = MaNK, @MaHang = MaHang, @SoLuong = SoLuong from inserted
	set @TongTien = (select sum(ThanhTien) from CT_PhieuNK where MaNK = @MaNK)
	set @ThanhToan = @TongTien - (@TongTien * (select ChietKhau from PhieuNK where MaNK = @MaNK))
	update HANG set SoLuong = SoLuong + @SoLuong where MaHang = @MaHang

	update PhieuNK 
	set TongTien = @TongTien, ThanhToan = @ThanhToan
	where MaNK = @MaNK
end
go

--Tạo index cho các thuộc tính cần thiết
--Tìm hàng hoá
create index SearchHang
on Hang(TenHang)
--Tìm nhà cung cấp
create index SearchNCC
on NCC(TenNCC)
--Tìm khách hàng
create unique index SearchKH
on KHACHHANG(SDT)
go


