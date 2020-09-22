namespace SwenetDev.Controls {
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
	using System.Threading;

	// for process stuff
	using System.Diagnostics;


	/// <summary>
	///	A control for adding and editing materials for a SWEnet module.
	/// </summary>
	public class MaterialsControl : System.Web.UI.UserControl, IEditControl 
	{
		protected Materials.MaterialInfo selectedMaterial;
		protected TextBox InfoTextBox;
		protected RadioButton NewRdBtn;
		protected RadioButton ExistingRdBtn;
		protected Panel InputPanel;
		protected Panel DDLPanel;
		protected DropDownList MatsDDL;
		protected RequiredFieldValidator FileValidator;
		
		public static int MaxLength 
		{
			get{ return DBAdapter.Modules.getColumnMaxLength("Materials","IdentifyingInfo"); }
		}

		protected DataEditControl MaterialsEditor;
   
		void Page_Load(Object sender, EventArgs e) 
		{

			MaterialsEditor.Text = "";
			MaterialsEditor.displayReOrgColumn();

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
			System.Web.UI.WebControls.DropDownList AccessLevelList = (System.Web.UI.WebControls.DropDownList) e.Item.Cells[1].FindControl( "AccessLevelList" );
			HtmlInputFile f = new HtmlInputFile();

			// Determine the original filename, removing the path.
			string fname = "";

			if( NewRdBtn.Checked ) {
				f = (HtmlInputFile)(e.Item.Cells[1].FindControl( "FileUpload" ));

				string fnameWithPath = f.PostedFile.FileName;
				int pos = fnameWithPath.LastIndexOf( "\\" ) + 1;
				fname = fnameWithPath.Substring( pos );

				selectedMaterial = new Materials.MaterialInfo( 0, "", "", 0, "", 0, 0 );
				selectedMaterial.Link = fname;
			} else if( ExistingRdBtn.Checked ) {
				selectedMaterial = Materials.getMaterialInfo( Convert.ToInt32( MatsDDL.SelectedValue ) );
			}

			selectedMaterial.IdentInfo = Globals.parseTextInput( InfoTextBox.Text );

			string accessLevel = AccessLevelList.SelectedValue;
			if (accessLevel.Equals("All")) selectedMaterial.AccessFlag = -1;
			else if (accessLevel.Equals("Users")) selectedMaterial.AccessFlag = 0;
			else if (accessLevel.Equals("Faculty")) selectedMaterial.AccessFlag = 1;

			MaterialsEditor.DataList.RemoveAt( index );
			MaterialsEditor.DataList.Add( selectedMaterial );

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
				else if( NewRdBtn.Checked ) 
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
				else if( ExistingRdBtn.Checked )
				{
					string sourcepath = ConfigurationSettings.AppSettings["MaterialsDir"] + 
									selectedMaterial.ModuleID + "\\" + selectedMaterial.Link;
					File.Copy( sourcepath, destinationDir + selectedMaterial.Link, true );
				}

			} 
			catch ( Exception ex ) 
			{
				MaterialsEditor.Text = "Error uploading file.  Try again.  " + ex.Message + " :: " + ex.Source.ToString();
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
			Materials.MaterialInfo mi = new Materials.MaterialInfo( 0, "", "", 0, "", 0, 0);
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

		#region WebControl Load Methods
		// Load methods for gaining reference to the ASP WebControls
		// Since they lay within an EditItemTemplate in the HTML,
		// they can not be automatically referenced.

		protected void InfoTextBox_Load( Object sender, EventArgs e ) 
		{
			InfoTextBox = (TextBox)sender;
		}

		protected void InputPanel_Load( Object sender, EventArgs e ) 
		{
			InputPanel = (Panel)sender;
		}

		protected void NewRdBtn_Load( Object sender, EventArgs e ) 
		{
			NewRdBtn = (RadioButton)sender;
		}

		protected void ExistingRdBtn_Load( Object sender, EventArgs e ) 
		{
			ExistingRdBtn = (RadioButton)sender;
		}

		protected void DDLPanel_Load( Object sender, EventArgs e )
		{
			DDLPanel = (Panel)sender;
		}

		protected void FileValidator_Load( Object sender, EventArgs e )
		{
			FileValidator = (RequiredFieldValidator)sender;
		}
		
		#endregion

		/// <summary>
		/// Gets a Hashtable of materials by materialID before placing the
		/// keys and values into the DropDownList.
		/// </summary>
		protected void Bind_MatsDDL( Object sender, EventArgs e ) 
		{
            Hashtable materials = Materials.getAll();

			if( materials != null ) {
				MatsDDL = (DropDownList)sender;

				if( MatsDDL.Items.Count == 0 ) {
					foreach( DictionaryEntry pair in materials ) {
						MatsDDL.Items.Add( new ListItem( Convert.ToString( pair.Key ), Convert.ToString( pair.Value ) ) );
					}
				}
			}
		}

/**		/// <summary>
		///  
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void MatsDDL_Changed( Object sender, EventArgs e ) 
		{
			MatsDDL = (DropDownList)sender;
			Session["DDLindex"] = MatsDDL.SelectedIndex;

			int matID = Convert.ToInt32( MatsDDL.SelectedValue );
			selectedMaterial = Materials.getMaterialInfo( matID );

			if( selectedMaterial != null ) {
				InfoTextBox.Text = selectedMaterial.IdentInfo;
			}

		}
**/
		/// <summary>
		/// Called when a different materials radio button is selected.
		/// Sets the proper upload method.
		/// </summary>
		protected void RdBtn_Changed( Object sender, EventArgs e ) {
			if( NewRdBtn.Checked ) 
			{
				MaterialsEditor.disableAllButtons();
				InputPanel.Visible = true;
				DDLPanel.Visible = false;

				FileValidator.ControlToValidate = "FileUpload";
			} 
			else if( ExistingRdBtn.Checked ) 
			{
				MaterialsEditor.disableAllButtons();
				InputPanel.Visible = false;
				DDLPanel.Visible = true;

				FileValidator.ControlToValidate = MatsDDL.ID;
			}

		}
		
		#region IEditControl Members

		/// <summary>
		/// Initialize the list of materials.
		/// </summary>
		/// <param name="moduleID">The module identifier for the
		/// initial list of materials or 0 if it should be initially empty.</param>
		public void initList( int moduleID ) {
			if ( moduleID == 0 ) {
				MaterialsEditor.DataList = new System.Collections.ArrayList();
				MaterialsEditor.DataBind();
			} else if ( moduleID > 0 ) {
				MaterialsEditor.DataList = Materials.getAll( moduleID );
				MaterialsEditor.DataBind();
			} else {
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
		public void insertAll( int moduleID, bool removePrevious ) {

			string tempDir = ConfigurationSettings.AppSettings["MaterialsTempDir"] +
				Context.User.Identity.Name + "\\";
			string dir = ConfigurationSettings.AppSettings["MaterialsDir"];
			
			if( !Directory.Exists( dir + moduleID + "\\" ) )
				Directory.CreateDirectory( dir + moduleID + "\\" );

			IList oldMats = Materials.getAll( moduleID );

			foreach ( Materials.MaterialInfo mi in oldMats ) {
				if ( !MaterialsEditor.DataList.Contains( mi ) ) {
					// If any of the previous materials are not in the new list
					// of materials, delete the file.
					File.Delete( dir + mi.ModuleID + "\\" + mi.Link );
				}
			}
			
			// This removes the links, but not the actual files.
			if ( removePrevious ) {
				Materials.removeAll( moduleID );
			}

			foreach ( Materials.MaterialInfo mi in MaterialsEditor.DataList ) {
				//MaterialsEditor.Text += "IdentInfo = " + mi.IdentInfo + ", Link = " + mi.Link + "<br>";
				
				if( File.Exists( tempDir + mi.Link ) ) 
				{
					if( File.Exists( dir + moduleID + "\\" + mi.Link ) )
						File.Delete( dir + moduleID + "\\" + mi.Link );
					File.Move( tempDir + mi.Link, dir + moduleID + "\\" + mi.Link );
				} 
				else if( mi.ModuleID != moduleID && mi.ModuleID != 0 ) // the file is not already in the directory
				{
					string sourcepath = ConfigurationSettings.AppSettings["MaterialsDir"] + 
										mi.ModuleID + "\\" + mi.Link;
					File.Copy( sourcepath, dir + moduleID + "\\" + mi.Link, true );
				}

				if( File.Exists( dir + moduleID + "\\" + mi.Link ) ) {
					mi.ModuleID = moduleID;
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
			if ( Directory.Exists( tempDir ) ) {
				Directory.Delete( tempDir, true );
			}
		}



		/// <summary>
		/// Validate the materials list.
		/// </summary>
		/// <returns>True if all input is valid.</returns>
		public bool validate() {
			return MaterialsEditor.validate();
		}

		#endregion
	}
}
