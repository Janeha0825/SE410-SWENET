using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AspNetForums.Components;
using SwenetDev.ZipLib.Checksums;
using SwenetDev.ZipLib.Zip;
//using ICSharpCode.SharpZipLib.Checksums;
//using ICSharpCode.SharpZipLib.Zip;

namespace SwenetDev {
	using DBAdapter;
	using Controls;
	/// <summary>
	/// Summary description for viewModule.
	/// </summary>
	public class viewModule : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Repeater MaterialsRepeater;
		protected System.Web.UI.WebControls.Repeater ObjectivesRepeater;
		protected System.Web.UI.WebControls.Repeater TopicsRepeater;
		protected System.Web.UI.WebControls.Repeater PrereqRepeater;
		protected System.Web.UI.WebControls.Repeater SizeRepeater;
		protected System.Web.UI.WebControls.Repeater ResourcesRepeater;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected RateModuleControl RateModuleControl1;
		protected System.Web.UI.WebControls.Repeater AuthorsRepeater;
		protected System.Web.UI.WebControls.Repeater CategoriesRepeater;
		protected System.Web.UI.WebControls.Label RatingsMessage;
		protected System.Web.UI.WebControls.Repeater RelatedRepeater;
		protected System.Web.UI.HtmlControls.HtmlGenericControl H21;
		private const int NUM_SIZES = 5;
		protected System.Web.UI.WebControls.Button MaterialsZipButton;
		protected System.Web.UI.WebControls.Repeater SeeAlsoRepeater;
		private const int REFRESH_DELAY = 3;
		protected System.Web.UI.WebControls.Image Stars;
		protected System.Web.UI.WebControls.Label MaterialRating;
		protected System.Web.UI.WebControls.Panel ButtonPanel;
		IList tempMats1; 
	    //protected int counter = 0;
		//protected bool materialAccessDenied = false;
		protected bool isSubmitter;
		protected bool isAuthor;

		protected Modules.ModuleInfo ModInfo {
			get {
				return (Modules.ModuleInfo)ViewState["ModInfo"];
			}
			set {
				ViewState["ModInfo"] = value;
			}
		}

		public void NewImage(Object sender, EventArgs e) {
 
			

		}

