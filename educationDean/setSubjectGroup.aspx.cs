using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class educationDean_setSubjectGroup : System.Web.UI.Page
{
    private ArrayList TableCellLists = new ArrayList();//存放所有科目名
    private ArrayList SubjectnoLists = new ArrayList();//存放所有科目号
    private ArrayList DropDownLists = new ArrayList();//存放所有供选择的学科组
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            if (!(LoginAndPermissionChecking.LoginChecking()))
            {
                Response.Redirect("/ErrorPage/error_NotLogin.aspx");
            }
            if (!(LoginAndPermissionChecking.PermissionChecking(PermissionEnum.EducationDean)))
            {
                Response.Redirect("/ErrorPage/error_DeniedPermission.aspx");
            }
            setItem_deleteDropDownList();
           
        }
        Page.MaintainScrollPositionOnPostBack = true;//刷新后滚动条回到之前的位置,但是会导致页面闪烁          
        CreateTableDynamicly();
        setItemsForSelectGroup();//为每一个科目的可选学科组下拉框准备数据    
    }

    private void CreateTableDynamicly()
    {   //动态生成表格
        Table_SubjectGroup.Controls.Clear();//把原先的表全部清干净
        TableCellLists.Clear();//把保存的上一次的控件引用清理掉，不需要了，这次会生成一个新的表
        DropDownLists.Clear();
        //重新设置表头
        TableRow tableRowHeader = new TableRow();
        tableRowHeader.CssClass = "aspTableRow1";
        TableCell tablecell_subjectNameHeader = new TableCell();
        tablecell_subjectNameHeader.Text = "科目";
        tablecell_subjectNameHeader.CssClass = "aspTableCell";
        //  tablecell_courseNameHeader.Style.Add() //= "width: 100px";
        TableCell tablecell_subjectGruopHeader = new TableCell();
        tablecell_subjectGruopHeader.Text = "学科组";
        tablecell_subjectGruopHeader.CssClass = "aspTableCell";
        tableRowHeader.Controls.Add(tablecell_subjectNameHeader);
        tableRowHeader.Controls.Add(tablecell_subjectGruopHeader);
        Table_SubjectGroup.Controls.Add(tableRowHeader);
        //查询所有学科组,将其离线
        DBManipulation dbm = new DBManipulation();
        string sql2 = "select subjectGroupNo,subjectGroupName from gcgSubjectGroup";
        DataSet ds_subjectGroup = dbm.ExecuteQueryOffLine(sql2, null);
        //根据年级查该年级的所有教学科目
        string sql = "select subjectno,subjectname,gcgSubject.subjectGroupNo,subjectGroupName from gcgSubject,gcgSubjectGroup where gcgSubject.subjectGroupNo = gcgSubjectGroup.subjectGroupNo";
        DataSet ds_subject = dbm.ExecuteQueryOffLine(sql, null);
        int count = 0;      
        foreach (DataRow r in ds_subject.Tables["defaultTable"].Rows)
        {
            //准备一个行对象，和两个单元格对象
            TableRow tableRow = new TableRow();
            TableCell tablecell_subjectName = new TableCell();
            TableCell tablecell_subjectGroup = new TableCell();
            //第一个单元格存放科目名
            tablecell_subjectName.ID = "TableCell_CourseName_" + count;
            tablecell_subjectName.Text = r["subjectname"].ToString();
            tablecell_subjectName.CssClass = "aspTableCell";
            TableCellLists.Add(tablecell_subjectName);//保存引用
            SubjectnoLists.Add(r["subjectno"].ToString());//同步保存subjectno
            //第二个单元格存放课时下拉框，记得添加事件处理
            tablecell_subjectGroup.ID = "TableCell_CourseCount_" + count;
            tablecell_subjectGroup.CssClass = "aspTableCell";
            DropDownList dropDownList = new DropDownList();
            dropDownList.ID = "DropDownCell_" + count;
            //dropDownList.SelectedIndexChanged += DropDownList_CourseCount_TextChanged;//监听事件
            /*
            dropDownList.AutoPostBack = true;
            dropDownList.DataSource = ds_subjectGroup.Tables["defaultTable"];
            dropDownList.DataTextField = ds_subjectGroup.Tables["defaultTable"].Columns[1].ColumnName;
            dropDownList.DataValueField = ds_subjectGroup.Tables["defaultTable"].Columns[0].ColumnName;
            dropDownList.DataBind();
            */
           // string subjectname = r["subjectGroupName"].ToString();
            //dropDownList.SelectedItem.Text = subjectname;//下拉框的初始值是以前该科目所属的学科组的组名
            this.DropDownLists.Add(dropDownList);//保存引用，方便其他函数使用
            tablecell_subjectGroup.Controls.Add(dropDownList);

            tableRow.ID = "TableRow_" + count;
            tableRow.CssClass = "aspTableRow2";
            tableRow.Controls.Add(tablecell_subjectName);
            tableRow.Controls.Add(tablecell_subjectGroup);
            //将这个行放入表中
            Table_SubjectGroup.Controls.Add(tableRow);
            count++;
        }
        //上面已经将表格生成了
        //setItemsForSelectGroup();//为每一个科目的可选学科组下拉框准备数据
        dbm.Close();
    }

    private void setItemsForSelectGroup() {
        //为每一个科目的可选学科组下拉框准备数据
        DBManipulation dbm = new DBManipulation();
        string sql = "select subjectno,subjectname,gcgSubject.subjectGroupNo,subjectGroupName from gcgSubject,gcgSubjectGroup where gcgSubject.subjectGroupNo = gcgSubjectGroup.subjectGroupNo";
        DataSet ds_subject = dbm.ExecuteQueryOffLine(sql, null);
        string sql2 = "select subjectGroupNo,subjectGroupName from gcgSubjectGroup";
        DataTable t = ds_subject.Tables["defaultTable"];
        DataSet ds_subjectGroup = dbm.ExecuteQueryOffLine(sql2, null);

        for(int i = 0;i < t.Rows.Count;i++) {
            Object o = DropDownLists[i];
            DropDownList d = (DropDownList)o;
            d.AutoPostBack = true;
            d.DataSource = ds_subjectGroup.Tables["defaultTable"];
            d.DataTextField = ds_subjectGroup.Tables["defaultTable"].Columns[1].ColumnName;
            d.DataValueField = ds_subjectGroup.Tables["defaultTable"].Columns[0].ColumnName;
            d.DataBind();
            DataRow r = t.Rows[i];
            string subjectGroupName = r["subjectGroupName"].ToString();
            // string subjectGroupNo = r["gcgSubject.subjectGroupNo"].ToString();
            int index = 0;
            for (index = 0;index < ds_subjectGroup.Tables["defaultTable"].Rows.Count;index++) {
                DataRow rowInSubjectGroup = ds_subjectGroup.Tables["defaultTable"].Rows[index];
                if (subjectGroupName.Equals(rowInSubjectGroup["subjectGroupName"].ToString())) {
                    break;
                }
            }
            d.SelectedIndex = index;
            //下拉框的初始值是以前该科目所属的学科组的组名
        }        
    }
    protected void ButtonSubmit_Click(object sender, EventArgs e)
    {
        string subjectGroupNo;
        string subjectNo;
        DBManipulation dbm = new DBManipulation();
        for (int i = 0; i < TableCellLists.Count; i++) {
            TableCell c = (TableCell)TableCellLists[i];
            DropDownList d = (DropDownList)DropDownLists[i];
            subjectNo = (string)SubjectnoLists[i];
            subjectGroupNo = d.SelectedValue;
            string sql = "update gcgSubject set subjectGroupNo = @subjectGroupNo where subjectno = @subjectno";
            ParameterStruct p1 = new ParameterStruct("@subjectGroupNo", subjectGroupNo);
            ParameterStruct p2 = new ParameterStruct("@subjectno", subjectNo);
            ArrayList parameterlist = new ArrayList();
            parameterlist.Add(p1);
            parameterlist.Add(p2);
            dbm.ExecuteNonQuery(sql, parameterlist);
        }
        System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "Button6_Click", "alert('修改已完成！')", true);
    }


    protected void Button_AddSubjectGroup_Click(object sender, EventArgs e)
    {//增加学科组
        string subjectGroupName = TextBox_SubjectGroup.Text;
        string subjectGroupId;
        DBManipulation dbm = new DBManipulation();
        string sql1 = "select subjectGroupNo from gcgSubjectGroup order by subjectGroupNo";
        DataSet ds_subjectGroup = dbm.ExecuteQueryOffLine(sql1, null);
        int i = 0, j;
        bool mark = true;//一个标志量
        for (i = 1; i < 90; i++) {//学科组号仅有2位
            mark = true;
            for (j = 0; j < ds_subjectGroup.Tables["defaultTable"].Rows.Count; j++) {
                string str = ds_subjectGroup.Tables["defaultTable"].Rows[j]["subjectGroupNo"].ToString();
                char[] ch = str.ToCharArray();
                if (ch[0] == '0') {
                    str = ch[1] + "";
                }
                if (i == int.Parse(str)) {
                    mark = false;
                    break;
                }
            }
            if (mark) {//如果没有任何一个id相等，那么这个就是可用id
                break;
            }
        }
        if (i < 10) {
            subjectGroupId = "0" + i;
        }
        else{
            Response.Write("<script>alert('i > 10')</script>");
            subjectGroupId = i + "";
        }        
        string sql2 = "insert into gcgSubjectGroup values(@subjectGroupNo,@subjectGroupName,null)";
        ParameterStruct p1 = new ParameterStruct("@subjectGroupNo", subjectGroupId);
        ParameterStruct p2 = new ParameterStruct("@subjectGroupName", subjectGroupName);
        ArrayList parameterlist = new ArrayList();
        parameterlist.Add(p1);
        parameterlist.Add(p2);
        debug1.Text = subjectGroupId;
        debug2.Text = subjectGroupName;
        dbm.ExecuteNonQuery(sql2, parameterlist);        
        CreateTableDynamicly();//生成表
        setItemsForSelectGroup();//为每一个科目的可选学科组下拉框准备数据
        setItem_deleteDropDownList();//为删除下拉框生成数据
        TextBox_SubjectGroup.Text = "";
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void Button2_Click(object sender, EventArgs e)
    {//删除一个学科组，先将组内的学科设为待定组
        // string sql2 = "select subjectGroupNo,subjectGroupName from gcgSubjectGroup";
        //string sql1 = "select subjectno from gcgSubject where subjectGroupNo in (select subjectGroupNo from gcgSubjectGroup where subjectGroupName = @subjectGroupName)";

        string subjectGroupNo = DropDownList1.SelectedValue;       
       
        DBManipulation dbm = new DBManipulation();
        string sql1 = "select subjectGroupNo from gcgSubjectGroup where subjectGroupName = '待定'";
        Object o = dbm.ExecuteScalar(sql1, null);
        string subjectGroup_DefalutNo = o.ToString();
        string sql2 = "select subjectno from gcgSubject where subjectGroupNo ='" + subjectGroupNo + "'";
        DataSet ds = dbm.ExecuteQueryOffLine(sql2,null);
        DataTable t = ds.Tables["defaultTable"];
        foreach ( DataRow r in t.Rows ) {
            string subjectno = r["subjectno"].ToString();
            string sql3 = "update gcgSubject set subjectGroupNo = @subjectGroupNo where subjectno = @subjectno";
            ParameterStruct p1 = new ParameterStruct("@subjectGroupNo", subjectGroup_DefalutNo);
            ParameterStruct p2 = new ParameterStruct("@subjectno", subjectno);
            ArrayList p = new ArrayList();
            p.Add(p1);
            p.Add(p2);
            dbm.ExecuteNonQuery(sql3, p);
        }

        string sql = "delete from gcgSubjectGroup where subjectGroupNo = '" + subjectGroupNo + "'";
        dbm.ExecuteNonQuery(sql, null);
        setItem_deleteDropDownList();//为删除下拉框提供新的数据
        this.Page_Load(this, null);       
    }

    private void setItem_deleteDropDownList() {
        DBManipulation dbm = new DBManipulation();
        string sql1 = "select subjectGroupNo,subjectGroupName from gcgSubjectGroup";
        DataSet ds_subjectGroup = dbm.ExecuteQueryOffLine(sql1, null);
        DropDownList1.AutoPostBack = true;
        DropDownList1.Items.Clear();
        DataTable t = ds_subjectGroup.Tables["defaultTable"];
        foreach (DataRow r in t.Rows)
        {
            string subjectGroupNo = r["subjectGroupNo"].ToString();
            string subjectGroupName = r["subjectGroupName"].ToString();
            if (subjectGroupNo.Equals("00"))
            {
                continue;
            }
            ListItem item = new ListItem(subjectGroupName, subjectGroupNo);
            DropDownList1.Items.Add(item);
        }
    }
}