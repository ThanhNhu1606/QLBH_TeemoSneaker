<%@ Page Title="" Language="C#" MasterPageFile="~/Header_Footer.Master" AutoEventWireup="true" CodeBehind="DangNhap.aspx.cs" Inherits="QLBH_TeemoSneaker.Page_Customer.TaiKhoanKH" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ThanhTimKiem_Header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="GioHang_Header" runat="server">
</asp:Content>
<asp:Content ID="DangNhap" ContentPlaceHolderID="Content" runat="server">
    <%--css nội dung trang--%>
    <style>
        h4 {
            text-align: center;
            color: #ff5e57;
            margin-bottom: 15px;
            font-family: 'themify';
        }

        /* Căn giữa table-view */
        .table-view {
            width: 30%; /* Hoặc bạn có thể điều chỉnh theo mong muốn */
            margin: 0 auto; /* Căn giữa theo chiều ngang */
            padding: 20px;
            /*border: 1px solid #ddd;*/ /* Có thể thêm border để nhìn rõ */
            border-radius: 20px;
            /*      background-color: #f9f9f9;*/
            background-color: #ffffff;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2); /* Tạo bóng nhẹ cho box */
            margin-top: 50px;
        }

        /* Căn đều nội dung trong table-content */
        .table-content {
            text-align: justify; /* Căn đều hai bên */
            padding: 15px;
        }

        .box-info {
            /*    width: 100%;*/
            display: flex;
            flex-direction: column;
            align-items: center; /* Căn giữa nội dung theo chiều ngang */
            padding: 10px;
        }

        /* Căn giữa box-content và thu nhỏ kích thước */
        .box-content {
            width: 100%;
        }

        .title-content {
            font-size: 14px;
        }

        .input-group {
            position: relative;
            width: 100%;
            display: flex;
            align-items: center;
        }

        .input {
            border: solid 1.5px #9e9e9e;
            background: none;
            padding: 10px; /* Giảm padding của input để nhỏ gọn hơn */
            padding-top: 20px; /* Cải thiện padding-top cho các label */
            padding-right: 40px;
            font-size: 14px; /* Giảm kích thước chữ trong input */
            color: #333;
            width: 100%;
            transition: border 150ms cubic-bezier(0.4, 0, 0.2, 1);
            box-sizing: border-box; /* Giúp padding không làm tăng kích thước */
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
            color: #999;
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
            width: 100%; /* Đảm bảo phần tử cha chiếm 100% chiều rộng */
            display: flex;
            justify-content: center; /* Căn giữa nút */
        }

            /* Điều chỉnh padding cho nút */
            .item-button .btn {
                padding: 10px 0; /* Điều chỉnh padding cho nút */
            }

            /* Loại bỏ quy tắc không cần thiết cho input[type="submit"] và input[type="button"] */
            .item-button input[type="submit"],
            .item-button input[type="button"] {
                width: 325px; /* Hoặc bạn có thể bỏ hẳn phần này nếu không cần */
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

        .lblDangKy {
            font-weight: 700;
            text-decoration: underline;
            cursor: pointer;
            color: #ff5e57;
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
    </style>
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
                    <p class="message-text">Đăng nhập thành công</p>
                    <p class="sub-text">Trở về trang chủ</p>
                </div>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
        </div>
    </div>

    <%--nội dung trang--%>
    <div class="table-view">
        <div class="table-content">
            <h4>ĐĂNG NHẬP</h4>
            <div class="box-big">
            </div>
            <div class="box-info">
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

                <div class="box-content">
                    <div class="title-content">
                        <div class="input-group">
                            <asp:TextBox ID="txtMatKhau" runat="server" TextMode="Password" CssClass="input" />
                            <label class="user-label" for="txtMatKhau">Mật khẩu</label>
                            <span class="toggle-password" onclick="togglePassword()">
                                <i id="eyeIcon" class="bi bi-eye-fill"></i>
                            </span>
                        </div>


                    </div>
                    <div class="content-box">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMatKhau" ErrorMessage="Mật khẩu không được bỏ trống!" CssClass="validator-error"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

            <div class="label-forgot">
                <asp:HyperLink ID="lblForgot" runat="server" NavigateUrl="~/Page_Customer/ThayDoiMatKhau.aspx" Style="color: #ff5e57;">Bạn muốn thay đổi mật khẩu?</asp:HyperLink>
            </div>
        </div>

        <div class="item-button" style="justify-content: center">
            <div class="item-btn-add">
                <asp:Button ID="butDangNhap" runat="server" Text="Đăng nhập" CssClass="btn" OnClick="butDangNhap_Click"></asp:Button>
            </div>
        </div>

        <p>
            Bạn chưa có tài khoản? <span>
                <asp:HyperLink ID="lblDangKy" runat="server" NavigateUrl="DangKy.aspx" CssClass="lblDangKy">Đăng ký</asp:HyperLink></span>
        </p>
        <asp:Label ID="lblMessage" runat="server" CssClass="center-message"></asp:Label>
    </div>

    <%--hiệu ứng textbox--%>
    <script>
        $(document).ready(function () {
            // Khi focus vào input, ẩn label
            $('.input').on('focus', function () {
                $(this).next('.user-label').css({
                    'transform': 'translateY(-80%) scale(0.8)',

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
                        'padding': '0',
                        'color': '#ff5e57'  // Màu #ff5e57 khi không có dữ liệu
                    });
                }
            });
        });


        function togglePassword() {
            var passwordField = document.getElementById('<%= txtMatKhau.ClientID %>');
            var eyeIcon = document.getElementById("eyeIcon");

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
                window.location.href = "TrangChu.aspx"; // Chuyển hướng sau khi đóng modal
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
