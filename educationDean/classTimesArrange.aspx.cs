using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class educationDean_classTimesArrange : System.Web.UI.Page
{
    //private bool HasNewCourse = false;//判断是否增加了科目
    //private bool HasNewCourse ;
    private string NewCourseName;
    private string NewCourseTime;
    //注意这里不能赋初始值！！！！否则HasNewCourse一直都是初始值，但是为什么呢？因为每一次激发时间都会导致页面回传，服务器会直接生成一个新的Page对象
    //原先的page对象的数据会全部丢失
    //可以选择使用static静态字段，就不会被初始化了,不过还是用Session吧，注意session有空间大小限制，太大了会导致字节流中断错误
    private ArrayList DropDownLists = new ArrayList();
    private ArrayList TableCellLists = new ArrayList();
    private ArrayList ButtonLists = new ArrayList();

    protected void Page_Load(object sender, EventArgs e){
        //页面载入之前，先进行登录检查以及权限检查
       
        ScriptManager1.RegisterAsyncPostBackControl(this.DropDownList_Grade);
       // ScriptManager1.RegisterAsyncPostBackControl(this.lessLessonTime);
        if (!IsPostBack) {// 如果是为响应客户端回发而加载该页，则为 true；否则为 false。          
            if (!(LoginAndPermissionChecking.LoginChecking())) {
                Response.Redirect("/ErrorPage/error_NotLogin.aspx");
            }
            if (!(LoginAndPermissionChecking.PermissionChecking(PermissionEnum.EducationDean))) {
                Response.Redirect("/ErrorPage/error_DeniedPermission.aspx");
            }            
            ListItem i1 = new ListItem(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + "初一", "1");
            ListItem i2 = new ListItem(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + "初二", "2");
            ListItem i3 = new ListItem(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + "初三", "3");
            DropDownList_Grade.Items.Add(i1);//这里这么做完全是为了显示的时候可以居中，没别的意思
            DropDownList_Grade.Items.Add(i2);
            DropDownList_Grade.Items.Add(i3);
            
            SetItemForChoosableCourse();                                    
        }
        Page.MaintainScrollPositionOnPostBack = true;//刷新后滚动条回到之前的位置,但是会导致页面闪烁        
                                                     // Page.MaintainScrollPositionOnPostBack = false;//刷新后滚动条回到顶部     
        SelectCourseByGrade();//一开始显示默认的年级的科目      
    }

    
    public void SetItemForChoosableCourse()
    {
        DataSet ds = getItems_unChosedCourse();
        DataTable t = ds.Tables["defaultTable"];
        DropDownList2.Items.Clear();
        foreach (DataRow r in t.Rows) {
            //select subjectno,subjectname from gcgSubject where subjectno not in(select SubjectNo from gcgCourse where Grade = @Grade)";
            string subjectNo = r["subjectno"].ToString();
            string subjectName = r["subjectname"].ToString();
            //  ListItem i1 = new ListItem(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + "初一", "1");
            ListItem item = new ListItem(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")+subjectName, subjectNo);
            DropDownList2.Items.Add(item);
        }
        /*
        DropDownList2.DataSource = ds.Tables["defaultTable"];
        DropDownList2.DataTextField = ds.Tables["defaultTable"].Columns[1].ColumnName;
        DropDownList2.DataValueField = ds.Tables["defaultTable"].Columns[0].ColumnName;
        DropDownList2.DataBind();
        DBManipulation dbm = new DBManipulation();
        */
    }

    protected void DropDownList_Grade_TextChanged(object sender, EventArgs e)
    {
        SelectCourseByGrade();
    }

    protected void DropDownList_CourseCount_TextChanged(object sender, EventArgs e)
    {
        CheckLectureHour();
    }

    protected void CheckLectureHour() {
        int courseTimeCount = 0;//计算当前已经设定的课程的分配的课时之和
        // DropDownList_academicYear.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["academicYear"].ToString();
        string key = "";        
        switch (DropDownList_Grade.SelectedValue)
        {
            case "1": key = "TotalCourseTimeOfGradeOne"; break;
            case "2": key = "TotalCourseTimeOfGradeTwo"; break;
            case "3": key = "TotalCourseTimeOfGradeThree"; break;
        }     
        int MaxCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings[key].ToString());
        for (int i = 0; i < DropDownLists.Count; i++)
        {
            DropDownList dropDownList = (DropDownList)(DropDownLists[i]);           
            courseTimeCount += int.Parse(dropDownList.SelectedValue);
        }
        lessLessonTime.Text = lessHour() + "";//更新最新课时剩余结果       
        if (courseTimeCount > MaxCount)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('已经超过总课时数！')", true);
            //这行代码比较好，可以防止由于后台向前台直接用Response输出alert弹窗使得css失效
        }
    }
    //先对事件做出反应，再page_load
    public int lessHour() {
        int courseTimeCount = 0;
        for (int i = 0; i < DropDownLists.Count; i++)
        {
            DropDownList dropDownList = (DropDownList)(DropDownLists[i]);
            courseTimeCount += int.Parse(dropDownList.SelectedValue);
        }
        return 40 - courseTimeCount;
    }

    private void SelectCourseByGrade() {
        Table1.Controls.Clear();//把原先的表全部清干净,注意动态生成的时候绝对要把全部的行都清掉，否则会出现玄学，仔细检查Rows.count

        TableCellLists.Clear();//把保存的上一次的控件引用清理掉，不需要了，这次会生成一个新的表
        DropDownLists.Clear();
        string grade = DropDownList_Grade.SelectedValue;
        //重新设置表头
        TableRow tableRowHeader = new TableRow();
        tableRowHeader.CssClass = "aspTableRow1";
        TableCell tablecell_courseNameHeader = new TableCell();
        tablecell_courseNameHeader.Text = "科目";

        //td.Attributes.Add("onclick", "javascript:alert('" + day + "');");
        //tablecell_courseNameHeader.Attributes.Add("onclick", "javascript:alert('1111111111')");
        //这段代码可以给动态生成的TableCell加事件，用来捕捉用户点击

        tablecell_courseNameHeader.CssClass = "aspTableCell";    
        TableCell tablecell_courseCountHeader = new TableCell();
        tablecell_courseCountHeader.Text = "课时";
        tablecell_courseCountHeader.CssClass = "aspTableCell";
        TableCell tablecell_deleteButtonHeader = new TableCell();
        tablecell_deleteButtonHeader.Text = "操作";
        tablecell_deleteButtonHeader.CssClass = "aspTableCell";
        tableRowHeader.Controls.Add(tablecell_courseNameHeader);
        tableRowHeader.Controls.Add(tablecell_courseCountHeader);
        tableRowHeader.Controls.Add(tablecell_deleteButtonHeader);
        Table1.Controls.Add(tableRowHeader);
        //根据年级查该年级的所有教学科目
        string sql = "select CourseNo,subjectname,CourseTime  from gcgCourse, gcgSubject where gcgCourse.SubjectNo = gcgSubject.subjectno and Grade = @Grade";
        ParameterStruct p = new ParameterStruct("@Grade", grade);
        ArrayList parameterList = new ArrayList();
        parameterList.Add(p);
        DBManipulation dbm = new DBManipulation();
        SqlDataReader dataReader = dbm.ExecuteQueryOnLine(sql, parameterList);
        int count = 0;
        //循环增加行
        while (dataReader.Read())
        {
            //准备一个行对象，和三个单元格对象
            TableRow tableRow = new TableRow();
            TableCell tablecell_courseName = new TableCell();
            TableCell tablecell_courseCount = new TableCell();
            TableCell tablecell_deleteButton = new TableCell();
            //第一个单元格存放科目名
            tablecell_courseName.ID = "TableCell_CourseName_" + count;
            tablecell_courseName.Text = dataReader.GetString(1);
            tablecell_courseName.CssClass = "aspTableCell";
            TableCellLists.Add(tablecell_courseName);//保存引用
            //第二个单元格存放课时下拉框，记得添加事件处理
            tablecell_courseCount.ID = "TableCell_CourseCount_" + count;
            tablecell_courseCount.CssClass = "aspTableCell";
            DropDownList dropDownList = new DropDownList();
            dropDownList.ID = "DropDownCell_" + count;
            dropDownList.SelectedIndexChanged += DropDownList_CourseCount_TextChanged;//监听事件
            dropDownList.AutoPostBack = true;
            ListItem[] items = new ListItem[9];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new ListItem();
                // string str = "     " + i;
                //ListItem.Text = HttpUtility.HtmlDecode("&nbsp;&nbsp;")+">>SubItem1"; 
                items[i].Text = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + i +"";
                //items[i].Text = str;
                items[i].Value = +i + "";
                dropDownList.Items.Add(items[i]);
            }           
            int CourseTime = dataReader.GetByte(2);//注意我们的CourseTime字段是tinyint，是Byte类型，不能用GetString()
            dropDownList.SelectedIndex = CourseTime;//下拉框的初始值                        
            this.DropDownLists.Add(dropDownList);//保存引用，方便其他函数使用
            tablecell_courseCount.Controls.Add(dropDownList);
            //第三个单元格存放删除按钮
            tablecell_deleteButton.ID = "TableButton_" + count;
            tablecell_deleteButton.CssClass = "aspTableCell";
            Button button = new Button();
            button.Text = "删除";
            //button.CommandArgument = button.ID;
            button.CssClass = "button white bigrounded";
            button.ID = "Button_" + count;
            button.Click += ButtonDelete_Click;
            ButtonLists.Add(button);
            tablecell_deleteButton.Controls.Add(button);
            //将三个单元格放入行对象内
            tableRow.ID = "TableRow_" + count;
            tableRow.CssClass = "aspTableRow2";
            tableRow.Controls.Add(tablecell_courseName);
            tableRow.Controls.Add(tablecell_courseCount);
            tableRow.Controls.Add(tablecell_deleteButton);
            //将这个行放入表中
            Table1.Controls.Add(tableRow);
            count++;
        }
        dataReader.Close();
        dbm.Close();       
        lessLessonTime.Text = lessHour() + "";//每次生成页面都计算一下剩余课时
    } 

    protected void Button_submit_Click(object sender, EventArgs e)
    {   //点击确认，提交课时安排
        //通过 TableCellLists  DropDownLists获得每一条记录的科目名以及课时，通过DropDownList_Grade获得年级
        //步骤：一条一条来看，如果原先有课时安排的，就更新，没有的，就插入        
        string grade = DropDownList_Grade.SelectedValue;                                             
        DBManipulation dbm = new DBManipulation();
       // Response.Write("<script>alert('有新"+ HasNewCourse+" ')</script>");
        for (int i = 0; i < DropDownLists.Count; i++) {
            //步骤一：查询这门科目是否有过旧的课时安排记录
            //注意：一开始没有学科，所以要通过按钮增加学科，此时会保证所有记录都有初始课时分布，所以提交按钮仅仅需要将这些记录更新就行了
            TableCell tc = (TableCell)TableCellLists[i];
            DropDownList ddl = (DropDownList)DropDownLists[i];
            string courseName = tc.Text;//获得科目名
            string courseCount = ddl.SelectedValue;//获得新的分配课时
            
            string SearchSql = "select CourseNo from gcgCourse,gcgSubject where gcgSubject.subjectno = gcgCourse.SubjectNo and Grade = @Grade and subjectname = @subjectname";
            ParameterStruct p1 = new ParameterStruct("@Grade", grade);
            ParameterStruct p2 = new ParameterStruct("@subjectname", courseName);
            ArrayList parameterList1 = new ArrayList();
            parameterList1.Add(p1);
            parameterList1.Add(p2);         
            Object o = dbm.ExecuteScalar(SearchSql, parameterList1);         
            string CourseNo = o.ToString();
            string sql4 = "update gcgCourse set CourseTime = "+ courseCount +" where CourseNo = " + CourseNo;
            dbm.ExecuteNonQuery(sql4, null);                
        }
        dbm.Close();        
        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('提交已完成！')", true);
    }

    protected DataSet getItems_unChosedCourse()
    {   //点击该按钮增加科目，从我们的确认按钮来看。确认按钮已经考虑到了新增加学科的情况
        //所以该按钮只需要提供增加现有科目中这个年级还没有上的学科，可选的科目选项从未选择的科目中取
        //步骤：查询当前年级开设的学科的科目，查询该校总共开设的科目，两者做一个差集即可
        //比如说：该校总共开设：语、数、英、物、化、生、政、史、地
        //但是初一以前只开设语、数、英、生、史、地
        //那么两者的差集就是：物、化、政
        //那么就可以从这三门可中选择
        /*  select * 
            from gcgSubject
            where subjectno not in (
	            select SubjectNo 
	            from gcgCourse
	            where Grade = '1' )
        */
        string sqlForChoosableCourse = "select subjectno,subjectname from gcgSubject where subjectno not in(select SubjectNo from gcgCourse where Grade = @Grade)";
        DBManipulation dbm = new DBManipulation();
        ParameterStruct p = new ParameterStruct("@Grade", DropDownList_Grade.SelectedValue);
        ArrayList parameterLists = new ArrayList();
        parameterLists.Add(p);        
        DataSet ds = dbm.ExecuteQueryOffLine(sqlForChoosableCourse, parameterLists);
        dbm.Close();
        return ds;       
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //点击增加科目
        //从年级已知，科目号和科目名已知，默认课时为0
        string grade = DropDownList_Grade.SelectedValue;
        string subjectno = DropDownList2.SelectedValue;
        if (subjectno == "") {//当下拉框已经没有可增加科目时，按钮点击无效
            return;
        }
        string CourseNo = grade + subjectno;
        string courseCount = "0";
        DBManipulation dbm = new DBManipulation();              
        string sql2 = "insert into gcgCourse values(@CourseNo,@SubjectNo,@Grade,@CourseTime)";
        ParameterStruct p3 = new ParameterStruct("@CourseNo", CourseNo + "");
        ParameterStruct p4 = new ParameterStruct("@SubjectNo", subjectno);
        ParameterStruct p5 = new ParameterStruct("@Grade", grade + "");
        ParameterStruct p6 = new ParameterStruct("@CourseTime", courseCount + "");
        ArrayList parameterList2 = new ArrayList();
        parameterList2.Add(p3);
        parameterList2.Add(p4);
        parameterList2.Add(p5);
        parameterList2.Add(p6);
        dbm.ExecuteNonQuery(sql2, parameterList2);
        dbm.Close();
        SetItemForChoosableCourse();
        this.Page_Load(this, null);
        // Response.Redirect("classTimesArrange.aspx");
    }

    protected void DropDownList_Grade_SelectedIndexChanged(object sender, EventArgs e)
    {       
        SetItemForChoosableCourse();
     //   CheckLectureHour();
    }

    protected void ButtonDelete_Click(object sender, EventArgs e) {
        Button b = (Button)sender;
        string ButtonID = b.ID;
        int i;
        for( i = 0;i < ButtonLists.Count;i++){
            Button b1 = (Button)ButtonLists[i];
            if (ButtonID.Equals(b1.ID)) {
                break;
            }
        }
        TableCell t = (TableCell)TableCellLists[i];
        string courseName = t.Text;
        string grade = DropDownList_Grade.SelectedValue;
        string sql1 = "select subjectno from gcgSubject where subjectname ='"+courseName+"'";
        DBManipulation dbm = new DBManipulation();
        Object o = dbm.ExecuteScalar(sql1, null);
        if (o != null) {
            string subjectno = o.ToString();
            string sql2 = "delete from gcgCourse where SubjectNo ='" + subjectno + "' and Grade ='" + grade+"'";
            dbm.ExecuteNonQuery(sql2, null);
        }
        dbm.Close();
        SetItemForChoosableCourse();
        this.Page_Load(this, null);
    }
}

