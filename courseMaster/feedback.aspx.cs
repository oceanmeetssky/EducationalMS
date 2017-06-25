using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class courseMaster_feedback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            if (!(LoginAndPermissionChecking.LoginChecking()))
            {
                Response.Redirect("/ErrorPage/error_NotLogin.aspx");
            }
            if (!(LoginAndPermissionChecking.PermissionChecking(PermissionEnum.CourseMaster)))
            {
                Response.Redirect("/ErrorPage/error_DeniedPermission.aspx");
            }
        }
        Label_SubjectGroup.Text = Session["subjectGroupName"].ToString();
        CreateTable();
    }




    private void CreateTable() {
        /*create table gcgFeedBack(
	        class_id char(4) not null,
	        CourseNo char(3) not null,
	        TeacherNo char(8) not null,
	        primary key(class_id,CourseNo)	
        )*/
        //首先这也是一个二维表，行显示各个学科，列属性为班级
        //先显示学科
        Table1.Rows.Clear();
        string sql_hasgcgFeedBackTable = "select * from sysobjects where name = 'gcgFeedBack'and xtype = 'U'";
        DBManipulation dbm = new DBManipulation();
        object gcgFeedBack = dbm.ExecuteScalar(sql_hasgcgFeedBackTable, null);
        if (gcgFeedBack == null) {//如果表都不存在，说明教导主任没有反对意见
            this.Button_submit.Visible = false;
            return;
        }
        string sql1 = "select distinct subjectname from gcgFeedBack, gcgCourse, gcgSubject, temp_class, gcgSubjectGroup where gcgFeedBack.CourseNo = gcgCourse.CourseNo and gcgCourse.SubjectNo = gcgSubject.subjectno and gcgSubject.subjectGroupNo = gcgSubjectGroup.subjectGroupNo and temp_class.class_id = gcgFeedBack.class_id and Grade = @Grade and subjectGroupName = @subjectGroupName";

        ParameterStruct p_grade = new ParameterStruct("@Grade",DropDownList_Grade.SelectedValue);
        ParameterStruct p_subjectGroupName = new ParameterStruct("@subjectGroupName", Label_SubjectGroup.Text);
        ArrayList plist1 = new ArrayList();
        plist1.Add(p_grade);
        plist1.Add(p_subjectGroupName);
       
        SqlDataReader dataReader1 = dbm.ExecuteQueryOnLine(sql1, plist1);
        TableRow tableRow = new TableRow();
        if (!dataReader1.HasRows)//如果没有反馈，那么什么都不显示
        {
            this.Button_submit.Visible = false;
            return;
        }
        else{
            this.Button_submit.Visible = true;
        }
        tableRow.CssClass = "aspTableRow1";
        //先加入一个空格子，这个格子是二维表的左上角的那个方格
        TableCell cell_blank = new TableCell();
        cell_blank.Text = " ";//没有文本显示
                              //  cell_blank.Attributes.Add("onclick", "getdate(this)");
        cell_blank.CssClass = "aspTableCell3";
        tableRow.Controls.Add(cell_blank);
        List<string> courseset = new List<string>();
        courseset.Clear();//习惯性清空
        while (dataReader1.Read())
        {
            TableCell cell_CourseName = new TableCell();
            cell_CourseName.Text = dataReader1.GetString(0);//文本为科目名
                                                            // cell_CourseName.Attributes.Add("onclick", "getdate(this)");
            cell_CourseName.CssClass = "aspTableCell3";
            tableRow.Controls.Add(cell_CourseName);
            courseset.Add(cell_CourseName.Text);//将这个科目名添加到
        }
        Table1.Controls.Add(tableRow);
        dataReader1.Close();
        dbm.Close();
        //---------------------------------------------分割线，自此表头学科一栏生成完毕--------------------------------------------
        //查询某一个年级在该学科组范围内出问题的所有班级
        string sql2 = "select distinct temp_class.class_id,class_name from temp_class,gcgFeedBack,gcgCourse,gcgSubject,gcgSubjectGroup where temp_class.class_id = gcgFeedBack.class_id and gcgFeedBack.CourseNo = gcgCourse.CourseNo and gcgCourse.SubjectNo = gcgSubject.subjectno and gcgSubject.subjectGroupNo = gcgSubjectGroup.subjectGroupNo and class_grade = @Grade and subjectGroupName = @subjectGroupName order by class_id asc";     

        SqlDataReader dataReader2 = dbm.ExecuteQueryOnLine(sql2, plist1);
        List<ClassInfo> classset = new List<ClassInfo>();
        classset.Clear();
        //班级编号写入链表
        while (dataReader2.Read())
        {
            ClassInfo cdata = new ClassInfo(dataReader2.GetString(0), dataReader2.GetString(1));//存放class_id，class_name
            classset.Add(cdata);
        }
        dataReader2.Close();

        SqlDataReader dataReader3;//准备接受DataReader对象
        string sql3;
        for (int i = 0; i < classset.Count; i++)
        {
            ClassInfo cinfo = classset[i];
            //依据班级号和学期号查询这些班级在特定时间段的授课记录，一个班一个班的显示数据
            sql3 = "select gcgFeedBack.CourseNo,gcgFeedBack.TeacherNo,TeacherName,subjectName from gcgFeedBack,gcgCourse,gcgSubject,temp_teacher where gcgFeedBack.CourseNo = gcgCourse.CourseNo and gcgFeedBack.TeacherNo = temp_teacher.TeacherNo and gcgCourse.SubjectNo = gcgSubject.subjectno and class_id = @class_id";
            ParameterStruct p_class_id = new ParameterStruct("@class_id", cinfo.No);
           // ParameterStruct p_semester = new ParameterStruct("@SemesterNo", semeterNo);
            ArrayList parameterList3 = new ArrayList();
            parameterList3.Add(p_class_id);
           // parameterList3.Add(p_semester);
            dataReader3 = dbm.ExecuteQueryOnLine(sql3, parameterList3);
            //保存这个班出问题的科目号,科目名与老师号
            List<TeaInfo> tset = new List<TeaInfo>();
            tset.Clear();
            while (dataReader3.Read())
            {
                TeaInfo tdata = new TeaInfo(dataReader3.GetString(1), dataReader3.GetString(2), dataReader3.GetString(3), dataReader3.GetString(0));//no,name,sbj,courseno
                tset.Add(tdata);
            }
            tableRow = new TableRow();
            //tableRow.CssClass = "aspTableRow1";
            //先加入一个班级格子,显示这个班级的名字
            TableCell cell_class = new TableCell();         
            cell_class.Text = cinfo.Name;
            cell_class.CssClass = "aspTableCell";
            tableRow.Controls.Add(cell_class);
            //循环学科链表，为每一行（每一个班）的每一门学科填入对应老师
            for (int j = 0; j < courseset.Count; j++)
            {
                string s1 = courseset[j];
                TableCell cell = new TableCell();
                //通过遍历这个班的老师链表实现
                foreach (TeaInfo s2 in tset)
                {
                    if (s1.Equals(s2.Sbj))
                    {                                                           
                        cell.Text = s2.Name;                                                                      
                        break;
                    }
                }
                cell.CssClass = "aspTableCell";
                tableRow.Controls.Add(cell);
            }
            Table1.Controls.Add(tableRow);
            dataReader3.Close();//注意每一次循环都会执行一次数据库查询，返回一个DataReader，所以这次循环体执行完之后必须将其关闭，避免占用connection
        }
        dbm.Close();
    }

    protected void DropDownList_Grade_SelectedIndexChanged(object sender, EventArgs e)
    {
        CreateTable();
    }

    protected void Button_submit_Click(object sender, EventArgs e)
    {
        //点击此按钮，清除该学科组收到的所有反馈信息
        /*create table gcgFeedBack(  --排课组内部专用表
    	    class_id char(4) not null,  
	        CourseNo char(3) not null,
	        TeacherNo char(8) not null,
	        primary key(class_id,CourseNo)	
        )*/
        string subjectGroupName = Label_SubjectGroup.Text;
        string sql0 = "select subjectGroupNo from gcgSubjectGroup where subjectGroupName ='" + subjectGroupName + "'";
        DBManipulation dbm = new DBManipulation();
        Object o = dbm.ExecuteScalar(sql0, null);
        if (o == null)
        {
            //说明没有这个学科组   
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('没有该学科组存在！')", true);
            //实际上是不可能的
            return;//不再查询数据
        }
        string subjectGroupNo = o.ToString();
        dbm.Close();
        string sql1 = "select CourseNo from gcgSubject,gcgCourse where gcgCourse.SubjectNo = gcgSubject.subjectno and Grade = @Grade and subjectGroupNo = @subjectGroupNo";
        ParameterStruct p1 = new ParameterStruct("@Grade", DropDownList_Grade.SelectedValue);
        ParameterStruct p2 = new ParameterStruct("@subjectGroupNo", subjectGroupNo);
        ArrayList pList1 = new ArrayList();
        pList1.Add(p1);
        pList1.Add(p2);
        SqlDataReader dr = dbm.ExecuteQueryOnLine(sql1,pList1);
        ArrayList courseNoList = new ArrayList();
        while (dr.Read()) {
            courseNoList.Add(dr.GetString(0));            
        }
        dr.Close();
        foreach (string courseno in courseNoList) {
            string sql_deletefromGcgFeedBack = "delete from gcgFeedBack where CourseNo = '" + courseno + "'";
            dbm.ExecuteNonQuery(sql_deletefromGcgFeedBack, null);
        }
        //如果发现反馈表中已经没有了记录，那么就直接删掉，神不知鬼不觉~
        string sql_checkgcgFeedBack = "select * from gcgFeedBack";
        object isEmpty = dbm.ExecuteScalar(sql_checkgcgFeedBack, null);
        if (isEmpty == null) {
            //没有反馈记录了
            string sql_droptable = "drop table gcgFeedBack";
            dbm.ExecuteNonQuery(sql_droptable, null);
        }
        dbm.Close();
        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('已清除反馈信息！')", true);
        CreateTable();
    }
}