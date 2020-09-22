using System;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Mail;
using System.Text;
using SwenetDev.DBAdapter;

namespace SwenetDev {
	/// <summary>
	/// Page for user feedback.
	/// </summary>
	public class feedback : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Button SendBtn;
		protected System.Web.UI.WebControls.TextBox NameTxt;
		protected System.Web.UI.WebControls.TextBox EmailTxt;
		protected System.Web.UI.WebControls.TextBox MessageTxt;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.TextBox SubjectTxt;
	
		private void Page_Load(object sender, System.EventArgs e) {
			
			if( !IsPostBack ) {
				UserAccounts.UserInfo user = UserAccounts.getUserInfo( Context.User.Identity.Name );
				NameTxt.Text = user.Name;
				EmailTxt.Text = user.Email;
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
			this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void SendBtn_Click(object sender, System.EventArgs e) {
			//construct the message
			MailMessage msg = new MailMessage();
			msg.Subject = "SWENET Feedback: " + SubjectTxt.Text;
			msg.To = Emails.getRoleEmail( (int)UserRole.Admin );
			msg.From = "feedback@swenet.org";
			StringBuilder message = new StringBuilder();
			message.Append( "SWENET Feedback\n\n" );
			message.Append( "From: " + NameTxt.Text + " (" + EmailTxt.Text + ")\n" );
			message.Append( "Subject: " + SubjectTxt.Text + "\n\n" );
			message.Append( MessageTxt.Text );
			msg.Body = message.ToString();

			//send the message
			SmtpMail.SmtpServer = AspNetForums.Components.Globals.SmtpServer;
			SmtpMail.Send( msg );
			
			//return to the homepage
			Response.Redirect( "default.aspx" );
		}
	}
}
