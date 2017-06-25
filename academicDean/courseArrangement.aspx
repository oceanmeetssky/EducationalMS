<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="courseArrangement.aspx.cs" Inherits="academicDean_courseArrangement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>教务员排课</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <nav class="nav">
        <span></span><a class="">导航</a>
        <ul>
            <li><a href="courseArrangement.aspx" style="background-color: lightgray">
                <img src="../img/icon7_schedule.png" />排课
            </a></li>
            <li><a href="viewSchedule.aspx">
                <img src="../img/icon5_schedule.png" />查看课表
            </a></li>
        </ul>
    </nav>
    <!--
    <div class="right2">
        <asp:Table ID="Table_classTimes" runat="server" CellPadding="5"
            GridLines="horizontal" HorizontalAlign="Center" CssClass="aspTable">
            <asp:TableRow CssClass="aspTableRow3">
                <asp:TableCell CssClass="aspTableCell">科目</asp:TableCell>
                <asp:TableCell CssClass="aspTableCell">课时数</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow CssClass="aspTableRow4">
                <asp:TableCell CssClass="aspTableCell">语文</asp:TableCell>
                <asp:TableCell>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    -->
    <div class="middle">
        <h1>排课</h1>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <!-- <asp:Label ID="debug" runat="server" Text="Label"></asp:Label>-->
                <div>
                    年级：
                    <asp:DropDownList Width="55px" ID="DropDownList_Grade" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Grade_SelectedIndexChanged">
                        <asp:ListItem Value="1">初一</asp:ListItem>
                        <asp:ListItem Value="2">初二</asp:ListItem>
                        <asp:ListItem Value="3">初三</asp:ListItem>
                    </asp:DropDownList>
                    班级：
                    <asp:DropDownList Width="55px" ID="DropDownList_class" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_class_SelectedIndexChanged">
                    </asp:DropDownList>
                    <br />

                    <div>
                        <label class="labelGreen">aaaaaa</label>
                        :当前选中课程&nbsp;&nbsp; 
                        <label class="labelPurple">aaaaaa</label>
                        :可对调课程
                    </div>
                    <div>
                        <p style="font-weight: bold">注：仅当更换当前年级全部班级排课方案时按下“排课”按钮</p>
                    </div>
                    <asp:Button ID="Button_CreateSchedule" runat="server" Text="排课" OnClick="Button_CreateSchedule_Click" CssClass="button white bigrounded " />&nbsp;&nbsp;&nbsp; 
                     <asp:Button ID="Button_SubmitSchedule" runat="server" Text="确定" OnClick="Button_SubmitSchedule_Click" CssClass="button white bigrounded " />

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

            </ContentTemplate>
        </asp:UpdatePanel>


    </div>
    </div>
</asp:Content>