		private void Page_Load(object sender, System.EventArgs e) {
			if ( !IsPostBack ) {
				try {
					string strModuleID = Request.Params["moduleID"];

					if ( strModuleID != null ) {
						int moduleID = int.Parse( Request.Params["moduleID"] );
						Modules.ModuleInfo modInfo = Modules.getModuleInfo( moduleID );
						ModInfo = modInfo;
						isSubmitter = ModulesControl.isModuleSubmitter( User.Identity.Name, modInfo.Id );
						isAuthor = ModulesControl.isModuleAuthor( User.Identity.Name, modInfo.Id );
						bool isEditor = User.IsInRole( UserRole.Editor.ToString() );

						// If the user is not an editor...
						if ( !isEditor ) {
							
							if ( modInfo.Status == ModuleStatus.PreviousVersion ) {
								// If the module being viewed is a previous version, display the newest
								// version of the module, and tell the user.
								Modules.ModuleInfo curVer = Modules.getModuleCurrentVersion( modInfo.BaseId );
								string newUrl = "viewModule.aspx?moduleID=" + curVer.Id;
								lblMessage.Text = "There is a <a href=\"" + newUrl + "\">newer version</a> of the module you are trying to access.  "
									+ "You will be directed to it momentarily.";
								Response.AppendHeader( "Refresh", REFRESH_DELAY + "; URL=" + newUrl );
								disableModule();
								return;
							} else if ( modInfo.Status != ModuleStatus.Approved && !isSubmitter ) {
								// If it is the newest version, but has not been approved
								// and this user is not the submitter, do not let them view
								// the module.								
								disableModule();
								lblMessage.Text = "Module not available.";
								return;
							}
						}

						// Modules.Size

						string[] labels = { "Lecture",
											  "Lab",
											  "Exercise",
											  "Homework",
											  "Other"
										  };
						string[] sizeText = { modInfo.LectureSize,
												modInfo.LabSize,
												modInfo.ExerciseSize,
												modInfo.HomeworkSize,
												modInfo.OtherSize
											};

						ArrayList sizes = new ArrayList( NUM_SIZES );

						for ( int i = 0; i < NUM_SIZES; i++ ) {
							if ( !sizeText[ i ].Equals( "" ) ) {
								sizes.Add( "<strong>" + labels[i] + "</strong>: " + sizeText[i] );
							}
						}

						SizeRepeater.DataSource = sizes;
						SizeRepeater.DataBind();

						// Authors

						AuthorsRepeater.DataSource = Authors.getAll( modInfo.Id );
						AuthorsRepeater.DataBind();

						// Materials
						// Since Materials aren't displayed as simply as other
						// areas, some modification needs to be done
						// Retrieve the materials that the user has the access level right to acquire
						tempMats1 = getAccessLevelFilesList(moduleID); //Materials.getAll( moduleID ); 
						ArrayList tempMats2 = new ArrayList();
						ArrayList knownTitles = new ArrayList();
						int pos = 0;

						foreach( Materials.MaterialInfo mi in tempMats1 ) {

							int position = mi.Link.LastIndexOf( '.' ); // temp value for extracting the title and extension
							string title; // the title part of the filename
							string extension; // the extension of the filename

							if( position == -1 ) {
								title = "(No Title)";
								extension = mi.Link;
							} else {
								title = mi.Link.Substring( 0, position );
								extension = getFormat( mi.Link.Substring( position + 1 ) );
							}

							// this is the reference to the current material that will
							// be added to the materials repeater in the html
							string reference = "(<a href='Materials/" + mi.ModuleID + "/" + mi.Link + 
												"' target='_blank'>" + extension + "</a>)";
							
							// see if we've already come across this title
							int index = knownTitles.IndexOf( title.ToLower() );

							if( index == -1 ) {
								// Means that the title hasn't already been found,
								// so we need to add it as a new Material group
								tempMats2.Add( new Materials.MaterialInfo( mi.Id, mi.IdentInfo, reference, 
																		   mi.ModuleID, mi.RatingImage, mi.Rating, 
																		   mi.AccessFlag) );
								knownTitles.Add( title.ToLower() );
							} else {
								// Means that the title has already been found, so we
								// need to add a link for this extra Material to the
								// appropriate existing Material group
								((Materials.MaterialInfo)(tempMats2[index])).Link += " " + reference;
							}

						}

						MaterialsRepeater.DataSource = tempMats2;
						MaterialsRepeater.DataBind();

						// Categories

						CategoriesRepeater.DataSource = Categories.getAll( modInfo.Id );
						CategoriesRepeater.DataBind();

						// Prerequisites

						PrereqRepeater.DataSource = Prerequisites.getAll( modInfo.Id );
						PrereqRepeater.DataBind();

						// Objectives

						ObjectivesRepeater.DataSource = Objectives.getAll( modInfo.Id );
						ObjectivesRepeater.DataBind();

						// Topics

						TopicsRepeater.DataSource = Topics.getAll( modInfo.Id );
						TopicsRepeater.DataBind();

						// Resources

						IList resourcesList = Resources.getAll( modInfo.Id );

						// If there are no resources, display text that says so.
						if ( resourcesList.Count == 0 ) {
							ResourcesRepeater.Controls.Add( new LiteralControl( "<p>No resources.</p>" ) );
						} else {
							ResourcesRepeater.DataSource = resourcesList;
							ResourcesRepeater.DataBind();
						}

						// See Also

						IList seeAlsoList = SeeAlso.getAll( modInfo.Id );

						// If there are no alternate modules, display text that says so.
						if( seeAlsoList.Count == 0 ) {
							SeeAlsoRepeater.Controls.Add( new LiteralControl( "<p>No alternate modules.</p>" ) );
						} else {
							SeeAlsoRepeater.DataSource = seeAlsoList;
							SeeAlsoRepeater.DataBind();
						}

						// Ratings

						if ( modInfo.Status == ModuleStatus.Approved ) {
							ModuleRatingInfo mci = ModuleRatings.getRating( modInfo.Id );
							RateModuleControl1.RatingInfo = mci;

							// Submitters and authors can't rate their own modules.
							if ( isSubmitter || isAuthor ) {
								RateModuleControl1.AddRatingEnabled = false;
								RatingsMessage.Text = "<p>You may not rate a module that " +
									"you have submitted or are listed as an author of.</p>";
							} else {
								Rating rating = ModuleRatingsControl.getRatingForUser( User.Identity.Name, mci );
								RateModuleControl1.UserRating = rating;
							}
						} else {
							disableRatings();
						}

						// Related Modules
						IList related = ModuleGroups.getRelatedModules( modInfo.BaseId );

						if ( related.Count == 0 ) {
							H21.Visible = false;
						} else {
							RelatedRepeater.DataSource = related;
							RelatedRepeater.DataBind();
						}
					} else {
						lblMessage.Text = 
							"An error occurred while attempting to get the requested module." +
							"  No module was specified.  <a href=\"browseModules.aspx\">Browse</a>" +
							" or <a href=\"search.aspx\">search</a> to select a module to view.";
					}
				} catch ( Exception ex ) {
					lblMessage.Text = 
						"An error occurred while attempting to get the requested module." +
						"  " + ex.Message + " " + ex.StackTrace;
				}
			}
		}

