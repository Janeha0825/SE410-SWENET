namespace SwenetDev.Controls 
{
	using System;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.IO;
	using SwenetDev.DBAdapter;
	using System.Configuration;
	using System.Collections;

	// for process stuff
	using System.Diagnostics;

	/// <summary>
	///	A control for adding and editing materials for a SWEnet module.
	/// </summary>
	public class MaterialsControlTest : System.Web.UI.UserControl, IEditControl 
	{
		
		public static int MaxLength 
		{
			get{ return DBAdapter.Modules.getColumnMaxLength("Materials","IdentifyingInfo"); }
		}

		protected DataEditControl MaterialsEditor;
   
		void Page_Load(Object sender, EventArgs e) 
		{
			MaterialsEditor.Text = "";
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() 
		{
			this.MaterialsEditor.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.MaterialsEditor_UpdateCommand);
			this.MaterialsEditor.NewItemEvent += new System.EventHandler(this.MaterialsEditor_NewItemEvent);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// Respond to the update event, when an item is being edited and
		/// subsequently updated, by updating the info object of the
		/// item being edited and saving the file to a temporary location.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void MaterialsEditor_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) 
		{
			int index = (int)e.Item.ItemIndex;
			Materials.MaterialInfo mi = ((Materials.MaterialInfo)MaterialsEditor.DataList[index]);

			TextBox infoBox = (TextBox)e.Item.Cells[1].FindControl( "InfoTextbox" );
			mi.IdentInfo = Globals.parseTextInput( infoBox.Text );

			HtmlInputFile f = (HtmlInputFile)(e.Item.Cells[1].FindControl( "FileUpload" ));

			// Determine the original filename, removing the path.
			string fnameWithPath = f.PostedFile.FileName;
			int pos = fnameWithPath.LastIndexOf( "\\" ) + 1;
			string fname = fnameWithPath.Substring( pos );
			mi.Link = fname;

			// Setup the destination path and create the temporary directory
			// for the user if it doesn't exist.  Delete any file that may be there,
			// then save the file.

			string destinationDir = ConfigurationSettings.AppSettings["MaterialsTempDir"] +
				Context.User.Identity.Name + "\\";

			try 
			{
			
				if ( !Directory.Exists( destinationDir ) ) 
				{
					Directory.CreateDirectory( destinationDir );
				}

				if( hasDuplicates() ) 
				{
					MaterialsEditor.Text = "A file with that name already exists.  "
						+ "Please remove the old file or rename the new file.  ";
					MaterialsEditor.DataList.RemoveAt( index );
				} 
				else 
				{
					MaterialsEditor.Text = "";
					string filePath = destinationDir + fname;
					f.PostedFile.SaveAs( filePath );

					// Virus scan the uploaded file and if it contains a virus, blow the file away and 
					// give the user and error message.
					if(hasVirus(filePath)) 
					{
						// remove the file
						File.Delete(filePath);
						MaterialsEditor.DataList.RemoveAt( index );
						MaterialsEditor.Text="*** Virus detected in \"" + fname + "\". File removed from server. ***";
					}
					
				}

			} 
			catch ( Exception ex ) 
			{
				Exception inner = ex.InnerException;
				MaterialsEditor.Text = "Error uploading file.  Try again.  " + ex.Message + "::" + ex.Source.ToString()
										+ "----" + ex.TargetSite + " " + ex.StackTrace + "::" + ex.ToString();
				if( inner != null ) {
					MaterialsEditor.Text += "<<<<" + inner.Message + "<<<" + inner.TargetSite + " " + inner.StackTrace + "::" + inner.ToString();
				}
				MaterialsEditor.DataList.RemoveAt( index );
			}

		}
	
		/// <summary>
		/// checks to see if the the file specified has a virus
		/// </summary>
		/// <param name="filePath">the path to the file to check</param>
		/// <returns>true if it has a virus, false if not</returns>
		private bool hasVirus(string filePath) 
		{
			bool retVal = false;
			ProcessStartInfo psi = new ProcessStartInfo();
			psi.FileName = ConfigurationSettings.AppSettings["VirusScanPath"];
			psi.Arguments = filePath;
			psi.RedirectStandardError = true;
			psi.UseShellExecute = false;
			Process p = Process.Start(psi);
			StreamReader err = p.StandardError;
			string s = err.ReadToEnd();
			if(s.IndexOf("Infected files: 0") == -1) 
			{
				retVal = true;
			}
			return retVal;
		}

		/// <summary>
		/// Checks the Materials control for duplicates
		/// </summary>
		/// <returns>False if any duplicates exist, true otherwise.</returns>
		public bool hasDuplicates() 
		{
			
			bool retval = false;
			ArrayList knownMaterials = new ArrayList();

			foreach( Materials.MaterialInfo mInfo in MaterialsEditor.DataList ) 
			{
				if( knownMaterials.Contains( mInfo.Link ) ) 
				{
					retval = true;
					break;
				} 
				else 
				{
					knownMaterials.Add( mInfo.Link );
				}
			}

			return retval;
		}

		/// <summary>
		/// Respond to the new item event, when the user elects to create a
		/// new item, by creating the specific info class and adding it to
		/// the editor control's data list.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void MaterialsEditor_NewItemEvent(object sender, EventArgs e) 
		{
			Materials.MaterialInfo mi = new Materials.MaterialInfo( 0, "", "" );
			MaterialsEditor.DataList.Add( mi );
		}
		
		/// <summary>
		/// Deletes all Materials associated with the given module ID from
		/// the permanent and temporary Materials directory
		/// </summary>
		/// <param name="moduleID">The module ID of the module to delete
		/// the Materials for.</param>
		public void deleteAllMaterials( int moduleID ) 
		{
			
			string PermDir = ConfigurationSettings.AppSettings["MaterialsDir"] +
				moduleID + "\\";
			string TempDir = ConfigurationSettings.AppSettings["MaterialsTempDir"] +
				Context.User.Identity.Name + "\\";

			foreach( Materials.MaterialInfo mi in MaterialsEditor.DataList ) 
			{
				if( File.Exists( PermDir + mi.Link ) )
					File.Delete( PermDir + mi.Link );
				else if( File.Exists( TempDir + mi.Link ) )
					File.Delete( TempDir + mi.Link );
			}

			MaterialsEditor.DataList.Clear();

		}

		/// <summary>
		/// Copies all the Materials from a given module ID to the current user's
		/// temporary Materials directory.
		/// </summary>
		/// <param name="moduleID">The module ID of the module from which we are
		/// retrieving Materials.</param>
		public void retrieveMaterials( int moduleID ) 
		{
			
			string PermDir = ConfigurationSettings.AppSettings["MaterialsDir"] + moduleID + "\\";
			string TempDir = ConfigurationSettings.AppSettings["MaterialsTempDir"] +
				Context.User.Identity.Name + "\\";

			if( !Directory.Exists( TempDir ) ) 
			{
				Directory.CreateDirectory( TempDir );
			}

			foreach( Materials.MaterialInfo mi in MaterialsEditor.DataList ) 
			{
				if( File.Exists( PermDir + mi.Link ) )
					File.Copy( PermDir + mi.Link, TempDir + mi.Link, false );
			}
		}

		#region IEditControl Members

		/// <summary>
		/// Initialize the list of materials.
		/// </summary>
		/// <param name="moduleID">The module identifier for the
		/// initial list of materials or 0 if it should be initially empty.</param>
		public void initList( int moduleID ) 
		{
			if ( moduleID == 0 ) 
			{
				MaterialsEditor.DataList = new System.Collections.ArrayList();
				MaterialsEditor.DataBind();
			} 
			else if ( moduleID > 0 ) 
			{
				MaterialsEditor.DataList = Materials.getAll( moduleID );
				MaterialsEditor.DataBind();
			} 
			else 
			{
				throw new ArgumentException( "Invalid module identifier.", "moduleID" );
			}
		}

		/// <summary>
		/// Insert all of the materials for the given module, and move the
		/// files from the temporary location to the permananent location.
		/// </summary>
		/// <param name="moduleID">The module to add the materials for.</param>
		/// <param name="removePrevious">Whether to remove any existing
		/// materials for the given module.</param>
		public void insertAll( int moduleID, bool removePrevious ) 
		{

			string tempDir = ConfigurationSettings.AppSettings["MaterialsTempDir"] +
				Context.User.Identity.Name + "\\";
			string dir = ConfigurationSettings.AppSettings["MaterialsDir"] + moduleID + "\\";
			
			if( !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			IList oldMats = Materials.getAll( moduleID );

			foreach ( Materials.MaterialInfo mi in oldMats ) 
			{
				if ( !MaterialsEditor.DataList.Contains( mi ) ) 
				{
					// If any of the previous materials are not in the new list
					// of materials, delete the file.
					File.Delete( dir + mi.Link );
				}
			}
			
			// This removes the links, but not the actual files.
			if ( removePrevious ) 
			{
				Materials.removeAll( moduleID );
			}

			foreach ( Materials.MaterialInfo mi in MaterialsEditor.DataList ) 
			{
				//MaterialsEditor.Text += "IdentInfo = " + mi.IdentInfo + ", Link = " + mi.Link + "<br>";
				if( File.Exists( tempDir + mi.Link ) ) 
				{
					if( File.Exists( dir + mi.Link ) )
						File.Delete( dir + mi.Link );
					File.Move( tempDir + mi.Link, dir + mi.Link );
				}
			}

			Materials.addAll( moduleID, MaterialsEditor.DataList );

			// Get the new list, which will contain MaterialIDs, so we know
			// which ones are new next time the module is "saved for later"
			// on the same page view.
			MaterialsEditor.DataList = Materials.getAll( moduleID );
			MaterialsEditor.DataBind();

			// Delete the temporary directory.  This will also delete any 
			// files that were uploaded and removed while editing.
			if ( Directory.Exists( tempDir ) ) 
			{
				Directory.Delete( tempDir, true );
			}
		}



		/// <summary>
		/// Validate the materials list.
		/// </summary>
		/// <returns>True if all input is valid.</returns>
		public bool validate() 
		{
			return MaterialsEditor.validate();
		}

		#endregion
	}
}
