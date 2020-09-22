namespace SwenetDev.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using DBAdapter;

	/// <summary>
	///	A control for requesting module submission privlidges.
	/// </summary>
	public class SubmitterRequestControl : System.Web.UI.UserControl {
		protected System.Web.UI.WebControls.Panel FormPanel;
		protected System.Web.UI.WebControls.TextBox MessageTxt;
		protected System.Web.UI.WebControls.Button ApplyBtn;
		protected System.Web.UI.WebControls.TextBox SubmitIdTxt;
		protected System.Web.UI.WebControls.RequiredFieldValidator SubmitterIdReqiredVal;
		protected System.Web.UI.WebControls.CustomValidator SubmitterIdCustomVal;
		protected System.Web.UI.WebControls.Label RequestLbl;

		private void Page_Load(object sender, System.EventArgs e) {
			if ( !Context.User.IsInRole( UserRole.Submitter.ToString() ) ) {
				initRequestInfo();

				if ( !IsPostBack ) {
					SubmitIdTxt.Text = Context.User.Identity.Name;
				}
			} else{
				Visible = false;
			}			
		}

		/// <summary>
		/// Initialize the request controls.
		/// </summary>
		private void initRequestInfo() {
			// Check to see if user entered submitter request.
			SubmitterRequestInfo sri = UsersControl.getSubmitterRequestInfo( Context.User.Identity.Name );

			// If no submitter request, display instructions
			// (input controls are visible by default).
			// Otherwise display request acknowledgement and date.
			if ( sri != null ) {
				FormPanel.Visible = false;
				RequestLbl.Text = "Your request for permission to submit modules was received on "
					+ sri.Date.ToShortDateString() + ".  You will be notified by email when your "
					+ "request has been evaluated.";
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
			this.SubmitterIdCustomVal.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(this.SubmitterIdCustomVal_ServerValidate);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Handle the apply button click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ApplyBtn_Click(object sender, System.EventArgs e) {
			try {
				if ( SubmitterIdCustomVal.IsValid ) {
					SubmitterRequestInfo sri = new SubmitterRequestInfo();

					sri.Date = DateTime.Now;
					sri.UserName = Context.User.Identity.Name;
					sri.Message = Globals.parseTextInput( MessageTxt.Text );
					sri.SubmitterId = SubmitIdTxt.Text;

					UsersControl.addSubmitterRequest( sri );
					initRequestInfo();
				}
			} catch ( Exception ex ) {
				RequestLbl.Text = ex.Message + ex.InnerException.Message;}
		}

		/// <summary>
		/// Validate the requested submitter identifier by checking to see if it alread exists.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		private void SubmitterIdCustomVal_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args) {
			if ( SubmitIdTxt.Text.Length > 0 ) {
				args.IsValid = !UsersControl.submitterIdExists( SubmitIdTxt.Text );
			} else {
				args.IsValid = true;
			}
		}
	}
}
