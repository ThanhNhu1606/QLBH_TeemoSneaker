using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace QLBH_TeemoSneaker.Page_Customer
{
    public partial class ThongTinDatHang : System.Web.UI.Page
    {

        // Chuỗi kết nối được khai báo 1 lần
        private readonly string conStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        DataTable dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }
        protected void LoadData()
        {
            dt = (DataTable)Session["tblGIAY"];
            grdDonHang.DataSource = dt;
            grdDonHang.DataBind();
            
            if (dt != null)
            {
                double tong = (double)Session["tong"];
                lblTongTien.Text =String.Format("{0:0,000}", tong)+" VNĐ";
            }
        }


        protected void butDongY_Click(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            string maDonHang = CreateKey("HD");
            Session["txtHoTen"] = txtHoTen.Text;
            Session["txtEmail"] = txtEmail.Text;
            Session["txtDienThoai"] = txtDienThoai.Text;
            Session["txtDiaChi"] = txtDiaChi.Text;
            Session["maDH"] = maDonHang;

            // Kiểm tra và lấy tổng tiền từ Session
            double tong = (Session["tong"] != null) ? Convert.ToDouble(Session["tong"]) : 0;

            // Định dạng tổng tiền theo kiểu VNĐ
            string formattedTong = String.Format("{0:0,000}", tong) + " VNĐ";

            // Lưu tổng tiền đã định dạng vào Session (nếu cần)
            Session["tong"] = formattedTong;


            string subject = $"Cửa hàng Teemo Sneaker - Xác nhận đơn hàng #{maDonHang}";
            string body = $@"
        <html>
        <head>
            <meta charset='UTF-8'>
            <title>Xác nhận đơn hàng</title>
        </head>

        <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <h2>Cảm ơn quý khách đã đặt hàng tại Cửa hàng Teemo Sneaker! 🎉</h2>
            <p>Xin chào anh/chị <span style='color: #ff5e57;'><strong><em>{Session["txtHoTen"]}</em></strong></span></p>
            <p>Chúng tôi rất vui khi được phục vụ quý khách! Cảm ơn vì đã lựa chọn <strong>Teemo Sneaker</strong>. Mong rằng đơn hàng này sẽ mang đến cho quý khách trải nghiệm tuyệt vời!</p>

            <p><strong>📦 Thông tin đơn hàng:</strong></p>
            <ul>
                <li><strong>Mã đơn hàng:</strong> {maDonHang}</li>
                <li><strong>Ngày đặt:</strong> {d}</li>
                <li><strong>Tổng tiền:</strong> {formattedTong}</li>
            </ul>

            <p><strong>📍 Địa chỉ giao hàng:</strong> {Session["txtDiaChi"]}</p>
            <p>Chúng tôi sẽ nhanh chóng xử lý đơn hàng và giao đến quý khách trong thời gian sớm nhất.</p>
            <p>Trân trọng,</p>
            <p><strong>Teemo Sneaker</strong></p>
        </body>
        </html>";

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(txtEmail.Text);
                mail.From = new MailAddress("2221004252@sv.ufm.edu.vn");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;  // Đảm bảo email sẽ được gửi dưới dạng HTML
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("2221004252@sv.ufm.edu.vn", "cqegytzgyzophqxs");
                client.Send(mail);
                Server.Transfer("GuiEmailDonHang.aspx");
            }
            catch (SmtpFailedRecipientException ex)
            {
                lblStatus.Text = "Mail không được gởi! " + ex.FailedRecipient;
            }

        }

        public static string CreateKey(string tiento)
        {
            string key = tiento;
            string[] partsDay;
            partsDay = DateTime.Now.ToShortDateString().Split('/');
            //Ví dụ 07/08/2009
            string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            key = key + d;
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            //Ví dụ 7:08:03 PM hoặc 7:08:03 AM
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            //Xóa ký tự trắng và PM hoặc AM
            partsTime[2] = partsTime[2].Remove(2, 3);
            string t;
            t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            key = key + t;
            return key;
        }
        public static string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }

    }
}