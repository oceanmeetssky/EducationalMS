using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClassInfo
/// </summary>
public class ClassInfo
{
    private string no;
    private string name;

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

    public ClassInfo(string _no,string _name)
    {
        name = _name;
        no = _no;
    }
}