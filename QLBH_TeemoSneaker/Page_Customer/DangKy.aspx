<%@ Page Title="" Language="C#" MasterPageFile="~/Header_Footer.Master" AutoEventWireup="true" CodeBehind="DangKy.aspx.cs" Inherits="QLBH_TeemoSneaker.Page_Customer.DangKy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ThanhTimKiem_Header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="GioHang_Header" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
    <%--css popup--%>
    <style>
        .icon-container {
            width: 35px;
            height: 35px;
            display: flex;
            justify-content: center;
            align-items: center;
            background-color: #04e40048;
            border-radius: 50%;
        }

        .icon {
            width: 17px;
            height: 17px;
            color: #269b24;
        }

        /* Modal (ẩn mặc định) */
        @keyframes slideInFromRight {
            0% {
                transform: translateX(100%); /* Bắt đầu từ ngoài màn hình, phía bên phải */
            }

            100% {
                transform: translateX(0); /* Dừng lại ở vị trí ban đầu */
            }
        }

        /* Nội dung của Modal */
        .message-text-container {
            display: flex;
            justify-content: center;
            align-items: flex-start;
            flex-grow: 1;
            gap: 20px;
        }

        .text {
            text-align: left;
            margin-left: -20px;
        }

        .message-text,
        .sub-text {
            margin: 0;
            cursor: default;
        }

        .message-text {
            color: #269b24;
            font-size: 17px;
            font-weight: 700;
        }

        .sub-text {
            font-size: 14px;
            color: #555;
        }

        .modal-content {
            background-color: #fff;
            margin: 15% auto; /* Căn giữa modal */
            padding: 20px;
            width: 50%;
            text-align: center;
            border-radius: 8px;
            width: 330px;
            height: 80px;
            display: flex;
            position: fixed;
            right: 20px;
            bottom: -150px;
            animation: slideInFromRight 0.5s ease-out;
            align-items: center; /* Căn giữa theo chiều dọc */
            justify-content: space-between; /* Tạo khoảng cách giữa dấu 'X' và thông báo */
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
        }

        /* Nút đóng */
        .close {
            color: #aaa;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }

            .close:hover,
            .close:focus {
                color: black;
                text-decoration: none;
                cursor: pointer;
            }
    </style>

    <style>
        h4 {
            text-align: center;
            color: #ff5e57;
            margin-bottom: 15px;
            font-family: 'themify';
        }

        /* Căn giữa table-view */
        .table-view {
            width: 50%; /* Hoặc bạn có thể điều chỉnh theo mong muốn */
            margin: 0 auto; /* Căn giữa theo chiều ngang */
            padding: 20px;
            /*border: 1px solid #ddd;*/ /* Có thể thêm border để nhìn rõ */
            border-radius: 20px;
            background-color: #ffffff;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2); /* Tạo bóng nhẹ cho box */
        }

        /* Căn đều nội dung trong table-content */
        .table-content {
            text-align: justify; /* Căn đều hai bên */
            padding: 15px;
        }

        .box-info {
            display: flex;
            flex-wrap: wrap; /* Cho phép các phần tử nằm trên nhiều hàng */
            gap: 20px; /* Khoảng cách giữa các ô */
            justify-content: space-between; /* Các ô được căn chỉnh đều nhau */
        }


        /* Cho các ô nhóm vào hàng ngang */
        .box-content {
            flex: 1 1 48%; /* Cứ 2 ô sẽ chiếm 48% chiều rộng */
        }

        .box-info .box-content {
            display: flex;
            flex-direction: column;
        }

        .box-content-full {
            flex: 1 1 100%; /* Địa chỉ chiếm toàn bộ chiều rộng */
        }

        .title-content {
            font-size: 14px;
        }

        /*        .input-group {
            position: relative;
            width: 100%;
        }*/

        .input-group {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .input {
            width: 100%;
            padding: 10px;
            font-size: 14px;
            border: 1px solid #9e9e9e;
            box-sizing: border-box;
        }

            .input:focus, .input:valid {
                outline: none;
                border: 1.5px solid #ff5e57;
            }

        .toggle-password {
            position: absolute;
            right: 10px; /* Điều chỉnh vị trí icon */
            top: 50%;
            transform: translateY(-50%);
            cursor: pointer;
            font-size: 18px;
            color: #ccc;
        }

            .toggle-password:hover {
                color: #ff5e57;
            }

        .user-label {
            position: absolute;
            left: 15px;
            top: 30%; /* Để văn bản ở vị trí giữa TextBox */
            color: #ff5e57;
            pointer-events: none;
            transform: translateY(0);
            transition: 150ms cubic-bezier(0.4, 0, 0.2, 1);
            font-size: 14px;
        }


        /* Validator styles */
        .validator-error {
            font-size: 12px; /* Chỉnh sửa font-size của thông báo lỗi */
            color: #808080 /* Giữ màu đỏ cho thông báo lỗi */
        }

        /* Đặt chiều rộng cho nút */
        .btn {
            padding: 10px 12px; /* Đảm bảo padding đầy đủ cho nút */
            border-radius: 5px;
            font-size: 14px;
            font-weight: bold;
            cursor: pointer;
            border: none;
            width: 100%; /* Đặt chiều rộng của nút bằng 100% */
            box-sizing: border-box; /* Đảm bảo padding không ảnh hưởng đến chiều rộng tổng thể */
            /*    min-width: 250px;*/
        }

        .item-button {
            width: 100%;
            display: flex;
            gap: 10px;
            justify-content: center;
            margin-bottom: 5px;
        }

            /* Điều chỉnh padding cho nút */
            .item-button .btn {
                padding: 10px 0; /* Điều chỉnh padding cho nút */
            }

            /* Loại bỏ quy tắc không cần thiết cho input[type="submit"] và input[type="button"] */
            .item-button input[type="submit"],
            .item-button input[type="button"] {
                width: 200px; /* Hoặc bạn có thể bỏ hẳn phần này nếu không cần */
            }

        .label-forgot {
            font-size: 12px;
            text-align: right;
            padding-right: 10px;
            /*    margin-top: -15px;*/
            text-decoration: underline;
            color: #ff5e57;
            font-weight: 600;
            cursor: pointer;
            margin-bottom: -10px;
        }

        p {
            padding-left: 24px;
            margin-top: 30px;
            font-size: 14px;
            color: #696969;
        }


        /* CSS cho radio button */
        input[type="radio"] {
            appearance: none; /* Xóa hình dáng mặc định */
            width: 20px;
            height: 20px;
            border-radius: 50%; /* Viền tròn */
            border: 2px solid #ccc; /* Màu viền xám */
            cursor: pointer;
            transition: background-color 0.3s ease, border-color 0.3s ease; /* Hiệu ứng chuyển màu */
            margin-right: 10px; /* Tạo khoảng cách giữa radio và label */
        }

        /* CSS cho label */
        label {
            font-size: 16px;
            color: #ff5e57; /* Màu chữ cho label */
            cursor: pointer;
            margin-left: 5px; /* Tạo khoảng cách giữa radio button và label */
            line-height: 20px; /* Căn chỉnh chiều cao của label với radio */
        }

        /* Thay đổi màu khi radio được chọn */
        input[type="radio"]:checked {
            background-color: #ff5e57;
            border-color: #ff5e57; /* Màu viền khi chọn */
        }

        /* Thêm hiệu ứng hover cho radio button */
        input[type="radio"]:hover {
            background-color: #ff5e57;
            border-color: #ff5e57;
        }

        table#Content_rblGioiTinh {
            width: 300px;
        }

        .center-message {
            text-align: center;
            color: red;
            font-weight: 400;
            padding: 10px 0;
            display: block;
            width: 100%;
            margin-top: 10px;
        }

        #lblMessage {
            text-align: center;
            color: red;
            font-weight: 400;
            padding: 10px 0;
            display: block;
            width: 100%;
            margin-top: 10px;
        }
    </style>


    <!-- Modal Popup -->
    <div id="loginSuccessModal" class="modal">
        <div class="modal-content">

            <div class="message-text-container">
                <div class="icon-container">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 512 512"
                        stroke-width="0"
                        fill="currentColor"
                        stroke="currentColor"
                        class="icon">
                        <path
                            d="M256 48a208 208 0 1 1 0 416 208 208 0 1 1 0-416zm0 464A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM369 209c9.4-9.4 9.4-24.6 0-33.9s-24.6-9.4-33.9 0l-111 111-47-47c-9.4-9.4-24.6-9.4-33.9 0s-9.4 24.6 0 33.9l64 64c9.4 9.4 24.6 9.4 33.9 0L369 209z">
                        </path>
                    </svg>
                </div>
                <div class="text">
                    <p class="message-text">Đăng ký thành công</p>
                    <p class="sub-text">Trở về trang đăng nhập</p>
                </div>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
        </div>
    </div>

    <div class="table-view">
        <div class="table-content">
            <h4 style="margin-bottom: 30px;">ĐĂNG KÝ</h4>
            <div class="box-info">
                <%--họ--%>
                <div class="box-content">
                    <div class="title-content">
                        <div class="input-group">
                            <!-- TextBox có CSS class "input" -->
                            <asp:TextBox ID="txtHo" runat="server" CssClass="input"></asp:TextBox>
                            <!-- Label đi kèm với TextBox -->
                            <label class="user-label" for="txtHoTen">Họ</label>
                        </div>
                    </div>
                    <div class="content-box">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHo" ErrorMessage="Trường không thể bỏ trống!" CssClass="validator-error"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <%--tên--%>
                <div class="box-content">
                    <div class="title-content">
                        <div class="input-group">
                            <!-- TextBox có CSS class "input" -->
                            <asp:TextBox ID="txtTen" runat="server" CssClass="input"></asp:TextBox>
                            <!-- Label đi kèm với TextBox -->
                            <label class="user-label" for="txtTen">Tên</label>
                        </div>
                    </div>
                    <div class="content-box">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTen" ErrorMessage="Trường không thể bỏ trống!" CssClass="validator-error"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <%--số điện thoại--%>
                <div class="box-content">
                    <div class="title-content">
                        <div class="input-group">
                            <asp:TextBox ID="txtDienThoai" runat="server" CssClass="input"></asp:TextBox>
                            <label class="user-label" for="txtDienThoai">Số điện thoại</label>
                        </div>
                    </div>
                    <div class="content-box">
                        <asp:RegularExpressionValidator
                            ID="revPhone"
                            runat="server"
                            ControlToValidate="txtDienThoai"
                            ErrorMessage="Số điện thoại phải có 10 chữ số!"
                            ValidationExpression="^0\d{9}$"
                            CssClass="validator-error">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

                <%--Email--%>
                <div class="box-content">
                    <div class="title-content">
                        <div class="input-group">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="input"></asp:TextBox>
                            <label class="user-label" for="txtEmail">Email</label>
                        </div>
                    </div>
                    <div class="content-box">
                        <asp:RegularExpressionValidator
                            ID="revEmail"
                            runat="server"
                            ControlToValidate="txtEmail"
                            ErrorMessage="Email không hợp lệ!"
                            ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                            CssClass="validator-error">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>

            <div class="box-info" style="margin-top: 18px;">
                <%--Giới tính--%>
                <asp:RadioButtonList ID="rblGioiTinh" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Nam" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Nữ" Value="0"></asp:ListItem>
                </asp:RadioButtonList>

                <%--ngày sinh--%>
                <div class="box-content">
                    <div class="title-content">
                        <div class="input-group">
                            <!-- TextBox có CSS class "input" -->
                            <asp:TextBox ID="txtNgaySinh" runat="server" CssClass="input"></asp:TextBox>
                            <!-- Label đi kèm với TextBox -->
                            <label class="user-label" for="txtNgaySinh">Ngày sinh (yyyy-MM-dd)</label>
                        </div>
                    </div>
                    <div class="content-box">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtNgaySinh" ErrorMessage="Trường không thể bỏ trống!" CssClass="validator-error"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <%--Địa chỉ--%>
            <div class="box-content" style="margin-top: 18px;">
                <div class="box-content-full">
                    <div class="input-group">
                        <!-- TextBox có CSS class "input" -->
                        <asp:TextBox ID="txtDiaChi" runat="server" CssClass="input"></asp:TextBox>
                        <!-- Label đi kèm với TextBox -->
                        <label class="user-label" for="txtDiaChi">Địa chỉ</label>
                    </div>
                </div>
                <div class="content-box">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDiaChi" ErrorMessage="Trường không thể bỏ trống!" CssClass="validator-error"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="box-info" style="margin-top: 18px;">
                <%--mật khẩu--%>
                <div class="box-content">
                    <div class="title-content">
                        <div class="input-group">
                            <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" CssClass="input" />
                            <label class="user-label" for="txtMatKhau">Mật khẩu</label>
                            <span class="toggle-password" onclick="togglePassword(this)"><i class="bi bi-eye-fill"></i>
                        </div>
                    </div>
                    <div class="content-box">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMatKhau" ErrorMessage="Mật khẩu không được bỏ trống!" CssClass="validator-error"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <%--xác nhận mật khẩu--%>
                <div class="box-content">
                    <div class="title-content">
                        <div class="input-group">
                            <asp:TextBox ID="txtXacNhan" runat="server" TextMode="Password" CssClass="input" />
                            <label class="user-label" for="txtXacNhan">Xác nhận mật khẩu</label>
                            <span class="toggle-password" onclick="togglePassword(this)"><i class="bi bi-eye-fill"></i>
                        </div>
                    </div>
                    <div class="content-box">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtXacNhan" ErrorMessage="Mật khẩu không được bỏ trống!" CssClass="validator-error"></asp:RequiredFieldValidator>


                    </div>
                </div>
            </div>

            <div class="item-button" style="justify-content: center">
                <div class="item-btn-add">
                    <asp:Button ID="butDangKy" runat="server" Text="Đăng ký" CssClass="btn" OnClick="butDangKy_Click"></asp:Button>
                </div>

                <div class="item-btn-back">
                    <asp:Button ID="butCancel" Text="Quay lại" runat="server" PostBackUrl="~/Page_Customer/DangNhap.aspx" CausesValidation="false" Style="font-weight: bold;" />
                </div>
            </div>
            <div class="center-message">
                <div class="matkhau-thongbao">
                    <asp:CompareValidator
                        ID="cvPassword"
                        runat="server"
                        ControlToValidate="txtXacNhan"
                        ControlToCompare="txtMatKhau"
                        Operator="Equal"
                        Type="String"
                        ErrorMessage="Mật khẩu xác nhận không khớp!"
                        ForeColor="#d10d0d">
                    </asp:CompareValidator>
                </div>

                <div class="email-thongbao">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="#d10d0d"></asp:Label>
                </div>

            </div>

        </div>
    </div>

    <script>
        $(document).ready(function () {
            // Khi focus vào input, ẩn label
            $('.input').on('focus', function () {
                $(this).next('.user-label').css({
                    'transform': 'translateY(-70%) scale(0.6)',
                    'padding': '0 .2em',
                    'color': '#ff5e57'

                });
            });

            // Khi mất focus và không có giá trị, label trở lại vị trí ban đầu và có màu #ff5e57
            $('.input').on('blur', function () {
                var inputVal = $(this).val();
                if (inputVal === '') {
                    $(this).next('.user-label').css({
                        'transform': 'translateY(0)',   // Trở về vị trí ban đầu
                        'background-color': 'none',
                        'padding-bottom': '',
                        'color': '#ff5e57'  // Màu #ff5e57 khi không có dữ liệu
                    });
                }
            });
        });

        function togglePassword(element) {
            var passwordField = element.previousElementSibling.previousElementSibling; // Tìm input trong cùng input-group
            var eyeIcon = element.querySelector("i"); // Lấy icon trong button

            if (passwordField.type === "password") {
                passwordField.type = "text";
                eyeIcon.classList.remove("bi-eye-fill");
                eyeIcon.classList.add("bi-eye-slash-fill");
            } else {
                passwordField.type = "password";
                eyeIcon.classList.remove("bi-eye-slash-fill");
                eyeIcon.classList.add("bi-eye-fill");
            }
        }
    </script>

    <%--hiệu ứng popup--%>
    <script>
        // Hiển thị modal
        function showModal() {
            document.getElementById("loginSuccessModal").style.display = "block";
            setTimeout(function () {
                closeModal();
                window.location.href = "DangNhap.aspx"; // Chuyển hướng sau khi đóng modal
            }, 1000); // 1 giây
        }

        // Đóng modal
        function closeModal() {
            document.getElementById("loginSuccessModal").style.display = "none";
        }

        // Đảm bảo đóng modal khi nhấn ra ngoài modal
        window.onclick = function (event) {
            if (event.target == document.getElementById("loginSuccessModal")) {
                closeModal();
            }
        }
    </script>
</asp:Content>
