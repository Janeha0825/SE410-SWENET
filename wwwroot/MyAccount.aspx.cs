using System;
using System.Collections;
using System.Collections.Specialized;
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
	/// Summary description for MyAccount.
	/// </summary>
	public class MyAccount : System.Web.UI.Page {
		protected System.Web.UI.WebControls.DataGrid ModulesGrid;
		protected System.Web.UI.WebControls.Panel MyModulesPanel;
		protected System.Web.UI.WebControls.LinkButton CancelLink;
		protected System.Web.UI.WebControls.Label ErrorMessage;

		private static string NotLocked {
			get { return "None"; }
		}
	
		private void Page_Load(object sender, System.EventArgs e) {
			if ( User.Identity.IsAuthenticated ) {
				if ( !IsPostBack ) {
					bindModuleGrid();

					string message = Request.QueryString["message"];

					if ( message != null ) {
						ErrorMessage.Text = "<p>" + message + "</p>";
					}
				}
			} else {
				throw new Exception( "User not authenticated." );
			}
		}

		/// <summary>
		/// Extracts info from the ModuleInfo object and sets members of
		/// the DataRow object accordingly
		/// </summary>
		/// <param name="row">The row of data to be set.</param>
		/// <param name="module">The ModuleInfo object associated with the
		/// corresponding module.</param>
		private static void initRow( DataRow row, Modules.ModuleInfo module ) {
			row["Id"] = module.Id;
			row["Title"] = module.Title;
			row["Version"] = module.Version;
			//row["Date"] = module.Date;
			row["LockedBy"] = module.LockedBy;

			if ( module.LockedBy == "" ) {
				row["LockedBy"] = NotLocked;
			}

			row["BaseId"] = module.BaseId;

			if ( module.Title == string.Empty ) {
				row["Title"] = "(No Title)";
			}

			row["Status"] = module.Status;
		}

		/// <summary>
		/// Sets members of the given data row according to the ModuleInfo associated
		/// with the BaseId member of the data row.
		/// </summary>
		/// <param name="dr">The data row to modify.</param>
		private static void initCurrentVersionRow( DataRow dr ) {
			Modules.ModuleInfo current = Modules.getModuleCurrentVersion( (int)dr["BaseId"] );
			dr["Version"] = current.Version;
			dr["Id"] = current.Id;
			dr["Date"] = current.Date;
		}

		private void bindModuleGrid() {
			DataTable table = new DataTable();
			//table.Columns.Add( new DataColumn( "ModuleInfo", typeof(int) ) );
			table.Columns.Add( new DataColumn( "BaseId", typeof(int) ) );
			table.Columns.Add( new DataColumn( "Id", typeof(int) ) );
			table.Columns.Add( new DataColumn( "Title", typeof(string) ) );
			table.Columns.Add( new DataColumn( "Version", typeof(int) ) );
			table.Columns.Add( new DataColumn( "Date", typeof(DateTime) ) );
			table.Columns.Add( new DataColumn( "LockedBy", typeof(string) ) );
			table.Columns.Add( new DataColumn( "Status", typeof(string) ) );
			table.Columns.Add( new DataColumn( "Author", typeof(string) ) );
			table.Columns.Add( new DataColumn( "Submitter", typeof(string) ) );
			DataColumn [] primaryKeys = new DataColumn[2];
			primaryKeys[0] = table.Columns["Id"];
			//primaryKeys[1] = table.Columns["BaseId"];
			table.PrimaryKey = primaryKeys;

			// Get all modules authored by the current user.

			IList authoredModules =
				Modules.getModulesByAuthor( User.Identity.Name, ModuleStatus.All );

			// Add modules to the table.
            foreach ( Modules.ModuleInfo module in authoredModules ) {
				DataRow row = table.NewRow();
				initRow( row, module );
				row["Author"] = "A";
				row["Submitter"] = "";
				table.Rows.Add( row );
			}

			// Get all modules submitted by the current user.

			IList submittedModules =
				Modules.getModulesBySubmitter( User.Identity.Name, ModuleStatus.All );

			// Add modules to the table if they're not already present.  If it's already
			// there, the user must have been an author, too, so set the S column.
			foreach ( Modules.ModuleInfo module in submittedModules ) {
				DataRow row = null;

				if ( table.Rows.Contains( module.Id ) ) {
					row = table.Rows.Find( module.Id );
				} else {
					row = table.NewRow();
					initRow( row, module );
					table.Rows.Add( row );
					row["Author"] = "";
				}
                    
				row["Submitter"] = "S";
			}

			HybridDictionary dict = new HybridDictionary();

			foreach ( DataRow dr in table.Rows ) {
				int baseId = (int)dr["BaseId"];
				object[] dictRow = (object[])dict[baseId];

				// If there's already a row (module) with the given base id...
				if ( dictRow != null ) {
					// If the version is later, replace it and carry over submitter indicator if necessary.
					if ( (int)dr["Version"] > (int)dictRow[table.Columns["Version"].Ordinal] ) {
						if ( dictRow[table.Columns["Submitter"].Ordinal].ToString().Length != 0 ) {
							dr["Submitter"] = "S";
						}
						dict[baseId] = dr.ItemArray;
						// Authors can't be removed, so if a user was an author on an earlier
						// version, they should still be listed as an author.
					}
				} else {
					dict[baseId] = dr.ItemArray;
				}
			}

			table.Clear();

			// Add each of the keys/values from the collection we created to the table
			foreach ( object[] values in dict.Values ) {
				DataRow row;
				row = table.Rows.Add( values );
				initCurrentVersionRow( row );
			}

			ModulesGrid.DataSource = table;
			ModulesGrid.DataBind();
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
			this.ModulesGrid.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.ModulesGrid_ItemCommand);
			this.ModulesGrid.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.ModulesGrid_ItemDataBound);
			this.CancelLink.Click += new System.EventHandler(this.CancelLink_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Handles the binding of data to the the "My Modules" table.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ModulesGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) {
			if ( e.Item.ItemType != ListItemType.Header &&
				e.Item.ItemType != ListItemType.Footer &&
				e.Item.ItemType != ListItemType.Separator ) {

				DataRow module = ((DataRowView)e.Item.DataItem).Row;
				LinkButton delete = (LinkButton)e.Item.Cells[2].FindControl( "DeleteLink" );
				LinkButton edit = (LinkButton)e.Item.Cells[1].FindControl( "EditLink" );
				LinkButton undo = (LinkButton)e.Item.Cells[1].FindControl( "UndoLink" );
				string status = (string)module["Status"];
				bool isSubmitter = User.IsInRole( UserRole.Submitter.ToString() );
				string lockedBy = (string)module["LockedBy"];

				// Handle Edit link button.
				if ( isSubmitter
					 && ( (status != ModuleStatus.PendingApproval.ToString() && lockedBy == User.Identity.Name)
					      || (status == ModuleStatus.Approved.ToString() && lockedBy == NotLocked)
						  || (status == ModuleStatus.InProgress.ToString() && (int)module["Version"] == 1) ) ) {
					// If not editing an in-progress module..
					if ( status != ModuleStatus.InProgress.ToString() &&
						!( lockedBy == User.Identity.Name && status == ModuleStatus.Approved.ToString() ) ) {
							edit.Attributes.Add( "onClick", "if ( !confirm('Would you like to check out this module for editing?') ) return false;" );
					}
				} else {
					edit.Enabled = false;
				}

				// Handle Delete link button.
				if ( isSubmitter
					&& status == ModuleStatus.InProgress.ToString() ) {
					delete.Attributes.Add( "onClick", "if ( !confirm( 'Are you sure you want to permanently delete this module?' ) ) return false;" );
				} else {
					delete.Enabled = false;
				}

				// Handle Undo link button.
				if ( isSubmitter
					&& status != ModuleStatus.PendingApproval.ToString()
					&& lockedBy == User.Identity.Name
					&& ( (int)module["Version"] != 1 || User.IsInRole( UserRole.Admin.ToString() ) ) ) {
					undo.Attributes.Add( "onClick", "if ( !confirm( 'Are you sure you want to delete this module version?  Any changes that you have made will be discarded.' ) ) return false;" );
				} else {
					undo.Enabled = false;
				}
			}
		}

		/// <summary>
		/// Handles clicks from links in the "My Modules" table.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void ModulesGrid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) {
			int moduleID = Convert.ToInt32( e.CommandArgument );

			if ( e.CommandName == "Delete" ) {
				ModulesControl.removeModule( moduleID );
				bindModuleGrid();
			} else if ( e.CommandName == "Edit" ) {
				if( !User.IsInRole( "Administrator" ) ) {
					ModulesControl.checkOutModule( moduleID, User.Identity.Name );
				}
				Response.Redirect( "editModule.aspx?moduleID=" + moduleID );
			} else if ( e.CommandName == "Undo" ) {
				ModulesControl.undoCheckOut( moduleID );
				bindModuleGrid();
			}

		}

		/// <summary>
		/// Handles button-click events for the Cancel membership button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CancelLink_Click(object sender, System.EventArgs e) {
			Session["CancelType"] = "Cancel";
			Response.Redirect( "AccountCanceled.aspx" );
		}



	}
}