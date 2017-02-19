using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Xml;
using System.Data;
using System.Text;
using System.IO;

namespace ppg.SDK
{
    public partial class API : System.Web.UI.Page
    {
        protected string APP_KEY = ppgSDKUtility.GetAppSetting("APP_KEY");
        protected string APP_SECRET = ppgSDKUtility.GetAppSetting("APP_SECRET");
        protected string API_URL = ppgSDKUtility.GetAppSetting("API_URL");

        protected string strResult = "";
        protected string strLastUrl = "";
        protected string strSDK = "";
        protected string strSDK_Method = "";
        protected string strSDK_PostData = "";
        protected string strMethod = "";
        protected string strPostParam = "";
        protected string strPostMethod = "GET";
        protected string strReqHtml = "";

        protected string contentType = "";

        protected string type = "";

        /// <summary>
        /// 显示API的请求参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt, dtParam;
            string temp = "";
            string required = "";
            type = Request["type"];
            dt = getMethod(type, out dtParam);

            if (dt != null)
            {
                strSDK_Method = dt.Rows[0]["method"].ToString();
                strReqHtml += "<tr class=\"tr_title\"><td>方法名</td><td>" + strSDK_Method.Replace("/rsService/", "") + "</td><td>" + dt.Rows[0]["desc"].ToString() + "</td></tr>";
                strPostMethod = dt.Rows[0]["postMethod"].ToString();
            }

            if (dtParam != null)
            {
                for (int i = 0; i < dtParam.Rows.Count; i++)
                {
                    temp = dtParam.Rows[i]["name"].ToString();
                    required = dtParam.Rows[i]["required"].ToString();
                    required = required.ToLower() == "true" ? "<span class=\"red\">*</span>" : "";

                    strReqHtml += "<tr class=\"td_title\"><td>" + required + temp + "</td>";

                    //file类型
                    if (dtParam.Rows[i]["type"].ToString().ToLower() == "byte[]")
                    {
                        strReqHtml += "<td><input type=\"text\" name=\"" + temp + "\" id=\"" + temp + "\" class=\"input\" /><br/>如：D:\\test.jpg</td>";
                    }
                    else
                    {
                        strReqHtml += "<td><input type=\"text\" name=\"" + temp + "\" id=\"" + temp + "\" class=\"input\" value=\"" + Request[temp] + "\" /></td>";
                    }

                    strReqHtml += "<td>" + dtParam.Rows[i]["desc"].ToString() + "</td></tr>";
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable dt, dtParam;
            string postData = "";
            string paramName = "";
            bool isFile = false;
            IDictionary<string, FileItem> fileParams = new Dictionary<string,FileItem>();

            strPostMethod = "GET";

            type = Request["type"];
            dt = getMethod(type, out dtParam); ;
            if (dt != null)
            {
                strMethod = dt.Rows[0]["method"].ToString();
                strPostMethod = dt.Rows[0]["postMethod"].ToString();

            }
            

            if (dtParam != null)
            {
                for (int i = 0; i < dtParam.Rows.Count; i++)
                {
                    paramName = dtParam.Rows[i]["name"].ToString();
                    if (i > 0)
                    {
                        postData += "&";
                    }



                    //上传文件类型的
                    if (dtParam.Rows[i]["type"].ToString().ToLower() == "byte[]" && Request.Form[paramName] != null && Request.Form[paramName] != "")
                    {
                        isFile = true;
                        string filePath = Request.Form[paramName];

                        filePath = filePath.Replace("//","////");

                        FileItem file = new FileItem(filePath);
                        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                        fileParams.Add(paramName, file);

                        //用于签名=即图片对应的文件流，哈希后的值
                        postData += paramName + "=" + ppgSDKUtility.SHA1File(fileStream).ToLower();
                    }
                    else
                    {
                        postData += paramName + "=" + Request.Form[paramName];
                    }


                }

            }


            //若没有提供APP相关KEY，则用默认的
            if (Request["appkey"] != null && Request["appkey"].ToString().Trim() != "")
            {
                APP_KEY = Request["appkey"].ToString().Trim();
            }
            if (Request["appsecret"] != null && Request["appsecret"].ToString().Trim() != "")
            {
                APP_SECRET = Request["appsecret"].ToString().Trim();
            }
            if (Request["apiurl"] != null && Request["apiurl"].ToString().Trim() != "")
            {
                API_URL = Request["apiurl"].ToString().Trim();
            }

            ppgSDKUtility client = new ppgSDKUtility(APP_KEY, APP_SECRET, API_URL, strMethod, postData, strPostMethod, isFile, fileParams);

            strResult = client.PostAndReturn(out strLastUrl);


            strSDK += "//引用ppg.SDK<br/>using ppg.SDK;<br/><br/>...<br/><br/>";
            strSDK += "//初始化数据<br/>string strMethod=\"" + strMethod + "\";<br/>";
            strSDK += "string postData=\"" + postData + "\";<br/>";
            strSDK += "string strPostMethod=\"" + strPostMethod + "\";<br/>";
            strSDK += "bool isFile = \"" + isFile.ToString() + "\";<br/>";
            strSDK += "IDictionary<string, FileItem> fileParams = null;<br/>";
            strSDK += "ppgSDKUtility client = new ppgSDKUtility(APP_KEY, APP_SECRET, API_URL, strMethod, postData, strPostMethod, isFile, fileParams);<br/>";
            strSDK += "//返回结果<br/>string strResult = client.PostAndReturn();";
            
        }


        /// <summary>
        /// 获取当前API的基础资料
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dtParam"></param>
        /// <returns></returns>
        protected DataTable getMethod(string type, out DataTable dtParam)
        {
            string fileName = Server.MapPath("~/template/" + type + ".xml");
            string xmlContent = ppgSDKUtility.ReadFileContent(fileName);
            XmlDocument xmlDoc;
            DataSet ds;
            DataTable dt = null;
            dtParam = null;
            if (xmlContent != "")
            {
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                ds = new DataSet();
                ds.ReadXml(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlContent)));

                dt = ds.Tables["data"];
                dtParam = ds.Tables["param"];
            }

            return dt;
        }
    }
}