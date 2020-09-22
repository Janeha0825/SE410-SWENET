using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using AdamKinney.RSS;

namespace RSSSample.SamplePage
{
	/// <summary>
	/// Summary description for SamplePage.
	/// </summary>
	public class SamplePage : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			//System.Text.StringBuilder sb = new System.Text.StringBuilder(256);
			System.Data.SqlClient.SqlDataReader dr;
			RSSFeed rssFeed = new RSSFeed("AdamKinney's Blog", "http://www.adamkinney.com/blog/", "GotDotNet Bard");

			rssFeed.ImplementsSlash = true;
			rssFeed.Language = "en-us";
			rssFeed.ManagingEditor = "adam@apterasoftware.com";
			rssFeed.WebMaster = "adam@apterasoftware.com";
			rssFeed.TTL = "60";

//			dr = Set dr to SQLDataReader that selects posts from database.

			RSSItem item;

			while(dr.Read())
			{
				item = new RSSItem(dr[1].ToString(), "<![CDATA[ " + dr[2].ToString() + " ]]>");
				item.Author = "Adam Kinney";
				item.Categories.Add(new RSSCategory(dr[3].ToString()));
				item.Comments = "http://www.adamkinney.com/blog/default.aspx?PostID=" + dr[0].ToString() + "#comments";
				item.Slash.Comments = dr[5].ToString();
				item.Guid = "http://www.adamkinney.com/blog/default.aspx?PostID=" + dr[0].ToString();
				item.GuidIsPermalink = true;
				item.PubDate = DateTime.Parse(dr[4].ToString()).ToString("r");
				item.SourceUrl = "http://www.adamkinney.com/blog/rss.aspx";

				rssFeed.Items.Add(item);
			}

			Response.ContentType = "text/xml";
			Response.Write(rssFeed.ToString());
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}