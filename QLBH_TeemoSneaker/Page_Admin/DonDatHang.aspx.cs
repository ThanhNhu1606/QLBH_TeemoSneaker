using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Admin
{
    public partial class DonDatHang : System.Web.UI.Page
    {
        private readonly string conStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true);
            }
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true);
            string timkiem = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(timkiem))
            {
                SqlDataSourceLoaiGiay.SelectCommand = "SELECT * FROM tblHOADON";
                SqlDataSourceLoaiGiay.SelectParameters.Clear();
            }
            else
            {
                string query = @"
    SELECT * FROM tblHOADON 
    WHERE 
        MaHD LIKE '%' + @timkiem + '%' 
        OR MaKH LIKE '%' + @timkiem + '%'";
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
                string maPN = e.CommandArgument.ToString();

                // Gọi hàm để lấy dữ liệu chi tiết NCC
                LoadChiTietNCC(maPN);
            }
        }

        private void LoadChiTietNCC(string maPN)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                string query = @"
        SELECT 
            g.TenGiay AS [Tên giày],
            CONVERT(varchar, ct.NgayLapHD, 103) AS [Ngày mua],
            ct.GioLapHD AS [Giờ mua],
            s.LoaiSize AS [Loại size],
FORMAT(g.GiaGiay, 'N0') as [Giá giày],
            kh.TenKH AS [Khách hàng]
        FROM tblCT_HOADON ct
        JOIN tblGIAY g ON ct.MaGiay = g.MaGiay
        JOIN tblSIZE s ON g.MaSize = s.MaSize
        JOIN tblHOADON h ON ct.MaHD = h.MaHD
        JOIN tblKHACHHANG kh ON h.MaKH = kh.MaKH
        WHERE ct.MaHD = @MaHD";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHD", maPN);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvChiTietNCC.DataSource = dt;
                gvChiTietNCC.DataBind();
                gvChiTietNCC.Visible = true;
                // Gọi Modal Bootstrap
                ScriptManager.RegisterStartupScript(this, this.GetType(), "modalChiTiet", "$('#popupChiTietNCC').modal('show');", true);
            }
        }

        protected void dsSanPham_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string maPN = dsSanPham.DataKeys[e.RowIndex].Value.ToString();

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    // 1. Xóa trước trong tblCT_HOADON
                    SqlCommand cmdCT = new SqlCommand("DELETE FROM tblCT_HOADON WHERE MaHD = @MaHD", conn, tran);
                    cmdCT.Parameters.AddWithValue("@MaHD", maPN);
                    cmdCT.ExecuteNonQuery();

                    // 2. Sau đó xóa trong tblHOADON
                    SqlCommand cmdPN = new SqlCommand("DELETE FROM tblHOADON WHERE MaHD = @MaHD", conn, tran);
                    cmdPN.Parameters.AddWithValue("@MaHD", maPN);
                    cmdPN.ExecuteNonQuery();

                    tran.Commit();

                    // Hiển thị modal hoặc cập nhật giao diện
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    string msg = HttpUtility.JavaScriptStringEncode(ex.Message);
                    string script = $"alert('Lỗi khi xóa: {msg}');";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alertError", script, true);
                    e.Cancel = true;
                }
            }
        }

        protected void butExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DS_Don_Hang.xls");
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
                dsSanPham.Columns[4].Visible = false;  // Ẩn cột "Sửa"
                dsSanPham.Columns[5].Visible = false;  // Ẩn cột "Xóa"

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