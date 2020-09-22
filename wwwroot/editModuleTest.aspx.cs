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
using System.Text;
using SwenetDev.DBAdapter;
using SwenetDev.Controls;

namespace SwenetDev 
{
	//public enum EditType { New = 0, InProgress = 1, Approved = 2 };

	/// <summary>
	/// The code behind class for the upload module web form.
	/// </summary>
	public class editModuleTest : System.Web.UI.Page 
	{
		protected System.Web.UI.WebControls.Label ErrorMessage;
		protected System.Web.UI.HtmlControls.HtmlForm Form1;
		protected CategoriesControl CategoriesControl1;
		protected ObjectivesControl ObjectivesControl1;
		protected TopicsControl TopicsControl1;
		protected ResourcesControl ResourcesControl1;
		protected PrerequisitesControl PrerequisitesControl1;
		protected AuthorsControl AuthorsControl1;
		protected MaterialsControlTest MaterialsControl1;
		protected SeeAlsoControl SeeAlsoControl1;
		protected System.Web.UI.WebControls.TextBox Abstract;
		protected System.Web.UI.WebControls.RequiredFieldValidator AbstractValidator;
		protected System.Web.UI.WebControls.TextBox Comments;
		protected System.Web.UI.WebControls.Button BackBtn;
		protected System.Web.UI.WebControls.Button NextBtn;
		protected System.Web.UI.WebControls.Button SubmitBtn;
		protected System.Web.UI.WebControls.Label StepLbl;
		protected System.Web.UI.WebControls.TextBox Lecture;
		protected System.Web.UI.WebControls.TextBox Homework;
		protected System.Web.UI.WebControls.TextBox Lab;
		protected System.Web.UI.WebControls.TextBox Other;
		protected System.Web.UI.WebControls.TextBox Exercise;
		
		protected System.Web.UI.WebControls.Panel Panel0;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Panel Panel2;
		protected System.Web.UI.WebControls.Panel Panel3;
		protected System.Web.UI.WebControls.Panel Panel4;
		protected const int NUM_PANELS = 5;

		protected System.Web.UI.WebControls.ValidationSummary ValidationSummary1;
		protected System.Web.UI.WebControls.TextBox Title;
		protected System.Web.UI.WebControls.RequiredFieldValidator TitleValidator;
		protected System.Web.UI.WebControls.Button SaveBtn;
		protected System.Web.UI.WebControls.Button VariantBtn;
		protected System.Web.UI.WebControls.DropDownList ModulesDdl;
		protected System.Web.UI.WebControls.Panel VariantPanel;
		protected System.Web.UI.WebControls.Button SaveBtnTop;
		protected System.Web.UI.WebControls.Button NextBtnTop;
		protected System.Web.UI.WebControls.Button BackBtnTop;
		protected System.Web.UI.WebControls.Button SubmitBtnTop;
		protected System.Web.UI.WebControls.Label DateLbl;
		protected System.Web.UI.WebControls.TextBox CheckInTxt;
		protected System.Web.UI.WebControls.CustomValidator CheckInValidator;
		protected System.Web.UI.WebControls.Label ZipLabel;
		protected IEditControl[] editControls = new IEditControl[8];

		private int ModuleID 
		{
			get 
			{
				if( ViewState["EditModuleID"] == null )
					return 0;
				else
					return (int)ViewState["EditModuleID"];
			}
			set { ViewState["EditModuleID"] = value; }
		}

		private EditType ModuleEditType 
		{
			get { return (EditType)ViewState["EditType"]; }
			set { ViewState["EditType"] = value; }
		}

		private int UploadStep 
		{
			get { return (int)ViewState["UploadStep"]; }
			set { ViewState["UploadStep"] = value; }
		}

		private int VariantOf 
		{
			get { return (int)ViewState["VariantOf"]; }
			set { ViewState["VariantOf"] = value; }
		}

