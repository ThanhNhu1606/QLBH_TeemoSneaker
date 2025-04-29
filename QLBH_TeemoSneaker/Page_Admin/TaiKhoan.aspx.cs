using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Admin
{
    public partial class TaiKhoan : System.Web.UI.Page
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
                SqlDataSourceLoaiGiay.SelectCommand = "SELECT * FROM tblCHUCVUNV";
                SqlDataSourceLoaiGiay.SelectParameters.Clear();
            }
            else
            {
                string query = @"
    SELECT * FROM tblCHUCVUNV 
    WHERE 
        TenChucVu LIKE '%' + @timkiem + '%'";
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
            string insertQuery = "INSERT INTO tblCHUCVUNV (MaChucVu, TenChucVu, MatKhau) VALUES (@MaChucVu, @TenChucVu, @MatKhau)";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@MaChucVu", maLoaiGiay);
                cmd.Parameters.AddWithValue("@TenChucVu", tenLoaiGiay);
                cmd.Parameters.AddWithValue("@MatKhau", dacDiem);

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
            string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(MaChucVu, 3, LEN(MaChucVu)) AS INT)), 0) + 1 FROM tblCHUCVUNV";

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                int newNumber = (int)cmd.ExecuteScalar();
                maLoaiGiay = newNumber.ToString().PadLeft(4, '0'); // Đảm bảo mã có 4 chữ số
                conn.Close();
            }

            return "CV" + maLoaiGiay; // Thêm "LG" vào trước mã
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
                    SqlCommand cmd = new SqlCommand("DELETE FROM tblCHUCVUNV WHERE MaChucVu = @MaChucVu", conn);
                    cmd.Parameters.AddWithValue("@MaChucVu", maLoai);
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
                SqlCommand cmd = new SqlCommand("UPDATE tblCHUCVUNV SET TenChucVu = @TenChucVu, MatKhau = @MatKhau WHERE MaChucVu = @MaChucVu", conn);
                cmd.Parameters.AddWithValue("@TenChucVu", tenLoai);
                cmd.Parameters.AddWithValue("@MatKhau", dacDiem);
                cmd.Parameters.AddWithValue("@MaChucVu", maLoai);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
            }

            dsSanPham.EditIndex = -1;
        }
    }
}