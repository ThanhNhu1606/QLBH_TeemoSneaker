using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Customer
{
    public partial class GuiEmailDonHang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.PreviousPage != null)
            {
                string txtHoTen = Session["txtHoTen"] as string ?? "Quý khách";
                string txtDienThoai = Session["txtDienThoai"] as string ?? "Không có";
                string txtDiaChi = Session["txtDiaChi"] as string ?? "Chưa cập nhật";
                string maDH = Session["maDH"] as string ?? "Không xác định";
                string tong = Session["tong"] as string ?? "0 VNĐ";

                lblThongBao.Text = $@"
        <p>Xin chào anh/chị <strong><em>{txtHoTen}</em></strong>,</p>
        <p>Đơn đặt hàng của quý khách đã được xác nhận, vui lòng kiểm tra thông tin sau:</p>

        <p>📦 <strong>Thông tin đơn hàng:</strong></p>
        <ul>
            <li><strong>Mã đơn hàng:</strong> {maDH}</li>
            <li><strong>Họ và tên khách hàng:</strong> {txtHoTen}</li>
            <li><strong>Số điện thoại:</strong> {txtDienThoai}</li>
            <li><strong>Tổng giá trị đơn hàng:</strong> 
                <strong><span style='color: #ff5e57;'>{tong}</span></strong>
            </li>
        </ul>

        <p>
        Sản phẩm sẽ được giao đến địa chỉ: <strong>{txtDiaChi}</strong> trong 2 - 3 ngày làm việc.<br/>
        Quý khách vui lòng chú ý điện thoại trong thời gian này để nhận hàng thuận tiện nhất.
        </p>

        <p>
        Mọi thông tin chi tiết về đơn hàng sẽ được gửi đến email của quý khách. Vui lòng kiểm tra email để biết thêm thông tin.
        </p>

        <p>
        Cảm ơn quý khách đã tin tưởng và lựa chọn <strong>Teemo Sneaker</strong>!<br/>
        Nếu có bất kỳ thắc mắc nào, quý khách vui lòng liên hệ qua 
        <strong style='color: #ff5e57;'>
            <a href='https://www.facebook.com/teemosneaker1989'>Teemo Sneaker</a>
        </strong> để được hỗ trợ nhanh chóng.
        </p>";
            }
        }
    }
}