		private void Page_Load(Object sender, EventArgs e) 
		{
			ErrorMessage.Text = "";

			editControls[0] = CategoriesControl1;
			editControls[1] = AuthorsControl1;
			editControls[2] = PrerequisitesControl1;
			editControls[3] = ObjectivesControl1;
			editControls[4] = TopicsControl1;
			editControls[5] = MaterialsControl1;
			editControls[6] = ResourcesControl1;
			editControls[7] = SeeAlsoControl1;

			if ( !IsPostBack ) 
			{
				DateLbl.Text = DateTime.Now.ToShortDateString();
				VariantOf = -1;
				Modules.ModuleInfo module = null;

				// Determine the edit type from moduleID param.
				if ( Request.QueryString["moduleID"] != null ) 
				{
					ModuleID = Convert.ToInt32( Request.QueryString["moduleID"] );

					if ( Session["EditModule"] != null ) 
					{
						module = (Modules.ModuleInfo)Session["EditModule"];
						Session.Remove( "EditModule" );
					} 
					else 
					{
						module = Modules.getModuleInfo( ModuleID );
					}

					determineEditType( module.Status );
				} 
				else 
				{
					ModuleID = 0;
					ModuleEditType = EditType.New;
				}

				switch( ModuleEditType ) 
				{
					case EditType.InProgress:
						if ( module.IsRevision ) 
						{
							VariantPanel.Visible = false;
							Title.Enabled = false;
						}

						initEditControls( module );
						initVariantControls();
						break;
						
					case EditType.Approved:
						VariantPanel.Visible = false;
						Title.Enabled = false;
						initEditControls( module );
						initVariantControls();
						break;

					case EditType.New:
						IList inProgMods =
							Modules.getModulesBySubmitter( User.Identity.Name, ModuleStatus.InProgress );

						// Ensure there aren't any in progress modules.
						if ( inProgMods.Count > 0 ) 
						{
							Session["EditModule"] = inProgMods[0];
							Response.Redirect( "uploadModule.aspx", true );
						} 
						else 
						{
							ModuleID = 0;
							initEditControls( module );
							initVariantControls();
							deleteTempMaterials();
						}

						break;
				}

				VariantBtn.Attributes.Add( "onClick", "if ( !confirm('This will overwrite any existing module information with the information from the selected module (or clear it if none is selected).  Do you want to continue?') ) return false;" );

				UploadStep = 0;
				toggleButtonsAndPanels( 0 );
			}
		}

		/// <summary>
		/// Deletes any materials that may exist in the user's temporary
		/// Materials folder.
		/// </summary>
		private void deleteTempMaterials() 
		{

			string path = System.Configuration.ConfigurationSettings.AppSettings["MaterialsTempDir"]
				+ Context.User.Identity.Name + "\\";
			
			if( Directory.Exists( path ) ) 
			{
				string[] filenames = Directory.GetFiles( path );
				int pos = 0;

				try 
				{
					foreach( string filename in filenames ) 
					{
						File.Delete( filename );
						pos++;
					}
				} 
				catch (Exception ex) 
				{
					Response.Redirect( "MyAccount.aspx?message=EditModuleTest: An error occured while trying to remove existing material: " + filenames[pos] );
				}
			}

		}

		/// <summary>
		/// Initializes each of the EditControls given a ModuleInfo object.
		/// </summary>
		/// <param name="module">The module's info.</param>
		protected void initEditControls( Modules.ModuleInfo module ) 
		{
			initGeneralControls( module );

			foreach ( IEditControl ec in editControls ) 
			{
				if ( module == null ) 
				{
					ec.initList( 0 );
				} 
				else 
				{
					ec.initList( module.Id );
				}
			}
		}

		/// <summary>
		/// Initializes the non-EditControl components given a ModuleInfo object
		/// (Generic text box inputs).
		/// </summary>
		/// <param name="mi">The module's info.</param>
		protected void initGeneralControls( Modules.ModuleInfo mi ) 
		{
			if ( mi != null ) 
			{
				Title.Text = Globals.formatTextOutput( mi.Title );
				Lecture.Text = Globals.formatTextOutput( mi.LectureSize );
				Lab.Text = Globals.formatTextOutput( mi.LabSize );
				Exercise.Text = Globals.formatTextOutput( mi.ExerciseSize );
				Homework.Text = Globals.formatTextOutput( mi.HomeworkSize );
				Abstract.Text = Globals.formatTextOutput( mi.Abstract );
				Other.Text = Globals.formatTextOutput( mi.OtherSize );
				Comments.Text = Globals.formatTextOutput( mi.AuthorComments );
			} 
			else 
			{
				Title.Text = "";
				Lecture.Text = "";
				Lab.Text = "";
				Exercise.Text = "";
				Homework.Text = "";
				Abstract.Text = "";
				Other.Text = "";
				Comments.Text = "";
			}
		}
	    
