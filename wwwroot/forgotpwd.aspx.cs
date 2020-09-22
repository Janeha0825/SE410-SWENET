using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SwenetDev.DBAdapter;

namespace SwenetDev {
	/// <summary>
	/// Summary description for forgotpwd.
	/// </summary>
	public class forgotpwd : System.Web.UI.Page {

		protected System.Web.UI.WebControls.Label lblMessage;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected SwenetDev.Controls.SecretQuestion secretQuestion;
		protected System.Web.UI.WebControls.Button SubmitBtn;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator;
		protected System.Web.UI.WebControls.Button ContinueBtn;

	
		private void Page_Load(object sender, System.EventArgs e) {

			if( !IsPostBack ) {
				lblMessage.Text = "";
				SubmitBtn.Visible = true;
				ContinueBtn.Visible = false;
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
			this.SubmitBtn.Click += new System.EventHandler(this.SubmitBtn_Click);
			this.ContinueBtn.Click += new System.EventHandler(this.ContinueBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void SubmitBtn_Click(object sender, System.EventArgs e) {
		
			// Try to retrieve user information with the given username
			UserAccounts.UserInfo user = UserAccounts.getUserInfo( txtUserName.Text );
			
			// Check if the user actually exists
			if ( user == null ) {
				lblMessage.Text = "Username not found.";
			} else {
				
				int index = secretQuestion.getQuestionID();
				
				// Check that the secret question and answer were right
				if( user.QuestionID != index || user.QuestionAnswer != secretQuestion.getAnswer() ) {
					lblMessage.Text = "Some or all of the data you entered was "
						+ "incorrect.  Please revise your answers and try again.  ";
				} else {

					// generate a password
					string newPwd = generatePassword();

					// set the password in the database
					user.Password = newPwd;
					UsersControl.updateUser( user );

					// send it to their email
					Email email = Emails.getEmail( EmailType.PasswordReset );
					Emails.formatEmail( email, user.Username, newPwd );
					MailMessage msg = Emails.constructMailMessage( email, user.Username, Globals.AdminsEmail );
					SmtpMail.SmtpServer = AspNetForums.Components.Globals.SmtpServer;
					SmtpMail.Send( msg );

					// give them further instructions and set the visibility
					// of the command buttons accordingly
					lblMessage.Text = "An email has been sent to: " + user.Email
						+ ".  Please check your email for your new password.  "
						+ "You may change this password after you log in.  "
						+ "Click Continue to go to the login page.  ";
					SubmitBtn.Visible = false;
					ContinueBtn.Visible = true;
				}
			}
		}

		/// <summary>
		/// Generates a password
		/// </summary>
		/// <returns>The generated password.</returns>
		private string generatePassword() {
			
			// Only use lowercase letters, uppercase letters, and numbers as available
			// characters for the password
			string available = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			int numAvailable = available.Length;
			string pwd = "";
			Random num = new Random();

			for( int i=0; i<10; i++ ) {
				pwd += available[ num.Next() % numAvailable ];
			}

			return pwd;
		}

		/// <summary>
		/// Handles Contine button-click events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContinueBtn_Click(object sender, System.EventArgs e) {
			Response.Redirect( "login.aspx" );
		}
	}
}
