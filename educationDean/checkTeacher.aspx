<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="checkTeacher.aspx.cs" Inherits="educationDean_checkTeacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>教导主任审核老师安排</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <nav class="nav">
        <span></span><a class="">导航</a>
        <ul>
            <li><a href="timeArrange.aspx">
                <img src="../img/icon1_time.png" />
                设置作息时间
            </a></li>
            <li><a href="classTimesArrange.aspx">
                <img src="../img/icon2_classTimes.png" />设置课时安排
            </a></li>
            <li><a href="setSubjectGroup.aspx">
                <img src="../img/icon3_subjectGroup.png" />
                设置学科组
            </a></li>
            <li><a href="checkTeacher.aspx" style="background-color: lightgray">
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
        <h1>审核老师安排</h1>
        <!--<asp:Label ID="debug1" runat="server" Text="Label"></asp:Label>-->
        <div>
            <label class="labelPurple">aaaaaa</label>
            :有异议的老师安排
        </div>
        <div>
            学期：<!--授课记录主码，前2位代表年份，一位代表年级+学期（初一1，2 初二3，4 初三5，6），2位代表班级，2位代表学科-->
            <asp:DropDownList ID="DropDownList_year" runat="server" OnSelectedIndexChanged="DropDownList_year_SelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem Value="2016">2016-2017</asp:ListItem>
                <asp:ListItem Value="2017">2017-2018</asp:ListItem>
                <asp:ListItem Value="2018">2018-2019</asp:ListItem>
                <asp:ListItem Value="2019">2019-2020</asp:ListItem>
                <asp:ListItem Value="2020">2020-2021</asp:ListItem>
                <asp:ListItem Value="2021">2021-2022</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="DropDownList_semester" runat="server" Width="73px" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_semester_SelectedIndexChanged">
                <asp:ListItem Value="01">上学期</asp:ListItem>
                <asp:ListItem Value="02">下学期</asp:ListItem>
            </asp:DropDownList>
            <br />
            年级：
                    <asp:DropDownList Width="55px" ID="DropDownList_Grade" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Grade_SelectedIndexChanged">
                        <asp:ListItem Value="1">初一</asp:ListItem>
                        <asp:ListItem Value="2">初二</asp:ListItem>
                        <asp:ListItem Value="3">初三</asp:ListItem>
                    </asp:DropDownList>
            <br />
        </div>

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:Table ID="Table1" runat="server" CellPadding="5"
                    GridLines="horizontal" HorizontalAlign="Center" CssClass="aspTable">
                </asp:Table>

                <br />
                </div>
    
            </ContentTemplate>
        </asp:UpdatePanel>

        <br />
        <!--    <asp:Button ID="Button_submit" runat="server" CssClass="bigrounded button white" Text="确定" />-->

    </div>
</asp:Content>


