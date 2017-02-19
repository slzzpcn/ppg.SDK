<%@ Page Title="" Language="C#" MasterPageFile="~/Web.Master" AutoEventWireup="true" CodeBehind="API.aspx.cs" Inherits="ppg.SDK.API" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" cellpadding="10">
        <tr>
            <td valign="top" width="55%">
                <table width="100%" cellpadding="5" cellspacing="1" style="background-color: #bbbbbb;">
                    <tr class="tr_title">
                        <td>参数名称</td>
                        <td>参数值</td>
                        <td>参数描述</td>
                    </tr>
                    <tr class="td_title">
                        <td>请求环境</td>
                        <td colspan="2">
                            <input type="text" name="apiurl" id="apiurl" class="input" value="<%=Request["apiurl"] %>" size="32" /><select onchange="document.getElementById('apiurl').value=this.value;"><option value="">选择</option>
                                <option value="http://515.seller.api.papago.hk">测试环境</option>
                                <option value="http://api.seller.papago.hk">正式环境</option>
                            </select></td>
                    </tr>
                    <tr class="td_title">
                        <td>APP_KEY</td>
                        <td>
                            <input type="text" name="appkey" id="appkey" class="input" value="<%=Request["appkey"] %>" /></td>
                        <td>为空用配置文件里的数据</td>
                    </tr>
                    <tr class="td_title">
                        <td>APP_SECRET</td>
                        <td>
                            <input type="text" name="appsecret" id="appsecret" class="input" value="<%=Request["appsecret"] %>" /></td>
                        <td>为空用配置文件里的数据</td>
                    </tr>
                    <%=strReqHtml %>
                </table>
                <br />
                <br />
                <asp:Button ID="btnSubmit" runat="server" Text="提交" OnClick="btnSubmit_Click" CssClass="btn" />
            </td>

            <td valign="top">提交参数：<%=strPostMethod %><br />
                <div id="post_param"><%=Server.HtmlEncode(strLastUrl) %></div>
                <br />
                <br />
                返回结果：<br />
                <div id="return_result"><%=strResult %></div>
                <br />
                <br />
                SDK调用：<br />
                <div id="return_result"><%=strSDK %></div>
            </td>
        </tr>
    </table>


</asp:Content>
