using System;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web;
using System.Data;
using System.IO;

namespace QLBH_TeemoSneaker.Page_Admin
{
    public partial class KhachHang : System.Web.UI.Page
    {
        private readonly string conStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "init", "hidePopup();", true);
            string timkiem = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(timkiem))
            {
                SqlDataSourceLoaiGiay.SelectCommand = "SELECT * FROM tblKHACHHANG";
                SqlDataSourceLoaiGiay.SelectParameters.Clear();
            }
            else
            {
                string query = @"
SELECT 
    MaKH, HoKH, TenKH, SdtKH,DiaChiKH, EmailKH, 
    CASE 
        WHEN GioiTinhKH = 1 THEN N'Nam'
        ELSE N'Nữ'
    END AS GioiTinhKH,
    CONVERT(varchar, NgaySinhKH, 103) AS NgaySinhKH 
FROM tblKHACHHANG 
WHERE 
    TenKH LIKE '%' + @timkiem + '%' 
    OR SdtKH LIKE '%' + @timkiem + '%'
    OR EmailKH LIKE '%' + @timkiem + '%'";
                SqlDataSourceLoaiGiay.SelectParameters.Clear();
                SqlDataSourceLoaiGiay.SelectParameters.Add("timkiem", timkiem);
                SqlDataSourceLoaiGiay.SelectCommand = query;
            }

            dsSanPham.DataBind();
        }

        protected void dsSanPham_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "XemChiTiet")
            {
                string maKH = e.CommandArgument.ToString();

                // Gọi hàm để lấy dữ liệu chi tiết NCC
                LoadChiTietNCC(maKH);
            }
        }

        private void LoadChiTietNCC(string maKH)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                string query = @"SELECT tk.TaiKhoanKH as [Tài khoản khách hàng], tk.MatKhauKH as [Mật khẩu khách hàng] FROM tblTAIKHOANKH tk JOIN tblKHACHHANG kh ON kh.MaKH = tk.MaKH WHERE tk.MaKH = @MaKH";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKH", maKH);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Thay đổi giá trị mật khẩu thành '***'
                foreach (DataRow row in dt.Rows)
                {
                    row["Mật khẩu khách hàng"] = "******"; 
                }

                gvChiTietNCC.DataSource = dt;
                gvChiTietNCC.DataBind();
                gvChiTietNCC.Visible = true;

                // Gọi Modal Bootstrap
                ScriptManager.RegisterStartupScript(this, this.GetType(), "modalChiTiet", "$('#popupChiTietNCC').modal('show');", true);
            }
        }

        protected void butExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DS_Khach_Hang.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                // Tắt phân trang nếu có
                dsSanPham.AllowPaging = false;

                // Nạp lại dữ liệu nếu bạn đang dùng GridView với DataSource
                dsSanPham.DataBind();

                // Xóa cột "Sửa" và "Xóa" khỏi GridView (tiêu đề và dữ liệu)
                dsSanPham.Columns[8].Visible = false;  // Ẩn cột "Sửa"
                // Render dữ liệu vào tệp Excel mà không có cột "Sửa" và "Xóa"
                dsSanPham.RenderControl(hw);

                // Gửi dữ liệu ra phản hồi HTTP
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        // Bắt buộc override để cho phép render GridView ra HTML
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Không cần xử lý gì ở đây
        }

    }
}