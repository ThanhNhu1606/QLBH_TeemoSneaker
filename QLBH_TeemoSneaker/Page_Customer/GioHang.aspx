<%@ Page Title="" Language="C#" MasterPageFile="~/Header_Footer.Master" AutoEventWireup="true" CodeBehind="GioHang.aspx.cs" Inherits="QLBH_TeemoSneaker.Page_Customer.GioHang" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ThanhTimKiem_Header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="GioHang_Header" runat="server">
</asp:Content>

<asp:Content ID="GioHang" ContentPlaceHolderID="Content" runat="server">

    <style>
        /* Căn giữa bảng */

        .table-view {
            margin: auto;
            width: fit-content;
            text-align: center; 
            margin-right: 25px;
        }

        /* Căn giữa nội dung trong từng cột */
        #dsSanPham th,
        #dsSanPham td {
            text-align: center; /* Căn giữa nội dung */
            vertical-align: middle; /* Căn giữa theo chiều dọc */
            padding: 10px; /* Tạo khoảng cách đẹp hơn */
        }

        /* Căn giữa tiêu đề h2 */
        h4 {
            text-align: center;
            color: #ff5e57;
            margin-bottom: 10px;
            font-family: 'themify';
            margin-top: 50px;
        }

        /* Căn giữa các nút */
        #lblTongTien,
        #butMuahang,
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

        .item-button input[type="submit"],
        .item-button input[type="button"] {
            width: 150px; /* Độ dài cố định */
        }


        .btn-warning {
            background-color: #f39c12;
            color: white;
        }

        .btn-danger {
            background-color: #e74c3c;
            color: white;
        }

        .btn-success {
            background-color: #2ecc71;
            color: white;
        }

        .btn-secondary {
            background-color: #95a5a6;
            color: white;
        }
    </style>

    <div class="table-view">
        <div class="label-table">
            <h4>GIỎ HÀNG CỦA BẠN</h4>
        </div>
        <div class="item-table">
            <asp:GridView ID="dsSanPham" runat="server"
                OnRowDeleting="dsSanPham_RowDeleting" OnRowEditing="dsSanPham_RowEditing"
                OnRowUpdating="dsSanPham_RowUpdating"
                OnRowCancelingEdit="dsSanPham_RowCancelingEdit" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                CellPadding="4" BackColor="White" BorderColor="#ffffff" BorderStyle="Double"
                BorderWidth="3px" GridLines="Horizontal" Height="140px" Width="1335px">

                <Columns>
                    <asp:BoundField DataField="MaGiay" HeaderText="Mã sản phẩm">
                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Hình ảnh">
                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Image ID="imgHinhAnh" runat="server" ImageUrl='<%# Eval("HinhAnh") %>' Width="80px" Height="80px" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="TenGiay" HeaderText="Tên sản phẩm">
                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="LoaiSize" HeaderText="Kích thước">
                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Số lượng">
                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="lblSoluong" runat="server" Text='<%# Eval("SoLuong") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSoluong" runat="server" Text='<%# Bind("SoLuong") %>' Width="50px"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <%-- <asp:BoundField DataField="TongTien" HeaderText="Thành tiền" >
        <headerstyle width="120px" horizontalalign="Center" />
        <itemstyle horizontalalign="Center" />
    </asp:BoundField>--%>

                    <asp:TemplateField HeaderText="Thành tiền">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Convert.ToDecimal(Eval("TongTien")).ToString("0,000") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="120px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <%-- Cột Sửa --%>
                    <asp:TemplateField HeaderText="Hành động">
                        <HeaderStyle Width="180px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <div class="design-action" style="border-left: 1px solid #ccc">
                                <asp:Button ID="btnSua" runat="server" Text="Sửa"
                                    CommandName="Edit" CssClass="btn btn-warning" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật"
                                CommandName="Update" CssClass="btn btn-success" />
                            <asp:Button ID="btnHuy" runat="server" Text="Hủy thao tác"
                                CommandName="Cancel" CssClass="btn btn-secondary" />
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <%-- Cột Xóa --%>
                    <asp:TemplateField HeaderText="Hành động">
                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Button ID="btnXoa" runat="server" Text="Xóa"
                                CommandName="Delete" CommandArgument='<%# Container.DataItemIndex %>'
                                CssClass="btn btn-danger" OnClientClick="return confirm('Bạn có chắc muốn xóa sản phẩm này?');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#333333" />
                <HeaderStyle BackColor="#ff5e57" Font-Bold="True" ForeColor="White" Height="60px" />
                <PagerStyle BackColor="#60676f" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="White" ForeColor="black" />
                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#487575" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#275353" />
            </asp:GridView>
        </div>
        <br />
        <div style="border-top: 1px solid #ccc; padding: 10px; margin-top: 10px; display: flex; justify-content: space-between; align-items: center;">
            <!-- Label hiển thị bên trái -->
            <asp:Label ID="lblHienThi" runat="server" Text="Tổng đơn hàng" ForeColor="#000333" Font-Bold="true" Style="font-size: 18px;"></asp:Label>

            <!-- Label tổng tiền bên phải -->
            <asp:Label ID="lblTongTien" runat="server" ForeColor="#FF5E57" Font-Bold="true" Style="font-size: 24px;"></asp:Label>
        </div>


        <div class="item-button" style="justify-content: center">
            <div class="item-btn-add">
                <asp:Button ID="butMuahang" runat="server" Text="Tiếp tục mua hàng" OnClick="butMuahang_Click"></asp:Button>
            </div>

            <div class="item-btn-back">
                <asp:Button ID="butDathang" runat="server" OnClick="butDathang_Click" PostBackUrl="~/Page_Customer/ThongTinDatHang.aspx" Text="Thanh toán"></asp:Button>
            </div>
        </div>

    </div>





</asp:Content>
