namespace SwenetDev.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using SwenetDev.DBAdapter;

	public enum UserInfoEditMode { New, Existing };

	/// <summary>
	///		Summary description for EditUserInfo.
	/// </summary>
	public class EditUserInfoControl : System.Web.UI.UserControl {
		protected System.Web.UI.WebControls.TextBox Webpage;
		protected System.Web.UI.WebControls.TextBox Fax2;
		protected System.Web.UI.WebControls.TextBox Phone3;
		protected System.Web.UI.WebControls.TextBox Phone2;
		protected System.Web.UI.WebControls.TextBox Phone1;
		protected System.Web.UI.WebControls.TextBox Zip;
		protected System.Web.UI.WebControls.TextBox State;
		protected System.Web.UI.WebControls.TextBox City;
		protected System.Web.UI.WebControls.TextBox Address2;
		protected System.Web.UI.WebControls.TextBox Address1;
		protected System.Web.UI.WebControls.RequiredFieldValidator AffilRequiredValidator;
		protected System.Web.UI.WebControls.TextBox Affiliation;
		protected System.Web.UI.WebControls.TextBox Title;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator1;
		protected System.Web.UI.WebControls.RequiredFieldValidator EmailRequiredValidator;
		protected System.Web.UI.WebControls.TextBox Email;
		protected System.Web.UI.WebControls.RequiredFieldValidator NameRequiredValidator;
		protected System.Web.UI.WebControls.TextBox Name;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator3;
		protected System.Web.UI.WebControls.TextBox txtPasswordConfirm;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.ValidationSummary ValidationSummary1;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.Label InstructionsLbl;
		protected System.Web.UI.WebControls.TextBox Country;
		protected System.Web.UI.WebControls.TextBox Fax1;
		protected System.Web.UI.WebControls.CustomValidator PasswordCompareValidator;
		protected System.Web.UI.WebControls.RegularExpressionValidator AlphaNumericUsername;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator2;
		protected SwenetDev.Controls.SecretQuestion QuestionControl;
		protected System.Web.UI.WebControls.RegularExpressionValidator RegExValidator1;
		

		protected const string PHONE_DELIMS = "() -";

		private void Page_Load(object sender, System.EventArgs e) {
			// Put user code to initialize the page here
		}

		public UserInfoEditMode EditMode {
			get { return (UserInfoEditMode)ViewState["EditMode"]; }
			set {
				ViewState["EditMode"] = value;

				// Cannot change username
				if ( value == UserInfoEditMode.Existing ) {
					EnsureChildControls();
					txtUserName.Enabled = false;
					InstructionsLbl.Text = "Your username cannot be changed.  "
						+ "Only enter your password and/or secret question "
						+ "information if you wish to change them.  "
						+ "The name, email, and affiliation fields are required.";
					RequiredFieldValidator2.Enabled = false;
					RequiredFieldValidator3.Enabled = false;
					QuestionControl.disableValidator();
				}
			}
		}

		/// <summary>
		/// Get or set the user info associated with this control.  Either
		/// get the values from the input controls, or set the values
		/// on the input controls.
		/// </summary>
		public UserAccounts.UserInfo UserInfo {
			get {
				string webpage = Webpage.Text == "http://" ? "" : Webpage.Text;

				UserAccounts.UserInfo ui = new UserAccounts.UserInfo();
				ui.Username = txtUserName.Text;
				ui.Name = Globals.parseTextInput( Name.Text );
				ui.Email = Globals.parseTextInput( Email.Text );
				ui.Title = Globals.parseTextInput( Title.Text );
				ui.Affiliation = Globals.parseTextInput( Affiliation.Text );
				ui.Street1 = Globals.parseTextInput( Address1.Text );
				ui.Street2 = Globals.parseTextInput( Address2.Text );
				ui.City = Globals.parseTextInput( City.Text );
				ui.State = Globals.parseTextInput( State.Text );
				ui.Zip = Globals.parseTextInput( Zip.Text );
				ui.Country = Globals.parseTextInput( Country.Text );
				ui.PhoneCountryCode = Globals.parseTextInput( Phone1.Text );
				ui.Phone = Globals.parseTextInput( Phone2.Text );
				ui.PhoneExtension = Globals.parseTextInput( Phone3.Text );
				ui.FaxCountryCode = Globals.parseTextInput( Fax1.Text );
				ui.Fax = Globals.parseTextInput( Fax2.Text );
				ui.Webpage = Globals.parseTextInput( webpage );
				ui.Password = txtPassword.Text;
				ui.QuestionID = QuestionControl.getQuestionID();
				ui.QuestionAnswer = QuestionControl.getAnswer();

				return ui;
			}
			set {
				txtUserName.Text = Globals.formatTextOutput( value.Username );
				Name.Text = Globals.formatTextOutput( value.Name );
				Email.Text = Globals.formatTextOutput( value.Email );
				Title.Text = Globals.formatTextOutput( value.Title );
				Affiliation.Text = Globals.formatTextOutput( value.Affiliation );
				Address1.Text = Globals.formatTextOutput( value.Street1 );
				Address2.Text = Globals.formatTextOutput( value.Street2 );
				City.Text = Globals.formatTextOutput( value.City );
				State.Text = Globals.formatTextOutput( value.State );
				Zip.Text = Globals.formatTextOutput( value.Zip );
				Country.Text = Globals.formatTextOutput( value.Country );
				Phone1.Text = Globals.formatTextOutput( value.PhoneCountryCode );
				Phone2.Text = Globals.formatTextOutput( value.Phone );
				Phone3.Text = Globals.formatTextOutput( value.PhoneExtension );
				Fax1.Text = Globals.formatTextOutput( value.FaxCountryCode );
				Fax2.Text = Globals.formatTextOutput( value.Fax );
				
				if ( value.Webpage != string.Empty ) {
					Webpage.Text = Globals.formatTextOutput( value.Webpage );
				}
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
			this.PasswordCompareValidator.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(this.PasswordCompareValidator_ServerValidate);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void PasswordCompareValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args) {
			args.IsValid = txtPassword.Text == txtPasswordConfirm.Text;
		}

	}
}
