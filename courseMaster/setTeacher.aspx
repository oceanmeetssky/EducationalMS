<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="setTeacher.aspx.cs" Inherits="courseMaster_setTeacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>学科组长安排老师</title>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <nav class="nav">
        <span></span><a class="">导航</a>
        <ul>
            <li><a href="setTeacher.aspx" style="background-color: lightgray">
                <img src="../img/icon6_teacher.png" />
                安排老师</a></li>
            <li><a href="feedBack.aspx">
                <img src="../img/icon4_teacher.png" />
                查看反馈</a></li>
            <li><a href="viewSchedule.aspx">
                <img src="../img/icon5_schedule.png" />
                查看课表</a></li>
        </ul>
    </nav>
    <div class="middle">
        <h1>安排老师</h1>
        <!--  <asp:Label ID="debug1" runat="server" Text="Label"></asp:Label>-->
        <h3>学科组：<asp:Label ID="Label_SubjectGroup" runat="server" Text="理化组"></asp:Label></h3>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    学期：
            <asp:DropDownList ID="DropDownList_year" runat="server" AutoPostBack="True">
                <asp:ListItem Value="2016">2016-2017</asp:ListItem>
                <asp:ListItem Value="2017">2017-2018</asp:ListItem>
                <asp:ListItem Value="2018">2018-2019</asp:ListItem>
                <asp:ListItem Value="2019">2019-2020</asp:ListItem>
                <asp:ListItem Value="2020">2020-2021</asp:ListItem>
                <asp:ListItem Value="2021">2021-2022</asp:ListItem>
            </asp:DropDownList>
                    <asp:DropDownList ID="DropDownList_semester" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="01">上学期</asp:ListItem>
                        <asp:ListItem Value="02">下学期</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <div>
                        年级：
                
                    <asp:DropDownList ID="DropDownList_Grade" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Grade_SelectedIndexChanged">
                        <asp:ListItem Value="1">初一</asp:ListItem>
                        <asp:ListItem Value="2">初二</asp:ListItem>
                        <asp:ListItem Value="3">初三</asp:ListItem>
                    </asp:DropDownList>
                        <br />
                        学科：
                
                    <asp:DropDownList ID="DropDownList_Course" runat="server" OnSelectedIndexChanged="DropDownList_Subject_SelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                    </div>


                    <asp:Table ID="Table1" runat="server" CellPadding="5"
                        GridLines="horizontal" HorizontalAlign="Center" CssClass="aspTable">
                    </asp:Table>


                    <asp:Button ID="Button_submit" class="button white bigrounded" runat="server" Text="确定" OnClick="Button_submit_Click" />    
                    <p></p>                
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </div>   
</asp:Content>

