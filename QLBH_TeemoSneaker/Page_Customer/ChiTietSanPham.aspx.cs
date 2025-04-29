using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Customer
{
    public partial class ChiTietSanPham : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string MaGiay = "";
            if (Request.QueryString["MaGiay"] != null)
                MaGiay = Request.QueryString["MaGiay"];
            Session["MaGiay"] = MaGiay;
            LoadSP(MaGiay);

            if (!IsPostBack)
            {
                // Kiểm tra xem Session["CartCount"] đã có chưa
                if (Session["CartCount"] != null)
                {
                    lblCartCount.Text = Session["CartCount"].ToString();
                }
                else
                {
                    lblCartCount.Text = "0";  // Nếu Session chưa có, hiển thị là 0
                }
            }

        }


        private void LoadCartCount()
        {

            int cartCount = 0;

            // Kiểm tra xem Session có giỏ hàng không
            if (Session["tblGiay"] != null)
            {
                DataTable dtGH = (DataTable)Session["tblGiay"];
                cartCount = dtGH.Rows.Count;  // Số lượng sản phẩm trong giỏ hàng
            }

            // Cập nhật số sản phẩm trong giỏ hàng
            lblCartCount.Text = cartCount.ToString();
        }

        protected void LoadSP(string MaGiay)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            SqlConnection con = new SqlConnection(conStr);
            SqlDataAdapter adapt = new SqlDataAdapter
                ("SELECT g.MaGiay, g.TenGiay, g.GiaGiay, g.MaLoaiGiay, g.SoLuongTon, g.HinhAnh, lg.DacDiem, s.LoaiSize " +
               "FROM tblGIAY g " +
               "JOIN tblLOAIGIAY lg ON lg.MaLoaiGiay = g.MaLoaiGiay " +
               "JOIN tblSIZE s ON s.MaSize = g.MaSize " +
               "WHERE MaGiay=" + "'" + MaGiay + "'", con);

            DataTable dt = new DataTable();
            adapt.Fill(dt);
            if (dt.Rows.Count == 0)
                return;
            lblTenSanPham.Text = dt.Rows[0]["TenGiay"].ToString();
            lblMaSanPham.Text = "Mã sản phẩm: " + dt.Rows[0]["MaGiay"].ToString();
            lblDonGiaBan.Text = string.Format("{0:0,000} VNĐ", dt.Rows[0]["GiaGiay"]);
            imgHinh.ImageUrl = dt.Rows[0]["HinhAnh"].ToString();
            lblGhiChu.Text = dt.Rows[0]["DacDiem"].ToString();
            lblSoLuongTon.Text = "Tồn kho: " + dt.Rows[0]["SoLuongTon"].ToString();
            lblSize.Text = dt.Rows[0]["LoaiSize"].ToString() +" cm";
            ViewState["tblGiay"] = dt;
        }
                

        protected void butInsert_Click(object sender, EventArgs e)
        {
            DataTable dtSP = (DataTable)ViewState["tblGiay"];
            DataTable dtGH;  
            int Soluong = 0;
            if (Session["tblGiay"] == null) 
            {
                dtGH = new DataTable();
                dtGH.Columns.Add("MaGiay");
                dtGH.Columns.Add("HinhAnh");
                dtGH.Columns.Add("TenGiay");
                dtGH.Columns.Add("GiaGiay");
                dtGH.Columns.Add("LoaiSize");
                dtGH.Columns.Add("SoLuong");
                dtGH.Columns.Add("TongTien");
                
            }
            else // lay gio hang tu Session
                dtGH = (DataTable)Session["tblGiay"];

            string MaGiay = (string)Session["MaGiay"];
            int pos = TimSP(MaGiay, dtGH);        // tim vi tri san pham trong gio hang
            if (pos != -1)  // neu tim thay tai vi tri pos
            {
                //cap nhat lai cot so luong, tong tien
                Soluong = Convert.ToInt32(dtGH.Rows[pos]["SoLuong"]) + int.Parse(txtSoluong.Text);
                dtGH.Rows[pos]["SoLuong"] = Soluong;
                dtGH.Rows[pos]["TongTien"] = Convert.ToDouble(dtSP.Rows[0]["GiaGiay"]) * Soluong;
            }
            else    //san pham chua co trong gio hang: Them vao gio hang
            {
                Soluong = int.Parse(txtSoluong.Text);
                DataRow dr = dtGH.NewRow();//tạo một dòng mới
                                           // gán dữ liệu cho từng cột trong dòng mới
                dr["MaGiay"] = dtSP.Rows[0]["MaGiay"];
                dr["HinhAnh"] = dtSP.Rows[0]["HinhAnh"];
                dr["TenGiay"] = dtSP.Rows[0]["TenGiay"];
                dr["GiaGiay"] = dtSP.Rows[0]["GiaGiay"];
                dr["LoaiSize"] = dtSP.Rows[0]["LoaiSize"];
                dr["SoLuong"] = int.Parse(txtSoluong.Text);
                dr["TongTien"] = Convert.ToDouble(dtSP.Rows[0]["GiaGiay"]) * Soluong;
                //thêm dòng mới vào giỏ hàng
                dtGH.Rows.Add(dr);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "showModal();", true);
            }
            //lưu gio hang vao session
            Session["tblGiay"] = dtGH;
            Session["CartCount"] = dtGH.Rows.Count;
            //  Label lbSoluong = (Label)this.Master.FindControl("lbSoluong");
            //lbSoluong.Text = dtGH.Rows.Count.ToString();
            //Response.Redirect("GioHang.aspx");
            LoadCartCount();
        }
       
        public static int TimSP(string MaGiay, DataTable dt)
        {
            int pos = -1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["MaGiay"].ToString().ToLower() == MaGiay.ToLower())
                {
                    pos = i;
                    break;
                }
            }
            return pos;
        }

        protected void btnMinus_Click(object sender, EventArgs e)
        {
            int soLuong = int.Parse(txtSoluong.Text);
            if (soLuong > 1)
            {
                soLuong--;
                txtSoluong.Text = soLuong.ToString();
            }
        }

        protected void btnPlus_Click(object sender, EventArgs e)
        {
            int soLuong = int.Parse(txtSoluong.Text);
            soLuong++;
            txtSoluong.Text = soLuong.ToString();
        }

        protected void butCancel_Click(object sender, EventArgs e)
        {
            // Giữ lại số lượng giỏ hàng trong Session
            Session["CartCount"] = Session["CartCount"] ?? 0;

            // Chuyển hướng sang TrangChu.aspx
            Response.Redirect("~/Page_Customer/TrangChu.aspx");
        }

    }
}