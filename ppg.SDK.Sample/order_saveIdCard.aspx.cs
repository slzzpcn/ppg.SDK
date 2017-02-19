using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using ppg.SDK;

namespace ppg.SDK.Sample
{
    public partial class order_saveIdCard : System.Web.UI.Page
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
            string strMethod = "/rsService/order/saveIdCard";//API方法
            string postData = "orderNumber=5491850734555033528&idCard.no=300622198810123522";   //请求数据
            string strPostMethod = "POST";   //请求方式GET/POST
            bool isFile = true;            //是否上传文件
            IDictionary<string, FileItem> fileParams = new Dictionary<string, FileItem>();//上传的文件

            string frontPic = "D:\\test.jpg";
            string backPic = "D:\\test2.jpg";
            string paramName = "idCard.frontPic";

            //图片-frontPic参数
            FileItem file = new FileItem(frontPic);
            var fileStream = new FileStream(frontPic, FileMode.Open, FileAccess.Read);

            fileParams.Add(paramName, file);
            //用于签名=即图片对应的文件流，哈希后的值
            postData += paramName + "=" + ppgSDKUtility.SHA1File(fileStream).ToLower();



            //图片-backPic
            paramName = "idCard.backPic";
            file = new FileItem(backPic);
            fileStream = new FileStream(backPic, FileMode.Open, FileAccess.Read);

            fileParams.Add(paramName, file);
            postData += paramName + "=" + ppgSDKUtility.SHA1File(fileStream).ToLower();



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