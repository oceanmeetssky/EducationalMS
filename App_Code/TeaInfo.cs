using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TeaInfo
/// </summary>
public class TeaInfo
{
    private string no;//教师工号
    private string name;//教师姓名
    private string sbj;//教师科目
    private string courseno;//学科号

    public TeaInfo()
    {        
    }
    public TeaInfo(string _no,string _name,string _sbj,string _courseno)
    {
        this.no = _no;
        this.name = _name;
        this.sbj = _sbj;
        this.courseno = _courseno;
    }
    public TeaInfo(string _no, string _name) {
        this.no = _no;
        this.name = _name;
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public string Sbj
    {
        get
        {
            return sbj;
        }

        set
        {
            sbj = value;
        }
    }

    public string No
    {
        get
        {
            return no;
        }

        set
        {
            no = value;
        }
    }

    public string Courseno
    {
        get
        {
            return courseno;
        }

        set
        {
            courseno = value;
        }
    }
}