		/// <summary>
		/// Converts the given file extension to another string to identify
		/// the type of file it is.
		/// </summary>
		/// <param name="extension">The extension of the file to identify</param>
		/// <returns>The type of file</returns>
		private string getFormat( string extension ) {
			string retval = extension;

			if( retval.ToLower().Equals( "doc" ) )
				retval = "Word";
			else if( retval.ToLower().Equals( "txt" ) )
				retval = "Text";
			else if( retval.ToLower().Equals( "zip" ) )
				retval = "Winzip";
			else if( retval.ToLower().Equals( "ppt" ) )
				retval = "Powerpoint";
			else if( retval.ToLower().Equals( "xls" ) )
				retval = "Excel";
			else if( retval.ToLower().Equals( "bin" ) )
				retval = "Binary";
			else if( retval.ToLower().Equals( "jpg" ) )
				retval = "JPEG Image";
			else if( retval.ToLower().Equals( "gif" ) )
				retval = "GIF Image";
			else if( retval.ToLower().Equals( "bmp" ) )
				retval = "Bitmap Image";
			else
				retval = retval.ToUpper();

			return retval;
		}

		/// <summary>
		/// Handles the "Rate Material" link to determine if the user can rate materials
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Rate_Ld(Object sender, EventArgs e) {

			System.Web.UI.WebControls.HyperLink rateMaterialLink = (System.Web.UI.WebControls.HyperLink) sender;
	
			if (!Context.User.Identity.IsAuthenticated || isAuthor || isSubmitter) rateMaterialLink.Enabled = false;

		}
		
		/// <summary>
		/// Disable the ratings control.
		/// </summary>
		private void disableRatings() {
			RateModuleControl1.Visible = false;
		}

		/// <summary>
		/// Disable the viewing of the module.
		/// </summary>
		private void disableModule() {
			ModInfo = null;
			disableRatings();
		}

		/// <summary>
		/// Obtain a text rendering of the resource link.
		/// </summary>
		/// <param name="res">The resource to render.</param>
		/// <returns>The string output for rendering.</returns>
		public string renderResourceLink( Resources.ResourceInfo res ) {
			string retVal = "";

			if ( !res.Link.Equals( "" ) ) {
				HyperLink link = new HyperLink();
				link.Text = res.Link;
				link.NavigateUrl = res.Link;
				retVal = "<a href=\"" + res.Link + "\" target=\"_blank\">(Link)</a>";
			}

			return retVal;
		}


		/// <summary>
		/// Handles when the visual rating loads
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Stars_Ld(Object sender, EventArgs e) {

			if (!IsPostBack) {

				System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image) sender;
				string originalImage = image.ImageUrl;

				//This sets the image to change to "View Comments" when the mouse is over it
				image.Attributes["OnMouseOver"] = "javascript:changeImage(this, 'images/View Comments.gif');";

				//Based on the numerical rating the associated visual rating is provided
				if (originalImage.Equals("images/stars0.gif")) image.Attributes["OnMouseOut"]       = "javascript:changeImage(this, 'images/stars0.gif');";
				else if (originalImage.Equals("images/stars05.gif")) image.Attributes["OnMouseOut"] = "javascript:changeImage(this, 'images/stars05.gif');";
				else if (originalImage.Equals("images/stars1.gif")) image.Attributes["OnMouseOut"]  = "javascript:changeImage(this, 'images/stars1.gif');";
				else if (originalImage.Equals("images/stars15.gif")) image.Attributes["OnMouseOut"] = "javascript:changeImage(this, 'images/stars15.gif');";
				else if (originalImage.Equals("images/stars2.gif")) image.Attributes["OnMouseOut"]  = "javascript:changeImage(this, 'images/stars2.gif');";
				else if (originalImage.Equals("images/stars25.gif")) image.Attributes["OnMouseOut"] = "javascript:changeImage(this, 'images/stars25.gif');";
				else if (originalImage.Equals("images/stars3.gif")) image.Attributes["OnMouseOut"]  = "javascript:changeImage(this, 'images/stars3.gif');";
				else if (originalImage.Equals("images/stars35.gif")) image.Attributes["OnMouseOut"] = "javascript:changeImage(this, 'images/stars35.gif');";
				else if (originalImage.Equals("images/stars4.gif")) image.Attributes["OnMouseOut"]  = "javascript:changeImage(this, 'images/stars4.gif');";
				else if (originalImage.Equals("images/stars45.gif")) image.Attributes["OnMouseOut"] = "javascript:changeImage(this, 'images/stars45.gif');";
				else if (originalImage.Equals("images/stars5.gif")) image.Attributes["OnMouseOut"]  = "javascript:changeImage(this, 'images/stars5.gif');";

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
			this.MaterialsZipButton.Click += new EventHandler(this.MaterialsZipButton_Click);
		}
		#endregion

