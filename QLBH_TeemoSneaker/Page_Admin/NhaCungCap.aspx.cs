using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Admin
{
    public partial class NhaCungCap : System.Web.UI.Page
    {
        private readonly string conStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup1", "hidePopup1();", true);
            }
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup1", "hidePopup1();", true);
            string timkiem = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(timkiem))
            {
                SqlDataSourceLoaiGiay.SelectCommand = "SELECT * FROM tblNHACUNGCAP";
                SqlDataSourceLoaiGiay.SelectParameters.Clear();
            }
            else
            {
                string query = @"
    SELECT * FROM tblNHACUNGCAP 
    WHERE 
        TenNCC LIKE '%' + @timkiem + '%' 
        OR SdtNCC LIKE '%' + @timkiem + '%'";
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup1", "hidePopup1();resetTextBoxes1();", true);
        }

        protected void butThemChiTiet_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popupChiTiet", "showPopup1();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();resetTextBoxes();", true);
        }

        protected void btnThemChiTiet_Click(object sender, EventArgs e)
        {
            string maGiay = ddlGiay.SelectedValue.Trim();
            string maNCC = ddlNCC.SelectedValue.Trim();
            string ngay = txtNgay.Text;
            string ghiChu = txtGhiChu.Text;

            // Thực hiện thêm vào cơ sở dữ liệu
            string insertQuery = "INSERT INTO tblCT_NHACUNGCAP (MaGiay, MaNCC, NgaySX, GhiChu) VALUES (@MaGiay, @MaNCC, @NgaySX, @GhiChu)";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@MaGiay", maGiay);
                cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                cmd.Parameters.AddWithValue("@NgaySX", ngay);
                cmd.Parameters.AddWithValue("@GhiChu", ghiChu);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Cập nhật lại GridView sau khi thêm
            dsChiTiet.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true); // Ẩn popup sau khi thêm xong
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup1", "hidePopup1();", true); // Ẩn popup sau khi thêm xong
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
        }

        // Thêm loại giày mới
        protected void butXacNhan_Click(object sender, EventArgs e)
        {
            string tenNCC = txtTenNCCMoi.Text;
            string diachiNCC = txtDiaChiNCCMoi.Text;
            string sdtNCC = txtSdtNCCMoi.Text;

            // Lấy mã loại giày tự động
            string maNCC = GetNewMaNCC();

            // Thực hiện thêm vào cơ sở dữ liệu
            string insertQuery = "INSERT INTO tblNHACUNGCAP (MaNCC, TenNCC, DiaChiNCC, SdtNCC) VALUES (@MaNCC, @TenNCC, @DiaChiNCC,@SdtNCC)";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                cmd.Parameters.AddWithValue("@TenNCC", tenNCC);
                cmd.Parameters.AddWithValue("@DiaChiNCC", diachiNCC);
                cmd.Parameters.AddWithValue("@SdtNCC", sdtNCC);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Cập nhật lại GridView sau khi thêm
            dsSanPham.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true); // Ẩn popup sau khi thêm xong
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup1", "hidePopup1();", true); // Ẩn popup sau khi thêm xong
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
        }

        private string GetNewMaNCC()
        {
            string maNCC = string.Empty;

            // Lấy số lớn nhất của mã loại giày hiện tại và tạo mã mới
            string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(MaNCC, 3, LEN(MaNCC)) AS INT)), 0) + 1 FROM tblNHACUNGCAP";

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                int newNumber = (int)cmd.ExecuteScalar();
                maNCC = newNumber.ToString().PadLeft(4, '0'); // Đảm bảo mã có 4 chữ số
                conn.Close();
            }

            return "SX" + maNCC;
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
                    SqlCommand cmd = new SqlCommand("DELETE FROM tblNHACUNGCAP WHERE MaNCC = @MaNCC", conn);
                    cmd.Parameters.AddWithValue("@MaNCC", maLoai);
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

        protected void dsChiTiet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string maGiay = dsChiTiet.DataKeys[e.RowIndex].Values["MaGiay"].ToString();
            string maNCC = dsChiTiet.DataKeys[e.RowIndex].Values["MaNCC"].ToString();

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM tblCT_NHACUNGCAP WHERE MaGiay = @MaGiay AND MaNCC = @MaNCC", conn);
                    cmd.Parameters.AddWithValue("@MaGiay", maGiay);
                    cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                    cmd.ExecuteNonQuery();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
                }
                catch (Exception ex)
                {
                    string script = "alert('Lỗi khi xóa: " + ex.Message.Replace("'", "\\'") + "');";
                    ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "alertError", script, true);
                    e.Cancel = true;
                }
            }
        }


        protected void dsSanPham_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsSanPham.EditIndex = e.NewEditIndex;
        }

        protected void dsChiTiet_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsChiTiet.EditIndex = e.NewEditIndex;
        }

        protected void dsSanPham_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsSanPham.EditIndex = -1;
        }

        protected void dsChiTiet_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsChiTiet.EditIndex = -1;
        }

        protected void dsSanPham_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string maLoai = dsSanPham.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = dsSanPham.Rows[e.RowIndex];
            string tenNCC = ((TextBox)row.FindControl("txtTenNCC")).Text.Trim();
            string diachiNCC = ((TextBox)row.FindControl("txtDiaChiNCC")).Text.Trim();
            string sdtNCC = ((TextBox)row.FindControl("txtSdtNCC")).Text.Trim();

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tblNHACUNGCAP SET TenNCC = @TenNCC, DiaChiNCC = @DiaChiNCC, SdtNCC = @SdtNCC WHERE MaNCC = @MaNCC", conn);
                cmd.Parameters.AddWithValue("@TenNCC", tenNCC);
                cmd.Parameters.AddWithValue("@DiaChiNCC", diachiNCC);
                cmd.Parameters.AddWithValue("@SdtNCC", sdtNCC);
                cmd.Parameters.AddWithValue("@MaNCC", maLoai);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
            }

            dsSanPham.EditIndex = -1;
        }

        //protected void dsChiTiet_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    string maLoai = dsChiTiet.DataKeys[e.RowIndex].Value.ToString();
        //    GridViewRow row = dsChiTiet.Rows[e.RowIndex];
        //    var ddlGiay = row.FindControl("ddlGiay") as DropDownList;
        //    var txtNgaySX = row.FindControl("txtNgaySX") as TextBox;
        //    var ddlNCC = row.FindControl("ddlNCC") as DropDownList;
        //    var txtGhiChu = row.FindControl("txtGhiChu") as TextBox;

        //    using (SqlConnection conn = new SqlConnection(conStr))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand("UPDATE tblCT_NHACUNGCAP SET NgaySX = @NgaySX, GhiChu = @GhiChu WHERE MaGiay = @MaGiay AND MaNCC = @MaNCC", conn);
        //        cmd.Parameters.AddWithValue("@MaGiay", ddlGiay.SelectedValue);
        //        cmd.Parameters.AddWithValue("@MaNCC", ddlNCC.SelectedValue);
        //        cmd.Parameters.AddWithValue("@NgaySX", txtNgaySX.Text);
        //        cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);
        //        cmd.ExecuteNonQuery();
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
        //    }

        //    dsSanPham.EditIndex = -1;
        //}

        protected void dsChiTiet_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string maLoai = dsChiTiet.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = dsChiTiet.Rows[e.RowIndex];
            var ddlGiay = row.FindControl("ddlGiay") as DropDownList;
            var txtNgaySX = row.FindControl("txtNgaySX") as TextBox;
            var ddlNCC = row.FindControl("ddlNCC") as DropDownList;
            var txtGhiChu = row.FindControl("txtGhiChu") as TextBox;

            // Chuyển đổi ngày tháng từ "dd/MM/yyyy" sang "yyyy-MM-dd HH:mm:ss"
            DateTime ngaySX;
            bool isValidDate = DateTime.TryParseExact(txtNgaySX.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaySX);

            if (!isValidDate)
            {
                string script = "alert('Ngày tháng không hợp lệ. Vui lòng nhập lại!');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertError", script, true);
                return;  // Dừng lại nếu ngày tháng không hợp lệ
            }

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tblCT_NHACUNGCAP SET NgaySX = @NgaySX, GhiChu = @GhiChu WHERE MaGiay = @MaGiay AND MaNCC = @MaNCC", conn);
                cmd.Parameters.AddWithValue("@MaGiay", ddlGiay.SelectedValue);
                cmd.Parameters.AddWithValue("@MaNCC", ddlNCC.SelectedValue);

                // Đảm bảo giá trị ngày tháng là "yyyy-MM-dd HH:mm:ss" trước khi gửi đến SQL Server
                cmd.Parameters.AddWithValue("@NgaySX", ngaySX.ToString("yyyy-MM-dd HH:mm:ss")); // Chuyển ngày thành định dạng đúng
                cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);
                cmd.ExecuteNonQuery();

                // Hiển thị thông báo thành công
                string scriptSuccess = "alert('Cập nhật thành công!');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertSuccess", scriptSuccess, true);
            }

            dsSanPham.EditIndex = -1;
        }




        protected void dsSanPham_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "XemChiTiet")
            {
                string maNCC = e.CommandArgument.ToString();

                // Gọi hàm để lấy dữ liệu chi tiết NCC
                LoadChiTietNCC(maNCC);
            }
        }

        private void LoadChiTietNCC(string maNCC)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                string query = @"SELECT ct.MaGiay as [Mã giày], g.TenGiay as [Tên giày], 
         CONVERT(varchar, ct.NgaySX, 103) AS [Ngày sản xuất], 
         ct.GhiChu as [Ghi chú]
         FROM tblCT_NHACUNGCAP ct
         JOIN tblGIAY g ON ct.MaGiay = g.MaGiay
         WHERE ct.MaNCC = @MaNCC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNCC", maNCC);

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

        protected void butExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DS_Nha_Cung_Cap.xls");
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
                dsSanPham.Columns[6].Visible = false;  // Ẩn cột "Xóa"

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