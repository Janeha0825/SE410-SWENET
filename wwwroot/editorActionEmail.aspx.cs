using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace SwenetDev {
	using DBAdapter;
	/// <summary>
	/// Handles completing an approval or rejection of a module submission.
	/// 
	/// The query string contains the following depending on the action type:
	///		Submitter Requests: approved, username
	///		Modules Awaiting Approval: approved, username, moduleID
	/// </summary>
	public class editorActionEmail : System.Web.UI.Page {
		protected System.Web.UI.WebControls.TextBox CustomMessage;
		protected System.Web.UI.WebControls.Button SendBtn;
		protected System.Web.UI.WebControls.Label SubjectLbl;
		protected System.Web.UI.WebControls.Label MessageBody;
		protected System.Web.UI.WebControls.Label ErrorMessage;
		protected System.Web.UI.WebControls.RequiredFieldValidator CustomMsgValidator;
		protected System.Web.UI.WebControls.Button CancelBtn;
		protected System.Web.UI.WebControls.CheckBox DeleteCheck;
		// defines the types of approve/reject messages that can be sent.
		protected enum editType { faculty = 0, submitter = 1, module = 2 };

		/// <summary>
		/// The email message currently displayed.
		/// </summary>
		public Email Message {
			set {
				EnsureChildControls();
				ViewState["ActionMessage"] = value;
				SubjectLbl.Text = HttpUtility.HtmlEncode( value.Subject );
				MessageBody.Text = value.Body;
				MessageBody.Text = HttpUtility.HtmlEncode( MessageBody.Text ).Replace( "\n", "<br>" );
			}
			get {
				if ( ViewState["ActionMessage"] != null ) {
					return (Email)ViewState["ActionMessage"];
				} else {
					return null;
				}
			}
		}

		/// <summary>
		/// The username of the user to receive the email.
		/// </summary>
		private string UserName {
			set { ViewState["To_UserName"] = value; }
			get {
				if ( ViewState["To_UserName"] != null ) {
					return (string)ViewState["To_UserName"];
				} else {
					return null;
				}
			}
		}

		/// <summary>
		/// The identifier of the module in question, or -1 if this is a
		/// submitter request.
		/// </summary>
		private int ModuleID { 
			set { ViewState["ModuleID"] = value; }
			get {
				if ( ViewState["ModuleID"] != null ) {
					return (int)ViewState["ModuleID"];
				} else {
					return -1;
				}
			}
		}
	
		private void Page_Load(object sender, System.EventArgs e) {
			if ( !User.Identity.IsAuthenticated || !User.IsInRole( UserRole.Editor.ToString() ) ) {
				throw new Exception( "You are not authorized to view this page." );
			}

			if ( !IsPostBack ) {
				bool approved;
				string username;
				int moduleID = -1;

				// If type is not specified in the query string, the
				// page was called incorrectly.  Exit immediately.
				if ( Request["type"] == null || Request["username"] == null || Request["approved"] == null ) {
					throw new Exception( "Page called incorrectly. Ln 97 " + Request.QueryString["type"] );
				}

				Email msg;
				EmailType type;
				approved = Convert.ToBoolean( Request.QueryString["approved"] );
				username = Request.QueryString["username"];
				editType request = (editType)( Convert.ToInt32( Request.QueryString["type"] ) );

				switch ( Convert.ToInt32( request ) ) {
					case (int)editType.faculty: // a faculty request is being approved/rejected
						if ( approved ) {
							// approved
							type = EmailType.FacultyApproved;
							CustomMsgValidator.Enabled = false;
						} 
						else 
						{
							// denied
							type = EmailType.FacultyDenied;
						}

						msg = Emails.getEmail( type );
						Emails.formatEmail( msg, username );
						break;

					case (int)editType.submitter: // a submitter request is being approved/rejected
						if ( approved ) {
							// approved
							type = EmailType.SubmitterApproved;
							CustomMsgValidator.Enabled = false;
						} 
						else 
						{
							// denied
							type = EmailType.SubmitterDenied;
						}

						msg = Emails.getEmail( type );
						Emails.formatEmail( msg, username );
						break;

					case (int)editType.module: // a module submission is being approved/rejected
						// The query string must include the
						// moduleID or the page was called incorrectly.
						if ( Request.QueryString["moduleID"] == null ) {
							throw new Exception( "Page called incorrectly. Ln 142" );
						}

						moduleID = Convert.ToInt32( Request.QueryString["moduleID"] );

						if ( approved ){
							// approved
							type = EmailType.ModuleApproved;
							CustomMsgValidator.Enabled = false;
						} 
						else 
						{
							// save by default
							type = EmailType.ModuleDeniedSave;
							DeleteCheck.Visible = true;
						}

						msg = Emails.getEmail( type );
						if ( msg == null ) 
						{
							throw new Exception( "Message template not found. " );
						}
						Emails.formatEmail( msg, username, moduleID );

						ModuleID = moduleID;
						break;

					default: // Othwerwise the page was called incorrectly.
						throw new Exception( "Page called incorrectly.  Ln 171" );
				}

				UserName = username;
				Message = msg;
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
			this.DeleteCheck.CheckedChanged += new System.EventHandler(this.DeleteCheck_CheckedChanged);
			this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Perform the necessary action to update the module's state, and
		/// send the email to the module submitter if emails are enabled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SendBtn_Click(object sender, System.EventArgs e) {
			Emails.formatEmailBody( Message, CustomMessage.Text );
			MailMessage msg = Emails.constructMailMessage( Message, UserName, Globals.EditorsEmail );

			switch ( Message.Type ) {
				case EmailType.SubmitterApproved:
					UsersControl.approveSubmitter( UserName );
					break;
				case EmailType.SubmitterDenied:
					UsersControl.rejectSubmitter( UserName );
					break;
				case EmailType.ModuleApproved:
					ModulesControl.approveModule( ModuleID );
					break;
				case EmailType.ModuleDeniedSave:
					ModulesControl.rejectModule( ModuleID, true );
					break;
				case EmailType.ModuleDeniedDelete:
					ModulesControl.rejectModule( ModuleID, false );
					deleteMaterials();
					break;
				case EmailType.FacultyApproved:
					UsersControl.approveFaculty( UserName );
					break;
				case EmailType.FacultyDenied:
					UsersControl.rejectFaculty( UserName );
					break;
			}

			// Message to be displayed on editors page.
			string successMessage = "Operation successful.  ";

			// Only send an email if they're enabled.
			if ( Globals.EmailsEnabled ) {
				try {
					SmtpMail.SmtpServer = AspNetForums.Components.Globals.SmtpServer;
					SmtpMail.Send( msg );
					successMessage += "Email sent to user.";
				} catch ( Exception ex ) {
					successMessage += "But an error occurred while sending an email to the user.";
				}
			} else {
				successMessage += "Email not sent to user because they are disabled.";
			}

			Response.Redirect( "EditorsPage.aspx?message=" +
				HttpUtility.UrlEncode( successMessage ) );
		}

		private void deleteMaterials() {
			string path = System.Configuration.ConfigurationSettings.AppSettings["MaterialsDir"] + ModuleID + "\\";
			string[] filenames = Directory.GetFiles( path );
			int pos = 0;

			try {
				foreach( string filename in filenames ) {
					File.Delete( filename );
					pos++;
				}
			} catch (Exception duh) {
				Response.Redirect( "EditorsPage.aspx?message=EditorsPage: An error occured while trying to remove existing material: " + filenames[pos] );
			}
		}

		/// <summary>
		/// Add prompt to ensure they really want to delete the module upon
		/// clicking send.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DeleteCheck_CheckedChanged(object sender, System.EventArgs e) {
			// Assume save
			EmailType type = EmailType.ModuleDeniedSave;

			if ( DeleteCheck.Checked ) {
				// delete
				type = EmailType.ModuleDeniedDelete;
				SendBtn.Attributes.Add( "onClick", "if ( !confirm('Are you sure you want to delete this module?') ) return false;" );
			} else {
				SendBtn.Attributes.Remove( "onClick" );
			}

			Email msg = Emails.getEmail( type );

			Emails.formatEmail( msg, UserName, ModuleID );
			Message = msg;
			Message.Type = type;
		}

		/// <summary>
		/// Redirect back to the editors page.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CancelBtn_Click(object sender, System.EventArgs e) {
			Response.Redirect( "EditorsPage.aspx", true );
		}
	}
}
