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
	/// <summary>
	/// This page is to view revision comments for a module.  Comments may
	/// be viewed by all users, but only editors can view previous versions,
	/// which are linked to from the version column.
	/// </summary>
	public class ViewComments : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Label TitleLabel;
		protected System.Web.UI.WebControls.DataGrid CommentsGrid;
	
		private void Page_Load(object sender, System.EventArgs e) {
			if ( !IsPostBack ) {
				string baseIDStr = Request.QueryString["baseID"];

				if ( baseIDStr != null ) {
					int baseID = Convert.ToInt32( baseIDStr );
					bool include = User.IsInRole( UserRole.Editor.ToString() );
					IList list = Modules.getModuleVersions( baseID, include );

					if ( list != null ) {
						TitleLabel.Text = ((Modules.ModuleInfo)list[0]).Title;
					}
					
					CommentsGrid.DataSource = list;
					CommentsGrid.DataBind();
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
