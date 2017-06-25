<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="feedback.aspx.cs" Inherits="courseMaster_feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>学科组长查看教导主任反馈</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <nav class="nav">
        <span></span><a class="">导航</a>
        <ul>
            <li><a href="setTeacher.aspx">
                <img src="../img/icon6_teacher.png" />
                安排老师</a></li>
            <li><a href="feedBack.aspx" style="background-color: lightgray">
                <img src="../img/icon4_teacher.png" />
                查看反馈</a></li>
            <li><a href="setTeacher.aspx">
                <img src="../img/icon5_schedule.png" />
                查看课表</a></li>
        </ul>
    </nav>
    <div class="middle">
        <h1>查看反馈</h1>
        <h3>学科组：<asp:Label ID="Label_SubjectGroup" runat="server" Text="英语组"></asp:Label></h3>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    年级：
                    <asp:DropDownList Width="55px" ID="DropDownList_Grade" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Grade_SelectedIndexChanged">
                        <asp:ListItem Value="1">初一</asp:ListItem>
                        <asp:ListItem Value="2">初二</asp:ListItem>
                        <asp:ListItem Value="3">初三</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                </div>
                <br />
                <asp:Table ID="Table1" runat="server" CellPadding="5"
                    GridLines="horizontal" HorizontalAlign="Center" CssClass="aspTable">
                </asp:Table>
                <br />
                 <asp:Button ID="Button_submit" class="button white bigrounded" runat="server" Text="确定" OnClick="Button_submit_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

