using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class educationDean_viewSchedule : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!(LoginAndPermissionChecking.LoginChecking()))
            {
                Response.Redirect("/ErrorPage/error_NotLogin.aspx");
            }
            SetItemForDropDownList_Class();
        }

        CreateTheWholeSchedule();

    }


    protected void CreateTheWholeSchedule()
    {
        Table1.Rows.Clear();
        Table1.Rows.Clear();
        TableRow head = new TableRow();
        TableCell t0 = new TableCell();
        TableCell t1 = new TableCell();
        t1.Text = "星期一";
        TableCell t2 = new TableCell();
        t2.Text = "星期二";
        TableCell t3 = new TableCell();
        t3.Text = "星期三";
        TableCell t4 = new TableCell();
        t4.Text = "星期四";
        TableCell t5 = new TableCell();
        t5.Text = "星期五";
        head.Controls.Add(t0);
        head.Controls.Add(t1);
        head.Controls.Add(t2);
        head.Controls.Add(t3);
        head.Controls.Add(t4);
        head.Controls.Add(t5);
        head.CssClass = "aspTableRow1";
        Table1.Controls.Add(head);
        DBManipulation dbm = new DBManipulation();
        string sql2 = "select subjectname,gcgLesson.Timeid from gcgLesson, gcgLectureForm, gcgSchedule, gcgCourse, gcgSubject, temp_class where gcgLesson.LectureNo = gcgLectureForm.LectureNo and gcgLectureForm.CourseNo = gcgCourse.CourseNo and gcgCourse.SubjectNo = gcgSubject.subjectno and gcgLesson.Timeid = gcgSchedule.Timeid and gcgLectureForm.ClassNo = temp_class.class_id and ClassNo = @classv and class_grade = @gradev order by gcgLesson.Timeid";
        ParameterStruct p1 = new ParameterStruct("@gradev", DropDownList_Grade.SelectedValue);
        ParameterStruct p2 = new ParameterStruct("@classv", DropDownList_class.SelectedValue);
        ArrayList _plist = new ArrayList();
        _plist.Add(p1);
        _plist.Add(p2);
        SqlDataReader dr = dbm.ExecuteQueryOnLine(sql2, _plist);

        string[] class_1 = new string[40 + 1];
        int kk = 0;
        while (dr.Read())
        {
            class_1[kk] = dr.GetString(0);
            kk++;
        }
        dr.Close();
        dbm.Close();
        for (int i = 0; i < 8; i++)
        {
            TableRow tablerow = new TableRow();
            tablerow.CssClass = "aspTableRow2";
            TableCell tc = new TableCell();
            tc.CssClass = "aspTableCell4";//第几节
            tc.Text = "第" + (i + 1) + "节";
            tablerow.Controls.Add(tc);
            for (int j = 0; j < 5; j++)
            {
                TableCell cell = new TableCell();
                cell.Text = class_1[i + j * 8];
                cell.CssClass = "aspTableCell2";//具体安排
                tablerow.Controls.Add(cell);
            }
            Table1.Controls.Add(tablerow);
        }
    }

    
    protected void SetItemForDropDownList_Class()
    {
        //string sql = "select class_id,class_name from temp_class where class_grade = '" + DropDownList_Grade.SelectedValue + "'";
        string sql = "select distinct ClassNo,class_name from gcgLectureForm, temp_class where gcgLectureForm.ClassNo = temp_class.class_id and class_grade = '" + DropDownList_Grade.SelectedValue + "'";
        DBManipulation dbm = new DBManipulation();
        DataSet ds = dbm.ExecuteQueryOffLine(sql, null);
        DropDownList_class.AutoPostBack = true;
        DropDownList_class.DataSource = ds.Tables["defaultTable"];
        DropDownList_class.DataTextField = ds.Tables["defaultTable"].Columns[1].ColumnName;
        DropDownList_class.DataValueField = ds.Tables["defaultTable"].Columns[0].ColumnName;
        DropDownList_class.DataBind();

    }
    protected void DropDownList_class_SelectedIndexChanged(object sender, EventArgs e)
    {
        CreateTheWholeSchedule();
    }
  
    protected void DropDownList_Grade_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetItemForDropDownList_Class();//设置新的年级的班级集合
        CreateTheWholeSchedule();
    }
  
}