using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// PermissionEnum 的摘要说明
/// </summary>
public enum PermissionEnum
{//访问权限设定
   Guest = 0,        //客人权限
   Student = 1,      //学生权限
   Teacher = 2,      //教师权限
   CourseMaster = 3, //学科组长权限
   AcademicDean = 4, //教务主任权限
   EducationDean = 5 //教导主任权限
}