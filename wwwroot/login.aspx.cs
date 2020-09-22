using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Web.Security;
using System.Data.SqlClient;
using SwenetDev.DBAdapter;

namespace SwenetDev {
	/// <summary>
	/// A page for a user to log in to the system.
	/// </summary>
	public class login : System.Web.UI.Page {
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected System.Web.UI.WebControls.LinkButton RegisterLinkBtn;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		protected System.Web.UI.WebControls.ValidationSummary ValidationSummary1;
		protected System.Web.UI.WebControls.LinkButton ForgotPwdBtn;
		protected System.Web.UI.WebControls.Button LoginBtn;
	
		private void Page_Load(object sender, System.EventArgs e) {
			if ( !IsPostBack && Request.QueryString["ReturnUrl"] != null ) {
				lblMessage.Text = "The requested page requires you to log in " + 
					"or you are not authorized to view that page.";
			}
		}

		/// <summary>
		/// Handles Login button-click events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LoginBtn_Click(object sender, System.EventArgs e) {
			bool passwordVerified = false;

			try {
				// Verify password and set authorization cookie, if valid.
				passwordVerified = 
					UserAccounts.VerifyPassword(txtUserName.Text,txtPassword.Text);
				UserAccounts.UserInfo user = UserAccounts.getUserInfo( txtUserName.Text );

				if ( !passwordVerified ) {
					// First, see if the username exists and if the password
					// is correct.  If the username doesn't exist, or if
					// the password is incorrect, reset the fields and notify
					// user of the problem.
					lblMessage.Text = "Invalid username or password.";
					txtUserName.Text = "";
					txtPassword.Text = "";
				} else if ( user.Role == UserRole.Disabled ) {
					// If the username exists and the password was correct,
					// check to see if the account has been disabled.  If so,
					// notify the user and reset the fields.
					lblMessage.Text = "That account has been disabled.";
					txtUserName.Text = "";
					txtPassword.Text = "";
				} else {
					
					// Keep track of redirection information
					string url = Request.QueryString["ReturnUrl"] == null ? "MyAccount.aspx" :
						FormsAuthentication.GetRedirectUrl( txtUserName.Text, false );

					if ( user.Role == UserRole.Canceled ) {
						// If this account had been Canceled, reset them to User status,
						// and redirect them to a page with the appropriate information.
						user.Role = UserRole.User;
						UsersControl.updateUserRole( user );
						Session["CancelType"] = "Reactivate";
						url = "AccountCanceled.aspx?ReturnUrl=" + url;
					}

					Response.Redirect( url );
				}

			} catch(Exception ex) {
				lblMessage.Text = ex.Message;
			}
		}

		/// <summary>
		/// Handles Register link button-clicks.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RegisterLinkBtn_Click(object sender, System.EventArgs e) {
			Response.Redirect( "register.aspx?" + Request.QueryString.ToString(), false );
		}

		/// <summary>
		/// Handles "Forgot Password" link button-clicks.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ForgotPwdLinkBtn_Click(object sender, System.EventArgs e) {
			Response.Redirect( "forgotpwd.aspx" );
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
			this.LoginBtn.Click += new System.EventHandler(this.LoginBtn_Click);
			this.RegisterLinkBtn.Click += new System.EventHandler(this.RegisterLinkBtn_Click);
			this.ForgotPwdBtn.Click += new System.EventHandler(this.ForgotPwdLinkBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
