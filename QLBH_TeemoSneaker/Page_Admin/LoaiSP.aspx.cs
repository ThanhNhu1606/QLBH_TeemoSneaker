using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Admin
{
    public partial class LoaiSP : System.Web.UI.Page
    {
        // Chuỗi kết nối được khai báo 1 lần
        private readonly string conStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "init", "hidePopup();", true);
            }
                else if (Request["__EVENTTARGET"] != null && Request["__EVENTTARGET"].Contains("butThemMoi"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "showPopup(); resetTextBoxes();", true);
                }
            }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "init", "hidePopup();", true);
            string timkiem = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(timkiem))
            {
                SqlDataSourceLoaiGiay.SelectCommand = "SELECT * FROM tblLOAIGIAY";
                SqlDataSourceLoaiGiay.SelectParameters.Clear();
            }
            else
            {
                string query = @"
        SELECT * FROM tblLOAIGIAY 
        WHERE 
            TenLoaiGiay LIKE '%' + @timkiem + '%' 
            OR DacDiem LIKE '%' + @timkiem + '%'";
                SqlDataSourceLoaiGiay.SelectParameters.Clear();
                SqlDataSourceLoaiGiay.SelectParameters.Add("timkiem", timkiem);
                SqlDataSourceLoaiGiay.SelectCommand = query;
            }

            dsSanPham.DataBind();
        }


        // Mở popup khi nhấn Thêm mới
        protected void butThemMoi_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "showPopup();", true);
        }

        // Thêm loại giày mới
        protected void butXacNhan_Click(object sender, EventArgs e)
        {
            string tenLoaiGiay = txtTenLoaiGiayMoi.Text;
            string dacDiem = txtDacDiemMoi.Text;

            // Lấy mã loại giày tự động
            string maLoaiGiay = GetNewMaLoaiGiay();

            // Thực hiện thêm vào cơ sở dữ liệu
            string insertQuery = "INSERT INTO tblLOAIGIAY (MaLoaiGiay, TenLoaiGiay, DacDiem) VALUES (@MaLoaiGiay, @TenLoaiGiay, @DacDiem)";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@MaLoaiGiay", maLoaiGiay);
                cmd.Parameters.AddWithValue("@TenLoaiGiay", tenLoaiGiay);
                cmd.Parameters.AddWithValue("@DacDiem", dacDiem);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Cập nhật lại GridView sau khi thêm
            dsSanPham.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true); // Ẩn popup sau khi thêm xong
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
        }

        private string GetNewMaLoaiGiay()
        {
            string maLoaiGiay = string.Empty;

            // Lấy số lớn nhất của mã loại giày hiện tại và tạo mã mới
            string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(MaLoaiGiay, 3, LEN(MaLoaiGiay)) AS INT)), 0) + 1 FROM tblLOAIGIAY";

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                int newNumber = (int)cmd.ExecuteScalar();
                maLoaiGiay = newNumber.ToString().PadLeft(4, '0'); // Đảm bảo mã có 4 chữ số
                conn.Close();
            }

            return "LG" + maLoaiGiay; // Thêm "LG" vào trước mã
        }
        protected void dsSanPham_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string maLoai = dsSanPham.DataKeys[e.RowIndex].Value.ToString();

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();

                // 1. Kiểm tra xem MaLoaiGiay có đang được dùng trong bảng tblGIAY
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tblGIAY WHERE MaLoaiGiay = @MaLoaiGiay", conn);
                checkCmd.Parameters.AddWithValue("@MaLoaiGiay", maLoai);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    // 2. Nếu đang được dùng → không xóa, hiện cảnh báo
                    string script = "alert('Không thể xóa! Loại giày này đang được sử dụng trong bảng sản phẩm.');";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", script, true);

                    e.Cancel = true; // Ngăn không cho GridView thực hiện xóa
                    return;
                }

                // 3. Nếu không bị ràng buộc, tiến hành xóa
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM tblLOAIGIAY WHERE MaLoaiGiay = @MaLoaiGiay", conn);
                    cmd.Parameters.AddWithValue("@MaLoaiGiay", maLoai);
                    cmd.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
                }
                catch (Exception ex)
                {
                    // Phòng khi có lỗi khác
                    string script = "alert('Lỗi khi xóa: " + ex.Message.Replace("'", "\\'") + "');";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alertError", script, true);
                    e.Cancel = true;
                }
            }
        }

        protected void dsSanPham_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsSanPham.EditIndex = e.NewEditIndex;
        }

        protected void dsSanPham_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsSanPham.EditIndex = -1;
        }

        protected void dsSanPham_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string maLoai = dsSanPham.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = dsSanPham.Rows[e.RowIndex];
            string tenLoai = ((TextBox)row.FindControl("txtTenLoaiGiay")).Text.Trim();
            string dacDiem = ((TextBox)row.FindControl("txtDacDiem")).Text.Trim();

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tblLOAIGIAY SET TenLoaiGiay = @TenLoaiGiay, DacDiem = @DacDiem WHERE MaLoaiGiay = @MaLoaiGiay", conn);
                cmd.Parameters.AddWithValue("@TenLoaiGiay", tenLoai);
                cmd.Parameters.AddWithValue("@DacDiem", dacDiem);
                cmd.Parameters.AddWithValue("@MaLoaiGiay", maLoai);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
            }

            dsSanPham.EditIndex = -1;
        }

        protected void butExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DS_Loai_Giay.xls");
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
                dsSanPham.Columns[3].Visible = false;  // Ẩn cột "Sửa"
                dsSanPham.Columns[4].Visible = false;  // Ẩn cột "Xóa"

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

        protected void GiaLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true); // Ẩn popup sau khi thêm xong
            dsSanPham.DataBind(); // Cập nhật GridView theo lọc giá
        }

    }
}
