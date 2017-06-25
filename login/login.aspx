<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>登录</title>
    <link rel="stylesheet" type="text/css" href="../css/login.css" />
    <link rel="icon" href="../img/middle-schoo-l.png" />
    <script type="text/javascript">
        function validate() {//先使用正则表达式初步检查输入格式
            if (!(/^\w{1,15}$/.test(document.getElementById("username").value))) {
                alert("用户名不能为空！");
                document.getElementById("username").focus();
                return;
            }
            if (!(/^\w{1,15}$/.test(document.getElementById("password").value))) {
                alert("密码不能为空！");
                document.getElementById("password").focus();
                return;
            }
            else {
                Welcome();//欢迎画面
                setTimeout("AspButtonClick()", 800); //让欢迎画面持续2秒中后调用AspButtonClick()          
                // alert("success！");
            }
            return;
        }

        function Welcome() {//欢迎画面
            event.preventDefault();
            $('form').fadeOut(500);
            $('.wrapper').addClass('form-success');/*对提交成功后的样式做出修改*/
        }

        function AspButtonClick() {//模拟asp的button被点击，来跳到后台逻辑
            document.getElementById("login").click();
        }

    </script>
</head>
<body>
    <div class="wrapper">
        <div id="logo" class="container">
            <div>
                <img style="height: 79px;" src="../img/school.png" />
                <h1>中加国际中学欢迎您</h1>
            </div>
            <!--检错区域
            <asp:Label ID="debugUsername" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="debugToken" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="debugSql" runat="server" Text="Label"></asp:Label>
            -->

            <form class="form" runat="server">
                <asp:TextBox ID="username" runat="server" type="text" placeholder="username"></asp:TextBox>
                <asp:TextBox ID="password" runat="server" type="password" placeholder="password"></asp:TextBox>
                <input id="HtmlButton" type="button" value="login" onclick="validate()" />
                <asp:Button ID="login" runat="server" Text="login" OnClick="login_Click" Style="display: none" />
            </form>
        </div>

        <ul class="bg-bubbles">
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
            <li></li>
        </ul>
    </div>
    <script>window.jQuery || document.write('<script src="js/jquery-2.1.1.min.js"><\/script>')</script>
</body>
</html>
