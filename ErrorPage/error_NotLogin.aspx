<%@ Page Language="C#" AutoEventWireup="true" CodeFile="error_NotLogin.aspx.cs" Inherits="ErrorPage_error_1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <script type="text/javascript">           
            

        function AspButtonClick() {//模拟asp的button被点击，来跳到后台逻辑
            document.getElementById("Jump").click();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server" >
    <div>
        <script type="text/javascript">
            alert('抱歉，请您先登入');
            window.location.href = "/login/login.aspx"
        </script>  
    </div>
    </form>
</body>
</html>
