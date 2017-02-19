<%@ Page Title="" Language="C#" MasterPageFile="~/Web.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ppg.SDK.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ul class="api_list">
        <li><a href="API.aspx?type=system_getTime" target="_blank">获取系统时间API</a></li>
        <li><a href="API.aspx?type=item_getStockExt" target="_blank">获取单个商品库存信息API</a></li>
        <li><a href="API.aspx?type=item_getStocksExt" target="_blank">批量获取商品库存信息API</a></li>
        <li><a href="API.aspx?type=order_addAndPayExt" target="_blank">添加并支付订单API (订单是否支付通过参数控制)</a></li>
        <li><a href="API.aspx?type=order_payForOrder" target="_blank">订单支付API （通过分销平台订单号支付）</a></li>
        <li><a href="API.aspx?type=order_payForOrderExt" target="_blank">订单支付API（通过第三方订单号进行支付）</a></li>
        <li><a href="API.aspx?type=order_getOrdersInfo" target="_blank">获取订单信息API</a></li>
        <li><a href="API.aspx?type=order_getTrackingNumber" target="_blank">获取订单物流跟踪号API</a></li>
        
        <li><a href="API.aspx?type=order_getTracking" target="_blank">获取订单物流轨迹API</a></li>
        <li><a href="API.aspx?type=order_saveIdCard" target="_blank">添加身份证信息API</a></li>
        
    </ul>
</asp:Content>
