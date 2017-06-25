<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewSchedule_Class.aspx.cs" Inherits="educationDean_viewSchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<title>教师查看课表</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <nav class="nav">
        <span></span><a class="">导航</a>
        <ul>
            <li><a href="viewSchedule_Teacher.aspx"  >
                <img src="../img/icon7_schedule.png" />查看我的课表
            </a></li>
            <li><a href="viewSchedule_Class.aspx" style="background-color: lightgray">
                <img src="../img/icon5_schedule.png" />查看班级课表
            </a></li>       
        </ul>
    </nav>
<div class="middle">       
       <h1>查看课表</h1>
        <div>
            年级：
                    <asp:DropDownList Width="55px" ID="DropDownList_Grade" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Grade_SelectedIndexChanged">
                        <asp:ListItem Value="1">初一</asp:ListItem>
                        <asp:ListItem Value="2">初二</asp:ListItem>
                        <asp:ListItem Value="3">初三</asp:ListItem>
                    </asp:DropDownList>
            班级：
                    <asp:DropDownList Width="55px" ID="DropDownList_class" runat="server" OnSelectedIndexChanged="DropDownList_class_SelectedIndexChanged">                       
                    </asp:DropDownList>
            <br />
        </div>
        <asp:Table ID="Table1" runat="server" CellPadding="5"
            GridLines="horizontal" HorizontalAlign="Center" CssClass="aspTable">
            <asp:TableRow CssClass="aspTableRow1">
                <asp:TableCell CssClass="aspTableCell"></asp:TableCell>
                <asp:TableCell CssClass="aspTableCell">星期一</asp:TableCell>
                <asp:TableCell CssClass="aspTableCell">星期二</asp:TableCell>
                <asp:TableCell CssClass="aspTableCell">星期三</asp:TableCell>
                <asp:TableCell CssClass="aspTableCell">星期四</asp:TableCell>
                <asp:TableCell CssClass="aspTableCell">星期五</asp:TableCell>
            </asp:TableRow>
         
        </asp:Table>
        <br />
          <!--  <asp:Button ID="Button_submit" runat="server" Text="确定" />-->
    </div>
</asp:Content>


