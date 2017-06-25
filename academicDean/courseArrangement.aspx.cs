using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class academicDean_courseArrangement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!(LoginAndPermissionChecking.LoginChecking()))
            {
                Response.Redirect("/ErrorPage/error_NotLogin.aspx");
            }
            if (!(LoginAndPermissionChecking.PermissionChecking(PermissionEnum.AcademicDean)))
            {
                Response.Redirect("/ErrorPage/error_DeniedPermission.aspx");
            }
            SetItemForDropDownList_Class();
        }
        // object o = Session["HasShownTable"];
        // if (o != null)
        //SetItemForDropDownList_Class();
        /*
        string select_class = "DropDownList_Class" + DropDownList_Grade.SelectedValue;
        if (Session[select_class] == null)
        {
            Session[select_class] = 0;
        }
        else
        {
          //  DropDownList_class.SelectedIndex = (int)Session[select_class];
        }*/
        ShowClassTable();
        // }
    }


    protected void CreateTheWholeSchedule()
    {
        string sql_courseNum = "select COUNT(*) from gcgCourse where Grade = '" + DropDownList_Grade.SelectedValue + "'";
        DBManipulation dbm = new DBManipulation();
        Object obj_countCourse = dbm.ExecuteScalar(sql_courseNum, null);
        if (obj_countCourse == null)
        {//这个年级还没有安排课程计划，无需接着往下了
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('没有安排学科！')", true);
            return;
        }
        int count_courseNum = int.Parse(obj_countCourse.ToString());
        if (DropDownList_class.Items.Count == 0)
        {
            //没有班级参与排课，无需接着往下了
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('没有班级参与排课！')", true);
            return;
        }
        string sql_checkTime = "select count(*) from gcgSchedule where Grade = " + DropDownList_Grade.SelectedValue;
        Object obj_time = dbm.ExecuteScalar(sql_checkTime, null);
        if (obj_time.ToString().Equals("0"))
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('尚未设置每节课的时间段！')", true);
            return;
        }
        // debug.Text = count_courseNum + "";

        Struct_CourseName[] courseList = new Struct_CourseName[count_courseNum];

        if (courseList == null)
        {
            Response.Redirect("1.aspx");
        }

        //准备科目名与学科号的映射,名字与年级有关，因为不同年级对于同一个科目的学科号是不一样的。,所以必须对应年级。
        string sql_MappingCourseNoAndCourseName = "select CourseNo,subjectname from gcgCourse,gcgSubject where gcgCourse.SubjectNo = gcgSubject.subjectno and Grade = '" + DropDownList_Grade.SelectedValue + "'";
        SqlDataReader dr00 = dbm.ExecuteQueryOnLine(sql_MappingCourseNoAndCourseName, null);
        Dictionary<string, string> dict_SubjectName_CourseNo = new Dictionary<string, string>();
        while (dr00.Read())
        {
            dict_SubjectName_CourseNo.Add(dr00.GetString(1), dr00.GetString(0));
            //因为年级一旦确定，科目名就与学科号 一 一 对应了
        }
        dr00.Close();
        string dict_SubjectName_CourseNo_Name = "dict_SubjectName_CourseNo" + DropDownList_Grade.SelectedValue;
        Cache[dict_SubjectName_CourseNo_Name] = dict_SubjectName_CourseNo;

        //准备学科号与学科名的映射     
        Dictionary<string, string> dict_CourseNo_SubjectName = new Dictionary<string, string>();
        SqlDataReader dr01 = dbm.ExecuteQueryOnLine(sql_MappingCourseNoAndCourseName, null);
        while (dr01.Read())
        {
            dict_CourseNo_SubjectName.Add(dr01.GetString(0), dr01.GetString(1));
        }
        dr01.Close();
        string dict_CourseNo_SubjectName_Name = "dict_CourseNo_SubjectName" + DropDownList_Grade.SelectedValue;
        Cache[dict_CourseNo_SubjectName_Name] = dict_CourseNo_SubjectName;

        //准备班级号与班级名的映射,查询授课安排表中的最新一个学期的某一个年级的班级号与其对应的班级名，肯定是 一 一 对应的。
        string sql_MappingClassNameAndClassNo = "select class_name,class_id from temp_class,(select distinct ClassNo from gcgLectureForm where SemesterNo = (select MAX(SemesterNo) from gcgLectureForm)) AS temp where class_id = ClassNo and class_grade = '" + DropDownList_Grade.SelectedValue + "'";
        SqlDataReader dr_MappingClassNameAndClassNo = dbm.ExecuteQueryOnLine(sql_MappingClassNameAndClassNo, null);
        Dictionary<string, string> dict_ClassName_ClassNo = new Dictionary<string, string>();
        while (dr_MappingClassNameAndClassNo.Read())
        {
            dict_ClassName_ClassNo.Add(dr_MappingClassNameAndClassNo.GetString(0), dr_MappingClassNameAndClassNo.GetString(1));
        }
        dr_MappingClassNameAndClassNo.Close();
        string dict_ClassName_ClassNo_Name = "dict_ClassName_ClassNo" + DropDownList_Grade.SelectedValue;
        Cache[dict_ClassName_ClassNo_Name] = dict_ClassName_ClassNo;


        string sql_fillCourseList = "select CourseNo,subjectname,CourseTime from gcgCourse,gcgSubject where gcgCourse.SubjectNo = gcgSubject.subjectno and Grade = '" + DropDownList_Grade.SelectedValue + "'";
        SqlDataReader dr1 = dbm.ExecuteQueryOnLine(sql_fillCourseList, null);
        if (dr1 == null)
        {
            Response.Redirect("1.aspx");
        }
        for (int i = 0; i < courseList.Length && dr1.Read(); i++)
        {
            courseList[i] = new Struct_CourseName();
            courseList[i].CourseNo1 = dr1.GetString(0);
            courseList[i].SubjectName1 = dr1.GetString(1);
            Byte b = dr1.GetByte(2);
            courseList[i].Hour1 = (int)b;
        }
        dr1.Close();
        dbm.Close();
        string sql_countClassNum = "select COUNT(*) from( select distinct ClassNo from gcgLectureForm, temp_class where gcgLectureForm.ClassNo = temp_class.class_id and class_grade = @Grade) AS temp ";
        //为什么上面这一句这么复杂，是为了防止有些班级还没有参与排课计划
        ParameterStruct p_grade = new ParameterStruct("@Grade", DropDownList_Grade.SelectedValue);
        ArrayList plist1 = new ArrayList();
        plist1.Add(p_grade);
        Object obj_countClassNum = dbm.ExecuteScalar(sql_countClassNum, plist1);
        if (obj_countClassNum == null)
        {
            return;
        }
        int countClassNum = int.Parse(obj_countClassNum.ToString());
        string sql_LectureForm_orderByClassNoAndCourseNo = "select ClassNo,gcgLectureForm.CourseNo,gcgLectureForm.TeacherNo,temp_teacher.TeacherName from gcgLectureForm, gcgCourse, gcgSubject, temp_teacher where gcgCourse.CourseNo = gcgLectureForm.CourseNo and gcgCourse.SubjectNo = gcgSubject.subjectno and temp_teacher.TeacherNo = gcgLectureForm.TeacherNo and Grade = @Grade order by gcgLectureForm.ClassNo,gcgLectureForm.CourseNo";
        SqlDataReader dr2 = dbm.ExecuteQueryOnLine(sql_LectureForm_orderByClassNoAndCourseNo, plist1);
        ArrayList plist2 = new ArrayList();
        TeaInfo[,] teacherArrange = new TeaInfo[countClassNum, count_courseNum];
        for (int i = 0; i < countClassNum; i++)
        {
            for (int j = 0; j < count_courseNum; j++)
            {
                teacherArrange[i, j] = new TeaInfo();
                if (!dr2.HasRows)
                {
                    break;
                }
                else
                {
                    dr2.Read();
                }
                try
                {
                    teacherArrange[i, j].No = dr2.GetString(2);//教师工号
                    teacherArrange[i, j].Name = dr2.GetString(3);//教师名
                    teacherArrange[i, j].Courseno = dr2.GetString(1);//学科号，可能是擅长的，可能是非擅长的
                }
                catch (Exception)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('数据不够，请检查是否已经安排所有班级的老师！')", true);
                    return;
                }
            }
            if (!dr2.HasRows)
            {
                break;
            }
        }
        dr2.Close();
        dbm.Close();

        AutoSchedule auto = new AutoSchedule();
        Cache["auto"] = auto;
        auto.CourseList = courseList;
        auto.TeacherArrange = teacherArrange;
        auto.MakingSchedule();
        Tea[,] Lessontable = auto.func();//注意：这个课表的行是一个个班级，列是这个班级一周的额全部课程分布，都是从1开始的
        string lessonTableName = "LessonTable" + DropDownList_Grade.SelectedValue;
        Cache[lessonTableName] = Lessontable;//使用ViewState保存，生命周期为该页面，只要这个页面没有关闭就存在 
        if (Lessontable == null)
        {
            Response.Redirect("1.aspx");
        }
        ShowClassTable();

    }

    //点击某一个按钮，激发可选择的替换项目    
    protected void Button_ShowOrChange(object sender, EventArgs e)
    {

        MyButton b = (MyButton)sender;
        //获得该班所有课的按钮映射
        //全部使用双击操作，双击选中待交换课时，再双击选中某一个可以交换的课时
        //因为我仅仅对点击黑色（未选中）按钮和蓝色按钮有反应，所以当一个按钮不论是在什么时候变红，都不会对下面操作有影响

        //  if ((string)Session[b.IDForThisButton] == "courseButton")//如果这个按钮是黑色
        if ((string)Session[b.IDForThisButton] == "aspTableCell")
        {//执行查询可交换选项
            ArrayList ChoseButtonIDList = null;
            if (Session["ChoseButtonIDList"] != null)
            {
                ChoseButtonIDList = (ArrayList)Session["ChoseButtonIDList"];
                for (int i = 0; i < ChoseButtonIDList.Count; i++)
                {
                    string buttonID = (string)ChoseButtonIDList[i];
                    //   Session[buttonID] = "courseButton";//把原先红色的恢复成黑色
                    Session[buttonID] = "aspTableCell";//把原先红色的恢复成黑色
                }
                ChoseButtonIDList.Clear();
            }
            else
            {
                ChoseButtonIDList = new ArrayList();//如果没有，说明是第一次点击按钮，则新建一个
            }

            /* //对于ans课表i班j节课 所能无冲突调换的 点
               public List<int> givepoint(int i, int j)*/
            //ArrayList ChoseButtonIDList = new ArrayList();
            //思路，点击一个按钮，如果这个按钮是黑色的，那么将先前的变红色的按钮恢复成黑色
            //调用函数，找出可替换位置，将这个按钮连同新的可替换位置放入一个记录中（存放这些按钮的ID）

            Session["WaittingChangeButton"] = b;//记录是谁要换位置
            //Session[b.IDForThisButton] = "nameRedButton";//切换状态
            ChoseButtonIDList.Add(b.IDForThisButton);

            string classNo = b.Class_id;
            string className = classNo.Substring(2);
            int classnameInt;
            if (className.StartsWith("0"))
            {
                classnameInt = int.Parse(className.Substring(1));
            }
            else
            {
                classnameInt = int.Parse(className);
            }
            //得到第一个参数
            AutoSchedule auto = (AutoSchedule)Cache["auto"];
            int lessonNo = (b.Day - 1) * 8 + b.Lesson;
            List<int> choseableItem = auto.givepoint(classnameInt, lessonNo);
            //给出一个序列，里面存放了某一个班的可以用来与选定课程交换的节次序号，1~8为周一的第1~8节课，9为周二第一节

            foreach (int index in choseableItem)
            {
                int day = (index - 1) / 8;
                int lesson = (index - 1) % 8;

                string IDForAButton = "Button_" + DropDownList_Grade.SelectedValue + DropDownList_class.SelectedValue + (day + 1) + "" + (lesson + 1);
                ChoseButtonIDList.Add(IDForAButton);
            }
            foreach (string name in ChoseButtonIDList)
            {
                //   Session[name] = "courseButton blue";
                Session[name] = "aspTableCellBrown";
            }
            //Session[b.IDForThisButton] = "courseButton red";
            Session[b.IDForThisButton] = "aspTableCellGreen";
            Session["ChoseButtonIDList"] = ChoseButtonIDList;//用Session保存颜色变化的按钮
            ShowClassTable();
            return;
        }
        else
        {
            //if ((string)Session[b.IDForThisButton] == "courseButton blue")
            if ((string)Session[b.IDForThisButton] == "aspTableCellBrown")
            {

                MyButton a = (MyButton)Session["WaittingChangeButton"];//获得上一次点击的对象

                string classNo = b.Class_id;
                string className = classNo.Substring(2);
                int classnameInt;
                if (className.StartsWith("0"))
                {
                    classnameInt = int.Parse(className.Substring(1));
                }
                else
                {
                    classnameInt = int.Parse(className);
                }
                //得到第一个参数
                AutoSchedule auto = (AutoSchedule)Cache["auto"];
                int lessonNo_a = (b.Day - 1) * 8 + b.Lesson;
                int lessonNo_b = (a.Day - 1) * 8 + a.Lesson;
                auto.changeclass(classnameInt, lessonNo_a, lessonNo_b);
                Tea[,] Lessontable = auto.func();
                string lessonTableName = "LessonTable" + DropDownList_Grade.SelectedValue;
                Cache[lessonTableName] = Lessontable;//更新表 
                ArrayList al = (ArrayList)Session["ChoseButtonIDList"];//获得所有变红色的格子
                for (int i = 0; i < al.Count; i++)
                {
                    string buttonID = (string)al[i];
                    //Session[buttonID] = "courseButton";//把原先红色的恢复成黑色
                    Session[buttonID] = "aspTableCell";//把原先红色的恢复成黑色
                }
                ShowClassTable();
            }
        }

    }

    protected void SetItemForDropDownList_Class()
    {
        string sql = "select distinct ClassNo,class_name from gcgLectureForm, temp_class where gcgLectureForm.ClassNo = temp_class.class_id and class_grade = '" + DropDownList_Grade.SelectedValue + "'";
        DBManipulation dbm = new DBManipulation();
        DropDownList_class.Items.Clear();
        /*
        DataSet ds = dbm.ExecuteQueryOffLine(sql, null);
        DropDownList_class.AutoPostBack = true;
        DropDownList_class.DataSource = ds.Tables["defaultTable"];
        DropDownList_class.DataTextField = ds.Tables["defaultTable"].Columns[1].ColumnName;
        DropDownList_class.DataValueField = ds.Tables["defaultTable"].Columns[0].ColumnName;
        DropDownList_class.DataBind();
        */
        SqlDataReader dr = dbm.ExecuteQueryOnLine(sql, null);
        while (dr.Read())
        {
            ListItem item = new ListItem(dr.GetString(1), dr.GetString(0));
            DropDownList_class.Items.Add(item);
        }
        dr.Close();
        dbm.Close();
    }
    protected void DropDownList_class_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*
        DropDownList d = (DropDownList)sender;
        string select_class = "DropDownList_Class" + DropDownList_Grade.SelectedValue;
        Session[select_class] = d.SelectedIndex;
        */
        ShowClassTable();
    }

    protected void ShowClassTable()
    {
        Session["HasShownTable"] = true;
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
        t0.CssClass = "aspTableCell";
        t1.CssClass = "aspTableCell";
        t2.CssClass = "aspTableCell";
        t3.CssClass = "aspTableCell";
        t4.CssClass = "aspTableCell";
        t5.CssClass = "aspTableCell";

        head.Controls.Add(t0);
        head.Controls.Add(t1);
        head.Controls.Add(t2);
        head.Controls.Add(t3);
        head.Controls.Add(t4);
        head.Controls.Add(t5);
        head.CssClass = "aspTableRow1";
        Table1.Controls.Add(head);
        bool hasTable = true;
        string lessonTableName = "LessonTable" + DropDownList_Grade.SelectedValue;
        if (Cache[lessonTableName] == null)
        {
            hasTable = false;
            //return;
        }
        string dict_CourseNo_SubjectName_Name = "dict_CourseNo_SubjectName" + DropDownList_Grade.SelectedValue;
        if (Cache[dict_CourseNo_SubjectName_Name] == null)
        {
            hasTable = false;
            //return;
        }
        string dict_SubjectName_CourseNo_Name = "dict_SubjectName_CourseNo" + DropDownList_Grade.SelectedValue;
        if (Cache[dict_SubjectName_CourseNo_Name] == null)
        {
            hasTable = false;
            //return;
        }
        string dict_ClassName_ClassNo_Name = "dict_ClassName_ClassNo" + DropDownList_Grade.SelectedValue;
        if (Cache[dict_ClassName_ClassNo_Name] == null)
        {
            hasTable = false;
            //return;
        }

        //  else
        // {
        Dictionary<string, string> dict_CourseNo_SubjectName = null;
        Dictionary<string, string> dict_SubjectName_CourseNo = null;
        Dictionary<string, string> dict_ClassName_ClassNo = null;
        Tea[,] Lessontable = null;
        Tea[] class_1 = null;
        if (hasTable)
        {
            if (DropDownList_class.Items.Count == 0)
            {
                return;
                //这一句是为了防止出现：选择某一个年级，由于没有班级，然后跳回有班级的年级之后无法显示课表
                //因为页面重载后，DropDownList_class的项都清空了，此时由于showTable()会在Page_load里面调用，而且
                //Cache里面存放了这几个映射和课表，所以会尝试显示课表，但是由于DropDownList_class的项是在后面
                //对年级切换的事件处理中才设定的，所以第一次执行Page_load会执行showTable函数，此时DropDownList_class
                //没有选项，所以出现异常！因此因为页面重载导致的showTable要判断这种情况

            }
            dict_CourseNo_SubjectName = (Dictionary<string, string>)Cache[dict_CourseNo_SubjectName_Name];
            dict_SubjectName_CourseNo = (Dictionary<string, string>)Cache[dict_SubjectName_CourseNo_Name];
            dict_ClassName_ClassNo = (Dictionary<string, string>)Cache[dict_ClassName_ClassNo_Name];
            Lessontable = (Tea[,])Cache[lessonTableName];
            class_1 = new Tea[Lessontable.GetLength(1) + 1];
            string className = DropDownList_class.SelectedValue.ToString().Substring(2);
            int classInt;
            if (className.StartsWith("0"))
            {
                classInt = int.Parse(className.Substring(1));
            }
            else
            {
                classInt = int.Parse(className);
            }
            for (int t = 1; t <= 40; t++)//从1开始
            {
                class_1[t] = Lessontable[classInt, t];
            }
        }
        //Dictionary<string, string> dict_CourseNo_SubjectName = (Dictionary<string, string>)Cache[dict_CourseNo_SubjectName_Name];
        //Dictionary<string, string> dict_SubjectName_CourseNo = (Dictionary<string, string>)Cache[dict_SubjectName_CourseNo_Name];
        //Dictionary<string, string> dict_ClassName_ClassNo = (Dictionary<string, string>)Cache[dict_ClassName_ClassNo_Name];

        //Tea[]            
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
                if (hasTable)
                {
                    MyButton button = new MyButton();
                    //  if (class_1[lesson + (day - 1) * 8] == null) {

                    // }
                    button.TeacherNo1 = class_1[lesson + (day - 1) * 8].id;//周四：day = 4,节次号为前三天的总次数加上这一天的节次号，3 * 8 + lesson+1
                    button.SubjectName1 = dict_CourseNo_SubjectName[class_1[lesson + (day - 1) * 8].cno];
                    //button.CourseNo1 = dict_SubjectName_CourseNo[button.SubjectName1];
                    button.CourseNo1 = class_1[lesson + (day - 1) * 8].cno;
                    button.Class_id = dict_ClassName_ClassNo[DropDownList_class.SelectedItem.Text];
                    button.Day = day;//day = 1,代表星期一，j = 1代表星期二
                    button.Lesson = lesson;
                    button.IDForThisButton = "Button_" + DropDownList_Grade.SelectedValue + DropDownList_class.SelectedValue + day + lesson;
                    //一个button的ID号是这样组成的:年级 + 班级号 + 星期几 + 第几节 （如初一1603班周三第1节：Button_1160331）
                    //button.Text = button.SubjectName1 + "\n\r" + "aaa";
                    button.Text = button.SubjectName1;
                    if (Session[button.IDForThisButton] == null)//初始化时设定每一个按钮的颜色
                    {
                        //     Session[button.IDForThisButton] = "courseButton";
                        Session[button.IDForThisButton] = "aspTableCell";
                    }
                    button.Click += new EventHandler(Button_ShowOrChange);
                    //button.CssClass = Session[button.IDForThisButton].ToString();//如果刷新之前已经有记录，那么直接用之前的样式           
                    button.CssClass = "courseButton";
                    //  button.CssClass = (string)Session[button.IDForThisButton];
                    //cell.CssClass = "aspTableCell";//具体安排
                    cell.CssClass = (string)Session[button.IDForThisButton];
                    cell.Controls.Add(button);
                }
                else
                {
                    cell.CssClass = "aspTableCell";
                }
                tablerow.Controls.Add(cell);
            }
            Table1.Controls.Add(tablerow);
        }
        // }
    }

    protected void DropDownList_Grade_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Response.Write("<script>alert('nihao')</script>");
        SetItemForDropDownList_Class();//设置新的年级的班级集合
        ShowClassTable();
    }

    protected void Button_CreateSchedule_Click(object sender, EventArgs e)
    {
        CreateTheWholeSchedule();
    }

    protected void Button_SubmitSchedule_Click(object sender, EventArgs e)
    {
        if (Cache["auto"] == null)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('请先生成课表。')", true);
            //Response.Write("<script>alert('请先生成课表。')</script>");
            return;
        }
        string lessonTableName = "LessonTable" + DropDownList_Grade.SelectedValue;
        if (Cache[lessonTableName] == null)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('请先生成课表。')", true);
            return;
        }
        string dict_CourseNo_SubjectName_Name = "dict_CourseNo_SubjectName" + DropDownList_Grade.SelectedValue;
        if (Cache[dict_CourseNo_SubjectName_Name] == null)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('请先生成课表。')", true);
            return;
        }
        string dict_SubjectName_CourseNo_Name = "dict_SubjectName_CourseNo" + DropDownList_Grade.SelectedValue;
        if (Cache[dict_SubjectName_CourseNo_Name] == null)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('请先生成课表。')", true);
            return;
        }
        string dict_ClassName_ClassNo_Name = "dict_ClassName_ClassNo" + DropDownList_Grade.SelectedValue;
        if (Cache[dict_ClassName_ClassNo_Name] == null)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('请先生成课表。')", true);
            return;
        }

        Dictionary<string, string> dict_SubjectName_CourseNo = (Dictionary<string, string>)Cache[dict_SubjectName_CourseNo_Name];
        Dictionary<string, string> dict_CourseNo_SubjectName = (Dictionary<string, string>)Cache[dict_CourseNo_SubjectName_Name];
        Dictionary<string, string> dict_ClassName_ClassNo = (Dictionary<string, string>)Cache[dict_ClassName_ClassNo_Name];
        Tea[,] Lessontable = (Tea[,])Cache[lessonTableName];
        //以上已获得某一个年级的全部课表，老师号-科目名映射，学科号-科目名映射，班级名和班级号映射
        /*
        *   create table gcgLectureForm(--授课安排表
	            LectureNo char(7) primary key,--授课安排编号
	            SemesterNo char(6),--学期号，蕴含年份和上下学期信息
	            TeacherNo char(8),--教师工号
	            CourseNo char(3) foreign key references gcgcourse(CourseNo),--学科编号
	            ClassNo char(4)--班级编号
            )
            create table gcgLesson(--课表
	            LectureNo char(7) foreign key references gcgLectureForm(LectureNo),--授课安排编号
	            --ClassRoomNo char(*)
	            Timeid char(4) foreign key references gcgSchedule(Timeid),--时间段编号
	            primary key(LectureNo,Timeid)--表级主码
            )
         */
        string sql_forSemester = "select MAX(SemesterNo) from gcgLectureForm ";
        DBManipulation dbm = new DBManipulation();
        object o = dbm.ExecuteScalar(sql_forSemester, null);
        string semesterNo = o.ToString();
        string grade = DropDownList_Grade.SelectedValue;
        string sql_deleteTheSameSemesterRecord = "delete from gcgLesson where LectureNo in ( select LectureNo from gcgLectureForm,temp_class where gcgLectureForm.ClassNo = temp_class.class_id and class_grade = @Grade and SemesterNo = @SemesterNo)";
        ParameterStruct p_grade = new ParameterStruct("@Grade", grade);
        ParameterStruct p_seme = new ParameterStruct("@SemesterNo", semesterNo);
        ArrayList palist = new ArrayList();
        palist.Add(p_grade);
        palist.Add(p_seme);

        dbm.ExecuteNonQuery(sql_deleteTheSameSemesterRecord, palist);

        /*string sql_MappingClassNameAndClassNo = 
         * "    select class_name,class_id 
         *      from temp_class,(
         *          select distinct ClassNo 
         *              from gcgLectureForm 
         *                  where SemesterNo = (
         *                      select MAX(SemesterNo) 
         *                          from gcgLectureForm)) AS temp 
         *      where class_id = ClassNo and class_grade = '" + DropDownList_Grade.SelectedValue + "'";
         */
        string sql_forClassNo = "select class_id from temp_class,(select distinct ClassNo from gcgLectureForm where SemesterNo = (select MAX(SemesterNo) from gcgLectureForm)) AS temp where class_id = ClassNo and class_grade = '" + DropDownList_Grade.SelectedValue + "'";
        ArrayList classlist = new ArrayList();
        SqlDataReader dr = dbm.ExecuteQueryOnLine(sql_forClassNo, null);
        while (dr.Read())
        {
            classlist.Add(dr.GetString(0));
        }
        dr.Close();
        for (int i = 0; i < classlist.Count; i++)
        {//一个一个班的写入
            //[,]Lessontable一行就存放了一个班的所有记录
            Tea[] classTable = new Tea[Lessontable.GetLength(1) + 1];
            string[] courseTable = new string[Lessontable.GetLength(1) + 1];
            for (int t = 0; t < 40; t++)
            {
                classTable[t] = Lessontable[i + 1, t + 1];//取到某一个班40节课的所有教师工号
                string subjectname = dict_CourseNo_SubjectName[classTable[t].cno];
                courseTable[t] = dict_SubjectName_CourseNo[subjectname];//取到这四十节课的学科号
            }
            for (int t = 0; t < 40; t++)
            {
                //classlist[i-1]就是对应的班级号
                string sql_forLectureID = "select LectureNo from gcgLectureForm where SemesterNo = @SemesterNo and ClassNo = @ClassNo and CourseNo = @CourseNo";
                ParameterStruct p_semester = new ParameterStruct("@SemesterNo", semesterNo);
                ParameterStruct p_classNo = new ParameterStruct("@ClassNo", (string)classlist[i]);//第i个班级的班级号
                ParameterStruct p_courseNo = new ParameterStruct("@CourseNo", courseTable[t]);
                ArrayList plist = new ArrayList();
                plist.Add(p_semester); plist.Add(p_classNo); plist.Add(p_courseNo);
                Object lectureID = dbm.ExecuteScalar(sql_forLectureID, plist);
                //string Timeid1 = grade + week + "0" + "1";
                string Time_part1 = DropDownList_Grade.SelectedValue;
                int week = 0;
                string Time_part3;
                int temp = t + 1;
                if (temp % 8 == 0)
                {
                    week = temp / 8;
                    Time_part3 = "0" + 8;
                }
                else
                {
                    week = temp / 8 + 1;
                    Time_part3 = "0" + (temp - (week - 1) * 8);
                }
                string Time_part2 = week + "";

                // string Time_part3 = "0" + (t - (t / 8) * 8);              

                string sql_insert = "insert into gcgLesson values(@lectureID,@TimeID)";
                ParameterStruct p_LectureID = new ParameterStruct("@lectureID", lectureID.ToString());
                ParameterStruct p_timeID = new ParameterStruct("@TimeID", Time_part1 + Time_part2 + Time_part3);//第i个班级的班级号
                ArrayList plist1 = new ArrayList();
                plist1.Add(p_LectureID);
                plist1.Add(p_timeID);
                dbm.ExecuteNonQuery(sql_insert, plist1);
            }
        }
        dbm.Close();
        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('课表已保存！')", true);
    }

}