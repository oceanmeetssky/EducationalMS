using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Collections;

/// <summary>
/// DBManipulation 的摘要说明
/// </summary>
public class DBManipulation {
    //在这里实现连接数据库，打开数据库，查询数据库，增加，删除，修改，关闭数据库
    private string connectionstring = null;
    private SqlConnection connection = null;
    private SqlDataReader dataReader = null;
    public DBManipulation() {
        //
        // TODO: 在此处添加构造函数逻辑
        //无参数的构造函数默认连接配置文件里面的数据库
        connectionstring = ConfigurationManager.ConnectionStrings["DBconnectionString"].ConnectionString;
        connection = new SqlConnection(connectionstring);
    }
    public DBManipulation(string _connectionstring) {
        //打开指定的数据库
        connectionstring = _connectionstring;
        connection = new SqlConnection(connectionstring);
    }
    public void Open() {
        //开启数据库连接
        try {
            if (connection.State == System.Data.ConnectionState.Closed) {
                //先检查连接状态，如果是关闭状态才打开连接
                connection.Open();
            }            
        }//可能会抛出异常
        catch (SqlException e) {
            Console.Write(e.StackTrace);
        }
        catch (Exception e) {
            Console.Write(e.StackTrace);
        }    
    }
    public void Close() {
        if (connection.State == System.Data.ConnectionState.Open) {
            //如果当前状态是连接状态，才把他关闭，防止因重复关闭抛出异常
            connection.Close();
        }
    }
    //查询数据库,使用DataReader的在线方式
    //使用在线方式查询数据时，数据库连接不能断，一旦断了DataReader就都不出数据了
    public SqlDataReader ExecuteQueryOnLine(string _sql,ArrayList _parameterList) 
    {   
        if (connection.State != System.Data.ConnectionState.Open ) {
            connection.Open();//先保证数据库连接已经打开
        }
        SqlCommand command = connection.CreateCommand();//获取对数据库的操作命令对象
        command.CommandText = _sql;//设置命令要执行的sql语句（含参数）
        SetParameter(command, _parameterList);//设置参数映射
        // SqlDataReader dataReader;
       // dataReader.Close();
        try{
            dataReader = command.ExecuteReader();//将查询结果以只读的方式放到DataReader
        }
        catch (Exception){
            throw;
        }

        return dataReader;
    }
    //查询数据库，使用DataSet的离线方式
    //使用离线方式查询数据时，数据是被下载到本地内存中的，所以查询后，数据库连接可以断开
    public DataSet ExecuteQueryOffLine(string _sql, ArrayList _parameterList) {
        //da.SelectCommand.Parameters.Add(para); 
        //SqlDataAdapter的四个命令对象都可以设置参数
        SqlDataAdapter dataAdapter = new SqlDataAdapter(_sql, connection);
        SetParameter(dataAdapter.SelectCommand, _parameterList);//设置SqlDataAdapter的SelectCommand的参数映射
        //SelectCommand仅仅是一个属性，其本质还是一个SqlCommand
        //只需指定dataAdapter操作的数据库以及查询的sql语句即可，SqlDataAdapter会自己打开数据库连接
        DataSet dataSet = new DataSet();
        dataAdapter.Fill(dataSet, "defaultTable");
        //使用dataAdapter的Fill()往DataSet离线数据集中添加记录行集合，这些记录都放入一张叫做defaultTable的DataTable中
        this.Close();//断开数据库连接       
        return dataSet;//返回包含查询结果的数据集
    }
    //增删改数据
    public void ExecuteNonQuery(string _sql, ArrayList _parameterList) {
        if (connection.State != System.Data.ConnectionState.Open) {
            connection.Open();//先保证数据库连接已经打开
        }
        SqlCommand command = connection.CreateCommand();
        command.CommandText = _sql;
        SetParameter(command, _parameterList);//设置参数映射

        try{
            int rows = command.ExecuteNonQuery();
        }
        catch (Exception){
            throw;
        }//这个rows返回这个操作影响了多少行，暂时先不用
        this.Close();
    }
    //执行含有聚集函数的sql
    public Object ExecuteScalar(string _sql, ArrayList _parameterList) {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();//先保证数据库连接已经打开
        }
        SqlCommand command = connection.CreateCommand();
        command.CommandText = _sql;
        SetParameter(command, _parameterList);//设置参数映射
        Object obj = command.ExecuteScalar();
        this.Close();
        return obj;
    }

    public string ParameterChecking(string _sql,ArrayList _parameterList) {
        //这个函数暂时没用
        string [] str = _sql.Split('?');
        /*  string s = abcdeabcdeabcde;
            string[] sArray=s.Split('c') ;
            foreach(string i in sArray)
            Console.Write(i.ToString()+" ");
            输出下面的结果: ab deab deab de
            Split('?')会将该字符串按照串内的？将字符串分割成若干个子串，然后返回一个子串数组
         */
        string sql = "";
        for (int i = 0,j = 0;i < str.Length;i++) {
            if (j < _parameterList.Count) {
                sql += str[i] + _parameterList[j++];
            }
            else {
                sql += str[i];
            }
        }
        return sql;//返回已经用参数替换了？的sql语句
    }
    //但是，注意这里由于sql是通用的，所以外界必然采用sql拼接的方式，无法使用参数化查询
    //因为没办法预先判断到底有多少参数，嗯，这种说法好像不成立。。。
    //也许可以这样做，sql语句用？作为占位符，就像JSP一样，并且同时传入一个参数List
    //然后接收到了这个字符串后，进行检查，将所有的？都替换成参数
    //然后再循环遍历list进行参数的映射
    //考虑使用一个数据结构，结构内有参数名，参数值两个属性
    //随着_sql传递进来的是一个该类对象的list
    public void SetParameter(SqlCommand _command,ArrayList _parameterList) {
        //该函数设置参数映射
        if (_parameterList == null) {
            return;//如果用户没有使用参数化查询
        }
        for (int i = 0; i < _parameterList.Count; i++) {
            ParameterStruct p = (ParameterStruct)_parameterList[i];
            // _command.Parameters[p.ParameterName].Value = p.ValueName;
            _command.Parameters.AddWithValue(p.ParameterName, p.ValueName);
        }
    }
    
}