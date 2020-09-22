namespace SwenetDev.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AspNetForums.Components;
	using SwenetDev.DBAdapter;

	/// <summary>
	///	A control encapsulating module ratings.  This includes current
	///	the current rating average and count, a user's rating, if available,
	///	and a form for submitting a new rating and comments.  Only logged in
	///	users can rate a module.  Authors or submitters of a module cannot
	///	rate it.  A module may be rated only once by a user.
	/// </summary>
	public class RateModuleControl : System.Web.UI.UserControl {
		protected System.Web.UI.WebControls.RadioButton Rating1;
		protected System.Web.UI.WebControls.RadioButton Rating2;
		protected System.Web.UI.WebControls.RadioButton Rating3;
		protected System.Web.UI.WebControls.RadioButton Rating4;
		protected System.Web.UI.WebControls.RadioButton Rating5;
		protected System.Web.UI.WebControls.TextBox SubjectTxtBox;
		protected System.Web.UI.WebControls.TextBox CommentsTxtBox;
		protected System.Web.UI.WebControls.Button SubmitBtn;
		protected System.Web.UI.WebControls.Label AveRatingLbl;
		protected System.Web.UI.WebControls.Label ErrorMessage;
		protected System.Web.UI.WebControls.Label NumRatingsLbl;
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.HtmlControls.HtmlTable RatingsTable;
		private ModuleRatingInfo ratingInfo;
		protected System.Web.UI.WebControls.HyperLink YourRatingLnk;
		protected System.Web.UI.WebControls.Label YourRatingLbl;
		protected System.Web.UI.WebControls.HyperLink RateLoginLnk;
		private Rating userRating = null;

		/// <summary>
		/// Gets or sets a module's rating information that this control displays.
		/// </summary>
		public ModuleRatingInfo RatingInfo {
			get {
				if ( ratingInfo != null ) {
					return ratingInfo;
				} else {
					return (ModuleRatingInfo)ViewState["RateModuleControl_ModuleRatingInfo"];
				}
			}
			set {
				ratingInfo = value;
				ViewState["RateModuleControl_ModuleRatingInfo"] = value;
				
				if ( value != null ) {
					EnsureChildControls();
					HyperLink1.NavigateUrl = AspNetForums.Components.Globals.UrlShowPost + RatingInfo.ThreadID;
					AveRatingLbl.Text = Convert.ToString( RatingInfo.Rating.ToString( "#.#" ) );
					NumRatingsLbl.Text = Convert.ToString( RatingInfo.NumRatings );
				}
			}
		}

		/// <summary>
		/// Gets or sets whether a rating can be added for a module.
		/// This should be disabled, or set to false if a given user
		/// has already rated a module, or a user is the submitter or
		/// an author of a  module.
		/// </summary>
		public bool AddRatingEnabled {
			get {
				EnsureChildControls();
				return RatingsTable.Visible;
			}
			set {
				EnsureChildControls();
				RatingsTable.Visible = value;
			}
		}

		/// <summary>
		/// Gets or sets a user's rating this control displays for a module.
		/// If a particular user hasn't rated the module, it will be null.
		/// </summary>
		public Rating UserRating {
			get {
				if ( userRating != null ) {
					return userRating;
				} else {
					return (Rating)ViewState["RateModuleControl_UserRating"];
				}
			}
			set {
				userRating = value;
				ViewState["RateModuleControl_UserRating"] = value;

				if ( value != null ) {
					EnsureChildControls();
					AddRatingEnabled = false;
					YourRatingLbl.Text = UserRating.Value.ToString();
					YourRatingLnk.NavigateUrl = AspNetForums.Components.Globals.UrlShowPost + UserRating.PostID;
				}
			}
		}

		/// <summary>
		/// Initialize this page.  Sets the login link's URL if the user is
		/// not logged in.
		/// </summary>
		/// <param name="sender">The sender of this event.</param>
		/// <param name="e">The event arguments.</param>
		private void Page_Load(object sender, System.EventArgs e) {
			// Only logged in users can rate a module.
			if ( !Context.User.Identity.IsAuthenticated ) {
				AddRatingEnabled = false;
				RateLoginLnk.NavigateUrl = "~/login.aspx?ReturnUrl=" + HttpUtility.UrlEncode( Parent.Page.Request.RawUrl );
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
			this.SubmitBtn.Click += new System.EventHandler(this.SubmitBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Respond to the button event for submitting a rating.
		/// </summary>
		/// <param name="sender">The sender of this event.</param>
		/// <param name="e">The event arguments.</param>
		private void SubmitBtn_Click(object sender, System.EventArgs e) {
			// An array of the radio buttons.
			RadioButton [] buttons = new RadioButton[5];
			buttons[0] = Rating1;
			buttons[1] = Rating2;
			buttons[2] = Rating3;
			buttons[3] = Rating4;
			buttons[4] = Rating5;
			int selected = 0;

			for ( int i = 0; i < buttons.Length && selected == 0; i++ ) {
				if ( buttons[i].Checked ) {
					selected = i + 1;
				}
			}

			try {
				if ( selected != 0 ) {
					Post post = null;

					if ( ( SubjectTxtBox.Text != String.Empty  && CommentsTxtBox.Text != String.Empty ) ||
						( SubjectTxtBox.Text == String.Empty  && CommentsTxtBox.Text == String.Empty ) ) {
						// Always create a post.  If no comments, an empty post will be added.
						post = new Post();
						post.Subject = SwenetDev.Globals.parseTextInput( SubjectTxtBox.Text );
						post.Body = SwenetDev.Globals.parseTextInput( CommentsTxtBox.Text );

						// Create the rating.
						Rating rating = new Rating();
						rating.Value = selected;

						// Add the rating.
						RatingInfo = ModuleRatingsControl.addRating( RatingInfo, post, rating );
						//ErrorMessage.Text = "<p>Rating Added.</p>";
						UserRating = ModuleRatingsControl.getRatingForUser(
							Context.User.Identity.Name, RatingInfo );

						// Get the page to refresh and display ratings.
						bool valid = this.Parent.Page.IsValid;
						this.Visible = true;
					} else {
						ErrorMessage.Text = "<p>You must provide both a subject and comments or neither.</p>";
					}
				} else {
					ErrorMessage.Text = "<p>You must select a rating.</p>";
				}
			} catch ( Exception ex ) {
				ErrorMessage.Text = "<p>An error occurred while adding your rating.  Try again.</p>  " + ex.Message;
			}
		}
	}
}
