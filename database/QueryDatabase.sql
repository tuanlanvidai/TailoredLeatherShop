create database handbag_shop_db
go
use handbag_shop_db
go

create table tbl_users (
    user_id int identity(1,1) primary key,
    username nvarchar(50) not null unique,
    password_hash nvarchar(255) not null,
    email nvarchar(100) default '',
    full_name nvarchar(100) default '',
    phone nvarchar(20) default '',
    role nvarchar(20) default 'Customer', -- Admin / Customer
    created_at datetime default getdate(),
    is_delete bit default 0
)

create table tbl_categories (
    category_id int identity(1,1) primary key,
    name nvarchar(100) not null,
    is_delete bit default 0
)

create table tbl_products (
    product_id int identity(1,1) primary key,
    name nvarchar(100) not null,
    description nvarchar(1000) default '',
    category_id int references tbl_categories(category_id),
    image_url nvarchar(255) default '',
    is_available bit default 1,
    created_at datetime default getdate(),
    is_delete bit default 0
)

CREATE TABLE tbl_orders (
    order_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT REFERENCES tbl_users(user_id),
    status NVARCHAR(50) DEFAULT 'New', -- New, Quoted, Confirmed, InProgress, Completed, Cancelled
    quote_price FLOAT DEFAULT 0,
    estimated_delivery_date DATETIME,
    shipping_address NVARCHAR(255) NULL,
    additional_request NVARCHAR(255) NULL,
    admin_note NVARCHAR(1000) NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME,
    is_delete BIT DEFAULT 0
);

CREATE TABLE tbl_order_details (
    order_detail_id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT REFERENCES tbl_orders(order_id),
    product_id INT REFERENCES tbl_products(product_id),
    quantity INT DEFAULT 1,
    custom_note NVARCHAR(1000) DEFAULT '',
    is_delete BIT DEFAULT 0
);

CREATE TABLE tbl_product_images (
    image_id INT IDENTITY(1,1) PRIMARY KEY,
    product_id INT NOT NULL,
    image_url NVARCHAR(255) NOT NULL,
    is_delete BIT DEFAULT 0,
    FOREIGN KEY (product_id) REFERENCES tbl_products(product_id)
);


-- THÊM DỮ LIỆU
insert into tbl_users(username, password_hash, email, full_name, phone, role)
values 
(N'admin', N'admin', N'admin@email.com', N'Admin Truong Tuan Lan', N'0901234567', N'Admin'),
(N'elonmusk', N'elonmusk', N'elon@email.com', N'Elon Musk', N'0912345678', N'Customer'),
(N'brucelee', N'brucelee', N'bruce@email.com', N'Bruce Lee', N'0923456789', N'Customer')

-- Thêm danh mục sản phẩm
INSERT INTO tbl_categories(name) VALUES
(N'Ví da'),
(N'Giày da'),
(N'Túi xách'),
(N'Thắt lưng'),
(N'Găng tay'),
(N'Dây da đồng hồ');

-- Ví da (category_id = 1)
INSERT INTO tbl_products(name, category_id, image_url)
VALUES
(N'Ví đựng thẻ nam dáng đứng trẻ trung cao cấp', 1, N'images/vidungthe_1.jpg'),
(N'Ví nam cao cấp da bò viền chỉ nổi', 1, N'images/vibovienchi_1.jpg'),
(N'Ví da nam cầm tay phối khóa viền thời trang', 1, N'images/vicamtay_1.jpg'),
(N'Ví nam cá sấu 2 mặt dáng ngang da hông cao cấp', 1, N'images/vicasau2mat_1.jpg'),
(N'Ví da bò nam dáng đứng cao cấp', 1, N'images/vidabo_1.jpg'),
(N'Ví da ngang hàng hiệu cao cấp', 1, N'images/vinang_1.jpg'),
(N'Ví nam cầm tay da bò thời trang cao cấp', 1, N'images/vicamtaybovip_1.jpg'),
(N'Ví cá sấu 2 mặt dáng ngang sang trọng', 1, N'images/vicasauvip_1.jpg'),
(N'Ví nam đựng hộ chiếu da trơn siêu mềm', 1, N'images/vihochieu_1.jpg'),
(N'Ví đựng thẻ nam da thật nhỏ gọn cao cấp', 1, N'images/vidungthemini_1.jpg'),
(N'Ví da nam đẹp dáng đứng đường may chỉ nổi', 1, N'images/vidungdep_1.jpg'),
(N'Ví nam da sần lịch lãm', 1, N'images/vidasan_1.jpg');

