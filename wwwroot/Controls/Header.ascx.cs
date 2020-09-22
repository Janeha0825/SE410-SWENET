namespace SwenetDev.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Web.Security;

	/// <summary>
	///		Summary description for Header.
	/// </summary>
	public class Header : System.Web.UI.UserControl {
		protected System.Web.UI.WebControls.LinkButton LogoutLink;

		private void Page_Load(object sender, System.EventArgs e) {
			// Put user code to initialize the page here
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.LogoutLink.Click += new System.EventHandler(this.LogoutLink_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void LogoutLink_Click(object sender, System.EventArgs e) {
			UsersControl.logoutUser();
			Response.Redirect( "default.aspx", true );
		}
	}
}