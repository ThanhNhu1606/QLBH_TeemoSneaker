using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Customer
{
    public partial class ThayDoiMatKhau : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void butDoiMatKhau_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            string email = txtEmail.Text.Trim();  // Email người dùng nhập vào
            string matKhauCu = txtMatKhau.Text.Trim();  // Mật khẩu hiện tại người dùng nhập vào
            string matKhauMoi = txtDoiMatKhau.Text.Trim();  // Mật khẩu mới người dùng nhập vào

            // Query để lấy MaKH và MatKhauKH từ bảng tblKHACHHANG và tblTAIKHOANKH
            string query = "SELECT tk.MaKH, tk.MatKhauKH FROM tblTAIKHOANKH tk INNER JOIN tblKHACHHANG kh ON tk.MaKH = kh.MaKH WHERE kh.EmailKH = @Email";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    // Kiểm tra mật khẩu cũ
                    using (SqlDataReader result = cmd.ExecuteReader()) // Sử dụng using để đảm bảo đóng DataReader sau khi sử dụng
                    {
                        if (result.Read())
                        {
                            string matKhauHienTai = result["MatKhauKH"].ToString();

                            // Kiểm tra mật khẩu cũ nhập vào có đúng không
                            if (matKhauCu == matKhauHienTai)
                            {
                                result.Close();
                                // Nếu đúng mật khẩu cũ, tiến hành cập nhật mật khẩu mới
                                string updateQuery = "UPDATE tblTAIKHOANKH SET MatKhauKH = @MatKhauMoi WHERE TaiKhoanKH = @Email";

                                using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, conn))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@MatKhauMoi", matKhauMoi);
                                    cmdUpdate.Parameters.AddWithValue("@Email", email);

                                    // Thực hiện cập nhật
                                    int rowsAffected = cmdUpdate.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        ClearFields();
                                        // Đổi mật khẩu thành công
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
                                    }
                                    else
                                    {
                                        ClearFields();
                                        lblMessage.Text = "Có lỗi xảy ra, vui lòng thử lại!";
                                    }
                                }
                            }
                            else
                            {
                                ClearFields();
                                // Mật khẩu cũ không đúng
                                lblMessage.Text = "Mật khẩu cũ không đúng!";
                            }
                        }
                        else
                        {
                            ClearFields();
                            // Không tìm thấy email trong hệ thống
                            lblMessage.Text = "Email không tồn tại!";
                        }
                    }
                }
            }
        }


        private void ClearFields()
        {
            txtEmail.Text = "";
            txtMatKhau.Text = "";
            txtDoiMatKhau.Text = "";
        }

    }
}