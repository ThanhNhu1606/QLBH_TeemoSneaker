using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Customer
{
    public partial class DangKy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string GenerateMaKH()
        {
            string newMaKH = "KH0001"; // Mã mặc định nếu không có dữ liệu

            string query = "SELECT TOP 1 MaKH FROM tblKHACHHANG ORDER BY MaKH DESC";

            string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string lastMaKH = result.ToString();
                        int number = int.Parse(lastMaKH.Substring(2)); // Lấy phần số
                        newMaKH = "KH" + (number + 1).ToString("D4"); // Tăng số thứ tự
                    }
                }
            }
            return newMaKH;
        }

        protected void butDangKy_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

            string maKH = GenerateMaKH();
            string hoKH = txtHo.Text.Trim();
            string tenKH = txtTen.Text.Trim();
            string sdtKH = txtDienThoai.Text.Trim();
            bool gioiTinhKH = rblGioiTinh.SelectedValue == "Nam" ? true : false;
            DateTime ngaySinhKH = DateTime.Parse(txtNgaySinh.Text);
            string diaChiKH = txtDiaChi.Text.Trim();
            string emailKH = txtEmail.Text.Trim();
            string matKhauKH = txtMatKhau.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // **1. Kiểm tra email đã tồn tại chưa**
                string checkEmailQuery = "SELECT COUNT(*) FROM tblKHACHHANG WHERE EmailKH = @EmailKH";
                using (SqlCommand checkCmd = new SqlCommand(checkEmailQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@EmailKH", emailKH);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        lblMessage.Text = "Email đã tồn tại! Vui lòng dùng email khác";
                        ClearFields();
                        return; // Dừng không thực hiện INSERT
                    }
                }

                // **2. Nếu email chưa tồn tại, thực hiện INSERT**
                string queryKH = "INSERT INTO tblKHACHHANG (MaKH, HoKH, TenKH, SdtKH, GioiTinhKH, NgaySinhKH, DiaChiKH, EmailKH) " +
                                "VALUES (@MaKH, @HoKH, @TenKH, @SdtKH, @GioiTinhKH, @NgaySinhKH, @DiaChiKH, @EmailKH)";

                using (SqlCommand cmd = new SqlCommand(queryKH, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKH", maKH);
                    cmd.Parameters.AddWithValue("@HoKH", hoKH);
                    cmd.Parameters.AddWithValue("@TenKH", tenKH);
                    cmd.Parameters.AddWithValue("@SdtKH", sdtKH);
                    cmd.Parameters.AddWithValue("@GioiTinhKH", gioiTinhKH);
                    cmd.Parameters.AddWithValue("@NgaySinhKH", ngaySinhKH);
                    cmd.Parameters.AddWithValue("@DiaChiKH", diaChiKH);
                    cmd.Parameters.AddWithValue("@EmailKH", emailKH);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        string queryTK = "INSERT INTO tblTAIKHOANKH (TaiKhoanKH, MatKhauKH, MaKH) " +
                                         "VALUES (@TaiKhoanKH, @MatKhauKH, @MaKH)";

                        using (SqlCommand cmdTK = new SqlCommand(queryTK, conn))
                        {
                            cmdTK.Parameters.AddWithValue("@TaiKhoanKH", emailKH);
                            cmdTK.Parameters.AddWithValue("@MatKhauKH", matKhauKH);
                            cmdTK.Parameters.AddWithValue("@MaKH", maKH);

                            int resultTK = cmdTK.ExecuteNonQuery();
                            if (resultTK > 0)
                            {
                                ClearFields();
                                //Response.Redirect("DangNhap.aspx");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
                            }
                            else
                            {
                                lblMessage.Text = "Lỗi khi tạo tài khoản!";
                                ClearFields();
                            }
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Đăng ký thất bại!";
                        ClearFields();
                    }
                }
            }
        }

        // **Hàm xóa tất cả các trường nhập**
        private void ClearFields()
        {
            txtHo.Text = "";
            txtTen.Text = "";
            txtDienThoai.Text = "";
            txtNgaySinh.Text = "";
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtMatKhau.Text = "";
            rblGioiTinh.ClearSelection(); // Xóa lựa chọn giới tính
        }
    }
}