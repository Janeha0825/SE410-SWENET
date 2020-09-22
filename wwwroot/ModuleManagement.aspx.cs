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
	/// The motivation behind letting Admins edit modules is simply to allow for 
	/// minor corrections, such as typographical errors.  No lock is received for
	/// these changes, although current locks are reported.  
	/// 
	/// There is the possibility that a Submitter and an Admin both editing a 
	/// module at the same time will result in the loss of the changes made by 
	/// whoever saves it first. 
	/// 
	/// Admins can delete modules for reasons of site clean-up, such as if a
	/// Submitter abandons his/her modules.
	/// </summary>
	public class ModuleManagement : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label PageMessageLbl;
		protected System.Web.UI.WebControls.DataGrid ModuleGrid;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( User.Identity.IsAuthenticated && User.IsInRole( UserRole.Admin.ToString() ) ) 
			{
				if ( !IsPostBack ) 
				{
					bindGrid();
				} 
				else 
				{
					PageMessageLbl.Text = "";
				}
			} 
			else 
			{
				if ( !User.Identity.IsAuthenticated && User.IsInRole( UserRole.Admin.ToString() ) ) 
				{
					throw new Exception( "Your session has expired." );
				} 
				else 
				{
					throw new Exception( "You are not authorized to view the requested page." );
				}
			}
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
			this.ModuleGrid.ItemCommand += new DataGridCommandEventHandler(this.ModuleGrid_ItemCommand);
			this.ModuleGrid.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.ModuleGrid_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Reveals every module to the Administrator, except for previous versions.
		/// </summary>
		private void bindGrid() 
		{
			try
			{
				ArrayList appModules = (ArrayList)Modules.getAll( ModuleStatus.Approved );
				ArrayList pendModules = (ArrayList)Modules.getAll( ModuleStatus.PendingApproval );
				ArrayList inModules = (ArrayList)Modules.getAll( ModuleStatus.InProgress );
				IList modules = new ArrayList();

				appModules.Sort();
				pendModules.Sort();
				inModules.Sort();

				foreach( Modules.ModuleInfo mi in appModules ) modules.Add( mi );
				foreach( Modules.ModuleInfo mi in inModules ) modules.Add( mi );
				foreach( Modules.ModuleInfo mi in pendModules ) modules.Add ( mi );

				ModuleGrid.DataSource = modules;
				ModuleGrid.DataBind();
			} 
			catch( Exception ex ) 
			{
				PageMessageLbl.Text = "An error occurred while obtaining the "
					+ "desired modules from the database.  Try again.";
			}

		}

		/// <summary>
		/// Handles clicks from links in the Modules table.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void ModuleGrid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) 
		{
			int moduleID = Convert.ToInt32( e.CommandArgument );

			if ( e.CommandName == "Delete" ) 
			{
				ModulesControl.removeModule( moduleID );
				bindGrid();
			} 
			else if ( e.CommandName == "Edit" ) 
			{
				Response.Redirect( "editModule.aspx?moduleID=" + moduleID );
			}
		}

		/// <summary>
		/// Restricts editing access to just those modules that have been approved.
		/// Warns the admin of existing locks on modules before editing.
		/// Prompts the admin when deleting modules.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ModuleGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) {
			if ( e.Item.ItemType != ListItemType.Header &&
						e.Item.ItemType != ListItemType.Footer &&
								e.Item.ItemType != ListItemType.Separator ) {
				
				HyperLink module = (HyperLink)e.Item.Cells[0].FindControl( "ModuleLink" );
				LinkButton edit = (LinkButton)e.Item.Cells[1].FindControl( "EditLink" );
				LinkButton delete = (LinkButton)e.Item.Cells[2].FindControl( "DeleteLink" );
				string status = ( (Modules.ModuleInfo)e.Item.DataItem ).Status.ToString();

				//handle the edit button
				if( !status.Equals( ModuleStatus.Approved.ToString() ) ) {
					edit.Enabled = false;
				} else {
					string url = module.NavigateUrl;
					int IDplace = url.IndexOf( '=' ) + 1;
					string modID = url.Substring( IDplace );
					string coBy = Modules.getModuleInfo( Convert.ToInt32( modID ) ).LockedBy;
					if( !coBy.Equals( "" ) ) 
					{
						edit.Attributes.Add( "onClick", "if ( !confirm('This module is already checked out. Changes you make may be lost in the next revision.') ) return false;" );
					}
				}

				//handle the delete button
				delete.Attributes.Add( "onClick", "if ( !confirm( 'Are you sure you want to permanently delete this module?' ) ) return false;" );
			}
		}
	}
}