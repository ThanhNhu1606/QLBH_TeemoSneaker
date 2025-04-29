using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace QLBH_TeemoSneaker.Page_Admin
{
    public partial class PhieuNhap : System.Web.UI.Page
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
                SqlDataSourceLoaiGiay.SelectCommand = "SELECT * FROM tblPHIEUNHAP";
                SqlDataSourceLoaiGiay.SelectParameters.Clear();
            }
            else
            {
                string query = @"
    SELECT * FROM tblPHIEUNHAP 
    WHERE 
        MaGiay LIKE '%' + @timkiem + '%' 
        OR MaNV LIKE '%' + @timkiem + '%'";
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
            // Lấy mã loại giày tự động
            string maPN = GetNewMaNCC();

            string maGiay = ddlGiay.SelectedValue;
            decimal giaPN = decimal.Parse(txtGia.Text);
            int slPN = int.Parse(txtSL.Text);
            decimal tongTienPN = giaPN * slPN;

            string ngayNhap = txtNgay.Text;
            string gioNhap = txtGio.Text;
            string luuY = txtLuuY.Text;

            string emailNV = Session["EmailNV"]?.ToString();
            string maNV = LayMaNVTuEmail(emailNV); // Viết hàm này bên dưới

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                con.Open();

                // 1. Thêm vào bảng tblPHIEUNHAP
                string queryPN = "INSERT INTO tblPHIEUNHAP (MaPN, MaGiay, GiaPN, SLPN, TongTienPN, MaNV) " +
                                 "VALUES (@MaPN, @MaGiay, @GiaPN, @SLPN, @TongTienPN, @MaNV)";
                SqlCommand cmd1 = new SqlCommand(queryPN, con);
                cmd1.Parameters.AddWithValue("@MaPN", maPN);
                cmd1.Parameters.AddWithValue("@MaGiay", maGiay);
                cmd1.Parameters.AddWithValue("@GiaPN", giaPN);
                cmd1.Parameters.AddWithValue("@SLPN", slPN);
                cmd1.Parameters.AddWithValue("@TongTienPN", tongTienPN);
                cmd1.Parameters.AddWithValue("@MaNV", maNV);
                cmd1.ExecuteNonQuery();

                // 2. Thêm vào bảng tblCT_PHIEUNHAP
                string queryCT = "INSERT INTO tblCT_PHIEUNHAP (MaPN, NgayNhap, GioNhap, LuuY) " +
                                 "VALUES (@MaPN, @NgayNhap, @GioNhap, @LuuY)";
                SqlCommand cmd2 = new SqlCommand(queryCT, con);
                cmd2.Parameters.AddWithValue("@MaPN", maPN);
                cmd2.Parameters.AddWithValue("@NgayNhap", DateTime.Parse(ngayNhap));
                cmd2.Parameters.AddWithValue("@GioNhap", TimeSpan.Parse(gioNhap));
                cmd2.Parameters.AddWithValue("@LuuY", luuY);
                cmd2.ExecuteNonQuery();

                con.Close();
            }

            // Cập nhật lại GridView sau khi thêm
            dsSanPham.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true); // Ẩn popup sau khi thêm xong
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
        }


        private string LayMaNVTuEmail(string emailNV)
        {
            string maNV = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT MaNV FROM tblNHANVIEN WHERE EmailNV = @EmailNV", con);
                cmd.Parameters.AddWithValue("@EmailNV", emailNV);
                object result = cmd.ExecuteScalar();
                if (result != null)
                    maNV = result.ToString();
            }
            return maNV;
        }

        private string GetNewMaNCC()
        {
            string maPN = string.Empty;

            // Lấy số lớn nhất của mã loại giày hiện tại và tạo mã mới
            string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(MaPN, 3, LEN(MaPN)) AS INT)), 0) + 1 FROM tblPHIEUNHAP";

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                int newNumber = (int)cmd.ExecuteScalar();
                maPN = newNumber.ToString().PadLeft(4, '0'); // Đảm bảo mã có 4 chữ số
                conn.Close();
            }

            return "PN" + maPN;
        }
        //protected void dsSanPham_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    string maLoai = dsSanPham.DataKeys[e.RowIndex].Value.ToString();

        //    using (SqlConnection conn = new SqlConnection(conStr))
        //    {
        //        conn.Open();
        //        // 3. Nếu không bị ràng buộc, tiến hành xóa
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("DELETE FROM tblPHIEUNHAP WHERE MaPN = @MaPN", conn);
        //            cmd.Parameters.AddWithValue("@MaPN", maLoai);
        //            cmd.ExecuteNonQuery();
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Phòng khi có lỗi khác
        //            string script = "alert('Lỗi khi xóa: " + ex.Message.Replace("'", "\\'") + "');";
        //            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alertError", script, true);
        //            e.Cancel = true;
        //        }
        //    }
        //}

        protected void dsSanPham_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string maPN = dsSanPham.DataKeys[e.RowIndex].Value.ToString();

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    // 1. Xóa trước trong tblCT_PHIEUNHAP
                    SqlCommand cmdCT = new SqlCommand("DELETE FROM tblCT_PHIEUNHAP WHERE MaPN = @MaPN", conn, tran);
                    cmdCT.Parameters.AddWithValue("@MaPN", maPN);
                    cmdCT.ExecuteNonQuery();

                    // 2. Sau đó xóa trong tblPHIEUNHAP
                    SqlCommand cmdPN = new SqlCommand("DELETE FROM tblPHIEUNHAP WHERE MaPN = @MaPN", conn, tran);
                    cmdPN.Parameters.AddWithValue("@MaPN", maPN);
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

            var ddlGiay = row.FindControl("ddlGiay") as DropDownList;
            var ddlNhanVien = row.FindControl("ddlNhanVien") as DropDownList;
            var txtGia = row.FindControl("txtGia") as TextBox;
            var txtSL = row.FindControl("txtSL") as TextBox;
            var txtTong = row.FindControl("txtTong") as TextBox;

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tblPHIEUNHAP SET MaGiay = @MaGiay, GiaPN = @GiaPN, SLPN = @SLPN, TongTienPN = @TongTienPN, MaNV = @MaNV WHERE MaPN = @MaPN", conn);
                cmd.Parameters.AddWithValue("@MaGiay", ddlGiay.SelectedValue);
                cmd.Parameters.AddWithValue("@GiaPN", decimal.Parse(txtGia.Text));
                cmd.Parameters.AddWithValue("@SLPN", int.Parse(txtSL.Text));
                cmd.Parameters.AddWithValue("@TongTienPN", decimal.Parse(txtTong.Text));
                cmd.Parameters.AddWithValue("@MaNV", ddlNhanVien.SelectedValue);
                cmd.Parameters.AddWithValue("@MaPN", maLoai);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
            }

            dsSanPham.EditIndex = -1;
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
                string query = @"SELECT MaPN as [Mã PN], CONVERT(varchar, NgayNhap, 103) as [Ngày nhập], GioNhap as [Giờ nhập], LuuY as [Lưu ý]
                 FROM tblCT_PHIEUNHAP WHERE MaPN = @MaPN";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPN", maPN);

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
            Response.AddHeader("content-disposition", "attachment;filename=DS_Phieu_Nhap.xls");
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
                dsSanPham.Columns[6].Visible = false;  // Ẩn cột "Sửa"
                dsSanPham.Columns[7].Visible = false;  // Ẩn cột "Xóa"
                dsSanPham.Columns[8].Visible = false;  // Ẩn cột "Xóa"

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