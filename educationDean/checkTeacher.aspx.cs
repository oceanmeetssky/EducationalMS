using System;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class educationDean_checkTeacher : System.Web.UI.Page
{
    /*
    protected override void OnLoad(EventArgs e)
    {
       
    }
    */
    protected void Page_Load(object sender, EventArgs e)
    {
        //外部添加局部刷新控件      
        ScriptManager1.RegisterAsyncPostBackControl(this.DropDownList_Grade);
        ScriptManager1.RegisterAsyncPostBackControl(this.DropDownList_semester);
        ScriptManager1.RegisterAsyncPostBackControl(this.DropDownList_year);
        if (!IsPostBack) {   
                                 
            if (!(LoginAndPermissionChecking.LoginChecking())) {
                Response.Redirect("/ErrorPage/error_NotLogin.aspx");
            }
            if (!(LoginAndPermissionChecking.PermissionChecking(PermissionEnum.EducationDean))) {
                Response.Redirect("/ErrorPage/error_DeniedPermission.aspx");
            }
            //首先先检查是否有gcgFeedBack这张表，如果不存在就新建
            string sql_LookForgcgFeedBack = "select * from sysobjects where name = 'gcgFeedBack' and xtype = 'U'";
            DBManipulation dbm = new DBManipulation();
            object o = dbm.ExecuteScalar(sql_LookForgcgFeedBack, null);
            if (o == null)
            {
                string sql_CreategcgFeedBack = "create table gcgFeedBack(" +
                                                    "class_id char(4) not null," +
                                                    "CourseNo char(3) not null," +
                                                    "TeacherNo char(8) not null," +
                                                    "primary key(class_id, CourseNo)" +
                                                ")";
                dbm.ExecuteNonQuery(sql_CreategcgFeedBack, null);
            }
            /*create table gcgFeedBack(  --排课组内部专用表
	            class_id char(4) not null,
	            CourseNo char(3) not null,
	            TeacherNo char(8) not null,
	            primary key(class_id,CourseNo)	
            )*/
            //页面第一次载入之前，要查询gcgFeedBack授课安排反馈表，看看是不是还有未确认的反馈信息  
            string sql = "select class_id,CourseNo from gcgFeedBack";
           // DBManipulation dbm = new DBManipulation();
            SqlDataReader dr = dbm.ExecuteQueryOnLine(sql, null);
            while (dr.Read())
            {
                string buttonName = dr.GetString(0) + dr.GetString(1);
                //Session[buttonName] = "nameRedButton";//我们使用Session保存有意见的老师安排的css样式
                Session[buttonName] = "aspTableCellBrown2";//把原先红色的恢复成黑色
            }
            dr.Close();
            dbm.Close();
        }
    
        createtable(GetSemesterNo());
    }

    protected void createtable(string semeterNo)
    {
        Table1.Rows.Clear();
        ArrayList buttonlist = new ArrayList();//存放所有按钮的信息
        //形参是学期号，蕴含年份和上下学期
        string grade = DropDownList_Grade.SelectedValue.ToString();
        //查出这个年级的所有学科，以便形成列表头
        //一个年级的学科是没有历史记录的，设定即覆盖
        string sql1 = "select CourseNo,subjectname from gcgCourse, gcgSubject where gcgCourse.SubjectNo = gcgSubject.subjectno and Grade = @Grade";
        ParameterStruct p_grade = new ParameterStruct("@Grade", grade);
        ArrayList parameterList1 = new ArrayList();
        parameterList1.Add(p_grade);
        DBManipulation dbm = new DBManipulation();
        SqlDataReader dataReader1 = dbm.ExecuteQueryOnLine(sql1, parameterList1);
        List<string> courseset = new List<string>();
        courseset.Clear();//习惯性清空
        if (!dataReader1.HasRows) {
            return;
        }
        TableRow tableRow = new TableRow();
        tableRow.CssClass = "aspTableRow1";
        //先加入一个空格子，这个格子是二维表的左上角的那个方格
        TableCell cell_blank = new TableCell();
        cell_blank.Text = " ";//没有文本显示
      //  cell_blank.Attributes.Add("onclick", "getdate(this)");
        cell_blank.CssClass = "aspTableCell3";      
        tableRow.Controls.Add(cell_blank);
       // int id = 0;
        while (dataReader1.Read()) {
            TableCell cell_CourseName = new TableCell();
            cell_CourseName.Text = dataReader1.GetString(1);//文本为科目名
           // cell_CourseName.Attributes.Add("onclick", "getdate(this)");
            cell_CourseName.CssClass = "aspTableCell3";           
            tableRow.Controls.Add(cell_CourseName);
            courseset.Add(cell_CourseName.Text);//将这个科目名添加到
        }
        Table1.Controls.Add(tableRow);
        dataReader1.Close();
        dbm.Close();
        //---------------------------------------------分割线，自此表头学科一栏生成完毕--------------------------------------------




        //-------------------------------------------------以下开始按行生成数据----------------------------------------------------
        // ParameterStruct p_year = new ParameterStruct("@year", year+"%");
        ParameterStruct p_semesterNo = new ParameterStruct("@SemesterNo", semeterNo);
        ArrayList parameterList2 = new ArrayList();
        parameterList2.Add(p_grade);
        parameterList2.Add(p_semesterNo);
        dbm = new DBManipulation();
        //查询出某一个学期某一个年级的所有班级
        string sql2 = "select distinct class_id,class_name from temp_class,gcgLectureForm where temp_class.class_id = gcgLectureForm.ClassNo and class_grade = @Grade and SemesterNo = @SemesterNo order by class_id asc";

        SqlDataReader dataReader2 = dbm.ExecuteQueryOnLine(sql2, parameterList2);
        List<ClassInfo> classset = new List<ClassInfo>();
        classset.Clear();

        //班级编号写入链表
        while (dataReader2.Read()) {
            ClassInfo cdata = new ClassInfo(dataReader2.GetString(0), dataReader2.GetString(1));//存放class_id，class_name
            classset.Add(cdata);
        }
        dataReader2.Close();

        SqlDataReader dataReader3;//准备接受DataReader对象
        string sql3;
        //string SemesterNo = GetSemesterNo();
        for(int i = 0;i < classset.Count; i++) {
            ClassInfo cinfo = classset[i];    
            //依据班级号和学期号查询这些班级在特定时间段的授课记录，一个班一个班的显示数据
            sql3 = "select gcgLectureForm.TeacherNo,TeacherName,subjectname,gcgCourse.CourseNo from gcgLectureForm,gcgCourse,gcgSubject,temp_teacher where gcgCourse.CourseNo = gcgLectureForm.CourseNo and gcgCourse.SubjectNo = gcgSubject.subjectno and temp_teacher.TeacherNo = gcgLectureForm.TeacherNo and ClassNo = @ClassNo and SemesterNo = @SemesterNo";
            ParameterStruct p_classNo = new ParameterStruct("@ClassNo", cinfo.No);
            ParameterStruct p_semester = new ParameterStruct("@SemesterNo", semeterNo);
            ArrayList parameterList3 = new ArrayList();
            parameterList3.Add(p_classNo);
            parameterList3.Add(p_semester);
            dataReader3 = dbm.ExecuteQueryOnLine(sql3, parameterList3);
            //保存这个班在某一个学期的所有老师
            List<TeaInfo> tset = new List<TeaInfo>();
            tset.Clear();
            while (dataReader3.Read())
            {
                TeaInfo tdata = new TeaInfo(dataReader3.GetString(0), dataReader3.GetString(1), dataReader3.GetString(2), dataReader3.GetString(3));//no,name,sbj,courseno
                tset.Add(tdata);
            }
            tableRow = new TableRow();
            //tableRow.CssClass = "aspTableRow1";
            //先加入一个班级格子,显示这个班级的名字
            TableCell cell_class = new TableCell();
           // cell_class.Attributes.Add("onclick", "getdate(this)");
            cell_class.Text = cinfo.Name;
         //   cell_class.ID = "cell_class_" + i + times;
            cell_class.CssClass = "aspTableCell4";
            tableRow.Controls.Add(cell_class);
            //循环学科链表，为每一行（每一个班）的每一门学科填入对应老师
            for (int j = 0;j < courseset.Count;j++) {
                string s1 = courseset[j];
           // foreach (string s1 in courseset) {
                TableCell cell = new TableCell();
                cell.CssClass = "aspTableCell";//先设置格子的样式为默认样式
                //  cell.Attributes.Add("onclick", "getdata()");
                //通过遍历这个班的老师链表实现
                foreach (TeaInfo s2 in tset) {                   
                    if (s1.Equals(s2.Sbj)) {
                        //    public MyButton(string class_id,string CourseNo,string TeacherNo) {                           
                        MyButton b = new MyButton(cinfo.No,s2.Courseno,s2.No);
                        b.Text = s2.Name;
                        //bt.Click += new EventHandler(bt_Click);                    
                        string buttonName = b.Class_id + b.CourseNo1;
                        if (Session[buttonName] == null ) {
                            // Session[buttonName] = "namebutton";
                            Session[buttonName] = "aspTableCell";
                        }                                               
                        b.Click += new EventHandler(ButtonForFeedBack_onClick);
                        //  b.CssClass = Session[buttonName].ToString();//如果刷新之前已经有记录，那么直接用之前的样式
                        b.CssClass = "namebutton";
                        buttonlist.Add(b);
                        //cell.Text = s2.Name;// dataReader.GetString(1);                                               
                        cell.Controls.Add(b);
                        cell.CssClass = Session[buttonName].ToString();//看要不要改样式
                        break;                        
                    }                    
                }               
                tableRow.Controls.Add(cell);
            }
            Table1.Controls.Add(tableRow);
            dataReader3.Close();//注意每一次循环都会执行一次数据库查询，返回一个DataReader，所以这次循环体执行完之后必须将其关闭，避免占用connection
        }    
        dbm.Close();
        Session["buttonlist"] = buttonlist;
       // times++;
    }

    protected void ButtonForFeedBack_onClick(object sender, EventArgs e) {
        MyButton button = (MyButton)sender;
        string buttonName = button.Class_id + button.CourseNo1;

        
        if (Session[buttonName] == null || Session[buttonName].ToString().Equals("aspTableCell"))
        {
            Session[buttonName] = "aspTableCellBrown2";//用session记录这个Button被点击过
            //插入反馈记录
            /*create table gcgFeedBack(
	            class_id char(4) not null,
	            CourseNo char(3) not null,
	            TeacherNo char(8) not null,
	            primary key(class_id,CourseNo)	
            )*/
            string sql = "insert into gcgFeedBack values(@class_id,@CourseNo,@TeacherNo)";
            ParameterStruct p_classid = new ParameterStruct("@class_id", button.Class_id);
            ParameterStruct p_courseno = new ParameterStruct("@CourseNo", button.CourseNo1);
            ParameterStruct p_teacherno = new ParameterStruct("@TeacherNo", button.TeacherNo1);
            ArrayList plist = new ArrayList();
            plist.Add(p_classid);
            plist.Add(p_courseno);
            plist.Add(p_teacherno);
            DBManipulation dbm = new DBManipulation();
            try {
                dbm.ExecuteNonQuery(sql, plist);
            }
            catch (Exception)
            {              
            }
        }
        else {
            Session[buttonName] = "aspTableCell";
            string sql = "delete from gcgFeedBack where class_id = @class_id and CourseNo = @CourseNo";
            ParameterStruct p_classid = new ParameterStruct("@class_id", button.Class_id);
            ParameterStruct p_courseno = new ParameterStruct("@CourseNo", button.CourseNo1);
            //ParameterStruct p_teacherno = new ParameterStruct("@TeacherNo", button.TeacherNo1);
            ArrayList plist = new ArrayList();
            plist.Add(p_classid);
            plist.Add(p_courseno);
            //plist.Add(p_teacherno);
            DBManipulation dbm = new DBManipulation();
            dbm.ExecuteNonQuery(sql, plist);
           // Session.Remove(name);
        }        
        createtable(GetSemesterNo());
    }

    protected void DropDownList_Grade_SelectedIndexChanged(object sender, EventArgs e){
        //createtable(DropDownList_year);
        createtable(GetSemesterNo());
    }

    private string GetSemesterNo() {
        //授课表的学期号是由4位年份2位学期位组成
        string year = DropDownList_year.SelectedValue.ToString();
        string semester = DropDownList_semester.SelectedValue.ToString();       

        return year + semester;
    }
    private string GetLectureNoLike() {//这个函数没用到
        string LectureNO_part1 = DropDownList_year.SelectedValue.ToString();
        string semester = DropDownList_semester.SelectedValue.ToString();
        string grade = DropDownList_Grade.SelectedValue.ToString();

        string LectureNo_part2 = "";
        switch (grade)
        {
            case "1":
                if (semester.Equals("01"))
                {
                    LectureNo_part2 = "1";
                }
                else
                {
                    LectureNo_part2 = "2";
                }
                break;
            case "2":
                if (semester.Equals("01"))
                {
                    LectureNo_part2 = "3";
                }
                else
                {
                    LectureNo_part2 = "4";
                }
                break;
            case "3":
                if (semester.Equals("01"))
                {
                    LectureNo_part2 = "5";
                }
                else
                {
                    LectureNo_part2 = "6";
                }
                break;
        }
        return LectureNO_part1 + LectureNo_part2;
    }
    protected void DropDownList_year_SelectedIndexChanged(object sender, EventArgs e)
    {
        //授课记录主码，前2位代表年份，一位代表年级 + 学期（初一1，2 初二3，4 初三5，6），2位代表班级，2位代表学科
        //string lectureNoLike = GetLectureNoLike();
        createtable(GetSemesterNo());
    }

    protected void DropDownList_semester_SelectedIndexChanged(object sender, EventArgs e)
    {      
        createtable(GetSemesterNo());
    }
}