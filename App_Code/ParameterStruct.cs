using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ParameterStruct 的摘要说明
/// </summary>
public class ParameterStruct
{   //这个类专门存放SQL中的参数以及其对应的映射值
    //select * from s where sno = ? and sname = ?
    //参数按顺序存放，按顺序取出
    //ParameterStruct p1 = new ParameterStruct("val1","val11")
    //ParameterStruct p2 = new ParameterStruct("val2","val22")
    /* 参数化查询（Parameterized Query 或 Parameterized Statement）是访问数据库时，
     * 在需要填入数值或数据的地方，使用参数 (Parameter) 来给值。在使用参数化查询的情况下，
     * 数据库服务器不会将参数的内容视为SQL指令的一部份来处理，而是在数据库完成SQL指令的编译后，
     * 才套用参数运行，因此就算参数中含有指令，也不会被数据库运行。
     * Access、SQL Server、MySQL、SQLite等常用数据库都支持参数化查询。
     */
    private string parameterName ;//参数名
    private string valueName;//映射值
    public ParameterStruct(string _arguments,string _value) {
        parameterName = _arguments;
        valueName = _value;
    }

    public string ParameterName {
        get {
            return parameterName;
        }
        set {
            parameterName = value;
        }
    }

    public string ValueName {
        get {
            return valueName;
        }
        set {
            valueName = value;
        }
    }
}