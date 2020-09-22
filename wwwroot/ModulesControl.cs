using System;
using System.Collections;
using System.IO;

namespace SwenetDev {
	using DBAdapter;

	/// <summary>
	/// A class for managing modules, including version control and approval.
	/// </summary>
	public class ModulesControl {

		/// <summary>
		/// Mark a module as approved and update lock state.
		/// </summary>
		/// <param name="moduleID">The identifier of the module to approve.</param>
		public static void approveModule( int moduleID ) {
			// Mark module as approved and unlock it.
			Modules.updateModuleStatus( moduleID, ModuleStatus.Approved );
			Modules.setLock( moduleID, "" );
			
			// If there was a previous version, mark it as PreviousVersion.

			Modules.ModuleInfo mod = Modules.getModuleInfo( moduleID );
			
			if ( mod.Version > 1 ) {
				IList versions = Modules.getModuleVersions( mod.BaseId, true );
				Modules.updateModuleStatus( ((Modules.ModuleInfo)versions[versions.Count - 2]).Id,
											ModuleStatus.PreviousVersion );
			} else {

				string newID;
				string lastID = Modules.getLastIDbySubmitter( mod.Submitter );
				int currentYear = System.DateTime.Now.Year;
				string moduleIdentifier = mod.BaseId == 0 ? "" : DBAdapter.Modules.getModuleIdentifierByBaseID( mod.BaseId );

				// If the baseID for this module already had a module identifier, reuse it
				if( moduleIdentifier != "" ) {
					newID = moduleIdentifier;
				} else if( lastID != "" ) {
					// if not, we need to set the identifier...
					// if the submitter has submitted modules before, the next identifier depends on the
					// identifier of the last module they submitted
					int pos1 = lastID.IndexOf('.');
					int pos2 = lastID.LastIndexOf('.');
					int year = int.Parse( lastID.Substring( pos1+1, pos2-pos1-1 ) );

					if( year == currentYear ) {
						// Get the last module number and increment it
						int moduleNum = int.Parse( lastID.Substring( pos2+1 ) );
						newID = lastID.Substring(0,pos2+1) + (moduleNum+1);
					} else {
						// If the last module that was submitted by this
						// submitter was in a previous year, use this year
						// and the module number 1
						newID = lastID.Substring(0,pos1+1) + currentYear + ".1";
					}
				} else {
					// If lastID was an empty string, the submitter hasn't submitted
					// any modules before, so we need to create a new identifier
					newID = mod.Submitter + "." + currentYear + ".1";
				}

				Modules.setModuleIdentifier( mod.BaseId, newID );
				Modules.setLastIDbySubmitter( mod.Submitter, newID );
			}

			ModuleRatingsControl.initModuleRating( moduleID );
		}

		/// <summary>
		/// Mark a module as rejected.  Delete if necessary.  If not
		/// deleted, module remains locked.
		/// </summary>
		/// <param name="moduleID">The module to reject.</param>
		/// <param name="save">Whether to save the module or delete it.</param>
		public static void rejectModule( int moduleID, bool save ) {
			if ( save ) {
				Modules.updateModuleStatus( moduleID, ModuleStatus.InProgress );
			} else {
				Modules.setLock( moduleID, "" );
				ModulesControl.removeModule( moduleID );
			}
		}

		/// <summary>
		/// Determine if the given user submitted the given module.
		/// </summary>
		/// <param name="username">The user to check.</param>
		/// <param name="moduleID">The module to check.</param>
		/// <returns>True if the user submitted the module, false if not.</returns>
		public static bool isModuleSubmitter( string username, int moduleID ) {
			IList list = Modules.getModulesBySubmitter( username, ModuleStatus.All );

			foreach ( Modules.ModuleInfo module in list ) {
				if ( module.Id == moduleID ) {
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determine if the given user is an author of the given module.
		/// </summary>
		/// <param name="username">The user to check.</param>
		/// <param name="moduleID">The module to check.</param>
		/// <returns>True if the user is an author of the module, false if not.</returns>
		public static bool isModuleAuthor( string username, int moduleID ) {
			IList list = Modules.getModulesByAuthor( username, ModuleStatus.All );

			foreach ( Modules.ModuleInfo module in list ) {
				if ( module.Id == moduleID ) {
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Remove a module from the system.  If the module was an initial version,
		/// also remove the base.
		/// </summary>
		/// <param name="moduleID">The identifier of the module to remove</param>
		public static void removeModule( int moduleID ) {
			IList materials = Materials.getAll( moduleID );
			
			foreach ( Materials.MaterialInfo mi in materials ) {
				try {
					File.Delete( Globals.MaterialsDir + mi.Link );
				} catch { }
			}

			// Cascades deletes, deletes base if necessary, unlocks if necessary
			Modules.remove( moduleID );
                                                                                                                                                                                                                                                                                                                                                                                                                                         
		}

		/// <summary>
		/// Check a module out of version control by designating that it is
		/// locked by the given user.
		/// </summary>
		/// <param name="moduleID">
		/// The module to check out.
		/// </param>
		/// <param name="username">
		/// The username of the user checking it out.
		/// </param>
		/// <returns>
		/// Whether the checkout was successful.
		/// </returns>
		public static bool checkOutModule( int moduleID, string username ) {
			return Modules.setLock( moduleID, username );
		}

		/// <summary>
		/// Check a module into version control.
		/// </summary>
		/// <param name="mi">
		/// The module to check in.
		/// </param>
		/// <param name="isUpdate">
		///		true if editing a previously checked in version (module changes
		///		were saved but not submitted), false if it's a new check in.
		/// </param>
		/// <returns>
		/// The identifier of the module checked in.
		/// </returns>
		public static int checkInModule( Modules.ModuleInfo mi, bool isUpdate ) {
			int retVal = 0;

			if ( isUpdate ) {
				retVal = mi.Id;
				Modules.updateModule( mi );
			} else {
				if ( mi.BaseId == 0 ) {
					int baseID = Modules.addModuleBase( mi );
					mi.BaseId = baseID;
				}
				retVal = Modules.addModule( mi );
			}

			return retVal;
		}

		/// <summary>
		/// Undo a check out previously made, but unlocking the module
		/// and deleting the in-progress version of it exists.
		/// </summary>
		/// <param name="moduleID">
		/// The identifier of the module for which to undo the check out.
		/// </param>
		public static void undoCheckOut( int moduleID ) {
			Modules.setLock( moduleID, "" );
			Modules.ModuleInfo mod = Modules.getModuleInfo( moduleID );

			if ( mod.Status == ModuleStatus.InProgress ) {
				Modules.remove( moduleID );
			}
		}
	}
}