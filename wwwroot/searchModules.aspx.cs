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
	/// Summary description for searchModules.
	/// </summary>
	public class searchModules : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblResults;
		protected System.Web.UI.WebControls.Button SearchBtn;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Repeater ResultsRepeater;
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.WebControls.TextBox txtSearch;
		protected System.Web.UI.WebControls.DropDownList ddlFields;
		protected System.Web.UI.WebControls.Label lblNoResults;
	
		private void Page_Load(object sender, System.EventArgs e) {
			
			if( !IsPostBack ) {
				// Initialize all the components
				initDdl();
				txtSearch.Text = "";
				lblResults.Visible = false;
				lblNoResults.Visible = false;
			}
		}

		/// <summary>
		/// Initializes the Fields dropdown list
		/// </summary>
		private void initDdl() {
			ddlFields.Items.Add( "-- Choose field --" );
			ddlFields.Items.Add( "Author" );
			ddlFields.Items.Add( "Submitter" );
			ddlFields.Items.Add( "Title" );
			ddlFields.Items.Add( "Module Identifier" );
			ddlFields.Items.Add( "Submitter Identifier" );
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
			this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Handles Search button-click events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchBtn_Click(object sender, System.EventArgs e) {
		 
			IList results = new ArrayList();
			
			if( ddlFields.SelectedIndex == 0 ) {
				// If they didn't choose a search field, tell them.
				lblError.Text = "Please select a field from the dropdown list.";
				lblResults.Visible = false;
				lblNoResults.Visible = false;
			} else if( txtSearch.Text == "" ) {
				// If they entered a search field but no text, tell them.
				lblError.Text = "Search field may not be blank.";
				lblResults.Visible = false;
				lblNoResults.Visible = false;
			} else {
				lblError.Text = "";

				// Search DB according to search criteria
				results = Modules.getModuleIDs( txtSearch.Text, ddlFields.SelectedIndex - 1 );

				if( results.Count == 0 ) {
					lblResults.Visible = false;
					lblNoResults.Visible = true;
				} else {
					lblResults.Visible = true;
					lblNoResults.Visible = false;
				}
			}

			// Add any matches to the results list
			ResultsRepeater.DataSource = results;
			ResultsRepeater.DataBind();
		}
	}
}
