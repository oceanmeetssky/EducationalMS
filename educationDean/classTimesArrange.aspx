<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="classTimesArrange.aspx.cs" Inherits="educationDean_classTimesArrange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>教导主任设置课时安排</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <nav class="nav">
        <span></span><a class="">导航</a>
        <ul>
            <li><a href="timeArrange.aspx">
                <img src="../img/icon1_time.png" />
                设置作息时间
            </a></li>
            <li><a href="classTimesArrange.aspx" style="background-color: lightgray">
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
            <li><a href="viewSchedule.aspx">
                <img src="../img/icon5_schedule.png" />
                查看课表
            </a></li>
        </ul>
    </nav>
    <div class="middle">
        <h1>设置课时安排</h1>
        <!--  
        <asp:Label ID="debug_courtime" runat="server" Text="Label"></asp:Label>
        -->

        &nbsp;&nbsp;&nbsp;年&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;级：<asp:DropDownList ID="DropDownList_Grade" runat="server" OnTextChanged="DropDownList_Grade_TextChanged" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Grade_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <!--  <asp:Label ID="debug_time" runat="server" Text="Time"></asp:Label>
                <br />-->
                <div class="right3">
                    <img src="../img/last.png" /><br />
                    剩余课时数：<asp:Label ID="lessLessonTime" runat="server" Text="40"></asp:Label>
                </div>
                <div>
                    <asp:Button ID="Button1" class="button small white bigrounded" runat="server" Text="增加科目" OnClick="Button1_Click" />
                    <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </div>

                <asp:Table ID="Table1" runat="server" CellPadding="5"
                    GridLines="horizontal" HorizontalAlign="Center" CssClass="aspTable">
                </asp:Table>

                <br />

                <asp:Button ID="Button_submit" class="button white bigrounded" runat="server" Text="提交" OnClick="Button_submit_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

