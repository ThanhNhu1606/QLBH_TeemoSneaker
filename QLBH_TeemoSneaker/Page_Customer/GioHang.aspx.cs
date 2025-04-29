using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker.Page_Customer
{
    public partial class GioHang : System.Web.UI.Page
    {
        DataTable dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();

        }
        
        protected void LoadData()
        {
            dt = (DataTable)Session["tblGIAY"];
            dsSanPham.DataSource = dt;
            dsSanPham.DataBind();

            for (int i = 0; i < dsSanPham.Columns.Count; i++)
            {
                dsSanPham.Columns[i].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }

            if (dt != null)
            {
                double tong = TinhTongTien(dt);
                Session["tong"] = tong;     // lưu để truyền qua trang DonHang.aspx
                lblTongTien.Text = String.Format("{0:0,000}", tong)+" VNĐ";
            }
        }


        protected void dsSanPham_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session["tblGIAY"];
            GridViewRow row = dsSanPham.Rows[e.RowIndex];

            // Lấy giá trị mới từ TextBox
            TextBox txtSoluong = (TextBox)row.FindControl("txtSoLuong");
            int soluong = Convert.ToInt32(txtSoluong.Text);

            dt.Rows[e.RowIndex]["SoLuong"] = soluong;
            dt.Rows[e.RowIndex]["TongTien"] = Convert.ToDouble(dt.Rows[e.RowIndex]["GiaGiay"]) * soluong;

            dsSanPham.EditIndex = -1;
            Session["tblGIAY"] = dt;

            // Cập nhật lại số lượng sản phẩm trong giỏ hàng
            Session["CartCount"] = dt.Rows.Count;
            LoadData();
        }

        protected void dsSanPham_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dsSanPham.EditIndex = e.NewEditIndex;
            LoadData();
        }


        protected void dsSanPham_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)Session["tblGIAY"];
            dt.Rows.RemoveAt(e.RowIndex);
            Session["tblGIAY"] = dt;
            // Cập nhật lại số lượng sản phẩm trong giỏ hàng
            Session["CartCount"] = dt.Rows.Count;
            LoadData();
        }

        protected void dsSanPham_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dsSanPham.EditIndex = -1;
            LoadData();
        }
        protected double TinhTongTien(DataTable dt)
        {
            if (dt == null)
                return 0;
            double sum = 0;
            foreach (DataRow row in dt.Rows)
                sum += Convert.ToDouble(row["TongTien"]);
            return sum;
        }
        protected void butDathang_Click(object sender, EventArgs e)
        {
            dt = (DataTable)Session["tblGIAY"];
        }

        protected void gvSanPham_DataBound(object sender, EventArgs e)
        {
            if (dsSanPham.Rows.Count == 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("TenSanPham");
                dt.Columns.Add("Gia");
                dt.Rows.Add(dt.NewRow()); // Thêm dòng trống
                dsSanPham.DataSource = dt;
                dsSanPham.DataBind();
                dsSanPham.Rows[0].Cells.Clear();
                dsSanPham.Rows[0].Cells.Add(new TableCell { ColumnSpan = dsSanPham.Columns.Count });
                dsSanPham.Rows[0].Cells[0].Text = "Không có sản phẩm nào.";
                dsSanPham.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        protected void butMuahang_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem Session["tblGIAY"] có tồn tại và không phải null
            if (Session["tblGIAY"] != null)
            {
                DataTable dt = (DataTable)Session["tblGIAY"];

                // Cập nhật lại số lượng giỏ hàng
                Session["CartCount"] = dt.Rows.Count;
            }
            else
            {
                // Nếu giỏ hàng rỗng, gán số lượng giỏ hàng là 0
                Session["CartCount"] = 0;
                // Cập nhật CSS để căn giữa giao diện khi giỏ hàng rỗng
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CeneterCart", "document.querySelector('.table-view').style.margin = 'auto'; document.querySelector('.table-view').style.textAlign = 'center'; document.querySelector('.table-view').style.width = 'fit-content';", true);
            }
            // Chuyển hướng sang TrangChu.aspx
            Response.Redirect("~/Page_Customer/TrangChu.aspx");
        }

    }
}