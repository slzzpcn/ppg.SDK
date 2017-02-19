using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ppg.SDK;

namespace ppg.SDK.Sample
{
    public partial class item_getStockExt : System.Web.UI.Page
    {
        /// <summary>
        /// API基本资料
        /// </summary>
        protected string APP_KEY = ppgSDKUtility.GetAppSetting("APP_KEY");
        protected string APP_SECRET = ppgSDKUtility.GetAppSetting("APP_SECRET");
        protected string API_URL = ppgSDKUtility.GetAppSetting("API_URL");

        protected void Page_Load(object sender, EventArgs e)
        {

            string strResult = "";
            string strMethod = "/rsService/item/getStockExt";//API方法
            string postData = "sku=1111";   //请求数据
            string strPostMethod = "GET";   //请求方式GET/POST
            bool isFile = false;            //是否上传文件
            IDictionary<string, FileItem> fileParams = null;//上传的文件

            ppgSDKUtility client = new ppgSDKUtility(APP_KEY, APP_SECRET, API_URL, strMethod, postData, strPostMethod, isFile, fileParams);

            //返回结果
            strResult = client.PostAndReturn();

            /*
             其它业务逻辑处理...
             */

            Response.Write(strResult);
        }
    }
}