<%@ Page Language="C#" AutoEventWireup="true" CodeFile="测试文件夹选项.aspx.cs" Inherits="测试文件夹选项" %>

<!DOCTYPE html>

<html>

<head>

    <meta http-equiv="Content-Type" content="text/html; charset=gb2312">
    <title>无标题文档</title>
</head>
<body>
    <table border="0" cellpadding="0" width="100%" id="tb_show">
        <tr>
            <td width="18%">文件保存位置:</td>
            <td width="82%">
                <%--<html:file property="file" size="40" styleClass="inputbox"/>--%>
                <input name="backDir" type="text" value="C:\" size="100" width="500">
            </td>
        </tr>

        <tr>
            <td>目录位置:</td>
            <td>
                <select name="tables_drive" id="tables_drives" onchange="get_drives()"></select>
            </td>
        </tr>

        <tr>
            <td colspan="2">
                <select name="table_folder" id="table_folder" size="10" multiple ondblclick="get_file()"></select>
            </td>
        </tr>

        <tr>
            <td colspan="2">
                <font color="red">说明：双击列表框的一个选项，就将该文件夹下面的文件夹显示在该列表框中。第一个就是根目录</font>
            </td>
        </tr>
    </table>
</body>
</html>
<script>

    window.onload = new function init() {
        var fso, s, n, e, x;
        fso = new ActiveXObject("Scripting.FileSystemObject");
        e = new Enumerator(fso.Drives);
        s = "";
        for (; !e.atEnd() ; e.moveNext()) {
            x = e.item();
            s = s + x.DriveLetter;
            s += ":";
            if (x.DriveType == 3)
                n = x.ShareName;
            else if (x.IsReady)
                n = x.VolumeName;
            else
                n = "[驱动器未就绪]";
            s += n + ",";
        }
        var drives = s.split(",");
        var tableDrives = document.getElementByIdx_x("tables_drives");
        for (var i = 0; i < drives.length - 1; i++) {
            var option = document.createElement_x("OPTION");
            drives[i].split(":");
            option.value = "[" + drives[i].split(":")[0] + ":]" + drives[i].split(":")[1];
            option.text = "[" + drives[i].split(":")[0] + ":]" + drives[i].split(":")[1];
            tableDrives.add(option);
        }
    }


    function get_drives() {
        var tableDrives = document.getElementByIdx_x("tables_drives");
        var tableFolders = document.getElementByIdx_x("table_folder");
        for (var i = 0; i < tableDrives.options.length; i++) {
            if (tableDrives.options[i].selected == true) {
                var fso, f, fc, s;
                var drive = tableDrives.options[i].value.split(":")[0].substring(1, tableDrives.options[i].value.split(":")[0].length);
                document.getElementByIdx_x("backDir").value = drive + ":\\";
                fso = new ActiveXObject("Scripting.FileSystemObject");
                if (fso.DriveExists(drive)) {
                    d = fso.GetDrive(drive);
                    if (d.IsReady) {
                        f = fso.GetFolder(d.RootFolder);
                        fc = new Enumerator(f.SubFolders);
                        s = "";
                        for (; !fc.atEnd() ; fc.moveNext()) {
                            s += fc.item();
                            s += ",";
                        }

                        var len = tableFolders.options.length;
                        while (len >= 0) {
                            tableFolders.options.remove(len);
                            len--;
                        }
                        var option = document.createElement_x("OPTION");
                        option.value = drive + ":\\";
                        option.text = drive + ":\\";
                        tableFolders.add(option);
                        var folders = s.split(",");
                        for (j = 0; j < folders.length - 1; j++) {
                            option = document.createElement_x("OPTION");
                            option.value = folders[j];
                            option.text = folders[j];
                            tableFolders.add(option);
                        }
                    }
                    else {
                        alert("无法改变当前内容！")
                    }
                }
                else
                    return false;
            }
        }
    }


    function get_file() {
        var tableFolders = document.getElementByIdx_x("table_folder");
        var tableDrives = document.getElementByIdx_x("tables_drives");
        for (var i = 0; i < tableFolders.options.length; i++) {
            if (tableFolders.options[i].selected == true) {
                var fso, f, fc, s;
                var folderpath = tableFolders.options[i].value.substring(0, tableFolders.options[i].value.length);
                if (folderpath.charAt(folderpath.length - 1) == "\\") {
                    document.getElementByIdx_x("backDir").value = folderpath;
                }
                else {
                    document.getElementByIdx_x("backDir").value = folderpath + "\\";
                }


                fso = new ActiveXObject("Scripting.FileSystemObject");
                f = fso.GetFolder(folderpath);
                fc = new Enumerator(f.SubFolders);
                s = "";
                for (; !fc.atEnd() ; fc.moveNext()) {
                    s += fc.item();
                    s += ",";
                }
                var len = tableFolders.options.length;
                while (len >= 0) {
                    tableFolders.options.remove(len);
                    len--;
                }
                var opt = "";
                var opt1 = "";
                for (j = 0; j < folderpath.split("\\").length; j++) {
                    var option = document.createElement_x("OPTION");
                    opt = opt + folderpath.split("\\")[j] + "\\";
                    if (j > 0) {
                        opt1 = opt;
                        option.value = opt1.substring(0, opt1.length - 1);
                        option.text = opt1.substring(0, opt1.length - 1);
                        tableFolders.add(option);
                    }
                    else {
                        option.value = opt;
                        option.text = opt;
                        tableFolders.add(option);
                    }

                }
                if (tableFolders.options[0].value == tableFolders.options[1].value) {
                    tableFolders.options.remove(1);
                }
                if (s != "") {
                    var folders = s.split(",");
                    for (j = 0; j < folders.length - 1; j++) {
                        option = document.createElement_x("OPTION");
                        option.value = folders[j];
                        option.text = folders[j];
                        tableFolders.add(option);
                    }
                }
            }
        }
    }
</script>
