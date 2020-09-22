namespace SwenetDev.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using DBAdapter;

	/// <summary>
	///		Summary description for FacultyConfirmation.
	/// </summary>
	public class FacultyConfirmation : System.Web.UI.UserControl 
	{
		protected System.Web.UI.WebControls.Label ConfirmLbl;
		protected System.Web.UI.WebControls.Panel FormPanel;
		protected System.Web.UI.WebControls.TextBox NameBox;
		protected System.Web.UI.WebControls.RequiredFieldValidator NameValidator;
		protected System.Web.UI.WebControls.TextBox AffiliationTxt;
		protected System.Web.UI.WebControls.TextBox ProofTxt;
		protected System.Web.UI.WebControls.RequiredFieldValidator AffiliationValidator;
		protected System.Web.UI.WebControls.Button ApplyBtn;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !Context.User.IsInRole( UserRole.Faculty.ToString() ) ) 
			{
				initRequestInfo();

				if ( !IsPostBack ) 
				{
					UserAccounts.UserInfo user = UserAccounts.getUserInfo( Context.User.Identity.Name );
					NameBox.Text = user.Name;
					AffiliationTxt.Text = user.Affiliation;
				}
			} 
			else
			{
				Visible = false;
			}			
		}

		/// <summary>
		/// Initialize the request controls.
		/// </summary>
		private void initRequestInfo() 
		{
			// Check to see if user entered faculty request.
			FacultyRequestInfo fri = UsersControl.getFacultyRequestInfo( Context.User.Identity.Name );

			// If no submitter request, display instructions
			// (input controls are visible by default).
			// Otherwise display request acknowledgement and date.
			if ( fri != null ) 
			{
				FormPanel.Visible = false;
				ConfirmLbl.Text = "Your registration for faculty user status was received on "
					+ fri.Date.ToShortDateString() + ".  You will be notified by email when your "
					+ "request has been evaluated.";
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion

		/// <summary>
		/// Handle the apply button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ApplyBtn_Click(object sender, System.EventArgs e) 
		{
			try 
			{
				if ( NameValidator.IsValid && AffiliationValidator.IsValid ) 
				{
					FacultyRequestInfo fri = new FacultyRequestInfo();

					fri.Date = DateTime.Now;
					fri.UserName = Context.User.Identity.Name;
					fri.Proof = Globals.parseTextInput( ProofTxt.Text );
					fri.Affiliation = AffiliationTxt.Text;
					fri.Name = NameBox.Text;

					UsersControl.addFacultyRequest( fri );
					initRequestInfo();
				}
			} 
			catch ( Exception ex ) 
			{
				ConfirmLbl.Text = ex.Message + ex.InnerException.Message;}
		}

	}
}
