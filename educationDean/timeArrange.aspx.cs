using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class educationDean_timeArrange : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_11);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_12);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_21);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_22);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_31);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_32);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_41);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_42);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_51);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_52);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_61);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_62);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_71);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_72);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_81);
        ScriptManager1.RegisterAsyncPostBackControl(this.TextBox_82);
        //ScriptManager1.RegisterAsyncPostBackControl(this.DropDownList_Grade);
        //ScriptManager1.RegisterAsyncPostBackControl(this.DropDownList_Week);
        //不能使用UpdatePanel绑定两个下拉框，否则页面不会刷新。，这是为什么呢？
        // ScriptManager1.RegisterAsyncPostBackControl(this.Button_submit);

        if (!IsPostBack)
        {// 如果是为响应客户端回发而加载该页，则为 true；否则为 false。
           // ScriptManager1.RegisterAsyncPostBackControl(this.Button_submit);
           
            if (!(LoginAndPermissionChecking.LoginChecking()))
            {
                Response.Redirect("/ErrorPage/error_NotLogin.aspx");
            }
            if (!(LoginAndPermissionChecking.PermissionChecking(PermissionEnum.EducationDean)))
            {
                Response.Redirect("/ErrorPage/error_DeniedPermission.aspx");
            }
            
            SetValueForHtmlTextBox();
        }
    }

    protected void Button_submit_Click(object sender, CommandEventArgs e)
    {
        if (sender == null || sender == this) {
            return;
        }
        if (e == null) {
            return;
        }
        if (e.CommandName == "SubmitCommand") {
            Update();
        }
        
    }

    protected void Update() {
       // Response.Write("<script>alert('nihao')</script>");
        int i;
        ArrayList startTime = new ArrayList();
        ArrayList endTime = new ArrayList();
        DBManipulation dbm = new DBManipulation();
        string grade = DropDownList_Grade.SelectedValue;
        string week = DropDownList_Week.SelectedValue;
        string sql1 = "delete from gcgSchedule where Grade = @Grade and DayNo =@DayNo";
        ParameterStruct p1 = new ParameterStruct("@Grade", grade);
        ParameterStruct p2 = new ParameterStruct("@DayNo", week);
        ArrayList plist1 = new ArrayList();
        plist1.Add(p1);
        plist1.Add(p2);
        dbm.ExecuteNonQuery(sql1, plist1);
      
        string starttime1 = TextBox_11.Text;
        string endtime1 = TextBox_12.Text;
        string Timeid1 = grade + week + "0" + "1";
        string[] stime1 = starttime1.Split(':');
        string[] etime1 = endtime1.Split(':');
        string sql11 = "insert into gcgSchedule values('" + Timeid1 + "'," + week + "," + grade + "," + 1 + "," + stime1[0] + "," + etime1[0] + "," + stime1[1] + "," + etime1[1] + ")";
        dbm.ExecuteNonQuery(sql11, null);

        string starttime2 = TextBox_21.Text;
        string endtime2 = TextBox_22.Text;
        string Timeid2 = grade + week + "0" + "2";
        string[] stime2 = starttime2.Split(':');
        string[] etime2 = endtime2.Split(':');
        string sql112 = "insert into gcgSchedule values('" + Timeid2 + "'," + week + "," + grade + "," + 1 + "," + stime2[0] + "," + etime2[0] + "," + stime2[1] + "," + etime2[1] + ")";
        dbm.ExecuteNonQuery(sql112, null);

        string starttime3 = TextBox_31.Text;
        string endtime3 = TextBox_32.Text;
        string Timeid3 = grade + week + "0" + "3";
        string[] stime3 = starttime3.Split(':');
        string[] etime3 = endtime3.Split(':');
        string sql113 = "insert into gcgSchedule values('" + Timeid3 + "'," + week + "," + grade + "," + 1 + "," + stime3[0] + "," + etime3[0] + "," + stime3[1] + "," + etime3[1] + ")";
        dbm.ExecuteNonQuery(sql113, null);

        string starttime4 = TextBox_41.Text;
        string endtime4 = TextBox_42.Text;
        string Timeid4 = grade + week + "0" + "4";
        string[] stime4 = starttime4.Split(':');
        string[] etime4 = endtime4.Split(':');
        string sql114 = "insert into gcgSchedule values('" + Timeid4 + "'," + week + "," + grade + "," + 1 + "," + stime4[0] + "," + etime4[0] + "," + stime4[1] + "," + etime4[1] + ")";
        dbm.ExecuteNonQuery(sql114, null);
      

        string starttime5 = TextBox_51.Text;
        string endtime5 = TextBox_52.Text;
        string Timeid5 = grade + week + "0" + "5";
        string[] stime5 = starttime5.Split(':');
        string[] etime5 = endtime5.Split(':');
        string sql115 = "insert into gcgSchedule values('" + Timeid5 + "'," + week + "," + grade + "," + 1 + "," + stime5[0] + "," + etime5[0] + "," + stime5[1] + "," + etime5[1] + ")";
        dbm.ExecuteNonQuery(sql115, null);

        string starttime6 = TextBox_61.Text;
        string endtime6 = TextBox_62.Text;
        string Timeid6 = grade + week + "0" + "6";
        string[] stime6 = starttime5.Split(':');
        string[] etime6 = endtime6.Split(':');
        string sql116 = "insert into gcgSchedule values('" + Timeid6 + "'," + week + "," + grade + "," + 1 + "," + stime6[0] + "," + etime6[0] + "," + stime6[1] + "," + etime6[1] + ")";
        dbm.ExecuteNonQuery(sql116, null);

        string starttime7 = TextBox_71.Text;
        string endtime7 = TextBox_72.Text;
        string Timeid7 = grade + week + "0" + "7";
        string[] stime7 = starttime6.Split(':');
        string[] etime7 = endtime7.Split(':');
        string sql117 = "insert into gcgSchedule values('" + Timeid7 + "'," + week + "," + grade + "," + 1 + "," + stime7[0] + "," + etime7[0] + "," + stime7[1] + "," + etime7[1] + ")";
        dbm.ExecuteNonQuery(sql117, null);

        string starttime8 = TextBox_81.Text;
        string endtime8 = TextBox_82.Text;
        string Timeid8 = grade + week + "0" + "8";
        string[] stime8 = starttime7.Split(':');
        string[] etime8 = endtime8.Split(':');
        string sql118 = "insert into gcgSchedule values('" + Timeid8 + "'," + week + "," + grade + "," + 1 + "," + stime8[0] + "," + etime8[0] + "," + stime8[1] + "," + etime8[1] + ")";
        dbm.ExecuteNonQuery(sql118, null);

    }


    protected void SetValueForHtmlTextBox()
    {
        string grade = DropDownList_Grade.SelectedValue;
        string week = DropDownList_Week.SelectedValue;
        //select * from gcgSchedule where Grade = '1' and DayNo = '1'
        string sql1 = "select LessonNo,StartH,StartM,EndH,EndM from gcgSchedule where Grade = @Grade and DayNo = @DayNo order by LessonNo";
        ParameterStruct p1 = new ParameterStruct("@Grade", grade);
        ParameterStruct p2 = new ParameterStruct("@DayNo", week);
        ArrayList plist1 = new ArrayList();
        plist1.Add(p1);
        plist1.Add(p2);
        DBManipulation dbm = new DBManipulation();
        // SqlDataReader dr = dbm.ExecuteQueryOnLine(sql1, plist1);
        DataSet ds = dbm.ExecuteQueryOffLine(sql1, plist1);
        DataTable schedule = ds.Tables["defaultTable"];
        if (schedule.Rows.Count == 0)
        {
            return;
        }
        string[] startTime = new string[8];
        string[] endTime = new string[8];
        for (int i = 0; i < 8; i++)
        {
            string startH = schedule.Rows[i][1].ToString();
            if (startH.Length == 1) {
                startH = "0" + startH;
            }
            string startM = schedule.Rows[i][2].ToString();
            if (startM.Length == 1)
            {
                startM = "0" + startM;
            }
            string endH = schedule.Rows[i][3].ToString();
            if (endH.Length == 1)
            {
                endH = "0" + endH;
            }
            string endM = schedule.Rows[i][4].ToString();
            if (endM.Length == 1)
            {
                endM = "0" + endM;
            }
            startTime[i] = startH + ":" + startM;
            endTime[i] = endH + ":" + endM;
        }
        
        this.TextBox_11.Text = startTime[0]; this.TextBox_12.Text = endTime[0];
        this.TextBox_21.Text = startTime[1]; this.TextBox_22.Text = endTime[1];
        this.TextBox_31.Text = startTime[2]; this.TextBox_32.Text = endTime[2];
        this.TextBox_41.Text = startTime[3]; this.TextBox_42.Text = endTime[3];
        this.TextBox_51.Text = startTime[4]; this.TextBox_52.Text = endTime[4];
        this.TextBox_61.Text = startTime[5]; this.TextBox_62.Text = endTime[5];
        this.TextBox_71.Text = startTime[6]; this.TextBox_72.Text = endTime[6];
        this.TextBox_81.Text = startTime[7]; this.TextBox_82.Text = endTime[7];
        
    }

    protected void DropDownList_Grade_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetValueForHtmlTextBox();
        this.Page_Load(this, null);
    }

    protected void DropDownList_Week_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetValueForHtmlTextBox();
        this.Page_Load(this, null);
    }
}