		/// <summary>
		/// Handles the Submit button-click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void SubmitBtn_Click(object sender, EventArgs e) 
		{
			string message = "";
			bool isValid = validate();

			// If invalid/incomplete data exists, alert the user to the problem,
			// redirect them to the right location, and don't attempt to save
			// or submit the module. (we don't save because duplicate items may
			// be the cause of invalid data, and the DB can't handle duplicates)
			if ( !isValid ) 
			{
				ErrorMessage.Text += "Some required items are missing or invalid.  " +
					"Please go back to review your submission.";
				
				// Now we should find where the first error is and redirect the
				// user to that upload step
				if( !(TitleValidator.IsValid && AbstractValidator.IsValid && CategoriesControl1.validate()) )
					UploadStep = 0;
				else if( !AuthorsControl1.validate() )
					UploadStep = 1;					
				else if( !(PrerequisitesControl1.validate() && ObjectivesControl1.validate() && TopicsControl1.validate()) )
					UploadStep = 2;
				else if( !(MaterialsControl1.validate() && ResourcesControl1.validate()) )
					UploadStep = 3;
				else if( !(SeeAlsoControl1.validate() && CheckInValidator.IsValid) )
					UploadStep = 4;
				else 
				{
					ErrorMessage.Text = "Could not locate problem.";
					UploadStep = 4;
				}

				StepLbl.Text = "" + (UploadStep + 1);
				toggleButtonsAndPanels( UploadStep );

				NextBtn.CausesValidation = true;
				return;
			}

			// If the module was valid, save it and submit it for approval.
			// (if an admin is submitting, it doesn't need approval)
			try 
			{
				ModuleStatus status;
				Modules.ModuleInfo module;

				if( (User.Identity.IsAuthenticated && User.IsInRole( UserRole.Admin.ToString() )) ) 
				{
					status = ModuleStatus.Approved;
					module = saveModule( status );
					ModulesControl.approveModule( module.Id );
				} 
				else 
				{
					status = ModuleStatus.PendingApproval;
					module = saveModule( status );
				}

				// Postprocessing
					
				// Only reset if everything worked.  Otherwise, the data will 
				// be saved so the user may try again.
				reset();

				Response.Redirect( "uploadResult.aspx?moduleID=" + ModuleID, true );
			} 
			catch ( Exception ex ) 
			{
				ErrorMessage.Text = ex.Message + "..." + ex.StackTrace;
			}
		}

		/// <summary>
		/// Handles the Save button-click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveBtn_Click(object sender, System.EventArgs e) 
		{

			// Iterate through each of the EditControls to see if any
			// duplicate entries exist.  If so, redirect the user to
			// the problem area, notify them of the problem, and don't
			// save the module. (again, we don't save because duplicates
			// can't go into the DB)
			foreach ( IEditControl ec in editControls ) 
			{
				if( ec.hasDuplicates() ) 
				{
					if( CategoriesControl1.hasDuplicates() )
						UploadStep = 0;
					else if( AuthorsControl1.hasDuplicates() )
						UploadStep = 1;					
					else if( PrerequisitesControl1.hasDuplicates() || ObjectivesControl1.hasDuplicates() || TopicsControl1.hasDuplicates() )
						UploadStep = 2;
					else if( MaterialsControl1.hasDuplicates() || ResourcesControl1.hasDuplicates() )
						UploadStep = 3;
					else if( SeeAlsoControl1.hasDuplicates() )
						UploadStep = 4;
					else 
					{
						ErrorMessage.Text = "Could not locate problem.";
						UploadStep = 4;
					}

					StepLbl.Text = "" + (UploadStep + 1);
					toggleButtonsAndPanels( UploadStep );
					ErrorMessage.Text = "Duplicates found.";
					NextBtn.CausesValidation = false;
					return;
				}
			}
			
			// If no duplicates exist, this module is ok to save.
			// (note that we don't check if each of the EditControls
			// is valid because most require at least one entry to be
			// completely valid, and this is not a necessity for
			// being able to save)
			try 
			{
				saveModule( ModuleStatus.InProgress );
				ModuleEditType = EditType.InProgress;
				VariantOf = -1;
				ErrorMessage.Text = "Module has been saved.";
			} 
			catch ( Exception ex ) 
			{
				ErrorMessage.Text += "An error occurred while uploading your module.  "
					+ ex.Message + ex.InnerException.Message;
			}
		}

