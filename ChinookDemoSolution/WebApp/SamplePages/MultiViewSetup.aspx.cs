using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.SamplePages
{
    public partial class MultiViewSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            //to switch views
            //   grab Value from MenuEventArgs
            int index = Int32.Parse(e.Item.Value);
            //   set .ActiveViewIndex
            MultiView1.ActiveViewIndex = index;
        }

        protected void SendToV2FromV1_Click(object sender, EventArgs e)
        {
            IODataV2.Text = IODataV1.Text;
            MultiView1.ActiveViewIndex = 1;
            Menu1.Items[1].Selected = true;
        }

        protected void SendToV3FromV1_Click(object sender, EventArgs e)
        {
            IODataV3.Text = IODataV1.Text;
            MultiView1.ActiveViewIndex = 2;
            Menu1.Items[2].Selected = true;
        }

        protected void SendToV1FromV2_Click(object sender, EventArgs e)
        {
            IODataV1.Text = IODataV2.Text;
        }

        protected void SendToV3FromV2_Click(object sender, EventArgs e)
        {
            IODataV3.Text = IODataV2.Text;

        }

        protected void SendToV2FromV3_Click(object sender, EventArgs e)
        {
            IODataV2.Text = IODataV3.Text;

        }

        protected void SendToV1FromV3_Click(object sender, EventArgs e)
        {
            IODataV1.Text = IODataV3.Text;

        }
    }
}