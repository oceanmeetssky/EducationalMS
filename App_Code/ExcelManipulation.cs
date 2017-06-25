using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Web;

/// <summary>
/// ExcelManipulation 的摘要说明
/// </summary>
public class ExcelManipulation
{
    private string connectionstring = null;
    private OleDbConnection connection = null;
    private ExcelManipulation(string _filePath) {
        this.connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                    "Data Source="+_filePath+";" +
                                    "Extended Properties=Excel 8.0;";
        connection = new OleDbConnection(connectionstring);
    }
    public ExcelManipulation()
    {
        connectionstring = ConfigurationManager.ConnectionStrings["ExcelconnectionString"].ConnectionString;
        connection = new OleDbConnection(connectionstring);
    }
    public void setFileName(string fileName) {
        //Data Source=d:/CourseTable.xls;
        this.connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                    "Data Source=d:/" + fileName + ".xls;" +
                                    "Extended Properties=Excel 8.0;";
        connection.Close();//关闭原有连接
        connection = new OleDbConnection(connectionstring);//重新打开新的连接
    }

    public void Open()
    {
        //开启数据库连接
        try
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                //先检查连接状态，如果是关闭状态才打开连接
                connection.Open();
            }
        }//可能会抛出异常
        catch (Exception e)
        {
            Console.Write(e.StackTrace);
        }
    }
    public void Close()
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            //如果当前状态是连接状态，才把他关闭，防止因重复关闭抛出异常
            connection.Close();
        }
    }
    //增删改数据
    public void ExecuteNonQuery(string _sql, ArrayList _parameterList)
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();//先保证数据库连接已经打开
        }
        OleDbCommand command = connection.CreateCommand();
        command.CommandText = _sql;
        //SetParameter(command, _parameterList);//设置参数映射
        try
        {
            int rows = command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw;
        }//这个rows返回这个操作影响了多少行，暂时先不用
        this.Close();
    }
    /*
    protected void func()
    {
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
   */
}