<%@ Page Language="C#" AutoEventWireup="true" CodeFile="control.aspx.cs" Inherits="educationDean_control" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
            <%/*
                int i;
                ArrayList startTime = new ArrayList();
                ArrayList endTime = new ArrayList();
                DBManipulation dbm = new DBManipulation();
               // string grade = Request.Form["DropDownList_Grade"].ToString();
               // string week = Request.Form["DropDownList_Week"].ToString();
               // string grade = "1";
               // string week = "1";
                string sql1 = "delete from gcgSchedule where Grade = @Grade and DayNo = @DayNo";
                ParameterStruct p1 = new ParameterStruct("@Grade", grade);
                ParameterStruct p2 = new ParameterStruct("@DayNo", week);
                ArrayList plist1 = new ArrayList();
                plist1.Add(p1);
                plist1.Add(p2);
                dbm.ExecuteNonQuery(sql1, plist1);
                for (i = 1; i < 9; i++)
                {
                    string leftid = "text" + i + 1;
                    string rightid = "text" + i + 2;

                    string start = Request.Form[leftid].ToString();
                    string end = Request.Form[rightid].ToString();
                    //string start = this.text11
                    Byte lessonNo = (Byte)i;
                    /*create table gcgSchedule(--时间段表
                            Timeid char(4) primary key,--时间段主码
                            DayNo tinyint not null, --星期几
                            Grade tinyint not null, --年级
                            LessonNo tinyint not null,--第几节次
                            StartH tinyint not null,--这节课开始小时
                            EndH tinyint not null,--这节课结束小时
                            StartM tinyint not null,--这节课开始分钟
                            EndM tinyint not null --这节课结束分钟
                        )*/
                        /*
                    string[] start_H_M = start.Split(':');
                    string[] end_H_M = end.Split(':');
                    string Timeid = grade + week + "0" + lessonNo;
                    string sql2 = "insert into gcgSchedule values('" + Timeid + "'," + week + "," + grade + "," + lessonNo + "," + start_H_M[0] + "," + end_H_M[0] + "," + start_H_M[1] + "," + end_H_M[1] + ")";
                    //为什么不用参数化查询是因为我的参数都是string类型，但是这里是tinyint类型
                    //也许应该设置一下
                    //debug1.Text = sql2;
                    dbm.ExecuteNonQuery(sql2, null);
                }
                Response.Redirect("timeArrange.aspx");*/
            %>
</body>
</html>