		/// <summary>
		/// Save a new module or changes to an existing module.
		/// </summary>
		/// <param name="status">The status to designate for the module.</param>
		private Modules.ModuleInfo saveModule( ModuleStatus status ) 
		{

			// tells whether old data exists that needs to be
			// removed before adding any new data
			bool removePrevious = false;

			// tells whether user is just updating an existing
			// module or if they want to create a new module
			// (possibly a new version of an existing module)
			bool isUpdate = false;

			Modules.ModuleInfo mi = createModuleInfo();

			ModuleID = mi.Id;
			ErrorMessage.Text += "Getting base ID for module: " + ModuleID;
			
			if( ModuleID > 0 ) 
			{
				mi.BaseId = Modules.getModuleInfo( ModuleID ).BaseId;
			} 
			else 
			{
				mi.BaseId = 0;
			}

			mi.Status = status;
			mi.Submitter = User.Identity.Name;
			mi.SubmitterID = UserAccounts.getUserInfo( User.Identity.Name ).SubmitterID;

			switch ( ModuleEditType ) 
			{
				case EditType.New:
					isUpdate = false;

					break;
				case EditType.InProgress:
					isUpdate = true;
					removePrevious = true;

					break;
				case EditType.Approved:
					Modules.ModuleInfo oldModule = Modules.getModuleInfo( ModuleID );
					mi.BaseId = oldModule.BaseId;
					// If this module was previously Approved, and an admin changed it,
					// just update the module without checking it out and creating a
					// new version.
					if( User.Identity.IsAuthenticated && User.IsInRole( UserRole.Admin.ToString() ) ) 
					{
						isUpdate = true;
						mi.Version = oldModule.Version;
					}
						// If this module was previously Approved, and a non-admin changed
						// it, create a new version of the module, and check it out accordingly.
					else 
					{
						isUpdate = false;
						mi.Version = oldModule.Version + 1;
					}
					removePrevious = true;

					break;
			}

			try 
			{

				ModuleID = ModulesControl.checkInModule( mi, isUpdate );

				ErrorMessage.Text += "Assigned ModuleID = " + ModuleID + ".  ";

				mi.Id = ModuleID;

				foreach ( IEditControl ec in editControls ) 
				{
					ec.insertAll( ModuleID, removePrevious );
				}

				/** HANDLE THE VARIANTS **/

				// is true if this module is new OR a variant is chosen (including <None>)
				if( VariantOf != -1 ) 
				{
					
					int groupID = ModuleGroups.getGroupID( mi.BaseId );

					// if this module was reset as a variant of the same module
					// as before, or if it was set as a variant of a module in
					// the same group as it already is in, do nothing
					if( groupID != -1 && groupID == ModuleGroups.getGroupID( VariantOf ) ) 
					{

					}
						// if this module was already in a group by itself, and the
						// user tries to put it in a new group by itself, ignore
						// the request and do nothing
					else if( groupID != -1 && VariantOf == 0 && ModuleGroups.getRelatedModules(mi.BaseId).Count == 0 ) 
					{

					}
					else 
					{

						// if <None> was chosen, add this module to its own module group
						if( VariantOf == 0 ) 
						{
							ModuleGroups.addToNew( mi.BaseId );
						}
							// else add this module to the group that was chosen
							// SQL code resolves duplicates
						else 
						{
							ModuleGroups.addToExisting( mi.BaseId, VariantOf );
						}
					}

				} 
				else 
				{
					// If the module was not a variant, add this module to its own module group
					ModuleGroups.addToNew( mi.BaseId );
				}

			} 
			catch ( Exception e ) 
			{
				string message = "An error occurred while saving your module.";

				// If a new module was being created and there was an error, remove
				// the module (and by cascading, any added pieces).
				if ( ModuleEditType == EditType.New && ModuleID != 0 ) 
				{
					ModulesControl.removeModule( ModuleID );
					message += "  Module was not saved.  Review the module and try to resubmit it.";
				} 
				else 
				{
					message += "  All of your changes were not saved.  Review the module and try to resubmit it.";
					ModuleEditType = EditType.InProgress;
				}

				message += e.StackTrace + e.Message;
				throw new Exception( message, e );
			}

			return mi;
		}

