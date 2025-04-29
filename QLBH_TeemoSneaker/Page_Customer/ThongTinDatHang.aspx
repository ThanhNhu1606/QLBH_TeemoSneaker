<%@ Page Title="" Language="C#" MasterPageFile="~/Header_Footer.Master" AutoEventWireup="true" CodeBehind="ThongTinDatHang.aspx.cs" Inherits="QLBH_TeemoSneaker.Page_Customer.ThongTinDatHang" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ThanhTimKiem_Header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="GioHang_Header" runat="server">
</asp:Content>
<asp:Content ID="DatHang" ContentPlaceHolderID="Content" runat="server">
     <style>
     /* Căn giữa bảng */

     .table-view {
         width: fit-content;
         text-align: center; /* Căn giữa nội dung */
     }

     /* Căn giữa tiêu đề h2 */
     h4 {
         text-align: center;
         color: #ff5e57;
         margin-bottom: 30px;
         font-family: 'themify';
     }

     /* Căn giữa các nút */
     #lblTongTien {
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

     .table-container {
    display: flex;
    gap: 20px; /* Khoảng cách giữa hai cột */
    align-items: flex-start; /* Canh đầu dòng */
    padding:30px;
}

.table-view, .table-thanhtoan {
    flex: 1; /* Cả hai phần sẽ chia đều */
}

.table-view {
    max-width: 50%; /* Giới hạn chiều rộng */
    margin-left:50px;
}

.table-thanhtoan {
    max-width: 50%; /* Nhỏ hơn thông tin đơn hàng */
    text-align:center;
}

.total-container {
    width: 100%; /* Chiều rộng bằng bảng */
    max-width: 600px; /* Bằng chiều rộng của GridView */
    border-top: 1px solid #ccc;
    padding: 10px;
    margin-top: 10px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    box-sizing: border-box;
}

.box-info {
    width: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;  /* Căn giữa nội dung theo chiều ngang */
    padding: 10px;
}

/* Căn giữa box-content và thu nhỏ kích thước */
.box-content {
    width: 60%; 
    
}
.title-content {
 font-size: 14px;
}

.input-group {
    position: relative;
    width: 100%;
}

.input {
   border: solid 1.5px #9e9e9e;
    background: none;
    padding: 10px;  /* Giảm padding của input để nhỏ gọn hơn */
    padding-top: 20px;  /* Cải thiện padding-top cho các label */
    font-size: 14px;  /* Giảm kích thước chữ trong input */
    color: #333;
    width: 100%;
    transition: border 150ms cubic-bezier(0.4, 0, 0.2, 1);
}

.input:focus, .input:valid {
    outline: none;
    border: 1.5px solid #ff5e57;
}

.user-label {
    position: absolute;
    left: 15px;
    top: 30%; /* Để văn bản ở vị trí giữa TextBox */
    color: #ff5e57;
    pointer-events: none;
    transform: translateY(0);
    transition: 150ms cubic-bezier(0.4, 0, 0.2, 1);
    font-size:14px;
}


/* Validator styles */
.validator-error {
    font-size: 12px;  /* Chỉnh sửa font-size của thông báo lỗi */
    color: #808080  /* Giữ màu đỏ cho thông báo lỗi */
}

/* Định dạng Button */
.thanhtoan .item-button {
    justify-content: center;
    text-align: center;
    margin-top: 1rem;
}

.thanhtoan .item-btn-add button {
    padding: 1rem 2rem;
    background-color: #2196f3;
    color: white;
    border: none;
    font-size: 1rem;
    cursor: pointer;
    transition: background-color 0.3s ease;
}

.thanhtoan .item-btn-add button:hover {
    background-color: #1a73e8;
}

/* Label Status */
.lblStatus {
    color: #0033cc;
    font-size: 1rem;
    margin-top: 1rem;
}


 </style>

    <div class="table-container">
    <!-- Cột thông tin đơn hàng -->
       <div class="table-view">      
        <div class="item-table">
            <div class="label-table">
    <h4>THÔNG TIN ĐƠN HÀNG</h4>
</div>
            <asp:GridView ID="grdDonHang" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                CellPadding="4" BackColor="White" BorderColor="#ffffff" BorderStyle="Double"
                BorderWidth="3px" GridLines="Horizontal" Height="140px" Width="600px" >

                <Columns>
                    <%-- <asp:BoundField DataField="MaGiay" HeaderText="Mã sản phẩm">
                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>--%>

                    <%--<asp:TemplateField HeaderText="Hình ảnh">
                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Image ID="imgHinhAnh" runat="server" ImageUrl='<%# Eval("HinhAnh") %>' Width="80px" Height="80px" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>

                    <asp:BoundField DataField="TenGiay" HeaderText="Tên sản phẩm">
                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left"/>
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

                    <asp:TemplateField HeaderText="Thành tiền">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Convert.ToDecimal(Eval("TongTien")).ToString("0,000") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="120px" HorizontalAlign="Center" />
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

            <br />
            <div class="total-container">
                <asp:Label ID="lblHienThi" runat="server" Text="Tổng đơn hàng" ForeColor="#000333" Font-Bold="true" Style="font-size: 18px;"></asp:Label>
                <asp:Label ID="lblTongTien" runat="server" ForeColor="#FF5E57" Font-Bold="true" Style="font-size: 22px;"></asp:Label>
            </div>
        </div>
        
    </div>

    <!-- Cột thông tin thanh toán -->
    <div class="table-thanhtoan">
        <div class="label-table">
            <h4>THÔNG TIN THANH TOÁN</h4>
        </div>
        <div class="box-big">

        </div>
        <div class="box-info"  style="text-align:left">
            <div class="box-content">
                <div class="title-content">
                    <div class="input-group">
                        <!-- TextBox có CSS class "input" -->
                        <asp:TextBox ID="txtHoTen" runat="server" CssClass="input"></asp:TextBox>
                        <!-- Label đi kèm với TextBox -->   
                        <label class="user-label" for="txtHoTen">Họ và tên</label>
                    </div>
                </div>
                <div class="content-box">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHoTen" ErrorMessage="Nhập vào họ và tên!" CssClass="validator-error"></asp:RequiredFieldValidator>
                </div>
            </div>

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

            <div class="box-content">
                <div class="title-content">
                    <div class="input-group">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="input" ></asp:TextBox>
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
                        <asp:TextBox ID="txtDiaChi" runat="server" CssClass="input"></asp:TextBox>
                        <label class="user-label" for="txtDiaChi">Địa chỉ</label>
                    </div>
                </div>
                <div class="content-box">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDiaChi" ErrorMessage="Địa chỉ không được bỏ trống!" CssClass="validator-error"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="thanhtoan">
    <div class="item-button" style="justify-content: center">
        <div class="item-btn-add">
            <asp:Button ID="butDongY" runat="server" Text="Đặt hàng" OnClick="butDongY_Click"></asp:Button>
        </div>
    </div>

    <asp:Label ID="lblStatus" runat="server"
        ForeColor="#0033CC" Style="font-size: 14px">
    </asp:Label>
</div>
        </div>

        

    </div>
</div>

    <script>
        $(document).ready(function () {
            // Khi focus vào input, ẩn label
            $('.input').on('focus', function () {
                $(this).next('.user-label').css({
                    'transform': 'translateY(-80%) scale(0.8)',
                    'background-color': '#fff',
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
    </script>

</asp:Content>
