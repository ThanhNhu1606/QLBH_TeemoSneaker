create database QLBH_TeemoSneaker
 
use [QLBH_TeemoSneaker]

DROP DATABASE QLBH_TeemoSneaker;

--bảng Khách hàng
create table tblKHACHHANG
(
	MaKH char(10),
	HoKH nvarchar (30) not null,
	TenKH nvarchar (10) not null,
	SdtKH char(10) not null,
	GioiTinhKH bit, -- 0 = Nữ, 1 = Nam
	NgaySinhKH date,
	DiaChiKH nvarchar (50) not null,
	EmailKH nvarchar(30) not null,
	constraint pk_KhachHang primary key (MaKH),
	constraint uq_EmailKH unique (EmailKH)
)

--bảng tài khoản khách hàng
create table tblTAIKHOANKH
(
	TaiKhoanKH varchar(50) not null,
	MatKhauKH varchar(20) not null,
	MaKH char(10),
	constraint fk_TaiKhoanKH_MaKH foreign key (MaKH) references tblKHACHHANG(MaKH)
)


-- bảng Chức vụ nhân viên
create table tblCHUCVUNV
(
	MaChucVu char (10),
	TenChucVu nvarchar (30) not null,
	MatKhau nvarchar(20) not null,
	constraint pk_ChucVuNV primary key (MaChucVu)
)


--bảng Nhân viên
create table tblNHANVIEN
(
	MaChucVu char (10),
	MaNV char(10),
	HoNV nvarchar (15) not null,
	TenNV nvarchar (10) not null,
	SdtNV char(10) not null,
	CccdNV char (12) not null,
	GioiTinhNV bit, -- 0 = Nữ, 1 = Nam
	NgaySinhNV date not null,
	DiaChiNV nvarchar (50) not null,
	EmailNV nvarchar(30) not null,
	constraint pk_NhanVien primary key (MaNV),
	constraint fk_NhanVien_MaChucVu foreign key (MaChucVu) references tblCHUCVUNV (MaChucVu),
	constraint uq_CccdNV unique (CccdNV),
	constraint uq_EmailNV unique (EmailNV)
)


--bảng Nhà cung cấp
create table tblNHACUNGCAP
(
	MaNCC char (10),
	TenNCC nvarchar (100) not null,
	DiaChiNCC nvarchar (100) not null,
	SdtNCC varchar (15) not null,	
	constraint pk_NhaCungCap primary key (MaNCC)
)

--bảng Loại giày
create table tblLOAIGIAY 
(
	MaLoaiGiay char (10),
	TenLoaiGiay nvarchar (50) not null,
	DacDiem nvarchar (MAX),
	constraint pk_LoaiGiay primary key (MaLoaiGiay)
)

create table tblSIZE
(
	MaSize char (10),
	LoaiSize int not null,
	constraint pk_MaSize primary key(MaSize)
)


--bảng Giày
create table tblGIAY
(
	MaLoaiGiay char (10),
	MaGiay char (10),
	TenGiay nvarchar (30) not null,
	MaSize char(10),
	GiaGiay money not null,
	SoLuongTon int not null,
	HinhAnh nvarchar(MAX) not null,
	constraint pk_Giay primary key (MaGiay),
	constraint fk_Giay_MaLoaiGiay foreign key (MaLoaiGiay) references tblLOAIGIAY (MaLoaiGiay),
	constraint fk_Giay_MaSize foreign key (MaSize) references tblSIZE (MaSize)
)


--bảng Chi tiết nhà cung cấp
	create table tblCT_NHACUNGCAP
	(
		MaGiay char (10),
		MaNCC char (10),
		NgaySX date not null,
		GhiChu nvarchar (50),
		constraint pk_CT_NhaCungCap primary key (MaGiay, MaNCC),
		constraint fk_CT_NhaCungCap_MaNCC foreign key (MaNCC) references tblNHACUNGCAP (MaNCC),
		constraint fk_CT_NhaCungCap_MaGiay foreign key (MaGiay) references tblGIAY (MaGiay)
	)

--bảng Hóa đơn
create table tblHOADON 
(
	MaHD varchar (50),
	SLGiay int not null,
	TongTienHD money not null,
	MaKH char (10),
	constraint pk_HoaDon primary key (MaHD),
	constraint fk_HoaDon_MaKH foreign key (MaKH) references tblKHACHHANG(MaKH)
)


--bảng Chi tiết hóa đơn
create table tblCT_HOADON
(
	MaHD varchar (50),
	MaGiay char(10),
	NgayLapHD date,
	GioLapHD time, 
	MaSize char(10),
	GiaGiay money not null,
	constraint fk_CT_HoaDon_MaHD foreign key (MaHD) references tblHOADON (MaHD),
	constraint fk_CT_HoaDon_MaGiay foreign key (MaGiay) references tblGIAY (MaGiay)
)

--bảng Phiếu nhập
create table tblPHIEUNHAP
(
	MaPN char (10),
	MaGiay char(10),
	GiaPN money,
	SLPN int not null,
	TongTienPN money not null,
	MaNV char (10),
	constraint pk_PhieuNhap primary key (MaPN),
	constraint fk_PhieuNhap_MaNV foreign key (MaNV) references tblNHANVIEN (MaNV),
	CONSTRAINT fk_PhieuNhap_MaGiay FOREIGN KEY (MaGiay) REFERENCES tblGIAY(MaGiay)
)


--bảng Chi tiết phiếu nhập
create table tblCT_PHIEUNHAP
(
	MaPN char (10),
	NgayNhap date,
	GioNhap time,
	LuuY nvarchar (50),
	constraint pk_CT_PhieuNhap primary key (MaPN),
	constraint fk_CT_PhieuNhap_MaPN foreign key (MaPN) references tblPHIEUNHAP (MaPN)
)


SELECT 
    lg.TenLoaiGiay AS [Loại giày],
    g.TenGiay AS [Tên giày],
    cth.NgayLapHD AS [Ngày lập],
    cth.GioLapHD AS [Giờ lập],
    COUNT(cth.MaGiay) AS [Số lượng giày], -- Đếm số lượng giày
    g.GiaGiay AS [Giá bán],
    COUNT(cth.MaGiay) * g.GiaGiay AS [Tổng tiền], -- Tính tổng tiền
    km.NoiDungKM AS [Nội dung khuyến mãi],
    km.ChietKhau AS [Chiết khấu]
FROM 
    tblCT_HOADON cth
INNER JOIN 
    tblGIAY g ON cth.MaGiay = g.MaGiay
INNER JOIN 
    tblLOAIGIAY lg ON g.MaLoaiGiay = lg.MaLoaiGiay
LEFT JOIN 
    tblKHUYENMAI km ON cth.MaKM = km.MaKM
WHERE 
    cth.MaHD = 'HD0001' -- Dùng tham số MaHD
GROUP BY 
    lg.TenLoaiGiay, g.TenGiay, cth.NgayLapHD, cth.GioLapHD, g.GiaGiay, km.NoiDungKM, km.ChietKhau



SELECT MONTH(ct.NgayLapHD) AS Month, SUM(hd.TongTienHD) AS TotalRevenue
FROM tblHOADON hd
JOIN tblCT_HOADON ct ON ct.MaHD = hd.MaHD
where MONTH(ct.NgayLapHD) = '2'
GROUP BY MONTH(ct.NgayLapHD)
ORDER BY MONTH(ct.NgayLapHD);