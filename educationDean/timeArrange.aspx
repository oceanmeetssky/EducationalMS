<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="timeArrange.aspx.cs" Inherits="educationDean_timeArrange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>教导主任设置作息时间</title>
    <link rel="stylesheet" type="text/css" href="assets/css/bootstrap.min.css" />
    <%-- 对下拉框的样式设计 --%>
    <link rel="stylesheet" type="text/css" href="dist/bootstrap-clockpicker.min.css" />
    <%-- 对时钟设计 --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <nav class="nav" id="nav">
        <span>导航</span>
        <ul>
            <li><a href="timeArrange.aspx" style="background-color: lightgray">
                <img src="../img/icon1_time.png" />
                设置作息时间</a></li>
            <li><a href="classTimesArrange.aspx">
                <img src="../img/icon2_classTimes.png" />
                设置课时安排</a></li>
            <li><a href="setSubjectGroup.aspx">
                <img src="../img/icon3_subjectGroup.png" />
                设置学科组</a></li>
            <li><a href="checkTeacher.aspx">
                <img src="../img/icon4_teacher.png" />
                审核老师安排</a></li>
            <li><a href="viewSchedule.aspx">
                <img src="../img/icon5_schedule.png" />
                查看课表</a></li>
        </ul>
    </nav>

    <div class="middle">
        <h1>设置作息时间</h1>
        <!-- <asp:Label ID="debug1" runat="server" Text="Label"></asp:Label>-->

        <div>
            <br />
            年级：<asp:DropDownList ID="DropDownList_Grade" runat="server" Width="55px" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Grade_SelectedIndexChanged">
                <asp:ListItem Value="1">初一</asp:ListItem>
                <asp:ListItem Value="2">初二</asp:ListItem>
                <asp:ListItem Value="3">初三</asp:ListItem>
            </asp:DropDownList><span>&nbsp&nbsp;</span>
            <asp:DropDownList ID="DropDownList_Week" runat="server" Width="73px" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_Week_SelectedIndexChanged">
                <asp:ListItem Value="1">星期一</asp:ListItem>
                <asp:ListItem Value="2">星期二</asp:ListItem>
                <asp:ListItem Value="3">星期三</asp:ListItem>
                <asp:ListItem Value="4">星期四</asp:ListItem>
                <asp:ListItem Value="5">星期五</asp:ListItem>
            </asp:DropDownList>
        </div>

        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="TimeArrange" class="table2">
            <thead>
                <tr>
                    <th style="width: 20px">节次</th>
                    <th>开始时间</th>
                    <th>结束时间</th>
                </tr>
            </thead>
            <tbody>
                <tr id="tr1">
                    <td>第1节</td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="left">
                            <!-- <input name="text11" id="text11" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_11" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="right">
                            <!--  <input name="text12" id="text12" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_12" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr id="tr2">
                    <td>第2节</td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="left">
                            <!-- <input name="text21" id="text21" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_21" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="right">
                            <!--<input name="text22" id="text22" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_22" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr id="tr3">
                    <td>第3节</td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="left">
                            <!-- <input name="text31" id="text31" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_31" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="right">
                            <!--<input name="text32" id="text32" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_32" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr id="tr4">
                    <td>第4节</td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="left">
                            <!--<input name="text41" id="text41" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_41" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="right">
                            <!-- <input name="text42" id="text42" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_42" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr id="tr5">
                    <td>第5节</td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="left">
                            <!-- <input name="text51" id="text51" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_51" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="right">
                            <!--<input name="text52" id="text52" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_52" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr id="tr6">
                    <td>第6节</td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="left">
                            <!--<input name="text61" id="text61" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_61" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="right">
                            <!--<input name="text62" id="text62" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_62" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr id="tr7">
                    <td>第7节</td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="left">
                            <!--<input name="text71" id="text71" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_71" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="right">
                            <!--<input name="text72" id="text72" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_72" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr id="tr8">
                    <td>第8节</td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="left">
                            <!-- <input name="text81" id="text81" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_81" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                    <td>
                        <div class=" input-group clockpicker" data-placement="right">
                            <!--<input name="text82" id="text82" type="text" class="form-control" value="08:00" style="text-align: center" />-->
                            <asp:TextBox ID="TextBox_82" runat="server" class="form-control" Text="08:00" Style="text-align: center" AutoPostBack="True"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
        <!--    <input id="Submit" type="submit" value="确定" class="button bigrounded white " />-->
        <br />
        <asp:Button ID="Button_submit" class="button white bigrounded" runat="server" Text="提交" CommandName="SubmitCommand" OnCommand="Button_submit_Click" />
        <br />

        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
    </div>
    <script type="text/javascript" src="assets/js/jquery.min.js"></script>
    <script type="text/javascript" src="assets/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="dist/bootstrap-clockpicker.min.js"></script>
    <script type="text/javascript" src="assets/js/clockpicker.js"></script>


</asp:Content>

