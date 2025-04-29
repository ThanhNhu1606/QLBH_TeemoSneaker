<%@ Page Title="" Language="C#" MasterPageFile="~/Header_Footer.Master" AutoEventWireup="true" CodeBehind="ChiTietSanPham.aspx.cs" Inherits="QLBH_TeemoSneaker.Page_Customer.ChiTietSanPham" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ThanhTimKiem_Header" runat="server">
</asp:Content>
<asp:Content ID="GioHang" ContentPlaceHolderID="GioHang_Header" runat="server">
    <asp:LinkButton ID="btnGioHang" runat="server" CssClass="cart-icon position-relative" OnClientClick="window.location.href='GioHang.aspx'; return false;">
        <i class="bi bi-cart"></i>
         <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
        <asp:Label ID="lblCartCount" runat="server" Text="0"></asp:Label>
    </span>
    </asp:LinkButton>
</asp:Content>

<asp:Content ID="ChiTietSanPham" ContentPlaceHolderID="Content" runat="server">
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
         gap: 35px;
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
                 <p class="message-text">Đã thêm vào giỏ hàng</p>
                 <p class="sub-text">Tiếp tục mua sắm</p>
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
        }, 3000); // 1 giây
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

    <!-- Nội dung chi tiết sản phẩm -->
    <div class="item-review">
        <div class="img-item">
            <asp:Image ID="imgHinh" runat="server" />
            <div class="item-id">
    <asp:Label ID="lblMaSanPham" runat="server"></asp:Label>
</div>
        </div>

        <div class="item-content">
            

            <div class="item-title">
                <h3>
                    <asp:Label ID="lblTenSanPham" runat="server"> </asp:Label>
                </h3>
            </div>

            <div class="item-price">
                <asp:Label ID="lblDonGiaBan" runat="server"></asp:Label>
            </div>

            <div class="item-size">
    <asp:Label ID="lblSize" runat="server"></asp:Label>
</div>
            <div class="soluong-conlai">
                <div class="quantity-container">
<asp:Button ID="btnMinus" runat="server" Text="-" CssClass="quantity-btn" OnClick="btnMinus_Click" />
<asp:TextBox ID="txtSoluong" runat="server" CssClass="quantity-input" Text="1" ReadOnly="true"></asp:TextBox>
<asp:Button ID="btnPlus" runat="server" Text="+" CssClass="quantity-btn" OnClick="btnPlus_Click" />  
</div>
                <div class="item-conlai">
    <asp:Label ID="lblSoLuongTon" runat="server" Text="Số lượng tồn"></asp:Label>
</div>
            </div>

            <div class="item-button">
                <div class="item-btn-add">
                     <asp:Button ID="butInsert" runat="server" Text="Thêm vào giỏ hàng" OnClick="butInsert_Click" />
                </div>

                <div class="item-btn-back">
                    <asp:Button ID="butCancel" Text="Quay về trang chủ" runat="server" OnClick="butCancel_Click" />
                </div>
            </div>
 
        </div>
    </div>
    
    <div class="item-dacdiem">

    <div class="item-lblDacDiem">
        <h5>Thông tin chi tiết</h5>
    </div>

    <div class="item-content-dacdiem">
        <asp:Label ID="lblGhiChu" runat="server"></asp:Label>
    </div>
</div>
</asp:Content>
