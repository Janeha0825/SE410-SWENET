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
	using DBAdapter;
	using Controls;

	/// <summary>
	/// Summary description for editUserInfo.
	/// </summary>
	public class editUserInfo : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Button Submit;
		protected System.Web.UI.WebControls.Label ErrorMessage;
		protected System.Web.UI.WebControls.Button CancelBtn;
		protected EditUserInfoControl EditUserInfoControl1;
	
		private void Page_Load(object sender, System.EventArgs e) {
			if ( !IsPostBack ) {
				EditUserInfoControl1.EditMode = UserInfoEditMode.Existing;
				EditUserInfoControl1.UserInfo = UserAccounts.getUserInfo( User.Identity.Name );
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
			this.Submit.Click += new System.EventHandler(this.Submit_Click);
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Handles the Submit button-click events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Submit_Click(object sender, System.EventArgs e) {
			try {
				if( EditUserInfoControl1.Page.IsValid ) {
					UsersControl.updateUser( EditUserInfoControl1.UserInfo );
					Response.Redirect( "MyAccount.aspx?message=User information updated successfully.", true );
				}
			} catch ( Exception ex ) {
				ErrorMessage.Text = ex.Message;
			}
		}

		/// <summary>
		/// Handles the Cancel button-click events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CancelBtn_Click(object sender, System.EventArgs e) {
			Response.Redirect( "MyAccount.aspx" );
		}
	}
}
