<%@ Page Title="" Language="C#" MasterPageFile="~/Header_Footer.Master" AutoEventWireup="true" CodeBehind="TrangChu.aspx.cs" Inherits="QLBH_TeemoSneaker.Page_Customer.TrangChu" %>

<%------------------------------header------------------------%>
<asp:Content ID="Header" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="ThanhTimKiem" ContentPlaceHolderID="ThanhTimKiem_Header" runat="server">
    <div class="search-bar" style="width: 200px;">
        <asp:TextBox ID="txtTimKiem" runat="server" CssClass="form-control search-input" placeholder="Tìm kiếm ở đây"></asp:TextBox>
        <asp:LinkButton ID="btnTimKiem" runat="server" CssClass="btn-search" OnClick="btnTimKiem_Click" Style="background-color: #dc3545;">
            <i class="bi bi-search"></i>
        </asp:LinkButton>
    </div>
</asp:Content>

<%--Giỏ hàng--%>
<asp:Content ID="GioHang" ContentPlaceHolderID="GioHang_Header" runat="server">
    <asp:LinkButton ID="btnGioHang" runat="server" CssClass="cart-icon position-relative" OnClick="btnGioHang_Click" OnClientClick="window.location.href='GioHang.aspx'; return false;">
        <i class="bi bi-cart"></i>
        <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
            <asp:Label ID="lblCartCount" runat="server" Text="0"></asp:Label>
        </span>
    </asp:LinkButton>
</asp:Content>

<%--người dùng--%>
<asp:Content ID="NguoiDung" ContentPlaceHolderID="NguoiDung_Header" runat="server">
    <asp:LinkButton ID="btnNguoiDung" runat="server" CssClass="cart-icon position-relative" OnClick="btnNguoiDung_Click" OnClientClick="window.location.href='DangNhap.aspx'; return false;">
        <i class="bi bi-people"></i>
    </asp:LinkButton>
</asp:Content>


<%--đăng xuất--%>
<asp:Content ID="DangXuat" ContentPlaceHolderID="DangXuat_Header" runat="server">
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
            gap: 32px;
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
                    <p class="message-text">Đã đăng xuất tài khoản</p>
                    <p class="sub-text">Trở về trang chủ</p>
                </div>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
        </div>
    </div>

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

    <asp:LinkButton ID="btnDangXuat" runat="server" CssClass="cart-icon position-relative" OnClick="btnDangXuat_Click">
        <i class="bi bi-door-open"></i>
    </asp:LinkButton>


</asp:Content>

