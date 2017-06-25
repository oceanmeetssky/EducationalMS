using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Teacher 的摘要说明
/// </summary>
public class Teacher
{
    private string TeacherNo;
    private string TeacherName;
    private string MainSubjectNo;
    private string MainSubjectName;
    private string ViceSubjectNo;
    private string ViceSubjectName;
    private int MaxLeadingClassNum;
    private int LeadingClassNum;

    public Teacher() {}
    public Teacher(int _max)
    {
        MaxLeadingClassNum1 = _max;
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

    public string TeacherName1
    {
        get
        {
            return TeacherName;
        }

        set
        {
            TeacherName = value;
        }
    }

    public string MainSubjectNo1
    {
        get
        {
            return MainSubjectNo;
        }

        set
        {
            MainSubjectNo = value;
        }
    }

    public string MainSubjectName1
    {
        get
        {
            return MainSubjectName;
        }

        set
        {
            MainSubjectName = value;
        }
    }

    public string ViceSubjectNo1
    {
        get
        {
            return ViceSubjectNo;
        }

        set
        {
            ViceSubjectNo = value;
        }
    }

    public string ViceSubjectName1
    {
        get
        {
            return ViceSubjectName;
        }

        set
        {
            ViceSubjectName = value;
        }
    }

    public int MaxLeadingClassNum1
    {
        get
        {
            return MaxLeadingClassNum;
        }

        set
        {
            MaxLeadingClassNum = value;
        }
    }

    public int LeadingClassNum1
    {
        get
        {
            return LeadingClassNum;
        }

        set
        {
            LeadingClassNum = value;
        }
    }
}