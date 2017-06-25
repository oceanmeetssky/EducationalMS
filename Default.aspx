<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type ="text/javascript">
        function a(){
            document.getElementById("Button1").click();
        }
    </script>
    <title></title>
</head>
<body>
    <!--这个页面用来测试通过Js来跳转到后台C#逻辑-->
    <form id="form1" runat="server">
        <div>
            <input type="button" value="htmlButton" onclick="a()"  />
        </div>

        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" style="display:none"/>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        <br />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
