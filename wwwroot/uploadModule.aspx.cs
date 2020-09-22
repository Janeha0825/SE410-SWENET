using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace SwenetDev {
	using DBAdapter;

	/// <summary>
	/// Summary description for uploadModule.
	/// </summary>
	public class uploadModule : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Button ContinueBtn;
		protected System.Web.UI.WebControls.Label ModuleLbl;
		protected System.Web.UI.WebControls.RadioButtonList RadioButtonList1;
	
		private void Page_Load(object sender, System.EventArgs e) {
			if ( Session["EditModule"] == null ) {
				Response.Redirect( "editModule.aspx", true );
			} else {
				Modules.ModuleInfo mod =
					(Modules.ModuleInfo)Session["EditModule"];

				if ( mod.Title == string.Empty ) {
					ModuleLbl.Text = ", Untitled, ";
				} else {
					ModuleLbl.Text = ", \"" + mod.Title + ",\" ";
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {    
			this.ContinueBtn.Click += new System.EventHandler(this.ContinueBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Handles Continue button-click events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContinueBtn_Click(object sender, System.EventArgs e) {
			Modules.ModuleInfo mod = (Modules.ModuleInfo)Session["EditModule"];
			string queryParam = "";

			if ( RadioButtonList1.SelectedIndex == 0 ) { // Edit
				queryParam = "?moduleID=" + mod.Id;
			} else { // Delete
				deleteMaterials( mod.Id );
				ModulesControl.removeModule( mod.Id );
				Session.Remove("EditModule");
			}

			Response.Redirect( "editModule.aspx" + queryParam, true );
		}

		/// <summary>
		/// Deletes all materials associated with the given ModuleID
		/// </summary>
		/// <param name="ModuleID">The ModuleID of the module.</param>
		private void deleteMaterials( int ModuleID ) {
			string path = System.Configuration.ConfigurationSettings.AppSettings["MaterialsDir"] + ModuleID + "\\";
			
			if( Directory.Exists(path) ) {
				string[] filenames = Directory.GetFiles( path );
				int pos = 0;

				try {
					foreach( string filename in filenames ) {
						File.Delete( filename );
						pos++;
					}
				} catch (Exception duh) {
					Response.Redirect( "MyAccount.aspx?message=UploadModule: An error occured while trying to remove existing material: " + filenames[pos] );
				}
			}
		}
	}
}
