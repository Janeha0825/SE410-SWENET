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
using SwenetDev.DBAdapter;

namespace SwenetDev
{
	/// <summary>
	/// Summary description for AccountCanceled.
	/// </summary>
	public class AccountCanceled : System.Web.UI.Page {

		protected System.Web.UI.WebControls.Label ACLabel;
		protected System.Web.UI.WebControls.Button ContinueButton;
		private string ReactivateText =
			"Your account was previously canceled.  It is now reactivated.  "
			+ "If you previously had Faculty, Submitter, or Editor status, you may "
			+ "reapply for them on the MyAccount page.";
		protected System.Web.UI.WebControls.Button YesButton;
		protected System.Web.UI.WebControls.Button NoButton;
		private string CancelText = "Before you cancel your membership, please "
			+ "note that your account and information are not being deleted; they "
			+ "will, however, be hidden from other SWENET users.  If you would like "
			+ "to reactivate your account, you may do so by logging back in to the "
			+ "site, but you will lose Submitter or Editor status if you currently "
			+ "have it.  You may reapply for them, though. <br><br>"
			+ "Are you sure you want to cancel your membership?";

		private void Page_Load(object sender, System.EventArgs e) {

			if( !IsPostBack && Session["CancelType"] != null ) {
				string CancelType = (string)Session["CancelType"];
				Session.Remove( "CancelType" );

				if( CancelType.Equals("Cancel") ) {
					// User is in the process of canceling membership.
					// Confirm whether or not they want to continue with
					// this action.
					ACLabel.Text = CancelText;
					ContinueButton.Visible = false;
					YesButton.Visible = true;
					NoButton.Visible = true;
				} else if( CancelType.Equals("Reactivate") ) {
					// User signed in while their account was canceled.
					// Tell them that their account is now reactivated
					// and what the implications are.
					ACLabel.Text = ReactivateText;
					ContinueButton.Visible = true;
					YesButton.Visible = false;
					NoButton.Visible = false;
				} else {
					// User got to this page erroneously
					Response.Redirect( "MyAccount.aspx" );
				}
			}

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
			this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
			this.YesButton.Click += new System.EventHandler(this.YesButton_Click);
			this.NoButton.Click += new System.EventHandler(this.NoButton_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Handles the 'Continue' button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContinueButton_Click(object sender, System.EventArgs e) {
			// Should be pressed after the user reads the details concerning their
			// newly-reactivated account.  Redirects them to the appropriate page.
			string url = Request.QueryString["ReturnUrl"] == null ? "MyAccount.aspx" :
						(string)(Request.QueryString["ReturnUrl"]);
			
			Response.Redirect( url );
		}

		/// <summary>
		/// Handles the 'Yes' button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void YesButton_Click(object sender, System.EventArgs e) {
			// If the user clicked Yes, update their user account to "Canceled"
			// and redirect them to the default page.
			UserAccounts.UserInfo user = UserAccounts.getUserInfo( User.Identity.Name );
			user.Role = UserRole.Canceled;
			// Updates the user's role and sends them an email telling them
			// about the change.
			UsersControl.updateUserRole( user );
			UsersControl.logoutUser();
			Response.Redirect( "default.aspx", true );
		}

		/// <summary>
		/// Handles the 'No' button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NoButton_Click(object sender, System.EventArgs e) {
			// If the user clicked No, redirect them back to the MyAccount page
			Response.Redirect( "MyAccount.aspx" );
		}
	}
}
