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
	/// Summary description for ChangePassword.
	/// </summary>
	public class ChangePassword : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label ErrorMessage;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.RegularExpressionValidator Regularexpressionvalidator3;
		protected System.Web.UI.WebControls.RegularExpressionValidator AlphaNumericUsername;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegExValidator1;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator2;
		protected System.Web.UI.WebControls.TextBox txtPasswordConfirm;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator3;
		protected System.Web.UI.WebControls.Button SubmitBtn;
		protected System.Web.UI.WebControls.CustomValidator PasswordCompareValidator;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		/// <summary>
		/// Handles Submit button-click events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SubmitBtn_Click(object sender, System.EventArgs e) 
		{
			Page.Validate();

			try 
			{
				if ( Page.IsValid ) 
				{
					UserAccounts.UserInfo user = UserAccounts.getUserInfo( txtUserName.Text );

					user.Password = txtPasswordConfirm.Text;

					UsersControl.updateUser( user );

					ErrorMessage.ForeColor = Color.Green;
					ErrorMessage.Text = "User password successful changed.";
				} 
				else 
				{
					ErrorMessage.ForeColor = Color.Red;
					ErrorMessage.Text = "Invalid form.  Please make the appropriate corrections.";
				}
			} 
			catch( Exception ex ) 
			{
				ErrorMessage.ForeColor = Color.Red;
				ErrorMessage.Text = "There was an error in processing your request. "
					+ "Make sure you have the correct Username and try again.";
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
			this.PasswordCompareValidator.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(this.PasswordCompareValidator_ServerValidate);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void PasswordCompareValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args) 
		{
			args.IsValid = txtPassword.Text == txtPasswordConfirm.Text;
		}
	}
}