<%------------------------------content-----------------------%>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <%--ảnh nền--%>
    <div class="slider-background">
        <div class="background">
            <div class="slide">
                <img src="/Image/background4.jpg" alt="Ảnh 1"></div>
            <div class="slide">
                <img src="/Image/background1.jpg" alt="Ảnh 2"></div>
            <div class="slide">
                <img src="/Image/background2.jpg" alt="Ảnh 3"></div>
        </div>

        <button class="prev" type="button" onclick="moveSlide(-1)">&#10094;</button>
        <button class="next" type="button" onclick="moveSlide(1)">&#10095;</button>
    </div>

    <%--cuộn hình ảnh--%>
    <script>

        let currentIndex = 0;
        const slides = document.querySelectorAll(".slide");
        const totalSlides = slides.length;
        const background = document.querySelector(".background");

        document.querySelector(".next").addEventListener("click", () => {
            if (currentIndex < totalSlides - 1) {
                currentIndex++;
            } else {
                currentIndex = 0; // Quay về ảnh đầu tiên nếu đã đến ảnh cuối
            }
            background.style.transform = `translateX(-${currentIndex * 100}%)`;
        });

        document.querySelector(".prev").addEventListener("click", () => {
            if (currentIndex > 0) {
                currentIndex--;
            } else {
                currentIndex = totalSlides - 1; // Quay về ảnh cuối nếu đang ở ảnh đầu
            }
            background.style.transform = `translateX(-${currentIndex * 100}%)`;
        });

    </script>


    <%--giới thiệu--%>
    <div id="introduce-content">
        <div id="band-section">
            <div class="introduce-content-section">
                <h2 class="section-text-heading">TEEMO SNEAKER</h2>
                <p class="section-sub-text-heading">Bước chất – Chất từng bước!</p>
                <p class="section-content-band">
                    Giầy dép là một trong những phụ kiện không thể thiếu góp phần tạo nên phong cách thời trang của mỗi người. Chính vì điều đó Teemo Sneaker mong muốn đem đến những mẫu giày mới nhất được cập nhật thường xuyên giúp các bạn dễ dàng hơn trong việc lựa chọn và cập nhật những style thịnh hành của thời trang giầy dép trong nước và thế giới.<br />

                    <br />
                    Teemo Sneaker có hàng hóa đa dạng với giày, dép, túi ... sẵn sàng phục vụ quý khách nhanh chóng. Ngoài việc đặt hàng trên website, quý khách hàng có thể đến chọn sản phẩm trực tiếp ở cửa hàng tại 63/19 Nguyễn Hữu Cầu Phường Tân Định, Quận 1, TPHCM.
                </p>

                <div class="band-list">
                    <div class="band-member">
                        <p class="member-name">Cửa hàng chúng tôi</p>
                        <img src="/Image/pic1.jpg" alt="Cửa hàng của tôi" class="member-img">
                    </div>

                    <div class="band-member">
                        <p class="member-name">Khách hàng chúng tôi</p>
                        <img src="/Image/pic2.jpg" alt="Khách hàng của tôi" class="member-img">
                    </div>

                    <div class="band-member">
                        <p class="member-name">Chúng tôi</p>
                        <img src="/Image/pic3.jpg" alt="Chúng tôi" class="member-img">
                    </div>

                    <!-- thêm thẻ này vào item cuối cùng trong danh sách, vì dùng thuộc tính float sẽ ko bao trọn được div con trong đó. Vì vậy phải css cho class này. -->
                    <div class="clear"></div>
                </div>
            </div>
        </div>
    </div>


    <div id="boloc-danhsach">
        <%--bộ lọc--%>
        <div class="container">
            <div class="filter-container">
                <asp:Label ID="Label2" runat="server" Text="LOẠI" CssClass="Label2"></asp:Label>
                <asp:RadioButtonList ID="LoaiSanPham" runat="server" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="LoaiGiay" DataTextField="TenLoaiGiay" DataValueField="MaLoaiGiay" OnSelectedIndexChanged="LoaiSanPham_SelectedIndexChanged" CssClass="radio-input" RepeatLayout="Flow">
                    <asp:ListItem Value="*">Tất cả</asp:ListItem>
                </asp:RadioButtonList>
                <asp:SqlDataSource ID="LoaiGiay" runat="server" ConnectionString="<%$ ConnectionStrings:conn %>" SelectCommand="SELECT * FROM [tblLOAIGIAY]"></asp:SqlDataSource>


                <asp:Label ID="Label1" runat="server" Text="SẮP XẾP" CssClass="Label1"></asp:Label>
                <asp:RadioButtonList ID="Xeptheo" runat="server" OnSelectedIndexChanged="Xeptheo_SelectedIndexChanged" AutoPostBack="True" CssClass="radio-input" RepeatLayout="Flow">
                    <asp:ListItem Value="*">Mặc định</asp:ListItem>
                    <asp:ListItem Value="AZ">Thứ tự A - Z</asp:ListItem>
                    <asp:ListItem Value="ZA">Thứ tự Z - A</asp:ListItem>
                    <asp:ListItem Value="GTD">Giá tăng dần</asp:ListItem>
                    <asp:ListItem Value="GDD">Giá giảm dần</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>

        <%--danh sách sản phẩm--%>
        <div class="content-wrapper">
            <asp:DataList ID="listSanPham" runat="server" DataKeyField="MaGiay" DataSourceID="SqlDataSource_listSanPham" RepeatColumns="3" RepeatDirection="Horizontal" Style="text-align: center" Width="100%" CssClass="listItem">
                <ItemTemplate>

                    <asp:HyperLink ID="HyperLink1" runat="server"
                        NavigateUrl='<%# "ChiTietSanPham.aspx?MaGiay=" + Eval("MaGiay") %>'>
                        <asp:Image CssClass="product-item" ID="imgSanPham" runat="server" Height="279px"
                            ImageUrl='<%# Eval("HinhAnh") %>' Width="260px" />
                    </asp:HyperLink>
                    <br />
                    <asp:HyperLink ID="linkSanPham" runat="server"
                        NavigateUrl='<%# "ChiTietSanPham.aspx?MaGiay=" + Eval("MaGiay") %>'
                        Text='<%# Eval("TenGiay") %>' CssClass="design-name">
                    </asp:HyperLink>
                    <br />

                    <asp:Label ID="lblDonGia" runat="server"
                        Text='<%# Eval("GiaGiay","{0:0,000 VNĐ}") %>' CssClass="design-price">
                    </asp:Label>

                    <br />
                </ItemTemplate>
            </asp:DataList>

            <asp:SqlDataSource ID="SqlDataSource_listSanPham" runat="server" ConnectionString="<%$ ConnectionStrings:conn %>"></asp:SqlDataSource>
        </div>
    </div>

    <%--cuộn trang khi chọn bộ lọc--%>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            var radioButtons = document.querySelectorAll("input[type='radio']");
            var storedValue = localStorage.getItem("selectedRadio");

            // Nếu có lựa chọn trước đó, thiết lập giá trị cho radio button
            if (storedValue) {
                for (var i = 0; i < radioButtons.length; i++) {
                    if (radioButtons[i].value === storedValue) {
                        radioButtons[i].checked = true;
                        break;
                    }
                }
            }

            // Khi người dùng thay đổi lựa chọn
            radioButtons.forEach(function (radio) {
                radio.addEventListener("change", function () {
                    localStorage.setItem("selectedRadio", this.value);
                    localStorage.setItem("scrollToFilter", "true");
                });
            });

            // Nếu cần cuộn, thực hiện cuộn
            if (localStorage.getItem("scrollToFilter") === "true") {
                var element = document.getElementById("boloc-danhsach");
                if (element) {
                    element.scrollIntoView({ behavior: "smooth", block: "start" });
                }
                localStorage.removeItem("scrollToFilter"); // Xóa flag sau khi cuộn
            }
        });
    </script>

</asp:Content>

