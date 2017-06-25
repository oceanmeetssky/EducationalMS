using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Struct_CourseName 的摘要说明
/// </summary>
public class Struct_CourseName
{
    private string CourseNo;//学科号
    private string SubjectName;//科目名
    private int Hour;//课时

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

    public int Hour1
    {
        get
        {
            return Hour;
        }

        set
        {
            Hour = value;
        }
    }
    public Struct_CourseName() {
    }
    public Struct_CourseName(string _courseNo, string _subjectName, int _hour)
    {
        this.CourseNo = _courseNo;
        this.SubjectName = _subjectName;
        this.Hour = _hour;
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
}