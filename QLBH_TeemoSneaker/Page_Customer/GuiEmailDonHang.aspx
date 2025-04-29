<%@ Page Title="" Language="C#" MasterPageFile="~/Header_Footer.Master" AutoEventWireup="true" CodeBehind="GuiEmailDonHang.aspx.cs" Inherits="QLBH_TeemoSneaker.Page_Customer.GuiEmailDonHang" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ThanhTimKiem_Header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="GioHang_Header" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">

    <style>
         /* Căn giữa tiêu đề h2 */
 h4 {
     text-align: center;
     color: #ff5e57;
     margin-bottom: 15px;
     font-family: 'themify';
 }

 em {
    color: #ff5e57;
    font-weight: 600;
}

 strong {
    font-weight: 600;
}

  /* Căn giữa table-view */
    .table-view {
        width: 50%; /* Hoặc bạn có thể điều chỉnh theo mong muốn */
        margin: 0 auto; /* Căn giữa theo chiều ngang */
        padding: 20px;
        /*border: 1px solid #ddd;*/ /* Có thể thêm border để nhìn rõ */
        border-radius: 10px;
        background-color: #f9f9f9;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1); /* Tạo bóng nhẹ cho box */
    }

    /* Căn đều nội dung trong table-content */
    .table-content {
        text-align: justify; /* Căn đều hai bên */
        padding: 15px;
    }
    </style>

     <h4>THÔNG BÁO</h4>
    <div class="table-view">
        <div class="table-content">
            <asp:Label ID="lblThongBao" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>
