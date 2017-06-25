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
        /*
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
        */
        string[] class_1 = ProvideLessonDataByClass(DropDownList_Grade.SelectedValue,DropDownList_class.SelectedValue);//拿到这个班的课程具体安排
        for (int lesson = 1; lesson <= 8; lesson++)
        {
            TableRow tablerow = new TableRow();
            tablerow.CssClass = "aspTableRow2";
            TableCell tc = new TableCell();
            tc.CssClass = "aspTableCell4";//第几节
            tc.Text = "第" + lesson + "节";
            tablerow.Controls.Add(tc);
            for (int day = 1; day <= 5; day++)
            {
                TableCell cell = new TableCell();
                cell.Text = class_1[lesson + (day - 1) * 8];
                cell.CssClass = "aspTableCell2";//具体安排
                tablerow.Controls.Add(cell);
            }
            Table1.Controls.Add(tablerow);
        }
    }

    protected string[] ProvideLessonDataByClass( string _grade,string _class ) {
        DBManipulation dbm = new DBManipulation();
        string sql2 = "select subjectname,gcgLesson.Timeid from gcgLesson, gcgLectureForm, gcgSchedule, gcgCourse, gcgSubject, temp_class where gcgLesson.LectureNo = gcgLectureForm.LectureNo and gcgLectureForm.CourseNo = gcgCourse.CourseNo and gcgCourse.SubjectNo = gcgSubject.subjectno and gcgLesson.Timeid = gcgSchedule.Timeid and gcgLectureForm.ClassNo = temp_class.class_id and ClassNo = @classv and class_grade = @gradev order by gcgLesson.Timeid";
        ParameterStruct p1 = new ParameterStruct("@gradev", _grade);
        ParameterStruct p2 = new ParameterStruct("@classv", _class);
        ArrayList _plist = new ArrayList();
        _plist.Add(p1);
        _plist.Add(p2);
        SqlDataReader dr = dbm.ExecuteQueryOnLine(sql2, _plist);

        string[] class_1 = new string[40 + 1];
        int i = 1;
        while (dr.Read())
        {
            class_1[i] = dr.GetString(0);
            i++;
        }
        dr.Close();
        dbm.Close();
        return class_1;
    }
    protected void Button_ExportIntoExcel_Click(object sender, EventArgs e)
    {
        //点击此按钮导出到Excel
        ExcelManipulation exc = new ExcelManipulation();
        string _year = DateTime.Now.Year + "";
        string _month = DateTime.Now.Month + "";
        string _day = DateTime.Now.Day + "";
        string _hour = DateTime.Now.Hour + "";
        string _minute = DateTime.Now.Minute + "";
        string _second = DateTime.Now.Second + "";
        string _miliSecond = DateTime.Now.Millisecond + "";
        string fileName = "Table_" + _year + "_" + _month + "_" + _day + "_" + _hour + "_" + _minute + "_" + _second + "_" + _miliSecond;
        exc.setFileName(fileName);
        for (int i = 0; i < DropDownList_class.Items.Count; i++)
        {
            ListItem item = DropDownList_class.Items[i];
            
            string SheetName = "Table_" + item.Text.ToString().Substring(0, 2);
            //string SheetName = "Table_" + item.Text.ToString().Substring(0,2) + "_" + _year + "_" + _month + "_" + _day + "_" + _hour + "_" + _minute + "_" + _second;
            string sql_createTable = "create table " + SheetName + " ([节次] VarChar,[星期一] VarChar,[星期二] VarChar,[星期三] VarChar,[星期四] VarChar,[星期五] VarChar)";
            exc.ExecuteNonQuery(sql_createTable, null);   //创建一个表单                    
            string [] class_1 = ProvideLessonDataByClass(DropDownList_Grade.SelectedValue, item.Value);
            for (int lesson = 1; lesson <= 8; lesson++)
            {                                                            
                string lessonNo = "第" + lesson + "节";
                string[] subjectName = new string[6];
                for (int day = 1; day <= 5; day++)
                {                    
                    //string subjectName = class_1[lesson + (day-1) * 8];
                    subjectName[day] = class_1[lesson + (day - 1) * 8];
                }
                // cmd.CommandText = "INSERT INTO TestSheet VALUES(1,'elmer','password')";        
                string sql_insert = "insert into " + SheetName + " values( '" + lessonNo + "','" + subjectName[1] + "','" + subjectName[2] + "','" + subjectName[3] + "','" + subjectName[4] + "','" + subjectName[5] + "')";
                exc.ExecuteNonQuery(sql_insert, null);
            }
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