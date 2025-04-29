<%@ Page Title="" Language="C#" MasterPageFile="~/Header_Footer_Admin.Master" AutoEventWireup="true" CodeBehind="NhanVien.aspx.cs" Inherits="QLBH_TeemoSneaker.Page_Admin.NhanVien" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="IDThongKe" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="IDLoaiSanPham" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="IDSanPham" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="IDNhaCungCap" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="IDNhanven" runat="server">

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

        .icon-minipopup {
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
        .table-view {
            margin: auto;
            width: fit-content;
            text-align: center;
        }

            .table-view table tr {
                height: 50px !important; /* Chiều cao dòng */
            }

            .table-view table td,
            .table-view table th {
                padding-top: 10px !important;
                padding-bottom: 10px !important;
            }

        #dsSanPham th,
        #dsSanPham td {
            text-align: center;
            vertical-align: middle;
            /*            padding: 10px;*/
            padding: 15px 12px !important;
        }

        h4 {
            text-align: center;
            color: #ff5e57;
            margin: 20px 0 20px 0;
            font-family: 'themify';
        }

        #lblTongTien,
        #butThemMoi,
        #butDathang {
            display: block;
            text-align: center;
            margin: 10px auto;
        }

        .btn {
            padding: 6px 12px;
            border-radius: 5px;
            font-size: 14px;
            font-weight: bold;
            cursor: pointer;
            border: none;
        }

        .btn-warning:hover {
            background-color: var(--warning); /* màu nền khi hover */
            color: white; /* chữ trắng */
            border-color: var(--warning); /* viền cùng màu */
        }

        .btn-danger:hover {
            background-color: var(--danger);
            color: white;
            border-color: var(--danger);
        }

        .item-button input[type="submit"],
        .item-button input[type="button"] {
            width: 150px;
        }

        .btn-warning {
            /*            background-color: #f39c12;
            color: white;*/

            background-color: rgba(255, 209, 102, 0.1);
            color: var(--warning);
            border: 2px solid var(--warning);
        }

        .btn-danger {
            /*            background-color: #e74c3c;
            color: white;*/

            background-color: rgba(239, 71, 111, 0.1);
            color: var(--danger);
            border: 2px solid var(--danger);
        }

        .btn-success {
            background-color: #2ecc71;
            color: white;
            width: 80px;
            margin-bottom: 5px;
        }

        .btn-secondary {
            background-color: #95a5a6;
            color: white;
            width: 80px;
        }

        .wrap-text {
            white-space: normal !important;
            word-break: break-word;
            display: block;
            text-align: left;
        }

        .form-control {
            width: 100%;
            padding: 0;
            margin: 0;
            font-family: inherit;
            font-size: inherit;
        }

        .custom-link-button {
            padding: 10px;
            font-size: 15px;
            color: #ffffff;
            border: none;
            border-radius: 10px;
            box-shadow: 0px 8px 15px rgba(0, 0, 0, 0.1);
            transition: all 0.3s ease 0s;
            cursor: pointer;
            outline: none;
            margin-top: 8px;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background-color: rgba(87, 204, 153, 0.1);
            color: var(--success);
            border: 2px solid var(--success);
        }

            .custom-link-button:hover {
                background-color: #23c483;
                box-shadow: 0px 15px 20px rgba(46, 229, 157, 0.4);
                transform: translateY(-7px);
                color: #fff;
            }

            .custom-link-button:active {
                transform: translateY(-1px);
            }

        .box-info {
            display: flex;
            flex-wrap: wrap; /* Cho phép các phần tử nằm trên nhiều hàng */
            gap: 20px; /* Khoảng cách giữa các ô */
            justify-content: space-between; /* Các ô được căn chỉnh đều nhau */
        }


        /* Cho các ô nhóm vào hàng ngang */
        .box-content {
            /*flex: 1 1 48%;*/ /* Cứ 2 ô sẽ chiếm 48% chiều rộng */
            flex: 1 1 calc(50% - 10px); /* Mỗi ô chiếm 50% trừ đi khoảng cách */
            box-sizing: border-box;
        }

        .box-info .box-content {
            display: flex;
            flex-direction: column;
        }

        .title-content {
            font-size: 14px;
        }


        .input-group {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 10px;
        }

        .input {
            width: 100%;
            padding: 10px;
            /*padding: 20px 10px 10px 10px;*/
            font-size: 14px;
            border: 1px solid #9e9e9e;
            box-sizing: border-box;
        }

            .input:focus, .input:valid {
                outline: none;
                border: 1.5px solid #ff5e57;
            }


        /* Nếu là textarea */
        textarea.input {
            min-height: 80px; /* hoặc tuỳ chỉnh theo Rows */
            padding-top: 40px; /* tăng thêm để tránh label đè lên dòng đầu */
        }


        .user-label {
            position: absolute;
            left: 10px;
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

        .label-forgot {
            font-size: 12px;
            text-align: right;
            padding-right: 10px;
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


        /* CSS cho label */
        label {
            font-size: 16px;
            color: #ff5e57; /* Màu chữ cho label */
            cursor: pointer;
            margin-left: 5px; /* Tạo khoảng cách giữa radio button và label */
            line-height: 20px; /* Căn chỉnh chiều cao của label với radio */
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

        .search-box {
            position: relative;
            width: 250px;
        }

            .search-box input {
                padding: 10px 40px 10px 14px; /* padding phải là 40px để chừa chỗ cho nút */
                border: 1px solid #ccc;
                border-radius: 8px;
                background-color: white;
                width: 100%;
                font-size: 14px;
                height: 40px;
                outline: none;
                transition: all 0.2s;
                box-sizing: border-box;
            }

                .search-box input:focus {
                    border-color: #dc3545;
                    box-shadow: 0 0 0 3px rgba(220, 53, 69, 0.1);
                }

            .search-box .btn-search {
                position: absolute;
                top: 50%;
                right: 0; /* điều chỉnh độ sát mép */
                transform: translateY(-50%);
                background-color: var(--primary);
                border: none;
                height: 40px;
                width: 40px;
                border-top-right-radius: 6px; /* Bo góc trên bên phải */
                border-bottom-right-radius: 6px; /* Bo góc dưới bên phải */
                cursor: pointer;
                display: flex;
                justify-content: center;
                align-items: center;
                padding: 0;
            }

        .header-actions {
            display: flex;
            align-items: center;
            gap: 40px;
        }

        .search-box .btn-search i {
            font-size: 14px;
            color: white;
        }

        .search-box .btn-search:hover {
            background-color: #23c483;
            box-shadow: 0px 15px 20px rgba(46, 229, 157, 0.4);
        }

        /* Áp dụng cho header */
        .table-view .item-table .GridViewStyle th {
            border-bottom: 1px solid inherit;
        }

        /* Áp dụng cho các dòng dữ liệu */
        .table-view .item-table .GridViewStyle td {
            border-bottom: 1px solid inherit;
        }

        /* Để giúp nhìn rõ hơn, có thể dùng fallback border màu cụ thể */
        .table-view .item-table .GridViewStyle th,
        .table-view .item-table .GridViewStyle td {
            border-bottom: 1px solid #ccc; /* hoặc dùng var(--your-border-color) */
        }

        .GridViewStyle {
            border: none;
            border-collapse: collapse;
        }

            .GridViewStyle th,
            .GridViewStyle td {
                border: none;
                border-bottom: 1px solid #ccc;
            }

        .item-table {
            /* box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2); */
            /* border-radius: 20px; */
            /* border: 1px solid #ddd; */
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 20px;
            /* background-color: #f9f9f9; */
            background-color: #ffffff;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
        }
    </style>

    <style>
   .gioitinh-list label:first-of-type {
    margin-right: 20px;
}
</style>


    <asp:ScriptManager ID="ScriptManager1" runat="server" />

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
                        class="icon-minipopup">
                        <path
                            d="M256 48a208 208 0 1 1 0 416 208 208 0 1 1 0-416zm0 464A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM369 209c9.4-9.4 9.4-24.6 0-33.9s-24.6-9.4-33.9 0l-111 111-47-47c-9.4-9.4-24.6-9.4-33.9 0s-9.4 24.6 0 33.9l64 64c9.4 9.4 24.6 9.4 33.9 0L369 209z">
                        </path>
                    </svg>
                </div>
                <div class="text">
                    <p class="message-text">Thao tác thành công</p>
                    <p class="sub-text">Tiếp tục thực hiện</p>
                </div>
                <span class="close" onclick="closeModal()">&times;</span>
            </div>
        </div>
    </div>

    <div class="main-content">
        <div class="header">
            <div class="page-title">
                <h1>Quản lý danh mục nhân viên</h1>
                <div class="page-subtitle">
                    Xem danh sách tại đây!
                </div>
            </div>

            <div class="header-actions">
                <div class="search-box">
                    <asp:TextBox ID="txtTimKiem" runat="server" CssClass="form-control search-input" placeholder="Tìm kiếm ở đây..."></asp:TextBox>
                    <asp:LinkButton ID="btnTimKiem" runat="server" CssClass="btn-search" OnClick="btnTimKiem_Click" ToolTip="Tìm kiếm">
        <i class="bi bi-search"></i>
                    </asp:LinkButton>
                </div>


                <div class="item-button" style="justify-content: center">
                    <div class="item-btn-add">
                        <asp:LinkButton ID="butThemMoi" runat="server" CssClass="custom-link-button" OnClick="butThemMoi_Click">
                  <i class="bi bi-plus-circle-fill me-1"></i> Thêm mới
                        </asp:LinkButton>
                    </div>

                                                            <div class="item-btn-add">
                        <asp:LinkButton ID="butExcel" runat="server" CssClass="custom-link-button" OnClick="butExcel_Click">
<i class="bi bi-file-earmark-spreadsheet-fill"></i> Xuất Excel
                        </asp:LinkButton>
                    </div>
                </div>

            </div>
        </div>

        <%-- BorderColor="#ffffff"
  BorderStyle="Double"
  BorderWidth="3px"--%>
        <div class="table-view">
            <%--<div class="label-table">
            <h4>DANH SÁCH LOẠI SẢN PHẨM</h4>
        </div>--%>
            <div class="item-table">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="dsSanPham" runat="server" DataKeyNames="MaNV" CssClass="GridViewStyle"
                            AutoGenerateColumns="False"
                            OnRowEditing="dsSanPham_RowEditing"
                            OnRowUpdating="dsSanPham_RowUpdating"
                            OnRowCancelingEdit="dsSanPham_RowCancelingEdit"
                            OnRowDeleting="dsSanPham_RowDeleting"
                            DataSourceID="SqlDataSourceLoaiGiay"
                            ShowHeaderWhenEmpty="True"
                            CellPadding="4"
                            BackColor="White"
                            GridLines="Horizontal"
                            Width="1000px">

                            <Columns>
                                <%--mã chức vụ--%>
                                <asp:TemplateField HeaderText="Chức vụ">
                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblMaChucVu" runat="server"
                                            Text='<%# Eval("MaChucVu") %>' />
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlChucVu" runat="server"
                                            CssClass="form-control"
                                            SelectedValue='<%# Bind("MaChucVu") %>'
                                            DataSourceID="dsChucVu"
                                            DataTextField="TenChucVu"
                                            DataValueField="MaChucVu"
                                            Style="width: 80px;" />
                                    </EditItemTemplate>
                                </asp:TemplateField>


                                <%--mã --%>
                                <asp:BoundField DataField="MaNV" HeaderText="Mã NV" ReadOnly="True">
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>

                                <%--họ--%>
                                <asp:TemplateField HeaderText="Họ">
                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblHo" runat="server"
                                            Text='<%# Eval("HoNV") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtHoNV" runat="server"
                                            Text='<%# Bind("HoNV") %>' CssClass="form-control"
                                            TextMode="MultiLine" Rows="2"
                                            Style="width: 80px;" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%--tên nhân viên--%>
                                <asp:TemplateField HeaderText="Tên">
                                    <ItemStyle Width="60px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTen" runat="server"
                                            Text='<%# Eval("TenNV") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtTenNV" runat="server"
                                            Text='<%# Bind("TenNV") %>' CssClass="form-control"
                                            Style="width: 50px;" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%--số điện thoại--%>
                                <asp:TemplateField HeaderText="SĐT">
                                    <ItemStyle Width="90px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSdt" runat="server"
                                            Text='<%# Eval("SdtNV") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSdtNV" runat="server"
                                            Text='<%# Bind("SdtNV") %>' CssClass="form-control"
                                            Style="text-align: justify; width: 70px;"
                                            TextMode="MultiLine" Rows="2" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%--căn cước--%>
                                <asp:TemplateField HeaderText="CCCD">
                                    <ItemStyle Width="130px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCccd" runat="server"
                                            Text='<%# Eval("CccdNV") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCccdNV" runat="server"
                                            Text='<%# Bind("CccdNV") %>' CssClass="form-control"
                                            Style="text-align: justify; width: 110px;"
                                            TextMode="MultiLine" Rows="2" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%--giới tính--%>
                                <asp:TemplateField HeaderText="Giới tính">
                                    <ItemStyle Width="50px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGioiTinh" runat="server"
                                            Text='<%# Eval("GioiTinhNV") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGioiTinh" runat="server">
                                            <asp:ListItem Text="Nam" Value="1" />
                                            <asp:ListItem Text="Nữ" Value="0" />
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%--ngày sinh--%>
                                <asp:TemplateField HeaderText="Ngày sinh">
                                    <ItemStyle Width="80px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblNgaySinh" runat="server"
                                            Text='<%# Eval("NgaySinhNV") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNgaySinh" runat="server"
                                            Text='<%# Bind("NgaySinhNV") %>' CssClass="form-control"
                                            Style="text-align: justify; width: 60px;"
                                            TextMode="MultiLine" Rows="2" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%--địa chỉ--%>
                                <asp:TemplateField HeaderText="Địa chỉ">
                                    <ItemStyle Width="160px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiaChi" runat="server"
                                            CssClass="wrap-text"
                                            Text='<%# Eval("DiaChiNV") %>'
                                            ToolTip='<%# Eval("DiaChiNV") %>'
                                            Style="margin-right: 20px; margin-left: 10px;" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDiaChiNCC" runat="server"
                                            Text='<%# Bind("DiaChiNV") %>'
                                            CssClass="form-control"
                                            TextMode="MultiLine" Rows="3"
                                            Style="width: 140px;" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%--email--%>
                                <asp:TemplateField HeaderText="Email">
                                    <ItemStyle Width="80px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server"
                                            Text='<%# Eval("EmailNV") %>'
                                            CssClass="wrap-text" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEmailNV" runat="server"
                                            Text='<%# Bind("EmailNV") %>' CssClass="form-control"
                                            Style="text-align: justify; width: 80px; margin-right: 10px;"
                                            TextMode="MultiLine" Rows="2" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- Cột Sửa --%>
                                <asp:TemplateField HeaderText="Hành động">
                                    <HeaderStyle Width="80px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <div class="design-action" style="border-left: 1px solid #ccc">
                                            <asp:Button ID="btnSua" runat="server" Text="Sửa"
                                                CommandName="Edit" CssClass="btn btn-warning" />
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật"
                                            CommandName="Update" CssClass="btn btn-success" />
                                        <asp:Button ID="btnHuy" runat="server" Text="Hủy"
                                            CommandName="Cancel" CssClass="btn btn-secondary" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>


                                <%--cột xóa--%>
                                <asp:TemplateField HeaderText="Hành động">
                                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Button ID="btnXoa" runat="server" Text="Xóa"
                                            CommandName="Delete"
                                            CssClass="btn btn-danger"
                                            OnClientClick="return confirm('Bạn có chắc muốn xóa nhà cung cấp này?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <%--BackColor="#ff5e57"--%>

                            <HeaderStyle Font-Bold="True" ForeColor="#6c757d" Height="60px" />
                            <RowStyle BackColor="White" ForeColor="black" />
                            <PagerStyle BackColor="#60676f" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <%--POPUP THÊM MỚI LOẠI GIÀY--%>
                <div id="popupForm" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.5); z-index: 9999; display: flex; justify-content: center; align-items: center;">
                    <div class="table-content" style="background: #fff; padding: 30px; border-radius: 10px; width: 500px; position: relative;">
                        <!-- Dấu "x" để đóng popup (Biểu tượng từ Bootstrap Icons) -->
                        <i class="bi bi-x" id="closePopup" style="position: absolute; top: 10px; right: 10px; font-size: 24px; cursor: pointer; color: #ff5e57;"></i>
                        <h4 style="margin-bottom: 30px;">THÊM NHÂN VIÊN</h4>

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
                            </div>
                        </div>

                        <div class="box-info">
                            <%--số điện thoại--%>
                            <div class="box-content">
                                <div class="title-content">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtSdt" runat="server" CssClass="input"></asp:TextBox>
                                        <label class="user-label" for="txtSdt">Số điện thoại</label>
                                    </div>
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
                            </div>
                        </div>

                        <div class="box-info">
                            <div class="box-content">
                                <div class="box-content-full">
                                    <div class="input-group">
                                        <!-- TextBox có CSS class "input" -->
                                        <asp:TextBox ID="txtCccd" runat="server" CssClass="input"></asp:TextBox>
                                        <!-- Label đi kèm với TextBox -->
                                        <label class="user-label" for="txtCccd">Căn cước công dân</label>
                                    </div>
                                </div>
                            </div>

                            <%--ngày sinh--%>
                            <div class="box-content">
                                <div class="title-content">
                                    <div class="input-group">
                                        <!-- TextBox có CSS class "input" -->
                                        <asp:TextBox ID="txtNgaysinh" runat="server" CssClass="input"></asp:TextBox>
                                        <!-- Label đi kèm với TextBox -->
                                        <label class="user-label" for="txtNgaySinh">Ngày sinh (yyyy-MM-dd)</label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="box-info">
                            <%--Giới tính--%>
                            <div class="box-content">
                                <div class="title-content">
                                    <div class="input-group">
                                        <asp:RadioButtonList ID="rblGioiTinh" runat="server" RepeatDirection="Horizontal" CssClass="gioitinh-list">
                                            <asp:ListItem Text="Nam" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Nữ" Value="0"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="box-content">
                                <div class="title-content">
                                    <div class="input-group">
                                        <label for="ddlLoaiGiay" style="margin-bottom: 5px; margin-top: 5px;">Chức vụ</label>
                                        <asp:DropDownList ID="ddlChucVu" runat="server"
                                            CssClass="input"
                                            DataSourceID="dsChucVu"
                                            DataTextField="TenChucVu"
                                            DataValueField="MaChucVu">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%--Địa chỉ--%>
                        <div class="box-content">
                            <div class="box-content-full">
                                <div class="input-group">
                                    <!-- TextBox có CSS class "input" -->
                                    <asp:TextBox ID="txtDiachi" runat="server" CssClass="input"></asp:TextBox>
                                    <!-- Label đi kèm với TextBox -->
                                    <label class="user-label" for="txtDiachi">Địa chỉ</label>
                                </div>
                            </div>
                        </div>

                        <!-- Button xác nhận -->
                        <div class="item-button" style="justify-content: center; margin-top: 20px;">
                            <div class="item-btn-add">
                                <asp:Button ID="butXacNhan" runat="server" Text="Xác nhận" CssClass="btn btn-success" OnClick="butXacNhan_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <asp:SqlDataSource ID="dsChucVu" runat="server"
        ConnectionString="<%$ ConnectionStrings:conn %>"
        SelectCommand="SELECT MaChucVu, TenChucVu FROM tblCHUCVUNV"></asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSourceLoaiGiay" runat="server"
        ConnectionString="<%$ ConnectionStrings:conn %>"
        SelectCommand="SELECT MaChucVu, MaNV, HoNV, TenNV, SdtNV, CccdNV, DiaChiNV, EmailNV, 
    CASE 
        WHEN GioiTinhNV = 1 THEN N'Nam'
        ELSE N'Nữ'
    END AS GioiTinhNV,
    CONVERT(varchar, NgaySinhNV, 103) AS NgaySinhNV 
FROM tblNHANVIEN"
        InsertCommand="INSERT INTO tblNHANVIEN (MaChucVu,MaNV,HoNV,TenNV,SdtNV,CccdNV,GioiTinhNV,NgaySinhNV,DiaChiNV,EmailNV) VALUES (@MaChucVu,@MaNV,@HoNV,@TenNV,@SdtNV,@CccdNV,@GioiTinhNV,@NgaySinhNV,@DiaChiNV,@EmailNV)"
        UpdateCommand="UPDATE tblNHANVIEN SET MaChucVu = @MaChucVu, HoNV = @HoNV, TenNV = @TenNV, SdtNV = @SdtNV, CccdNV = @CccdNV, GioiTinhNV = @GioiTinhNV, NgaySinhNV = @NgaySinhNV, DiaChiNV = @DiaChiNV, EmailNV = @EmailNV WHERE MaNV = @MaNV"
        DeleteCommand="DELETE FROM tblNHANVIEN WHERE MaNV = @MaNV">

        <%--tham số thêm--%>
        <InsertParameters>
            <asp:Parameter Name="MaNV" Type="String" />
            <asp:Parameter Name="HoNV" Type="String" />
            <asp:Parameter Name="TenNV" Type="String" />
            <asp:Parameter Name="SdtNV" Type="String" />
            <asp:Parameter Name="CccdNV" Type="String" />
            <asp:Parameter Name="GioiTinhNV" Type="String" />
            <asp:Parameter Name="NgaySinhNV" Type="String" />
            <asp:Parameter Name="DiaChiNV" Type="String" />
            <asp:Parameter Name="EmailNV" Type="String" />
        </InsertParameters>

        <%--tham số cập nhập--%>
        <UpdateParameters>
            <asp:Parameter Name="MaNV" Type="String" />
            <asp:Parameter Name="HoNV" Type="String" />
            <asp:Parameter Name="TenNV" Type="String" />
            <asp:Parameter Name="SdtNV" Type="String" />
            <asp:Parameter Name="CccdNV" Type="String" />
            <asp:Parameter Name="GioiTinhNV" Type="String" />
            <asp:Parameter Name="NgaySinhNV" Type="String" />
            <asp:Parameter Name="DiaChiNV" Type="String" />
            <asp:Parameter Name="EmailNV" Type="String" />
        </UpdateParameters>

        <%--tham số xóa--%>
        <DeleteParameters>
            <asp:Parameter Name="MaNV" Type="String" />
        </DeleteParameters>
    </asp:SqlDataSource>


    <script>
        // Hiệu ứng nhãn
        $(document).ready(function () {
            $('.input').on('focus', function () {
                $(this).next('.user-label').css({
                    'transform': 'translateY(-70%) scale(0.6)',
                    'padding': '0 .2em',
                    'color': '#ff5e57'
                });
            });

            $('.input').on('blur', function () {
                var inputVal = $(this).val();
                if (inputVal === '') {
                    $(this).next('.user-label').css({
                        'transform': 'translateY(0)',
                        'color': '#ff5e57'
                    });
                }
            });
        });

    </script>

    <script type="text/javascript">
        function resetTextBoxes() {
        document.getElementById("<%= txtHo.ClientID %>").value = "";  // Reset Tên loại giày
        document.getElementById("<%= txtTen.ClientID %>").value = "";  // Reset Đặc điểm
        document.getElementById("<%= txtSdt.ClientID %>").value = "";  // Reset Đặc điểm
        document.getElementById("<%= txtCccd.ClientID %>").value = "";  // Reset Tên loại giày
        document.getElementById("<%= rblGioiTinh.ClientID %>").value = "";  // Reset Đặc điểm
        document.getElementById("<%= txtEmail.ClientID %>").value = "";  // Reset Đặc điểm
        document.getElementById("<%= txtDiachi.ClientID %>").value = "";  // Reset Đặc điểm
        document.getElementById("<%= ddlChucVu.ClientID %>").value = "";  // Reset Đặc điểm
            document.getElementById("<%= txtNgaysinh.ClientID %>").value = "";  // Reset Đặc điểm
        }

        function hidePopup(event) {
            // Kiểm tra nếu người dùng nhấn vào phần nền ngoài form (popupForm)
            if (event.target.id === "popupForm") {
                document.getElementById("popupForm").style.display = "none";
            }
        }

        function showPopup() {

            var popup = document.getElementById('popupForm');
            popup.style.display = 'flex';
        }

        function hidePopup() {
            var popup = document.getElementById('popupForm');
            popup.style.display = 'none';
        }

        // Đóng popup khi click vào dấu "x"
        document.getElementById('closePopup').addEventListener('click', hidePopup);

        // Lắng nghe sự kiện click trên toàn bộ trang để ẩn popup khi nhấn bên ngoài form
        document.getElementById('popupForm').addEventListener('click', function (event) {
            // Nếu người dùng nhấn vào phần nền ngoài form, ẩn popup
            if (event.target.id === "popupForm") {
                hidePopup(event);
            }
        });

        // Ngừng sự kiện khi người dùng nhấn vào bất kỳ phần tử bên trong form
        document.querySelector('.table-content').addEventListener('click', function (event) {
            event.stopPropagation();
        });
    </script>


    <%--hiệu ứng popup--%>
    <script>
        // Hiển thị modal
        function showModal() {
            document.getElementById("loginSuccessModal").style.display = "block";
            setTimeout(function () {
                closeModal();
            }, 1500);
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
<asp:Content ID="Content7" ContentPlaceHolderID="IDPhieuNhap" runat="server">
</asp:Content>
