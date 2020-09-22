namespace SwenetDev.Controls {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;
	using System.Collections.Specialized;
	using DBAdapter;

	/// <summary>
	///		Summary description for Sidebar.
	/// </summary>
	public class Sidebar : System.Web.UI.UserControl {
		protected System.Web.UI.WebControls.DataGrid FacultyGrid;
		protected System.Web.UI.WebControls.DataGrid SubmitterGrid;
		protected System.Web.UI.WebControls.DataGrid ModulesGrid;
		protected System.Web.UI.WebControls.Repeater SEEKAreaRepeater;
		protected System.Web.UI.WebControls.Label UserLabel;
		protected System.Web.UI.WebControls.Panel UserPanel;
		protected System.Web.UI.WebControls.Label EditorLabel;
		protected System.Web.UI.WebControls.Label Editor_FacReqs;
		protected System.Web.UI.WebControls.Label Editor_SubmReqs;
		protected System.Web.UI.WebControls.Label Editor_ModuleReqs;
		protected System.Web.UI.WebControls.Panel EditorPanel;
		protected System.Web.UI.WebControls.Label AdminLabel;
		protected System.Web.UI.WebControls.Label Admin_UserManLabel;
		protected System.Web.UI.WebControls.Label Admin_ChangePassLabel;
		protected System.Web.UI.WebControls.Label Admin_ModuleManLabel;
		protected System.Web.UI.WebControls.Panel AdminPanel;
		protected System.Web.UI.WebControls.HyperLink SEEKLabel;
		protected System.Web.UI.WebControls.Label AllLabel;
		protected System.Web.UI.WebControls.Panel RepeaterPanel;
		protected int userRole;

		private void Page_Load(object sender, System.EventArgs e) {
			if ( !IsPostBack ) {
				SEEKAreaRepeater.DataSource = Globals.SEEKAreas;
				SEEKAreaRepeater.DataBind();

				FacultyGrid.DataSource = FacultyRequests.getFacultyRequests();
				FacultyGrid.DataBind();

				SubmitterGrid.DataSource = SubmitterRequests.getSubmitterRequests();
				SubmitterGrid.DataBind();

				DataTable table = new DataTable();
				table.Columns.Add( new DataColumn( "Id", typeof(int) ) );
				table.Columns.Add( new DataColumn( "Title", typeof(string) ) );
				table.Columns.Add( new DataColumn( "Date", typeof(DateTime) ) );
				table.Columns.Add( new DataColumn( "ApproveUrl", typeof(string) ) );
				table.Columns.Add( new DataColumn( "RejectUrl", typeof(string) ) );

				IList modules = Modules.getAll( ModuleStatus.PendingApproval );
					
				foreach( Modules.ModuleInfo module in modules ) 
				{
					DataRow row = table.NewRow();

					row["Id"] = module.Id;
					row["Title"] = module.Title;
					row["Date"] = module.Date;

					row["ApproveUrl"] = "editorActionEmail.aspx?type=2&username="
						+ module.Submitter + "&approved=true&moduleID=" + module.Id;
					row["RejectUrl"] = "editorActionEmail.aspx?type=2&username="
						+ module.Submitter + "&moduleID=" + module.Id + "&approved=false";
						
					table.Rows.Add( row );
				}

				ModulesGrid.DataSource = table;
				ModulesGrid.DataBind();

				
				}

				userRole = -1;
				if (Context.User.Identity.IsAuthenticated) 
				{

					UserAccounts.UserInfo cui = UserAccounts.getUserInfo(Context.User.Identity.Name);
					userRole = (int) cui.Role;


				}


		}

		#region SEEKArea Load Methods
		public void RepeaterPanel_Ld(object sender, EventArgs e) 
		{

			System.Web.UI.WebControls.Panel myPanel = (System.Web.UI.WebControls.Panel) sender;
			
			if (userRole == -1) 
			{
				SEEKLabel.CssClass = "ViewSEEKStyle";
				myPanel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(-1);");
			}
			else if (userRole == 0) 
			{
				myPanel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(0);");
				SEEKLabel.CssClass = "ViewSEEKStyle0";
			}
			else if (userRole == 1) 
			{
				myPanel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(0);");
				SEEKLabel.CssClass = "ViewSEEKStyle0";
			}
			else if (userRole == 2) 
			{
				myPanel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(2);");
				SEEKLabel.CssClass = "ViewSEEKStyle2";
			}
			else if (userRole == 3) 
			{
				myPanel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(3);");
				SEEKLabel.CssClass = "ViewSEEKStyle3";
			}
			else if (userRole == 4) 
			{
				myPanel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(4);");
				SEEKLabel.CssClass = "ViewSEEKStyle4";
			}
			myPanel.Attributes.Add("OnMouseOut" , "javascript:undisplaySEEK();");

		}

		public void SEEKAreasLabel_Ld(object sender, EventArgs e) 
		{

			System.Web.UI.WebControls.Label myItem = (System.Web.UI.WebControls.Label) sender;

			myItem.Attributes.Add("OnMouseOver", "javascript:highlightLabel(this);");
			myItem.Attributes.Add("OnMouseOut", "javascript:unhighlightLabel(this);");

		}

		public void SEEKLabel_Ld(object sender, EventArgs e) 
		{

			System.Web.UI.WebControls.HyperLink myLabel= (System.Web.UI.WebControls.HyperLink) sender;

			if (userRole == -1) myLabel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(-1);");
			else if (userRole == 0) myLabel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(0);");
			else if (userRole == 1) myLabel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(0);");
			else if (userRole == 2) myLabel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(2);");
			else if (userRole == 3) myLabel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(3);");
			else if (userRole == 4) myLabel.Attributes.Add("OnMouseOver", "javascript:displaySEEK(4);");
			myLabel.Attributes.Add("OnMouseOut",  "javascript:undisplaySEEK();");

		}
		#endregion

		#region User Load Methods
		public void UserPanel_Ld(object sender, EventArgs e) 
		{

			System.Web.UI.WebControls.Panel userPanel = (System.Web.UI.WebControls.Panel) sender;

			userPanel.Attributes.Add("OnMouseOver", "javascript:displayUser();");
			userPanel.Attributes.Add("OnMouseOut" , "javascript:undisplayUser();");

		}

		public void UserLabel_Ld(object sender, EventArgs e) 
		{

			System.Web.UI.WebControls.Label userLabel = (System.Web.UI.WebControls.Label) sender;

			userLabel.Attributes.Add("OnMouseOver", "javascript:displayUser();");
			userLabel.Attributes.Add("OnMouseOut" , "javascript:undisplayUser();");

		}
		#endregion
		
		#region Administrator Load Methods
		public void AdminPanel_Ld(object sender, EventArgs e) 
		{

			System.Web.UI.WebControls.Panel adminPanel = (System.Web.UI.WebControls.Panel) sender;

			adminPanel.Attributes.Add("OnMouseOver", "javascript:displayAdmin();");
			adminPanel.Attributes.Add("OnMouseOut" , "javascript:undisplayAdmin();");

		}

		public void AdminLabel_Ld(object sender, EventArgs e) 
		{

			System.Web.UI.WebControls.Label adminLabel = (System.Web.UI.WebControls.Label) sender;

			adminLabel.Attributes.Add("OnMouseOver", "javascript:displayAdmin();");
			adminLabel.Attributes.Add("OnMouseOut" , "javascript:undisplayAdmin();");

		}
		#endregion

		#region Editor Load Methods
		public void EditorPanel_Ld(object sender, EventArgs e) 
		{

			System.Web.UI.WebControls.Panel editorPanel = (System.Web.UI.WebControls.Panel) sender;

			editorPanel.Attributes.Add("OnMouseOver", "javascript:displayEditor();");
			editorPanel.Attributes.Add("OnMouseOut" , "javascript:undisplayEditor();");

		}

		public void EditorLabel_Ld(object sender, EventArgs e) 
		{

			System.Web.UI.WebControls.Label editorLabel = (System.Web.UI.WebControls.Label) sender;

			editorLabel.Attributes.Add("OnMouseOver", "javascript:displayEditor();");
			editorLabel.Attributes.Add("OnMouseOut" , "javascript:undisplayEditor();");

		}
		#endregion


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
		private void InitializeComponent() {
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
