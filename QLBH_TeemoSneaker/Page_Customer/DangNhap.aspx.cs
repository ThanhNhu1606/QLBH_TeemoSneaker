using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Customer
{
    public partial class TaiKhoanKH : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void butDangNhap_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            string email = txtEmail.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            // Kiểm tra đăng nhập nhân viên
            string queryNV = @"
        SELECT n.EmailNV
        FROM tblNHANVIEN n
        JOIN tblCHUCVUNV c ON n.MaChucVu = c.MaChucVu
        WHERE n.EmailNV = @EmailNV AND c.MatKhau = @MatKhau";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(queryNV, conn))
                {
                    cmd.Parameters.AddWithValue("@EmailNV", email);  // Sử dụng email nhập vào làm EmailNV
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                    var resultNV = cmd.ExecuteScalar();
                    if (resultNV != null)
                    {
                        // Lưu EmailNV vào session nếu đăng nhập thành công
                        string emailNVLogin = resultNV.ToString();
                        Session["EmailNV"] = emailNVLogin;

                        // Chuyển hướng đến trang ThongKe.aspx
                        Response.Redirect("/Page_Admin/LoaiSP.aspx");
                    }
                    else
                    {
                        // Nếu không phải nhân viên, kiểm tra đăng nhập khách hàng
                        string queryKhachHang = "SELECT MaKH FROM tblTAIKHOANKH WHERE TaiKhoanKH = @Email AND MatKhauKH = @MatKhau";

                        using (SqlCommand cmdKH = new SqlCommand(queryKhachHang, conn))
                        {
                            cmdKH.Parameters.AddWithValue("@Email", email);
                            cmdKH.Parameters.AddWithValue("@MatKhau", matKhau);

                            var resultKH = cmdKH.ExecuteScalar();
                            if (resultKH != null)
                            {
                                // Lưu MaKH vào session
                                string maKH = resultKH.ToString();
                                Session["MaKH"] = maKH;

                                // Query để lấy TenKH từ bảng tblKHACHHANG
                                string queryTenKH = "SELECT TenKH FROM tblKHACHHANG WHERE MaKH = @MaKH";
                                SqlCommand cmdTenKH = new SqlCommand(queryTenKH, conn);
                                cmdTenKH.Parameters.AddWithValue("@MaKH", maKH);

                                var tenKH = cmdTenKH.ExecuteScalar();
                                if (tenKH != null)
                                {
                                    // Lưu TenKH vào session
                                    Session["TenKH"] = tenKH.ToString();
                                }

                                ClearFields();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
                            }
                            else
                            {
                                ClearFields();
                                lblMessage.Text = "Email hoặc mật khẩu không đúng!";
                            }
                        }
                    }
                }
            }
        }

        private void ClearFields()
        {
            txtEmail.Text = "";
            txtMatKhau.Text = "";

        }

    }
}