		/// <summary>
		/// Handles "Download Zip" button-click events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MaterialsZipButton_Click(object sender, System.EventArgs e) {
			
			string path = ConfigurationSettings.AppSettings["MaterialsDir"] + ModInfo.Id + "\\";
			string[] filenames = getAccessLevelFiles(ModInfo.Id); //string[] filenames = Directory.GetFiles( path );
			string userAccessLevel = "All";
			if (Context.User.Identity.IsAuthenticated) {

			UserAccounts.UserInfo zui = UserAccounts.getUserInfo(Context.User.Identity.Name);
			if (((int) zui.Role) == 0) userAccessLevel = "Users";
			else if (((int) zui.Role) >= 1 && ((int) zui.Role) < 5) userAccessLevel = "Faculty";

			}
			userAccessLevel = leastAccessLevel(ModInfo.Id, userAccessLevel);
			string fileWithoutPath = "materials-" + userAccessLevel + "-" + ModInfo.Id + ".zip";
			string fileWithPath = path + fileWithoutPath;

			// Once the zip file is created, it is stored and not created again.
			// If this module is in the 'InProgress' or 'PendingApproval' states,
			// we need to delete the zip file if it exists, because the submitter
			// can edit the materials before the module has been approved.  If the
			// Zip file is not deleted, it may contain links to Materials that don't
			// exist, and it may be missing links to Materials that do exist.
			if( ModInfo.Status == ModuleStatus.InProgress || ModInfo.Status == ModuleStatus.PendingApproval ) {
				// Need to store file under different file name than the usual filename
				// since the submitter may create the zip file before submission,
				// submit the module (turning the status potentially to approved),
				// and having an incorrect zip file exist later
				fileWithPath = path + "tempMaterials-" + userAccessLevel + "-" + ModInfo.Id + ".zip";
				if( File.Exists( fileWithPath ) )
					File.Delete( fileWithPath );
			}

			// If the zip file does not already exist, create it
			if( !File.Exists( fileWithPath ) ) {
		
				ZipOutputStream s = new ZipOutputStream( File.Create( fileWithPath ) );
				
				//*****************************************************************************//
				//********** NOTE: Code taken from \samples\cs\CreateZipFile\Main.cs **********//
				//**********       from file: 050SharpZipLib_SourceSamples.zip       **********//
				//**********       made available by SharpDevelop at:                **********//
				//**********       http://www.icsharpcode.net/OpenSource/SD/Download **********//
				//*****************************************************************************//
				// Modified to use MaterialInfo objects instead of strings

				Crc32 crc = new Crc32();
				s.SetLevel(6); // 0 - store only to 9 - means best compression

				foreach (string file in filenames) {
					
					if (file != null) {
						//string filepath = ConfigurationSettings.AppSettings["MaterialsDir"]	+ file.ModuleID + "\\" + file.Link;
						FileStream fs = File.OpenRead(path + file);
					
						byte[] buffer = new byte[fs.Length];
						fs.Read(buffer, 0, buffer.Length);
					
						// our code - hides our internal file structure
						ZipEntry entry = new ZipEntry(file);
						// end of our code

						entry.DateTime = DateTime.Now;

						// set Size and the crc, because the information
						// about the size and crc should be stored in the header
						// if it is not set it is automatically written in the footer.
						// (in this case size == crc == -1 in the header)
						// Some ZIP programs have problems with zip files that don't store
						// the size and crc in the header.
						entry.Size = fs.Length;
						fs.Close();
					
						crc.Reset();
						crc.Update(buffer);
					
						entry.Crc  = crc.Value;
					
						s.PutNextEntry(entry);
					
						s.Write(buffer, 0, buffer.Length);
					
					}

				}
				
				s.Finish();
				s.Close();
				//*****************************************************************************//
				//*************************** End of Borrowed Code ****************************//
				//*****************************************************************************//
			}

