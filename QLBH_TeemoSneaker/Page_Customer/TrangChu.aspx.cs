using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Customer
{
    public partial class TrangChu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            SqlDataSource_listSanPham.SelectCommand = "SELECT * FROM tblGIAY";
            listSanPham.DataBind();

            if (Session["ScrollToFilter"] != null && (bool)Session["ScrollToFilter"])
            {
                ClientScript.RegisterStartupScript(this.GetType(), "scrollScript", "scrollToFilter();", true);
                Session["ScrollToFilter"] = null; // Xóa session để không cuộn mỗi lần tải lại trang
            }

            if (!IsPostBack)
            {
                if (Session["CartCount"] != null)
                {
                    lblCartCount.Text = Session["CartCount"].ToString();
                }
                else
                {
                    lblCartCount.Text = "0";  // Nếu Session bị mất
                }
            }

            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (Session["MaKH"] != null)
            {
                
                btnDangXuat.Visible = true;
            }
            else
            {
                
                btnDangXuat.Visible = false;
            }

        }


        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            string timkiem = txtTimKiem.Text.Trim();
            string querry = "SELECT * FROM tblGIAY WHERE (TenGiay LIKE '%' + @timkiem + '%')";

            // Xóa các tham số trước khi thêm mới
            SqlDataSource_listSanPham.SelectParameters.Clear();

            // Thêm tham số mới
            SqlDataSource_listSanPham.SelectParameters.Add("timkiem", timkiem);

            // Thiết lập truy vấn
            SqlDataSource_listSanPham.SelectCommand = querry;

            // Làm mới dữ liệu
            listSanPham.DataBind();

            // Cuộn xuống phần danh sách sản phẩm
            //        ScriptManager.RegisterStartupScript(this, GetType(), "ScrollToProducts",
            //"setTimeout(function() { window.scrollTo({ top: document.getElementById('#boloc-danhsach').offsetTop, behavior: 'smooth' }); }, 300);", true);

            ClientScript.RegisterStartupScript(this.GetType(), "ScrollToDiv", "location.hash = '#boloc-danhsach';", true);
        }

        protected void btnGioHang_Click(object sender, EventArgs e)
        {

        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            // Xóa thông tin người dùng khỏi session khi đăng xuất
            Session["MaKH"] = null;
            Session["TenKH"] = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
        }

        protected void btnNguoiDung_Click(object sender, EventArgs e)
        {

        }

        protected void Xeptheo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string querry = "SELECT * FROM [tblGIAY]";
            if (Xeptheo.SelectedValue.Trim() == "*")
            {
                SqlDataSource_listSanPham.SelectCommand = "SELECT * FROM [tblGIAY]";
            }
            else if (Xeptheo.SelectedValue.Trim() == "AZ")
            {
                SqlDataSource_listSanPham.SelectCommand = "SELECT * FROM [tblGIAY] ORDER BY TenGiay ASC";
            }
            else if (Xeptheo.SelectedValue.Trim() == "ZA")
            {
                SqlDataSource_listSanPham.SelectCommand = "SELECT * FROM [tblGIAY] ORDER BY TenGiay DESC";
            }
            else if (Xeptheo.SelectedValue.Trim() == "GTD")
            {
                SqlDataSource_listSanPham.SelectCommand = "SELECT * FROM [tblGIAY] ORDER BY GiaGiay ASC";
            }
            else if (Xeptheo.SelectedValue.Trim() == "GDD")
            {
                SqlDataSource_listSanPham.SelectCommand = "SELECT * FROM [tblGIAY] ORDER BY GiaGiay DESC";
            }

            // Thiết lập truy vấn
            //SqlDataSource_listSanPham.SelectCommand = querry;

            // Làm mới dữ liệu
            listSanPham.DataBind();

            Session["ScrollToFilter"] = true;
        }

        protected void LoaiSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoaiSanPham.SelectedValue.Trim() == "*")
            {
                SqlDataSource_listSanPham.SelectCommand = "SELECT * FROM [tblGIAY]";
            }
            else
            {
                // Sử dụng tham số để tránh SQL Injection
                SqlDataSource_listSanPham.SelectCommand = "SELECT * FROM [tblGIAY] WHERE MaLoaiGiay = @MaLoaiGiay";
                SqlDataSource_listSanPham.SelectParameters.Clear();
                SqlDataSource_listSanPham.SelectParameters.Add("MaLoaiGiay", LoaiSanPham.SelectedValue.Trim());
            }

            // Làm mới dữ liệu
            listSanPham.DataBind();

            Session["ScrollToFilter"] = true;
        }

      
    }
}