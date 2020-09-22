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

namespace SwenetDev {
	using Controls;
	using DBAdapter;

	/// <summary>
	/// Page encapsulating the ViewUserInfo control, which obtains the
	/// username to display from the query string.
	/// </summary>
	public class viewUserInfo : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Label ErrorMessage;
		protected ViewUserInfoControl ViewUserInfoControl1;
	
		private void Page_Load(object sender, System.EventArgs e) {
			
			bool showInfo = true;

			if ( Request.QueryString["username"] != null ) {
				
				UserAccounts.UserInfo user = null;
				
				try {
					user = UserAccounts.getUserInfo( Request.QueryString["username"] );

					if ( user != null ) {

						if ( user.Role == UserRole.Canceled ) {
							// If user has canceled his/her membership, only show the
							// info to users with Admin privileges
							ErrorMessage.Text = User.Identity.Name + " has canceled his/her membership.";
							if( !(User.Identity.IsAuthenticated && User.IsInRole( UserRole.Admin.ToString() )) )
								showInfo = false;
						} else if ( user.Role == UserRole.Disabled ) {
							// If the user's account has been disabled, only show the
							// info to users with Admin privileges
							ErrorMessage.Text = User.Identity.Name + "'s account has been disabled.";
							if( !(User.Identity.IsAuthenticated && User.IsInRole( UserRole.Admin.ToString() )) )
								showInfo = false;
						}
					} else {
						ErrorMessage.Text = "User not found.";
						showInfo = false;
					}
				} catch ( Exception ex ) {
					ErrorMessage.Text = "An error occurred while obtaining the user's information.";
					showInfo = false;
				}

				if( showInfo ) {
					ViewUserInfoControl1.UserInfo = user;
				} else {
					ViewUserInfoControl1.Visible = false;
				}

			} else {
				ErrorMessage.Text = "An error has occurred.  " 
					+ "No user was selected.";
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
