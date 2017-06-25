using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorPage_CleanSession : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //这个页面用来清理Session和Cache
        Session.Abandon();
        Session.Clear();                           
        List <string> cacheKeys = new List<string>();
        IDictionaryEnumerator cacheEnum = Cache.GetEnumerator();
        while (cacheEnum.MoveNext())
        {
            cacheKeys.Add(cacheEnum.Key.ToString());
        }
        foreach (string cacheKey in cacheKeys)
        {
            Cache.Remove(cacheKey);
        }
        Response.Redirect("../login/login.aspx");
    }
}