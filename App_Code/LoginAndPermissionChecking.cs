using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// LoginAndPermissionChecking 的摘要说明
/// </summary>
public class LoginAndPermissionChecking
{   //这个类的作用类似于过滤器一样，检查用户是否登录以及是否达到相应的页面访问权限
    public LoginAndPermissionChecking()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public static bool LoginChecking() {
        /* public static void is_login()
            {
                if (HttpContext.Current.Session["sUserid"] == null)//原理和Jsp一样，使用Session检查
                    HttpContext.Current.Response.Redirect("../error/error.aspx");//如果不是合法用户意图直接访问受限页面，则跳转到错误页面
            }
         */
        if(HttpContext.Current.Session["username"] == null) {
           // HttpContext.Current.Response.Redirect("../error/error.aspx");
            return false;
        }
        return true;
    }
    public static bool PermissionChecking(PermissionEnum _requiredRank) {
        /*权限检查可以设置静态的权限在程序本身
         * 1.学生
         * 2.教师
         * 3.排课组长
         * 4.学科组长
         * 5.教导主任
         */
        
        Object o = HttpContext.Current.Session["UserRank"];
        if (o == null) {
            return false;
        }
        else{
            PermissionEnum UserRank = (PermissionEnum)o;
            if (UserRank != _requiredRank) {
                return false;
            }
            else {
                return true;
            }
        }
        

    }
}