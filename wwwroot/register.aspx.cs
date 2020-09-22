using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SwenetDev.DBAdapter;

namespace SwenetDev {
	/// <summary>
	/// Summary description for login.
	/// </summary>
	public class register : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Button RegisterBtn;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected Controls.EditUserInfoControl EditUserInfoControl1;
	
		private void Page_Load(object sender, System.EventArgs e) {
			// Put user code to initialize the page here
		}

		/// <summary>
		/// Handles Register button-click events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RegisterBtn_Click(object sender, System.EventArgs e) {
			Page.Validate();

			if ( Page.IsValid ) {
				UserAccounts.UserInfo ui = EditUserInfoControl1.UserInfo;

				try {
					UsersControl.registerUser( ui );				
					FormsAuthentication.RedirectFromLoginPage( ui.Username, false );
				} catch( Exception ex ) {
					lblMessage.Text = "<p>Error registering user.  " + ex.Message + "</p>";
				}
			}
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {    
			this.RegisterBtn.Click += new System.EventHandler(this.RegisterBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
