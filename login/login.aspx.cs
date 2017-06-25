using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {       
    }

    public void func() { }

    protected void login_Click(object sender, EventArgs e)
    {   //用户点击登入
        string userName = username.Text;
        string token = password.Text;//Token：令牌
        string sql = "select * from UserTable where username = @username and token = @token";
      
        ArrayList parameterList = new ArrayList();
        ParameterStruct p1 = new ParameterStruct("@username", userName);//设置参数映射列表
        ParameterStruct p2 = new ParameterStruct("@token", token);
        parameterList.Add(p1);
        parameterList.Add(p2);
        DBManipulation dbm = new DBManipulation();
        
        SqlDataReader dataReader = dbm.ExecuteQueryOnLine(sql, parameterList);
        if (dataReader.HasRows) {//如果有记录，说明是合法账户
            dataReader.Read();//指针后移，读取第一条记录
            string position = dataReader.GetString(3);//获得身份              
            Session["username"] = dataReader.GetString(1);
            dataReader.Close();
            dbm.Close();
            switch (position) {
                case "教导主任":                    
                    Session["UserRank"] = PermissionEnum.EducationDean;
                    Response.Redirect("../educationDean/TimeArrange.aspx");
                    break;
                case "学科组长": 
                    Session["UserRank"] = PermissionEnum.CourseMaster;
                    Session["subjectGroupName"] = "综合组";
                    Response.Redirect("../courseMaster/setTeacher.aspx");
                    break;
                case "排课组长":
                    Session["UserRank"] = PermissionEnum.AcademicDean;
                    Response.Redirect("../academicDean/courseArrangement.aspx");
                    break;
                case "教师":
                    Session["UserRank"] = PermissionEnum.Teacher;
                    Response.Redirect("../Teacher/viewSchedule_Class.aspx");
                    break;
                default: 
                    break;
            }            
        }
        else{
            Response.Redirect("error.aspx");                     
        }
    }
}