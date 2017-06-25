using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class courseMaster_setTeacher : System.Web.UI.Page
{
    //注意：ASP控件一旦激活postback,会导致页面重载，原先所有保存的类数据成员会被重新初始化！
    //要么重新赋值，要么保存到session或者application获得ViewState或者Cookie中。
    private ArrayList TableCellList_class = new ArrayList();
    private ArrayList DropDownList_select = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager1.RegisterAsyncPostBackControl(this.DropDownList_Grade);
        ScriptManager1.RegisterAsyncPostBackControl(this.DropDownList_Course);
        ScriptManager1.RegisterAsyncPostBackControl(this.DropDownList_semester);
        ScriptManager1.RegisterAsyncPostBackControl(this.DropDownList_year);
        if (!IsPostBack)
        {

            if (!(LoginAndPermissionChecking.LoginChecking()))
            {
                Response.Redirect("/ErrorPage/error_NotLogin.aspx");
            }
            if (!(LoginAndPermissionChecking.PermissionChecking(PermissionEnum.CourseMaster)))
            {
                Response.Redirect("/ErrorPage/error_DeniedPermission.aspx");
            }

            Object o = Session["subjectGroupName"];
            if (o == null)
            {
                Response.Redirect("/login/login.aspx");
            }
            else
            {
                //Label_SubjectGroup.Text = Request.QueryString["subjectGroupName"].ToString();
                Label_SubjectGroup.Text = Session["subjectGroupName"].ToString();
            }
            ProvideDataForSelectSubject();

            //页面刚刚载入的时候查询学科表中所有学科的对应课时数，并放置在一张学科课时表中，以待后面使用
            string sql_MappingCourseNoAndCourseTime = "select CourseNo,CourseTime from gcgCourse";
            DBManipulation dbm = new DBManipulation();
            SqlDataReader dr = dbm.ExecuteQueryOnLine(sql_MappingCourseNoAndCourseTime, null);
            Dictionary<string, Byte> dict_CourseNo_CourseTime = new Dictionary<string, Byte>();
            while (dr.Read())
            {
                dict_CourseNo_CourseTime.Add(dr.GetString(0), dr.GetByte(1));
            }
            dr.Close();
            dbm.Close();
            Cache["dict_CourseNo_CourseTime"] = dict_CourseNo_CourseTime;

            //同时计算所有老师的授课课时数，放在一个映射表中，以待后面使用
            string sql_getTeacherSet = "select TeacherNo from temp_teacher";
            DataSet ds = dbm.ExecuteQueryOffLine(sql_getTeacherSet, null);
            DataTable teacherTable = ds.Tables["defaultTable"];
            Dictionary<string, int> dict_TeacherNo_CourseTime = new Dictionary<string, int>();
            foreach (DataRow r in teacherTable.Rows)
            {
                string TeacherNo = r["TeacherNo"].ToString();
                string sql_countTeacherTime = "select sum(CourseTime) from gcgLectureForm,temp_teacher,gcgCourse " +
                                       "where gcgLectureForm.TeacherNo = temp_teacher.TeacherNo " +
                                       "and gcgLectureForm.CourseNo = gcgCourse.CourseNo " +
                                       "and temp_teacher.TeacherNo = '" + TeacherNo + "'";
                //  DBManipulation dbm = new DBManipulation();
                object obj = dbm.ExecuteScalar(sql_countTeacherTime, null);
                if (obj == null || obj.ToString() == "")
                {//还没有给这个老师安排带班
                    dict_TeacherNo_CourseTime[TeacherNo] = 0;
                }
                else
                {
                    dict_TeacherNo_CourseTime[TeacherNo] = int.Parse(obj.ToString());
                }
            }
            Cache["dict_TeacherNo_CourseTime"] = dict_TeacherNo_CourseTime;
            //保存每一个下拉菜单的索引值，方便下面进行课时计算
            CreateTableBasedOnClass();
            setDataForEachTeacherDropDownList();
            foreach (DropDownList l in DropDownList_select)
            {
                Session[l.ID] = l.SelectedIndex;//使用对象的唯一标识符作为唯一ID保存
            }
        }
        //debug1.Text = Request.QueryString["subjectGroupName"].ToString();
        CreateTableBasedOnClass();
        setDataForEachTeacherDropDownList();
        //  Page.MaintainScrollPositionOnPostBack = true;//刷新后滚动条回到之前的位置,但是会导致页面闪烁    
    }
    //基于班级，动态生成表
    protected void CreateTableBasedOnClass()
    {

        Table1.Controls.Clear();//把原先的表全部清干净
        TableCellList_class.Clear();//把保存的上一次的控件引用清理掉，不需要了，这次会生成一个新的表
        DropDownList_select.Clear();
        //重新设置表头
        TableRow tableRowHeader = new TableRow();
        tableRowHeader.CssClass = "aspTableRow1";
        TableCell tablecell_classNameHeader = new TableCell();
        tablecell_classNameHeader.Text = "班级";
        tablecell_classNameHeader.CssClass = "aspTableCell";
        //  tablecell_courseNameHeader.Style.Add() //= "width: 100px";
        TableCell tablecell_SelectTeacherHeader = new TableCell();
        tablecell_SelectTeacherHeader.Text = "授课教师";
        tablecell_SelectTeacherHeader.CssClass = "aspTableCell";

        tableRowHeader.Controls.Add(tablecell_classNameHeader);
        tableRowHeader.Controls.Add(tablecell_SelectTeacherHeader);

        Table1.Controls.Add(tableRowHeader);
        //开始生成正文
        //先把班级表查出来 class_id class_name class_grade
        //select * from 
        string grade = DropDownList_Grade.SelectedValue;
        //string grade = "01";
        string sql1 = "select * from temp_class where class_grade = @Grade";
        ParameterStruct p1 = new ParameterStruct("@Grade", grade);
        ArrayList pList1 = new ArrayList();
        pList1.Add(p1);
        DBManipulation dbm = new DBManipulation();
        DataSet ds_class = dbm.ExecuteQueryOffLine(sql1, pList1);
        //现在ds_class里面有一个defaultTable存放了temp_class的全部内容
        DataTable t_class = ds_class.Tables["defaultTable"];
        foreach (DataRow r in t_class.Rows)
        {
            TableRow newRow = new TableRow();

            TableCell cell_class = new TableCell();
            cell_class.Text = r["class_name"].ToString();
            // tablecell_subjectName.CssClass = "aspTableCell";
            cell_class.CssClass = "aspTableCell";
            TableCellList_class.Add(cell_class);

            TableCell cell_selectTeacher = new TableCell();
            cell_selectTeacher.CssClass = "aspTableCell";
            DropDownList dropDownList_selectTeacher = new DropDownList();

            //DropDownList_SelectTeacher
            dropDownList_selectTeacher.SelectedIndexChanged += DropDownList_SelectTeacher;
            dropDownList_selectTeacher.AutoPostBack = true;
            DropDownList_select.Add(dropDownList_selectTeacher);
            cell_selectTeacher.Controls.Add(dropDownList_selectTeacher);

            newRow.Controls.Add(cell_class);
            newRow.Controls.Add(cell_selectTeacher);

            Table1.Controls.Add(newRow);
        }
    }
    //生成该年级该学科组所包含的所有科目，为下拉框绑定数据
    protected void ProvideDataForSelectSubject()
    {
        string grade = DropDownList_Grade.SelectedValue;
        //debug1.Text = "年级：" + grade;
        string subjectGroupName = Label_SubjectGroup.Text;
        string sql0 = "select subjectGroupNo from gcgSubjectGroup where subjectGroupName ='" + subjectGroupName + "'";
        DBManipulation dbm = new DBManipulation();
        Object o = dbm.ExecuteScalar(sql0, null);
        if (o == null)
        {
            //说明没有这个学科组   
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('没有该学科组存在！')", true);
            return;//不再查询数据
        }
        string subjectGroupNo = o.ToString();
        dbm.Close();
        string sql1 = "select CourseNo,subjectname from gcgSubject,gcgCourse where gcgCourse.SubjectNo = gcgSubject.subjectno and Grade = @Grade and subjectGroupNo = @subjectGroupNo";
        ParameterStruct p1 = new ParameterStruct("@Grade", grade);
        ParameterStruct p2 = new ParameterStruct("@subjectGroupNo", subjectGroupNo);
        ArrayList pList1 = new ArrayList();
        pList1.Add(p1);
        pList1.Add(p2);
        /*
        SqlDataReader dr = dbm.ExecuteQueryOnLine(sql1,pList1);        
        while (dr.Read()) {
            ListItem item = new ListItem(dr.GetString(1),dr.GetString(0));
            item.Attributes.Add("style", "color:red");
            DropDownList_Course.Items.Add(item);
        }*/

        DataSet ds_Course = dbm.ExecuteQueryOffLine(sql1, pList1);
        DropDownList_Course.AutoPostBack = true;
        DropDownList_Course.DataSource = ds_Course.Tables["defaultTable"];
        DropDownList_Course.DataTextField = ds_Course.Tables["defaultTable"].Columns[1].ColumnName;
        DropDownList_Course.DataValueField = ds_Course.Tables["defaultTable"].Columns[0].ColumnName;
        ListItem item = new ListItem();
        //DropDownList_Subject.SelectedIndex = 0;
        DropDownList_Course.DataBind();
        //要先清洗掉上一个年级残存的老师集合
        foreach (DropDownList d in DropDownList_select)
        {
            d.Items.Clear();
        }
        setDataForEachTeacherDropDownList();
    }


    protected void DropDownList_Grade_SelectedIndexChanged(object sender, EventArgs e)
    {   //选择当前年级，修改可供设置的科目所在下拉框的数据集
        //  Response.Write("<script>alert('111')</script>");
        // Response.Write("<script>alert('aaaaaaa')</script>");
        // System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('年级修改！')", true);
        ProvideDataForSelectSubject();
    }

    protected void DropDownList_Subject_SelectedIndexChanged(object sender, EventArgs e)
    {   //选择科目，应该使得每一行的老师选择框发生变化
        setDataForEachTeacherDropDownList();
    }

    protected void DropDownList_SelectTeacher(object sender, EventArgs e)
    {
        DropDownList l = (DropDownList)sender;
        if (Cache["dict_CourseNo_CourseTime"] == null)
        {
            return;
        }
        Dictionary<string, Byte> dict_CourseNo_CourseTime = (Dictionary<string, Byte>)Cache["dict_CourseNo_CourseTime"];
        Dictionary<string, int> dict_TeacherNo_CourseTime = (Dictionary<string, int>)Cache["dict_TeacherNo_CourseTime"];

        //如果这一次选中的是某一位老师
        if (!l.SelectedItem.Text.Equals("待定"))
        {
            string teacherNo = l.SelectedValue;//拿到教师工号
            int TeacherTime = dict_TeacherNo_CourseTime[teacherNo];//查到教师课时数
            int addnum = (int)(dict_CourseNo_CourseTime[DropDownList_Course.SelectedValue]);//加数
            TeacherTime += addnum;
            dict_TeacherNo_CourseTime[teacherNo] = TeacherTime;//更新课时数           
            //我们仅仅提示，但是并不禁止           
            int max = int.Parse(ConfigurationManager.AppSettings["MaxLimitForTakingClassPreDay"].ToString());
            if (TeacherTime > max * 5)
            {
                string teacherName = l.SelectedItem.Text;
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('已经超过" + teacherName.Substring(0, 1) + "老师的最大授课数，请体谅老师的辛劳')", true);
                // return;
            }
        }
        else//如果这一次并没有选择某一位老师，那么不做加法
        {
        }
        //加法环节结束，开始减法环节
        int preindex = (int)Session[l.ID];//Session[l.ID]存放有这个下拉框上一次选中的索引值
        ListItem item = l.Items[preindex];//取得那一项
        if (item.Text.Equals("待定"))//如果上一次并没有选择老师，则不做减法
        {
        }
        else//否则对应老师减去相应的课时数
        {
            int time = dict_TeacherNo_CourseTime[item.Value];
            time -= dict_CourseNo_CourseTime[DropDownList_Course.SelectedValue];
            dict_TeacherNo_CourseTime[item.Value] = time;
        }
        Session[l.ID] = l.SelectedIndex;//更新Session[l.ID]的值
        setDataForEachTeacherDropDownList();
        this.Page_Load(null, null);
    }
    protected void setDataForEachTeacherDropDownList()
    {
        if (DropDownList_Course.Items.Count == 0)
        {
            //说明没有课，停止查询
            return;
        }
        string courseNo = DropDownList_Course.SelectedValue;//注意这里选出来的是学科号        
        // debug1.Text = "courseNo:"+courseNo;
        // Response.Write("<script>alert('" + courseNo + "')</script>");\

        string subjectno = courseNo.Substring(1);//学科号的后两位是科目号，因为我们的学科Id就是按照这样的规则计算出来的
        string sql_forA = "select TeacherNo,TeacherName from temp_teacher where SubjectA ='" + subjectno + "' order by TeacherNo asc";
        DBManipulation dbm = new DBManipulation();
        DataSet ds_teacher_A = dbm.ExecuteQueryOffLine(sql_forA, null);//查询出所有可以教这门课的擅长老师
        string sql_forB = "select TeacherNo,TeacherName from temp_teacher where SubjectB ='" + subjectno + "' order by TeacherNo asc";
        string className = "";
        DataSet ds_teacher_B = dbm.ExecuteQueryOffLine(sql_forB, null);//查询出所有可以教这门课的非擅长老师
        Dictionary<string, int> dict_TeacherNo_CourseTime = (Dictionary<string, int>)Cache["dict_TeacherNo_CourseTime"];
        for (int i = 0; i < DropDownList_select.Count; i++)
        {
            DropDownList d = (DropDownList)DropDownList_select[i];
            d.Items.Clear();
            d.ID = DropDownList_Grade.SelectedValue + DropDownList_Course.SelectedValue + i;

            ///这里可以排个版
            foreach (DataRow teacher_A in ds_teacher_A.Tables["defaultTable"].Rows)
            {
                ListItem item = new ListItem(teacher_A[1].ToString() + "  " + dict_TeacherNo_CourseTime[teacher_A[0].ToString()], teacher_A[0].ToString());
                //ListItem item = new ListItem(itemText, teacher_A[0].ToString());
                item.Attributes.Add("style", "color:red");
                d.Items.Add(item);
            }
            foreach (DataRow teacher_B in ds_teacher_B.Tables["defaultTable"].Rows)
            {
                ListItem item = new ListItem(teacher_B[1].ToString() + "  " + dict_TeacherNo_CourseTime[teacher_B[0].ToString()], teacher_B[0].ToString());
                item.Attributes.Add("style", "color:blue");
                d.Items.Add(item);
            }
            int count_Teacher = ds_teacher_A.Tables["defaultTable"].Rows.Count + ds_teacher_B.Tables["defaultTable"].Rows.Count;
            //干脆下标为count_Teacher的就作为默认待定项吧，如果数据库里面没有给这个班分配老师，就设置为待定
            ListItem default_item = new ListItem("待定", "-1");
            d.Items.Add(default_item);
            //d.Items.Insert(d.Items.Count, new ListItem( "待定","-1"));
            //debug1.Text = d.Items.Count.ToString();

            TableCell tc = (TableCell)TableCellList_class[i];
            /* if (Session[d.UniqueID] == null)
            {//不能使用UniqueID，这个是系统自动生成的，下一次页面刷新会重新赋予新的值！
                d.SelectedIndex = index;//如果o != null,那么不管怎么样总是会拿到一个索引值
            }
            else {
                d.SelectedIndex = (int)Session[d.UniqueID];
            }*/
            if (Session[d.ID] != null)
            {
                d.SelectedIndex = (int)Session[d.ID];
            }
            else
            {
                className = tc.Text;
                string grade = DropDownList_Grade.SelectedValue;
                string sql2 = "select class_id from temp_class where class_name = @ClassName and class_grade = @Grade";
                ParameterStruct p1 = new ParameterStruct("@ClassName", className);
                ParameterStruct p2 = new ParameterStruct("@Grade", grade);
                ArrayList plist1 = new ArrayList();
                plist1.Add(p1);
                plist1.Add(p2);
                Object o = dbm.ExecuteScalar(sql2, plist1);
                string classNo = o.ToString();
                // string sql1 = "select TeacherNo,TeacherName from temp_teacher where SubjectA ='" + subjectno + "'";     
                string sql3 = "select gcgLectureForm.TeacherNo from gcgLectureForm,temp_teacher where gcgLectureForm.TeacherNo = temp_teacher.TeacherNo and ClassNo = @class_id and CourseNo = @CourseNo";
                ParameterStruct p3 = new ParameterStruct("@class_id", classNo);
                ParameterStruct p4 = new ParameterStruct("@CourseNo", courseNo);
                ArrayList plist2 = new ArrayList();
                plist2.Add(p3);
                plist2.Add(p4);
                Object o1 = dbm.ExecuteScalar(sql3, plist2);
                int index = 0;
                if (o1 != null)
                {
                    string DefaultTeacherNo = o1.ToString();//拿到这个班这门课原先定下的老师的工号
                    //先检查这个老师的这门科目是不是擅长的                
                    bool isSubjectA = false;
                    DataTable ta = ds_teacher_A.Tables["defaultTable"];
                    for (index = 0; index < ta.Rows.Count; index++)
                    {
                        if (DefaultTeacherNo.Equals(ta.Rows[index]["TeacherNo"].ToString()))
                        {
                            isSubjectA = true;//是擅长的
                            break;
                        }
                    }
                    if (!isSubjectA)
                    {//否则检查是不是非擅长的
                        DataTable tb = ds_teacher_B.Tables["defaultTable"];
                        for (index = 0; index < tb.Rows.Count; index++)
                        {
                            if (DefaultTeacherNo.Equals(tb.Rows[index]["TeacherNo"].ToString()))
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {//如果这个班真的没有分配过老师
                    index = count_Teacher;
                }
                d.SelectedIndex = index;//如果o != null,那么不管怎么样总是会拿到一个索引值
                Session[d.ID] = index;
            }
        }

    }


    protected void Button_submit_Click(object sender, EventArgs e)
    {
        //点击确认按钮，写入数据库 
        //首先检查是否该年级该门学科所有班级都已经分配了老师，如果没有，则不允许插入，并弹窗提醒
        foreach (DropDownList d in DropDownList_select)
        {
            if (d.SelectedValue == "-1")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('尚未完成所有班级的教师安排！')", true);
                return;
            }
        }
        //  Response.Write("<script>alert('aaa')</script>");     
        string year = DropDownList_year.SelectedValue;
        string semester = DropDownList_semester.SelectedValue;
        string semesterNo = year + semester;
        //得到学期号
        DBManipulation dbm = new DBManipulation();
        string sql1 = "select class_id,class_name from temp_class where class_grade = '" + DropDownList_Grade.SelectedValue + "'";
        DataSet ds_class = dbm.ExecuteQueryOffLine(sql1, null);
        DataTable classTable = ds_class.Tables["defaultTable"];
        for (int i = 0; i < TableCellList_class.Count; i++)
        {
            TableCell tc = (TableCell)TableCellList_class[i];
            string className = tc.Text;
            string classNo = "";
            //查询班级编号
            foreach (DataRow r in classTable.Rows)
            {
                if (className.Equals(r["class_name"].ToString()))
                {
                    classNo = r["class_id"].ToString();
                    break;
                }
            }
            DropDownList d = (DropDownList)DropDownList_select[i];
            string TeacherNo = d.SelectedValue;
            /*
            if (TeacherNo.Equals("-1")) {//如果教师号为-1，说明还没有安排老师，不写入
                continue;
            }
            */
            //得到教师工号
            string courseNo = DropDownList_Course.SelectedValue;
            //得到学科号  

            /*主码，前2位代表年份，一位代表年级+学期（初一1，2 初二3，4 初三5，6），2位代表班级，2位代表学科*/
            string LectureNo_part1 = year.Substring(2);
            string LectureNo_part2 = "";
            switch (DropDownList_Grade.SelectedValue)
            {
                case "1":
                    {
                        if (DropDownList_semester.SelectedValue.Equals("01"))
                        {
                            LectureNo_part2 = "1";
                        }
                        else
                        {
                            LectureNo_part2 = "2";
                        }
                        break;
                    }
                case "2":
                    {
                        if (DropDownList_semester.SelectedValue.Equals("01"))
                        {
                            LectureNo_part2 = "3";
                        }
                        else
                        {
                            LectureNo_part2 = "4";
                        }
                        break;
                    }
                case "3":
                    {
                        if (DropDownList_semester.SelectedValue.Equals("01"))
                        {
                            LectureNo_part2 = "5";
                        }
                        else
                        {
                            LectureNo_part2 = "6";
                        }
                        break;
                    }
            }
            /*主码，前2位代表年份，一位代表年级+学期（初一1，2 初二3，4 初三5，6），2位代表班级，2位代表学科*/
            string LectureNo_part3 = classNo.Substring(2);
            string LectureNo_part4 = courseNo.Substring(1);
            string LectureNo = LectureNo_part1 + LectureNo_part2 + LectureNo_part3 + LectureNo_part4;
            //得到授课记录主码
            //查询gcgLectureForm看是不是已经记录了这条记录，如果存在，则删除
            string sql3 = "select * from gcgLectureForm where LectureNo ='" + LectureNo + "'";
            Object o = dbm.ExecuteScalar(sql3, null);
            if (o != null)
            {
                string sql4 = "delete from gcgLectureForm where LectureNo ='" + LectureNo + "'";
                dbm.ExecuteNonQuery(sql4, null);
            }
            /*create table gcgLectureForm(--授课安排表
	        LectureNo char(7) primary key,--授课安排编号
	        SemesterNo char(6),--学期号   201601
	        TeacherNo char(8),--教师工号
	        CourseNo char(3) ,--学科编号
	        ClassNo char(4)--班级编号
        )*/
            string sql2 = "insert into gcgLectureForm values(@LectureNo,@SemesterNo,@TeacherNo,@CourseNo,@ClassNo)";
            ParameterStruct p1 = new ParameterStruct("@LectureNo", LectureNo);
            ParameterStruct p2 = new ParameterStruct("@SemesterNo", semesterNo);
            ParameterStruct p3 = new ParameterStruct("@TeacherNo", TeacherNo);
            ParameterStruct p4 = new ParameterStruct("@CourseNo", courseNo);
            ParameterStruct p5 = new ParameterStruct("@ClassNo", classNo);
            ArrayList plist1 = new ArrayList();
            plist1.Add(p1);
            plist1.Add(p2);
            plist1.Add(p3);
            plist1.Add(p4);
            plist1.Add(p5);
            dbm.ExecuteNonQuery(sql2, plist1);
        }
        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('修改已完成！')", true);
    }
}