-- Giày da (category_id = 2)
INSERT INTO tbl_products(name, category_id, image_url)
VALUES
(N'Giày da nam Penny Loafer trẻ trung cao cấp', 2, N'images/giayloafer_1.jpg'),
(N'Giày da nam kiểu dáng Oxford lịch lãm', 2, N'images/giayoxford_1.jpg'),
(N'Giày Oxford tăng chiều cao nam sang trọng', 2, N'images/giayoxfordtcc_1.jpg'),
(N'Giày lười da nam họa tiết caro lịch lãm', 2, N'images/giayluoicaro_1.jpg'),
(N'Giày lười nam sang trọng dáng Penny Loafer', 2, N'images/giayluoisangtrong_1.jpg'),
(N'Giày da nam Oxford cao cấp', 2, N'images/giayoxfordvip_1.jpg'),
(N'Giày tăng chiều cao dáng Monkstrap cổ điển', 2, N'images/giaymonkstrap_1.jpg'),
(N'Giày Loafer tăng chiều cao nam thiết kế sang trọng', 2, N'images/giayloafertcc_1.jpg'),
(N'Giày nam da bóng phối chun hai bên hông', 2, N'images/giaybongphoi_1.jpg'),
(N'Giày lười nam phối họa tiết lịch lãm', 2, N'images/giayluoiphoi_1.jpg'),
(N'Giày da nam Horse Bit Loafer họa tiết đục lỗ', 2, N'images/giayhorsebit_1.jpg'),
(N'Giày da Penny Loafer nam công sở trẻ trung', 2, N'images/giaypennycs_1.jpg');

-- Túi xách (category_id = 3)
INSERT INTO tbl_products(name, category_id, image_url)
VALUES
(N'Túi hộp da bò Saffiano màu đen', 3, N'images/tuibosaffiano_1.jpg'),
(N'Túi xách nam da đựng laptop', 3, N'images/tuixachlaptop_1.jpg'),
(N'Túi nam công sở đựng laptop', 3, N'images/tuicongso_1.jpg'),
(N'Túi xách nam da đựng laptop', 3, N'images/tuinamdlaptop_1.jpg'),
(N'Túi hộp da bò thiết kế lịch lãm', 3, N'images/tuihoplichlam_1.jpg'),
(N'Túi hộp da bò thiết kế trẻ trung', 3, N'images/tuihoptrẻ_1.jpg'),
(N'Túi hộp đeo chéo nam', 3, N'images/tuideochao_1.jpg'),
(N'Cặp da bò thiết kế hiện đại', 3, N'images/capdahd_1.jpg'),
(N'Túi đeo ipad nam da trơn dây đeo phối màu', 3, N'images/tuiipad1_1.jpg'),
(N'Túi đựng ipad nam dáng ngang', 3, N'images/tuiipad2_1.jpg'),
(N'Túi đeo ipad cho nam khóa viền', 3, N'images/tuiipad3_1.jpg'),
(N'Cặp đựng ipad nam khóa viền', 3, N'images/capipad_1.jpg');

-- Thắt lưng (category_id = 4)
INSERT INTO tbl_products(name, category_id, image_url)
VALUES
(N'Thắt lưng nam công sở vân nổi cao cấp', 4, N'images/thatlung1_1.jpg'),
(N'Thắt lưng da nam công sở khóa kim', 4, N'images/thatlung2_1.jpg'),
(N'Dây thắt lưng quần jean nam đẹp trẻ trung', 4, N'images/thatlung3_1.jpg'),
(N'Thắt lưng nam da bò khóa lăn quần tây', 4, N'images/thatlung4_1.jpg'),
(N'Thắt lưng nam công sở khóa kim cao cấp', 4, N'images/thatlung5_1.jpg'),
(N'Thắt lưng nam quần jean khóa kim', 4, N'images/thatlung6_1.jpg'),
(N'Dây lưng da cá sấu nam liền bản 3.4cm chất lượng', 4, N'images/thatlung7_1.jpg'),
(N'Thắt lưng nam da bò trơn mặt khóa hoạ tiết', 4, N'images/thatlung8_1.jpg'),
(N'Thắt lưng nam da bò khóa cài bền đẹp', 4, N'images/thatlung9_1.jpg'),
(N'Dây lưng nam quần tây da vân khóa lăn', 4, N'images/thatlung10_1.jpg'),
(N'Dây lưng nam quần tây khóa lăn da trơn', 4, N'images/thatlung11_1.jpg'),
(N'Thắt lưng nam công sở khóa trượt cao cấp', 4, N'images/thatlung12_1.jpg');

