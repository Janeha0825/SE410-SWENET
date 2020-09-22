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
	/// Summary description for EditorsPage.
	/// </summary>
	public class EditorsPage : System.Web.UI.Page {
		protected System.Web.UI.WebControls.DataGrid FacultyGrid;
		protected System.Web.UI.WebControls.DataGrid SubmitterGrid;
		protected System.Web.UI.WebControls.Label PageMessageLbl;
		protected System.Web.UI.WebControls.DataGrid ModulesGrid;
	
		private void Page_Load(object sender, System.EventArgs e) {
			if ( User.Identity.IsAuthenticated && User.IsInRole( UserRole.Editor.ToString() ) ) {
				if ( !IsPostBack ) {
					if ( Request.QueryString["message"] != null ) {
						PageMessageLbl.ForeColor = Color.Green;
						PageMessageLbl.Text = "<p>" + Request.QueryString["message"] + "</p>";
					}

					FacultyGrid.DataSource = FacultyRequests.getFacultyRequests();
					FacultyGrid.DataBind();

					SubmitterGrid.DataSource = SubmitterRequests.getSubmitterRequests();
					SubmitterGrid.DataBind();

					DataTable table = new DataTable();
					table.Columns.Add( new DataColumn( "Id", typeof(int) ) );
					table.Columns.Add( new DataColumn( "Title", typeof(string) ) );
					table.Columns.Add( new DataColumn( "UserName", typeof(string) ) );
					table.Columns.Add( new DataColumn( "Date", typeof(DateTime) ) );
					table.Columns.Add( new DataColumn( "ApproveUrl", typeof(string) ) );
					table.Columns.Add( new DataColumn( "RejectUrl", typeof(string) ) );

					IList modules = Modules.getAll( ModuleStatus.PendingApproval );
					
					foreach( Modules.ModuleInfo module in modules ) {
						DataRow row = table.NewRow();

						row["Id"] = module.Id;
						row["Title"] = module.Title;
						row["Date"] = module.Date;
						row["UserName"] = module.Submitter;

						row["ApproveUrl"] = "editorActionEmail.aspx?type=2&username="
								+ module.Submitter + "&approved=true&moduleID=" + module.Id;
						row["RejectUrl"] = "editorActionEmail.aspx?type=2&username="
								+ module.Submitter + "&moduleID=" + module.Id + "&approved=false";
						
						table.Rows.Add( row );
					}

					ModulesGrid.DataSource = table;
					ModulesGrid.DataBind();
				} else {
					PageMessageLbl.Text = "";
				}
			} else {
				if ( !User.Identity.IsAuthenticated && User.IsInRole( UserRole.Editor.ToString() ) ) {
					throw new Exception( "Your session has expired." );
				} else {
					throw new Exception( "You are not authorized to view the requested page." );
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
			this.FacultyGrid.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.FacultyGrid_ItemCommand);
			this.SubmitterGrid.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.SubmitterGrid_ItemCommand);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void FacultyGrid_ItemCommand(object source, DataGridCommandEventArgs e) 
		{
			TableCell cell = e.Item.Cells[2];
			LinkButton link = (LinkButton)cell.FindControl("ShowHideBtn0");
			Label proofLabel = (Label)cell.FindControl("ProofLbl");

			if ( proofLabel.Visible == true ) 
			{
				// hide it
				proofLabel.Visible = false;
				link.Text = "Show Evidence";
			} 
			else 
			{ 
				// show it
				proofLabel.Visible = true;
				link.Text = "Hide Evidence";
			}
		}

	
		private void SubmitterGrid_ItemCommand(object source, DataGridCommandEventArgs e) {
			TableCell cell = e.Item.Cells[2];
			LinkButton link = (LinkButton)cell.FindControl("ShowHideBtn");
			Label messageLabel = (Label)cell.FindControl("MessageLbl");

			if ( messageLabel.Visible == true ) {
				// hide it
				messageLabel.Visible = false;
				link.Text = "Show Message";
			} else { 
				// show it
				messageLabel.Visible = true;
				link.Text = "Hide Message";
			}
		}
	}
}
