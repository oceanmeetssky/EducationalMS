using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Tea
/// </summary>
public class Tea
{
    private string tea_id;//教师工号
    private string course_no;//课程编号

    public Tea()
    {

    }

    public string id
    {
        get
        {
            return tea_id;
        }
        set
        {
            tea_id = value;
        }
    }

    public string cno
    {
        get
        {
            return course_no;
        }
        set
        {
            course_no = value;
        }
    }

}
