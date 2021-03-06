﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewSchedule.aspx.cs" Inherits="educationDean_viewSchedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
    <title>教务员查看总课表</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <nav class="nav">
        <span></span><a class="">导航</a>
        <ul>
            <li><a href="timeArrange.aspx">
                <img src="../img/icon1_time.png" />
                设置作息时间
            </a></li>
            <li><a href="classTimesArrange.aspx" >
                <img src="../img/icon2_classTimes.png" />设置课时安排
            </a></li>
            <li><a href="setSubjectGroup.aspx">
                <img src="../img/icon3_subjectGroup.png" />
                设置学科组
            </a></li>
            <li><a href="checkTeacher.aspx">
                <img src="../img/icon4_teacher.png" />
                审核老师安排
            </a></li>
            <li><a href="viewSchedule.aspx" style="background-color: lightgray">
                <img src="../img/icon5_schedule.png" />
                查看课表
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
            <asp:Button ID="Button_ExportIntoExcel" runat="server" Text="导出" CssClass="button white bigrounded " OnClick="Button_ExportIntoExcel_Click" />
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


