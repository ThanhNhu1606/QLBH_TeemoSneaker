using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker
{
    public partial class Header_Footer : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra nếu người dùng đã đăng nhập
            string tenKH = Session["TenKH"] as string;

            if (!string.IsNullOrEmpty(tenKH))
            {
                // Hiển thị tên người dùng sau khi đăng nhập
                LabelHoTen.Text = "Xin chào " + tenKH+"!";
                LabelHoTen.Visible = true;  // Hiển thị tên người dùng
            }
            else
            {
                LabelHoTen.Text = "Xin chào quý khách!";
            }

            //if (!IsPostBack)
            //{
            //    if (Session["TenKH"] != null)
            //    {
            //        LabelHoTen.Text = Session["TenKH"].ToString();
            //        LabelHoTen.Visible = true;
            //    }
            //    else
            //    {
            //        LabelHoTen.Text = "";
            //        LabelHoTen.Visible = false;
            //    }
            //}
        }

        
    }
}