<%@ Page Language="C#" AutoEventWireup="true" CodeFile="error_DeniedPermission.aspx.cs" Inherits="ErrorPage_error_Permission" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <script type="text/javascript">
            alert('抱歉，请您没有相应的权限');
            window.location.href = "/login/login.aspx"
        </script> 
    </div>
    </form>
</body>
</html>