-- Găng tay (category_id = 5)
INSERT INTO tbl_products(name, category_id, image_url)
VALUES
(N'Bao tay da cừu cảm ứng cho nam', 5, N'images/gangtay1_1.jpg'),
(N'Găng tay da cừu lót nhung', 5, N'images/gangtay2_1.jpg'),
(N'Găng tay da cừu nam cảm ứng', 5, N'images/gangtay3_1.jpg'),
(N'Găng tay da nam cảm ứng phối sọc chéo', 5, N'images/gangtay4_1.jpg'),
(N'Găng tay da nam phong cách lịch lãm', 5, N'images/gangtay5_1.jpg'),
(N'Găng tay da nữ cảm ứng đính nơ', 5, N'images/gangtay6_1.jpg'),
(N'Găng tay da nữ phối họa tiết', 5, N'images/gangtay7_1.jpg'),
(N'Găng tay da nữ thiết kế sang trọng', 5, N'images/gangtay8_1.jpg'),
(N'Găng tay da phối họa tiết nam tính', 5, N'images/gangtay9_1.jpg'),
(N'Găng tay nam da cừu phối họa tiết lịch lãm', 5, N'images/gangtay10_1.jpg'),
(N'Găng tay nam phối đường viền sang trọng', 5, N'images/gangtay11_1.jpg'),
(N'Găng tay nữ cảm ứng da cừu', 5, N'images/gangtay12_1.jpg');

-- Dây da đồng hồ (category_id = 6)
INSERT INTO tbl_products(name, category_id, image_url)
VALUES
(N'Dây da cá sấu đeo đồng hồ Henglong handmade', 6, N'images/daydongho1_1.jpg'),
(N'Dây da đồng hồ cá sấu màu nâu café', 6, N'images/daydongho2_1.jpg'),
(N'Dây da đồng hồ cá sấu màu nâu đỏ', 6, N'images/daydongho3_1.jpg'),
(N'Dây da đồng hồ cá sấu sang trọng', 6, N'images/daydongho4_1.jpg'),
(N'Dây da đồng hồ da bò Laforce', 6, N'images/daydongho5_1.jpg'),
(N'Dây đeo đồng hồ da bò Laforce', 6, N'images/daydongho6_1.jpg'),
(N'Dây đeo đồng hồ da cá sấu Henglong handmade', 6, N'images/daydongho7_1.jpg'),
(N'Dây đeo đồng hồ da cá sấu vân da tự nhiên', 6, N'images/daydongho8_1.jpg'),
(N'Dây đeo đồng hồ da kỳ đà Laforce', 6, N'images/daydongho9_1.jpg'),
(N'Dây đeo đồng hồ da trăn độc đáo', 6, N'images/daydongho10_1.jpg'),
(N'Dây đồng hồ da bò', 6, N'images/daydongho11_1.jpg'),
(N'Dây đồng hồ da bò Laforce', 6, N'images/daydongho12_1.jpg');


INSERT INTO tbl_product_images (product_id, image_url)
SELECT 
    p.product_id,
    REPLACE(p.image_url, '_1.jpg', '_2.jpg')
FROM tbl_products p
WHERE p.is_delete = 0;

INSERT INTO tbl_product_images (product_id, image_url)
SELECT 
    p.product_id,
    REPLACE(p.image_url, '_1.jpg', '_3.jpg')
FROM tbl_products p
WHERE p.is_delete = 0;
