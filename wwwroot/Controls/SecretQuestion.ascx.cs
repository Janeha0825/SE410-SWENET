namespace SwenetDev.Controls {

	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using SwenetDev.DBAdapter;

	/// <summary>
	///	Summary description for SecretQuestion.
	/// </summary>
	public class SecretQuestion : System.Web.UI.UserControl {

		protected System.Web.UI.WebControls.DropDownList SecretQuestionDdl;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredfieldValidator;
		protected System.Web.UI.WebControls.TextBox txtAnswer;

		private void Page_Load(object sender, System.EventArgs e) {
			
			if( !IsPostBack ) {
				IList questions = Questions.getQuestions();
				SecretQuestionDdl.DataSource = questions;
				SecretQuestionDdl.DataBind();
			}

		}

		/// <summary>
		/// Returns the ID for the selected Secret Question.
		/// </summary>
		/// <returns>The ID for the selected Secret Question.</returns>
		public int getQuestionID() {
			if( SecretQuestionDdl.SelectedIndex == -1 )
				return 0;
			else
				return SecretQuestionDdl.SelectedIndex;
		}

		/// <summary>
		/// Returns the answer to the Secret Question.
		/// </summary>
		/// <returns>The answer to the Secret Question.</returns>
		public string getAnswer() {
			return txtAnswer.Text;
		}

		/// <summary>
		/// This method disables the RequiredFieldValidator for this control
		/// since we don't need them to enter data when editing their user info.
		/// </summary>
		public void disableValidator() {
			RequiredfieldValidator.Enabled = false;
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
