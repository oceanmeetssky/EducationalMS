<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="setSubjectGroup.aspx.cs" Inherits="educationDean_setSubjectGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>教导主任设置学科组</title>

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
            <li><a href="setSubjectGroup.aspx" style="background-color: lightgray">
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
        <h1>设置学科组</h1>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="right">
                    <div></div>
                    <asp:Button ID="Button_AddSubjectGroup" class="button small white bigrounded" runat="server" Text="增加学科组" OnClick="Button_AddSubjectGroup_Click" />
                    <asp:TextBox ID="TextBox_SubjectGroup" runat="server" CssClass="textbox"></asp:TextBox>
                    <asp:Button ID="Button2" class="button small white bigrounded" runat="server" Text="删除学科组" OnClick="Button2_Click" />
                    <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <asp:Table ID="Table_SubjectGroup" runat="server" CellPadding="5"
                    GridLines="horizontal" HorizontalAlign="Center" CssClass="aspTable">
                </asp:Table>

                <!--<asp:Button ID="Button_add" class="button white bigrounded" runat="server" Text="增加学科组" />-->
                <asp:Button ID="Button_submit" class="button white bigrounded" runat="server" Text="确定" OnClick="ButtonSubmit_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <!--
    <asp:Label id ="debug1" runat="server" Text="Label1"></asp:Label>
    <asp:Label ID="debug2" runat="server" Text="Label2"></asp:Label>
    -->
    <br />
</asp:Content>


