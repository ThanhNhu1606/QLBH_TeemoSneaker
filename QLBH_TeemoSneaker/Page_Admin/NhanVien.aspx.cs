using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Admin
{
    public partial class NhanVien : System.Web.UI.Page
    {
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
                SqlDataSourceLoaiGiay.SelectCommand = "SELECT * FROM tblNHANVIEN";
                SqlDataSourceLoaiGiay.SelectParameters.Clear();
            }
            else
            {
                string query = @"
SELECT 
    MaChucVu, MaNV, HoNV, TenNV, SdtNV, CccdNV, DiaChiNV, EmailNV, 
    CASE 
        WHEN GioiTinhNV = 1 THEN N'Nam'
        ELSE N'Nữ'
    END AS GioiTinhNV,
    CONVERT(varchar, NgaySinhNV, 103) AS NgaySinhNV 
FROM tblNHANVIEN 
WHERE 
    TenNV LIKE '%' + @timkiem + '%' 
    OR SdtNV LIKE '%' + @timkiem + '%'
    OR CccdNV LIKE '%' + @timkiem + '%'
    OR EmailNV LIKE '%' + @timkiem + '%'";
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
            string maChucVu = ddlChucVu.SelectedValue;

            string hoNV = txtHo.Text;
            string tenNV = txtTen.Text;
            string sdtNV = txtSdt.Text;
            string cccdNV = txtCccd.Text;
            bool gioiTinh = rblGioiTinh.SelectedValue == "Nam" ? true : false;
            //bool gioiTinh = rblGioiTinh.SelectedValue == "1";
            string ngaySinh = txtNgaysinh.Text;
            string diaChi = txtDiachi.Text;
            string email = txtEmail.Text;

            // Lấy mã loại giày tự động
            string maNV = GetNewMaNV();

            // Thực hiện thêm vào cơ sở dữ liệu
            string insertQuery = "INSERT INTO tblNHANVIEN (MaChucVu,MaNV,HoNV,TenNV,SdtNV,CccdNV,GioiTinhNV,NgaySinhNV,DiaChiNV,EmailNV) VALUES (@MaChucVu,@MaNV,@HoNV,@TenNV,@SdtNV,@CccdNV,@GioiTinhNV,@NgaySinhNV,@DiaChiNV,@EmailNV)";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@MaChucVu", maChucVu);
                cmd.Parameters.AddWithValue("@HoNV", hoNV);
                cmd.Parameters.AddWithValue("@TenNV", tenNV);
                cmd.Parameters.AddWithValue("@SdtNV", sdtNV);
                cmd.Parameters.AddWithValue("@CccdNV", cccdNV);
                cmd.Parameters.AddWithValue("@GioiTinhNV", gioiTinh);
                cmd.Parameters.AddWithValue("@NgaySinhNV", ngaySinh);
                cmd.Parameters.AddWithValue("@DiaChiNV", diaChi);
                cmd.Parameters.AddWithValue("@EmailNV", email);
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Cập nhật lại GridView sau khi thêm
            dsSanPham.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true); // Ẩn popup sau khi thêm xong
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
        }

        private string GetNewMaNV()
        {
            string maNV = string.Empty;

            // Lấy số lớn nhất của mã loại giày hiện tại và tạo mã mới
            string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(MaNV, 3, LEN(MaNV)) AS INT)), 0) + 1 FROM tblNHANVIEN";

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                int newNumber = (int)cmd.ExecuteScalar();
                maNV = newNumber.ToString().PadLeft(4, '0'); // Đảm bảo mã có 4 chữ số
                conn.Close();
            }

            return "NV" + maNV;
        }
        protected void dsSanPham_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string maLoai = dsSanPham.DataKeys[e.RowIndex].Value.ToString();

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                // 3. Nếu không bị ràng buộc, tiến hành xóa
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM tblNHANVIEN WHERE MaNV = @MaNV", conn);
                    cmd.Parameters.AddWithValue("@MaNV", maLoai);
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
                string maNV = dsSanPham.DataKeys[e.RowIndex].Value.ToString();
                GridViewRow row = dsSanPham.Rows[e.RowIndex];

                DropDownList ddlChucVu = (DropDownList)row.FindControl("ddlChucVu");
                string maChucVu = ddlChucVu.SelectedValue;

                string hoNV = ((TextBox)row.FindControl("txtHoNV")).Text.Trim();
                string tenNV = ((TextBox)row.FindControl("txtTenNV")).Text.Trim();
                string sdtNV = ((TextBox)row.FindControl("txtSdtNV")).Text.Trim();
                string cccdNV = ((TextBox)row.FindControl("txtCccdNV")).Text.Trim();
                string ngaySinh = ((TextBox)row.FindControl("txtNgaySinh")).Text.Trim();
                string diaChi = ((TextBox)row.FindControl("txtDiaChiNCC")).Text.Trim();
                string email = ((TextBox)row.FindControl("txtEmailNV")).Text.Trim();

                DropDownList ddlGioiTinh = (DropDownList)row.FindControl("ddlGioiTinh");

            if (ddlGioiTinh.SelectedIndex == -1) // Kiểm tra nếu không có giá trị nào được chọn
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Vui lòng chọn giới tính.');", true);
                return;
            }
            bool gioiTinh = ddlGioiTinh.SelectedValue == "1" ? true : false;

            using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"UPDATE tblNHANVIEN SET 
                    MaChucVu = @MaChucVu, 
                    HoNV = @HoNV, 
                    TenNV = @TenNV, 
                    SdtNV = @SdtNV, 
                    CccdNV = @CccdNV, 
                    GioiTinhNV = @GioiTinhNV, 
                    NgaySinhNV = @NgaySinhNV, 
                    DiaChiNV = @DiaChiNV, 
                    EmailNV = @EmailNV 
                WHERE MaNV = @MaNV", conn);

                    cmd.Parameters.AddWithValue("@MaChucVu", maChucVu);
                    cmd.Parameters.AddWithValue("@HoNV", hoNV);
                    cmd.Parameters.AddWithValue("@TenNV", tenNV);
                    cmd.Parameters.AddWithValue("@SdtNV", sdtNV);
                    cmd.Parameters.AddWithValue("@CccdNV", cccdNV);
                    cmd.Parameters.AddWithValue("@GioiTinhNV", gioiTinh);
                    cmd.Parameters.AddWithValue("@NgaySinhNV", ngaySinh);
                    cmd.Parameters.AddWithValue("@DiaChiNV", diaChi);
                    cmd.Parameters.AddWithValue("@EmailNV", email);
                    cmd.Parameters.AddWithValue("@MaNV", maNV);

                    cmd.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
                }

                dsSanPham.EditIndex = -1;
            }

        protected void butExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DS_Nhan_Vien.xls");
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
                dsSanPham.Columns[10].Visible = false;  // Ẩn cột "Sửa"
                dsSanPham.Columns[11].Visible = false;  // Ẩn cột "Xóa"

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