using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;

namespace CacheApp
{
    public partial class _Default : System.Web.UI.Page
    {
        Business.BusinessLayer b = new Business.BusinessLayer();
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lbl_valor.Text = b.ObterValor().ToString();
        }


    
    }
}
