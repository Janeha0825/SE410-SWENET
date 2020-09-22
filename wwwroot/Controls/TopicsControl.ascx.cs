namespace SwenetDev.Controls {
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using SwenetDev.DBAdapter;

	/// <summary>
	///	An data input control for SWEnet topics.
	/// </summary>
	public class TopicsControl : System.Web.UI.UserControl, IEditControl {
		
		public static int MaxLength {
			get{ return DBAdapter.Modules.getColumnMaxLength("Topics","TopicText"); }
		}
		
		protected DataEditControl TopicsEditor;

		private void Page_Load(object sender, System.EventArgs e) {
			TopicsEditor.Text = "";
			TopicsEditor.displayReOrgColumn();
		}

		/// <summary>
		/// Respond to the update event, when an item is being edited and
		/// subsequently updated, by updating the info object of the
		/// item being edited.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void TopicsEditor_UpdateCommand(object source, DataGridCommandEventArgs e) {
			TextBox topicBox = (TextBox)e.Item.Cells[1].FindControl( "EditTextbox" );
			string newText = Globals.parseTextInput( topicBox.Text );
			((Topics.TopicInfo)TopicsEditor.DataList[(int)e.Item.ItemIndex]).Text = newText;

			if( hasDuplicates() ) {
				TopicsEditor.Text = "That topic has already been added.";
				TopicsEditor.DataList.RemoveAt((int)e.Item.ItemIndex);
			} else {
				TopicsEditor.Text = "";
			}

		}

		/// <summary>
		/// Respond to the new item event, when the user elects to create a
		/// new item, by creating the specific info class and adding it to
		/// the editor control's data list.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments</param>
		private void TopicsEditor_NewItemEvent(object sender, EventArgs e) 
		{
			Topics.TopicInfo ti = new Topics.TopicInfo("", TopicsEditor.DataList.Count);
			TopicsEditor.DataList.Add( ti );
		}

		/// <summary>
		/// Checks the Topics control for duplicates
		/// </summary>
		/// <returns>False if any duplicates exist, true otherwise.</returns>
		public bool hasDuplicates() {
			
			bool retval = false;
			ArrayList knownTopics = new ArrayList();

			foreach( Topics.TopicInfo ti in TopicsEditor.DataList ) {
				if( knownTopics.Contains( ti.Text ) ) {
					retval = true;
					break;
				} else {
					knownTopics.Add( ti.Text );
				}
			}

			return retval;
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.Load += new System.EventHandler(this.Page_Load);
			TopicsEditor.UpdateCommand += new DataGridCommandEventHandler(TopicsEditor_UpdateCommand);
			TopicsEditor.NewItemEvent += new EventHandler(TopicsEditor_NewItemEvent);

		}
		#endregion

		#region IEditControl Members

		/// <summary>
		/// Initialize the topics list.
		/// </summary>
		/// <param name="moduleID">The module identifier </param>
		public void initList( int moduleID ) {
			if ( moduleID == 0 ) {
				TopicsEditor.DataList = new System.Collections.ArrayList();
				TopicsEditor.DataBind();
			} else if ( moduleID > 0 ) {
				TopicsEditor.DataList = Topics.getAll( moduleID );
				TopicsEditor.DataBind();
			} else {
				throw new ArgumentException( "Invalid module identifer.", "moduleID" );
			}
		}

		/// <summary>
		/// Insert all of the topics for the given module.
		/// </summary>
		/// <param name="moduleID">The module to add the topics for.</param>
		public void insertAll( int moduleID, bool removePrevious ) {
			if ( removePrevious ) {
				Topics.removeAll( moduleID );
			}

			Topics.addAll( moduleID, TopicsEditor.DataList );
		}

		/// <summary>
		/// Check this control for validity.
		/// </summary>
		/// <returns>Whether the control is valid.</returns>
		public bool validate() {
			return TopicsEditor.validate() & !hasDuplicates();
		}

		#endregion
	}
}