			// Redirect them directly to the file for downloading/opening

			Response.Redirect( "Materials/" + ModInfo.Id + "/" + fileWithoutPath );
		}

		/// <summary>
		/// Gets all of the materials that the user has the right to access in the module
		/// </summary>
		/// <param name="moduleID">The moduleID of the associated module</param>
		/// <returns>Returns all the materials that the user has access level rights to</returns>
		private string[] getAccessLevelFiles(int moduleID) {

			IList accessMaterials = Materials.getAll(moduleID);
			string[] accessAuthorizedMaterials; 
			int role  = -1;
			int size  = 0;
			int index = 0;
			if (Context.User.Identity.IsAuthenticated) {

				UserAccounts.UserInfo cui =  UserAccounts.getUserInfo(Context.User.Identity.Name);
				role = (int) cui.Role;

			}

			//Determine how many files the user has access rights to
			foreach (Materials.MaterialInfo material in accessMaterials) {

				if ((role >= material.AccessFlag) && (role < 5)) size++;

			}

			accessAuthorizedMaterials = new string[size];

			//Add each of the materials the user has rights to
			foreach (Materials.MaterialInfo material in accessMaterials) {

				if ((role >= material.AccessFlag) && (role < 5)) {

					accessAuthorizedMaterials[index] = material.Link;
					index++;

				}

			}

			return accessAuthorizedMaterials;

		}

		/// <summary>
		/// Gets all of the materials that the user has the right to access in the module
		/// </summary>
		/// <param name="moduleID">The module that the materials are in</param>
		/// <returns>All of the materials the user has access rights to</returns>
		private IList getAccessLevelFilesList(int moduleID) 
		{

			IList accessMaterials = Materials.getAll(moduleID);
			IList accessAuthorizedMaterials = new ArrayList();
			int role  = -1;
			
			if (Context.User.Identity.IsAuthenticated) 
			{

				UserAccounts.UserInfo cui =  UserAccounts.getUserInfo(Context.User.Identity.Name);
				role = (int) cui.Role;

			}

			//Add each of the materials the user has rights to
			foreach (Materials.MaterialInfo material in accessMaterials) 
			{

				if ((role >= material.AccessFlag) && (role < 5)) 
				{

					accessAuthorizedMaterials.Add(material);

				} 

			}

			return accessAuthorizedMaterials;

		}

		/// <summary>
		/// Determines the least access level necessary to access all of the materials the user has the proper
		/// access level for
		/// </summary>
		/// <param name="moduleID">The moduleID associated with the module that these materials are in</param>
		/// <param name="currentAccessLevel">The current access level that the user has</param>
		/// <returns></returns>
		private string leastAccessLevel(int moduleID, string currentAccessLevel) {

			IList accessLevelMaterials = Materials.getAll(moduleID);
			string newAccessLevel = "All";
			int highestAccessLevel = -1;
			int role = -1;
			if (currentAccessLevel.Equals("Users")) role = 0;
			else if (currentAccessLevel.Equals("Faculty")) role = 1;

			//Each material in the list is checked to see if first it is allowed to be accessed by the
			//user and if it can be accessed a cross reference will be done to determine if the access level
			//required by the material is greater than the largest currently known access level and if it
			//is the current material's access level will be stored as the highest know access level
			foreach (Materials.MaterialInfo material in accessLevelMaterials) {

				if (role >= material.AccessFlag) {

					if (material.AccessFlag > highestAccessLevel) highestAccessLevel = material.AccessFlag;

				}

			}

			//The highest access level of the materials is the least access level required for the user 
			//to view all of the materials that they have access level rights to
			if (highestAccessLevel == 0) newAccessLevel = "Users";
			else if (highestAccessLevel == 1) newAccessLevel = "Faculty";

			return newAccessLevel;

		}

	}
}
