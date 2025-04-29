using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QLBH_TeemoSneaker
{
    public partial class Header_Footer_Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DangXuat_ServerClick(object sender, EventArgs e)
        {
            Session.Clear(); // hoặc Session["EmailNV"] = null;

            // Chuyển hướng đến trang đăng nhập sử dụng MasterPage2
            Response.Redirect("~/Page_Customer/DangNhap.aspx");
        }
    }
}