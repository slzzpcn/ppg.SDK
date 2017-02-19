using System;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ppg.SDK
{
    public class ppgSDKUtility
    {

        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string ApiUrl { get; set; }
        public string Method { get; set; }
        public string PostData { get; set; }
        public string PostMethod { get; set; }
        public bool IsFile { get; set; }
        public IDictionary<string, FileItem> FileParams { get; set; }

        public string CHARSET = "UTF-8";

        public ppgSDKUtility()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="apiUrl"></param>
        /// <param name="method"></param>
        /// <param name="postData"></param>
        /// <param name="isFile">是否上传文件</param>
        public ppgSDKUtility(string appKey, string appSecret, string apiUrl, string method, string postData, string postMethod,bool isFile, IDictionary<string, FileItem> fileParams)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            ApiUrl = apiUrl;
            Method = method;
            PostData = postData;
            PostMethod = postMethod;
            IsFile = isFile;
            FileParams = fileParams;
        }

        /// <summary>
        /// 发送请求并返回结果
        /// </summary>
        /// <param name="lastUrl">最终请求Url</param>
        /// <returns></returns>
        public string PostAndReturn(out string lastUrl)
        {
            string urlParm = "";
            string lastUrlParm = "";
            string strSign = "";
            string strResult = "";
            string[] arrParm, arrSortString, arrTemp;
            int i = 0;
            StringBuilder prestr = new StringBuilder();
            long timestamp = GetTimestamp(DateTime.Now);

            urlParm = PostData;

            //追加系统参数
            if (urlParm != "")
            {
                urlParm += "&";
            }
            urlParm += "appKey=" + AppKey;
            urlParm += "&timestamp=" + timestamp.ToString();


            arrParm = urlParm.Split('&');
            //进行排序            
            arrSortString = BubbleSort(arrParm);


            //value为空的，剔除掉
            for (i = 0; i < arrSortString.Length; i++)
            {
                strSign = arrSortString[i].ToString();
                arrTemp = strSign.Split('=');
                if(arrTemp.Length>=2 && arrTemp[1].ToString().Length>=1)
                {
                    if(lastUrlParm != "")
                    {
                        lastUrlParm += "&";
                    }
                    lastUrlParm += strSign;
                    prestr.Append(strSign);
                }

                
            }

            urlParm = lastUrlParm;

            //替换特殊字符
            urlParm = urlParm.Replace("+", "%2B");
            urlParm = urlParm.Replace("#", "%23");
            urlParm = urlParm.Replace("/", "%2F");

            strSign = prestr.ToString().Replace("=", "");

            strSign = GenerateSign(Method + strSign, AppSecret);

            urlParm = "sign=" + strSign + "&" + urlParm;

            lastUrl = ApiUrl + Method + "?" + urlParm;

            strResult = PostDataAndReturnString(ApiUrl + Method, urlParm, PostMethod, IsFile, FileParams);

            return strResult;
        }

        /// <summary>
        /// 发送请求并返回结果
        /// </summary>
        /// <returns>string</returns>
        public string PostAndReturn()
        {
            string lastUrl;
            return PostAndReturn(out lastUrl);
        }
        /// <summary>
        /// 发送数据,并返回结果
        /// </summary>
        /// <param name="reqUrl">请求url</param>
        /// <param name="postData">name=bb&user=hill</param>
        /// <param name="postMothod">POST OR GET</param>
        private string PostDataAndReturnString(string reqUrl, string postData, string postMothod)
        {
            return PostDataAndReturnString(reqUrl, postData, postMothod, false, null);
        }
        /// <summary>
        /// 发送数据,并返回结果
        /// </summary>
        /// <param name="reqUrl">请求url</param>
        /// <param name="postData">name=bb&user=hill</param>
        /// <param name="postMothod">POST OR GET</param>
        /// <param name="isFile">是否上传文件</param>
        /// <param name="fileParams">IDictionary</param>
        private string PostDataAndReturnString(string reqUrl, string postData, string postMothod, bool isFile, IDictionary<string, FileItem> fileParams)
        {
            string strResult = "";
            WebUtils utils = new WebUtils();

            if(isFile)
            {
                strResult = utils.DoPost(reqUrl, postData, fileParams);
            }
            else
            { 
                if (postMothod == "POST")
                {
                    strResult = utils.DoPost(reqUrl, postData);
                }
                else
                {
                    strResult = utils.DoGet(reqUrl, postData);
                }
            }
            return strResult;

        }


        /// <summary>
        /// 冒泡排序法
        /// </summary>
        private string[] BubbleSort(string[] r)
        {
            int i, j; //交换标志 
            string temp;

            bool exchange;

            for (i = 0; i < r.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = r.Length - 2; j >= i; j--)
                {
                    if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)　//交换条件
                    {
                        temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }

            }
            return r;
        }



        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="strParam"></param>
        /// <param name="strAppSecret"></param>
        /// <returns></returns>
        public string GenerateSign(string strParam, string strAppSecret)
        {
            return SHA1_Hash(strParam + strAppSecret).ToLower();
        }

        /// <summary>
        /// 计算SHA1哈希值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string SHA1_Hash(string source)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(source);
            SHA1 sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public long GetTimestamp(DateTime param)
        {
            if (param == null)
            {
                param = DateTime.Now;
            }
            return (long)Math.Floor((param.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds);
        }


        /// <summary>
        /// 通过键名获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            //return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath).AppSettings.Settings[key].Value;

            return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath).AppSettings.Settings[key].Value;
        }

        #region 读取文件内容
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="Path">Server.MapPath后的 </param>
        /// <returns></returns>
        public static string ReadFileContent(string fileName)
        {
            string result = "";
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                result = System.IO.File.ReadAllText(fileName, System.Text.Encoding.UTF8); 
            }
            return result;
        }
        #endregion

        #region C# 获取文件SHA1

        /// <summary>
        /// 计算文件的 sha1 值
        /// </summary>
        /// <param name="fileName">要计算 sha1 值的文件名和路径</param>
        /// <returns>sha1 值16进制字符串</returns>
        public static string SHA1File(System.IO.Stream stream)
        {
            byte[] hashBytes = HashData(stream, "sha1");
            stream.Seek(0, SeekOrigin.Begin);
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="stream">要计算哈希值的 Stream</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值字节数组</returns>
        private static byte[] HashData(System.IO.Stream stream, string algName)
        {
            System.Security.Cryptography.HashAlgorithm algorithm;
            if (algName == null)
            {
                throw new ArgumentNullException("algName 不能为 null");
            }
            if (string.Compare(algName, "sha1", true) == 0)
            {
                algorithm = System.Security.Cryptography.SHA1.Create();
            }
            else
            {
                if (string.Compare(algName, "md5", true) != 0)
                {
                    throw new Exception("algName 只能使用 sha1 或 md5");
                }
                algorithm = System.Security.Cryptography.MD5.Create();
            }
            return algorithm.ComputeHash(stream);
        }

        /// <summary>
        /// 字节数组转换为16进制表示的字符串
        /// </summary>
        private static string ByteArrayToHexString(byte[] buf)
        {
            return BitConverter.ToString(buf).Replace("-", "");
        }

        #endregion

    }
}