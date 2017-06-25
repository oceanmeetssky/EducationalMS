using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Text = "模拟成功！";
        func();
    }
    protected void func() {
        String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                    "Data Source=d:/test111.xls;" +
                                    "Extended Properties=Excel 8.0;";
        OleDbConnection cn = new OleDbConnection(sConnectionString);

        string sqlCreate = "CREATE TABLE TestSheet ([ID] INTEGER,[Username] VarChar,[UserPwd] VarChar)";

        OleDbCommand cmd = new OleDbCommand(sqlCreate, cn);

        //创建Excel文件：C:/test.xls

        cn.Open();

        //创建TestSheet工作表

        cmd.ExecuteNonQuery();

        //添加数据

        cmd.CommandText = "INSERT INTO TestSheet VALUES(1,'elmer','password')";

        cmd.ExecuteNonQuery();

        //关闭连接

        cn.Close();
    }
}