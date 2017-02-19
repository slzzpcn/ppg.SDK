using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

namespace ppg.SDK.OPEN
{
    public partial class SaveIdCard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(DateTime.Now.ToString() + "<br>");
            for (int i = 0; i < Request.Form.Count; i++)
            {
                Response.Write(Request.Form.Keys[i] + "=" + Request.Form[i].ToString() + "<br>");
            }

            Response.Write(Request.Files.Count + "<br>");
            if (Request.Files.Count>0)
            {
                for(int i =0;i<Request.Files.Count;i++)
                {
                    HttpPostedFile file = Request.Files[i];
                    string fileName = DateTime.Now.ToString("yyMMddHHmmss");
                    fileName += "-" + (i+1).ToString() + ".jpg";

                    file.SaveAs(Server.MapPath(fileName));

                    Response.Write(fileName + " 保存成功<br>");
                }
            }
        }
    }
}