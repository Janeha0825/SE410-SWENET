namespace SwenetDev.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using SwenetDev.DBAdapter;

	/// <summary>
	///	A control that displays user information.
	/// </summary>
	public class ViewUserInfoControl : System.Web.UI.UserControl {
		protected System.Web.UI.WebControls.Label UsernameLbl;
		protected System.Web.UI.WebControls.Label NameLbl;
		protected System.Web.UI.WebControls.Label TitleLbl;
		protected System.Web.UI.WebControls.Label AffiliationLbl;
		protected System.Web.UI.WebControls.Label Address1Lbl;
		protected System.Web.UI.WebControls.Label Address2Lbl;
		protected System.Web.UI.WebControls.Label CityLbl;
		protected System.Web.UI.WebControls.Label StateLbl;
		protected System.Web.UI.WebControls.Label ZipLbl;
		protected System.Web.UI.WebControls.Label PhoneLbl;
		protected System.Web.UI.WebControls.Label FaxLbl;
		protected System.Web.UI.WebControls.HyperLink WebpageLnk;
		protected System.Web.UI.WebControls.Label CountryLbl;
		protected System.Web.UI.WebControls.Label PhoneCountryLbl;
		protected System.Web.UI.WebControls.Label PhoneExtensionLbl;
		protected System.Web.UI.WebControls.Label FaxCountryLbl;
		protected System.Web.UI.WebControls.Label ExtensionLbl;
		protected System.Web.UI.WebControls.Label EmailLbl;

		private UserAccounts.UserInfo userInfo;

		/// <summary>
		/// Get or set the user information displayed.
		/// </summary>
		public UserAccounts.UserInfo UserInfo {
			get { return userInfo; }
			set {
				EnsureChildControls();
				userInfo = value;

				if ( value != null ) {
					UsernameLbl.Text = value.Username;

					// Only logged in users can view extended user information.
					if ( Context.User.Identity.IsAuthenticated ) {
						NameLbl.Text = value.Name;
						int position = value.Email.IndexOf('@');
						String account = value.Email.Substring(0, position);
						String domain = value.Email.Substring(position + 1);
						EmailLbl.Text = "<script>generateEmail( \"" + account + "\", \"" + domain + "\" )</script>";
						TitleLbl.Text = value.Title;
						AffiliationLbl.Text = value.Affiliation;
						Address1Lbl.Text = value.Street1.Length > 0 ? value.Street1 + "<br/>" : "";
						Address2Lbl.Text = value.Street2.Length > 0 ? value.Street2 + "<br/>" : "";
						CityLbl.Text = value.City.Length > 0 ? value.City + ", " : "";
						StateLbl.Text = value.State;
						ZipLbl.Text = value.Zip;
						CountryLbl.Text = value.Country;
						PhoneCountryLbl.Text = value.PhoneCountryCode;
						PhoneLbl.Text = value.Phone;
						PhoneExtensionLbl.Text = value.PhoneExtension;
						if ( value.PhoneExtension == "" ) { ExtensionLbl.Visible = false; }
						FaxCountryLbl.Text = value.FaxCountryCode;
						FaxLbl.Text  = value.Fax;
						WebpageLnk.Text = value.Webpage;
						WebpageLnk.NavigateUrl = value.Webpage;
					}
				}
			}
		}

		private void Page_Load(object sender, System.EventArgs e) {
			// Put user code to initialize the page here
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
