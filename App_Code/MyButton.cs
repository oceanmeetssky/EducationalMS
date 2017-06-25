using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Windows;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Drawing.Design;
/// <summary>
/// MyButton 的摘要说明
/// </summary>
public class MyButton : System.Web.UI.WebControls.Button
{
    /*create table gcgFeedBack(
	    class_id char(4) not null,
	    CourseNo char(3) not null,
	    TeacherNo char(8) not null,
	    primary key(class_id,CourseNo)	
    )*/
    //这个按钮继承Button类
    //存放有class_id,CourseNo,TeacherNo
    private string class_id;
    private string CourseNo;
    private string SubjectName;
    private string TeacherNo;
    private bool isClick;
    private int day;
    private int lesson;
    public string IDForThisButton;

    public MyButton(){}
    public MyButton(string class_id,string CourseNo,string TeacherNo) {
        this.isClick = false;
        this.class_id = class_id;
        this.CourseNo = CourseNo;
        this.TeacherNo = TeacherNo;

    }

    public string Class_id
    {
        get
        {
            return class_id;
        }

        set
        {
            class_id = value;
        }
    }

    public string CourseNo1
    {
        get
        {
            return CourseNo;
        }

        set
        {
            CourseNo = value;
        }
    }

    public string TeacherNo1
    {
        get
        {
            return TeacherNo;
        }

        set
        {
            TeacherNo = value;
        }
    }

    public bool IsClick
    {
        get
        {
            return isClick;
        }

        set
        {
            isClick = value;
        }
    }

    public string SubjectName1
    {
        get
        {
            return SubjectName;
        }

        set
        {
            SubjectName = value;
        }
    }

    public int Day
    {
        get
        {
            return day;
        }

        set
        {
            day = value;
        }
    }

    public int Lesson
    {
        get
        {
            return lesson;
        }

        set
        {
            lesson = value;
        }
    }
}