		/// <summary>
		/// Initialize the variant controls.
		/// </summary>
		private void initVariantControls() 
		{
			// Module drop down for variants
			ModulesDdl.DataTextField = "Title";
			ModulesDdl.DataValueField = "Id";
			ArrayList modules = new ArrayList();
			Modules.ModuleInfo mod1 = new Modules.ModuleInfo();
			mod1.Id = 0;
			mod1.Title = "<None Selected>";
			modules.Add( mod1 );
			modules.AddRange( Modules.getAll( ModuleStatus.Approved ) );
			ModulesDdl.DataSource = modules;
			ModulesDdl.DataBind();
		}
	    
		private void reset() 
		{
			Title.Text = "";
			Abstract.Text = "";
			Lecture.Text = "";
			Lab.Text = "";
			Exercise.Text = "";
			Homework.Text = "";
			Other.Text = "";
			Comments.Text = "";
		}
	    
		/// <summary>
		/// Tells whether the info provided for this module is completely
		/// valid (including whether duplicates exist and whether all
		/// necessary information was provided).
		/// </summary>
		/// <returns>True if the module info is valid, false otherwise.</returns>
		private bool validate() 
		{
			ErrorMessage.Text = "";
			AbstractValidator.Validate();
			CheckInValidator.Validate();
			bool retVal = Title.Text.Length != 0 && AbstractValidator.IsValid && CheckInValidator.IsValid;
			//bool retVal = Title.Text.Length != 0 && Abstract.Text.Length != 0 && CheckInTxt.Text.Length != 0;

			if( Abstract.Text.Length > 0 )
				ErrorMessage.Text += "Abstract error.  ";
			if( CheckInTxt.Text.Length > 0 )
				ErrorMessage.Text += "CheckIn error.  ";

			// Notify the user of any/all EditControl errors.
			foreach ( IEditControl ec in editControls ) 
			{
				if( !ec.validate() ) 
				{
					retVal = false;
					ErrorMessage.Text += "Error at: " + ec.ToString() + "  ";
				}
			}

			return retVal;
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
			this.BackBtnTop.Command += new System.Web.UI.WebControls.CommandEventHandler(this.OnNavigateCommand);
			this.NextBtnTop.Command += new System.Web.UI.WebControls.CommandEventHandler(this.OnNavigateCommand);
			this.SaveBtnTop.Click += new System.EventHandler(this.SaveBtn_Click);
			this.VariantBtn.Click += new System.EventHandler(this.VariantBtn_Click);
			this.CheckInValidator.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(this.CheckInValidator_ServerValidate);
			this.BackBtn.Command += new System.Web.UI.WebControls.CommandEventHandler(this.OnNavigateCommand);
			this.NextBtn.Command += new System.Web.UI.WebControls.CommandEventHandler(this.OnNavigateCommand);
			this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
			this.SubmitBtn.Click += new System.EventHandler(this.SubmitBtn_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void OnNavigateCommand(object sender, System.Web.UI.WebControls.CommandEventArgs e) 
		{
			if ( e.CommandName == "Back" ) 
			{
				UploadStep--;
			} 
			else 
			{
				// Only validate if going forward and we want to validate.
				if ( NextBtn.CausesValidation ) 
				{
					switch ( UploadStep ) 
					{
						case 0:
							if ( !CategoriesControl1.validate() ) { return; }
							break;
						case 1:
							if ( !AuthorsControl1.validate() ) { return; }
							break;
						case 2:
							bool prereqValid = PrerequisitesControl1.validate();
							bool objectValid = ObjectivesControl1.validate();
							bool topicsValid = TopicsControl1.validate();

							if ( !prereqValid || !objectValid || !topicsValid ) { return; }
							break;
						case 3:
							if ( !MaterialsControl1.validate() ) { return; }
							break;
					}
				}

				// If we weren't validating or if we were validating and the current
				// page has no errors, progress to the next page.
				UploadStep++;
			}

			// Updates the label that signifies which page we're on
			StepLbl.Text = "" + (UploadStep + 1);

			toggleButtonsAndPanels( UploadStep );
		}

		/// <summary>
		/// Sets the visibility for panels according to the current step in
		/// the upload process
		/// </summary>
		/// <param name="position"></param>
		private void toggleButtonsAndPanels( int position ) 
		{
			WebControl[] panels = new WebControl[NUM_PANELS];
			panels[0] = Panel0;
			panels[1] = Panel1;
			panels[2] = Panel2;
			panels[3] = Panel3;
			panels[4] = Panel4;

			// Sets the visibility for each of the panels:
			// True if we are now on the given panel, False otherwise.
			for ( int i = 0; i < NUM_PANELS; i++ ) 
			{
				if ( i == position ) 
				{
					panels[i].Visible = true;
				} 
				else 
				{
					panels[i].Visible = false;
				}
			}

			// Sets the visibility of the Back, Next, and Submit buttons
			// according to the upload step.
			if ( position == 0 ) 
			{
				BackBtn.Enabled = false;
				NextBtn.Enabled = true;
				SubmitBtn.Enabled = false;
				BackBtnTop.Enabled = false;
				NextBtnTop.Enabled = true;
				SubmitBtnTop.Enabled = false;
			} 
			else if ( position == NUM_PANELS - 1 ) 
			{
				BackBtn.Enabled = true;
				NextBtn.Enabled = false;
				SubmitBtn.Enabled = true;
				BackBtnTop.Enabled = true;
				NextBtnTop.Enabled = false;
				SubmitBtnTop.Enabled = true;
			} 
			else 
			{
				BackBtn.Enabled = true;
				NextBtn.Enabled = true;
				SubmitBtn.Enabled = false;
				BackBtnTop.Enabled = true;
				NextBtnTop.Enabled = true;
				SubmitBtnTop.Enabled = false;
			}
		}

		/// <summary>
		/// Create a ModuleInfo object.  If an existing module is being
		/// edited, the ID will be the current ID, otherwise it will
		/// be 0, indicating that it doesn't have an ID yet.
		/// </summary>
		/// <returns>An object encapsulating information about
		/// the module being uploaded.</returns>
		private Modules.ModuleInfo createModuleInfo() 
		{
			Modules.ModuleInfo mi = new Modules.ModuleInfo();
			mi.Id = ModuleID;
			mi.Title = Globals.parseTextInput( Title.Text );
			mi.Abstract = Globals.parseTextInput( Abstract.Text );
			mi.LectureSize = Globals.parseTextInput( Lecture.Text );
			mi.LabSize = Globals.parseTextInput( Lab.Text );
			mi.ExerciseSize = Globals.parseTextInput( Exercise.Text );
			mi.HomeworkSize = Globals.parseTextInput( Homework.Text );
			mi.OtherSize = Globals.parseTextInput( Other.Text );
			mi.AuthorComments = Globals.parseTextInput( Comments.Text );
			mi.CheckInComments = Globals.parseTextInput( CheckInTxt.Text );

			return mi;
		}

		/// <summary>
		/// Handle the click event for the Create Variant button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void VariantBtn_Click(object sender, System.EventArgs e) 
		{
			
			// First, delete any existing Materials while we still
			// have access to the current Materials list
			MaterialsControl1.deleteAllMaterials( ModuleID );

			if ( ModulesDdl.SelectedIndex != 0 ) 
			{
				int moduleNum = Convert.ToInt32( ModulesDdl.SelectedItem.Value );
				Modules.ModuleInfo module =	Modules.getModuleInfo( moduleNum );
				VariantOf = module.BaseId;
				module.Title = "Variant of " + module.Title;

				// Initialize all the controls with the original modules values.
				initEditControls( module );

				// Reset authors.  They shouldn't be copied.
				AuthorsControl1.initList( 0 );

				// Now, copy over all the Materials from the target
				// Module to the temp directory for this Module
				MaterialsControl1.retrieveMaterials( moduleNum );
			} 
			else 
			{
				VariantOf = 0;
				initEditControls( null );
			}

		}

		/// <summary>
		/// Sets the EditType of this module given a ModuleStatus.
		/// </summary>
		/// <param name="status">The appropriate ModuleStatus value.</param>
		private void determineEditType( ModuleStatus status ) 
		{
			switch ( status ) 
			{
				case ModuleStatus.InProgress:
					ModuleEditType = EditType.InProgress;
					break;
				case ModuleStatus.Approved:
					ModuleEditType = EditType.Approved;
					break;
				default:
					throw new Exception( "Cannot edit this module." );
			}
		}

		/// <summary>
		/// Tells whether the Check In Comments text box is valid
		/// (ie - if it is not blank).
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		private void CheckInValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args) 
		{
			args.IsValid = CheckInTxt.Text.Trim().Length > 0;
		}
	}
}
