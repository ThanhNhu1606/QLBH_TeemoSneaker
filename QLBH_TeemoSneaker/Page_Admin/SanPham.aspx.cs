using QLBH_TeemoSneaker.Page_Admin;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Admin
{
    public partial class SanPham : System.Web.UI.Page
    {
        // Chuỗi kết nối được khai báo 1 lần
        private readonly string conStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoaiGiay_SelectedIndexChanged(sender, e);
                LoaiSize_SelectedIndexChanged(sender, e);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "init", "hidePopup();", true);
            }
            else if (Request["__EVENTTARGET"] != null && Request["__EVENTTARGET"].Contains("butThemMoi"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "showPopup(); resetTextBoxes();", true);
            }
        }

        protected void LoaiGiay_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true); // Ẩn popup sau khi thêm xong
        }

        protected void LoaiSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true); // Ẩn popup sau khi thêm xong
        }

        protected void GiaLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true); // Ẩn popup sau khi thêm xong
            dsSanPham.DataBind(); // Cập nhật GridView theo lọc giá
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "init", "hidePopup();", true);
            string timkiem = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(timkiem))
            {
                SqlDataSourceLoaiGiay.SelectCommand = "SELECT * FROM tblGIAY";
                SqlDataSourceLoaiGiay.SelectParameters.Clear();
            }
            else
            {
                string query = @"
    SELECT * FROM tblGIAY 
    WHERE 
        TenGiay LIKE '%' + @timkiem + '%'";
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
            string maLoaiGiay = ddlLoaiGiay.SelectedValue.Trim();
            string maGiay = GetNewMaGiay(maLoaiGiay);
            string tenGiay = txtTenGiayMoi.Text;
            string maSize = ddlLoaiSize.SelectedValue.Trim();
            string giaGiay = txtGiaGiayMoi.Text;
            string soLuongTon = txtSoLuongTonMoi.Text;

            // Lấy tên file và tạo đường dẫn ảnh
            string fileName = Path.GetFileName(UpHinhAnh.FileName);
            string hinhAnh = "/Image/" + fileName;

            // Đường dẫn vật lý để lưu file vào thư mục Image trong project
            string absolutePath = Server.MapPath(hinhAnh);

            // Lưu file ảnh vào thư mục Images
            UpHinhAnh.SaveAs(absolutePath);

            // Thực hiện thêm vào cơ sở dữ liệu
            string insertQuery = "INSERT INTO tblGIAY (MaLoaiGiay, MaGiay, TenGiay,MaSize, GiaGiay,SoLuongTon,HinhAnh) VALUES (@MaLoaiGiay, @MaGiay, @TenGiay,@MaSize, @GiaGiay,@SoLuongTon,@HinhAnh)";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@MaLoaiGiay", maLoaiGiay);
                cmd.Parameters.AddWithValue("@MaGiay", maGiay);
                cmd.Parameters.AddWithValue("@TenGiay", tenGiay);
                cmd.Parameters.AddWithValue("@MaSize", maSize);
                cmd.Parameters.AddWithValue("@GiaGiay", giaGiay);
                cmd.Parameters.AddWithValue("@SoLuongTon", soLuongTon);
                cmd.Parameters.AddWithValue("@HinhAnh", hinhAnh);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Cập nhật lại GridView sau khi thêm
            dsSanPham.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidePopup", "hidePopup();", true); // Ẩn popup sau khi thêm xong
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
        }

        private string GetNewMaGiay(string maLoaiGiay)
        {
            string maGiay = string.Empty;

            // Lấy ký tự thứ 4 trong MaLoaiGiay (tức là số thứ nhất sau 'LG')
            string maLoaiGiaySubstring = maLoaiGiay.Substring(5, 1); // Lấy ký tự thứ 4 trong MaLoaiGiay, ví dụ: từ "LG0001" sẽ lấy "1"

            // Truy vấn để lấy số lớn nhất của mã giày có cùng định dạng "G" + maLoaiGiaySubstring
            string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(MaGiay, 3, LEN(MaGiay) - 2) AS INT)), 0) + 1 FROM tblGIAY WHERE MaGiay LIKE 'G" + maLoaiGiaySubstring + "%'";

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                int newNumber = (int)cmd.ExecuteScalar();
                //maGiay = newNumber.ToString().PadLeft(4, '0'); // Đảm bảo mã có 4 chữ số
                maGiay = newNumber.ToString("D4");
                conn.Close();
            }

            return "G" + maLoaiGiaySubstring + maGiay;  // Tạo mã giày theo định dạng Gx000y
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
                    SqlCommand cmd = new SqlCommand("DELETE FROM tblGIAY WHERE MaGiay = @MaGiay", conn);
                    cmd.Parameters.AddWithValue("@MaGiay", maLoai);
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
            string maGiay = dsSanPham.DataKeys[e.RowIndex].Value.ToString(); // đúng hơn là MaGiay
            GridViewRow row = dsSanPham.Rows[e.RowIndex];

            var ddlLoaiGiay = row.FindControl("ddlLoaiGiay") as DropDownList;
            var txtTenGiay = row.FindControl("txtTenGiay") as TextBox;
            var ddlLoaiSize = row.FindControl("ddlLoaiSize") as DropDownList;
            var txtGiaGiay = row.FindControl("txtGiaGiay") as TextBox;
            var txtSoLuongTon = row.FindControl("txtSoLuongTon") as TextBox;
            var txtHinhAnh = row.FindControl("txtHinhAnh") as TextBox;

            if (ddlLoaiGiay == null || txtTenGiay == null || ddlLoaiSize == null || txtGiaGiay == null || txtSoLuongTon == null || txtHinhAnh == null)
            {
                throw new Exception("Một hoặc nhiều control không tồn tại trong EditItemTemplate.");
            }

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tblGIAY SET MaLoaiGiay = @MaLoaiGiay, TenGiay = @TenGiay, MaSize = @MaSize, GiaGiay = @GiaGiay, SoLuongTon = @SoLuongTon, HinhAnh = @HinhAnh WHERE MaGiay = @MaGiay", conn);
                cmd.Parameters.AddWithValue("@MaLoaiGiay", ddlLoaiGiay.SelectedValue);
                cmd.Parameters.AddWithValue("@TenGiay", txtTenGiay.Text.Trim());
                cmd.Parameters.AddWithValue("@MaSize", ddlLoaiSize.SelectedValue);
                cmd.Parameters.AddWithValue("@GiaGiay", txtGiaGiay.Text.Trim());
                cmd.Parameters.AddWithValue("@SoLuongTon", txtSoLuongTon.Text.Trim());
                cmd.Parameters.AddWithValue("@HinhAnh", txtHinhAnh.Text.Trim());
                cmd.Parameters.AddWithValue("@MaGiay", maGiay);

                cmd.ExecuteNonQuery();
            }

            dsSanPham.EditIndex = -1;
            dsSanPham.DataBind();
        }

        protected void butExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DS_San_Pham.xls");
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
                dsSanPham.Columns[7].Visible = false;  // Ẩn cột "Sửa"
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


