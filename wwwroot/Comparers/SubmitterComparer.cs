using System;
using System.Collections;
using SwenetDev.DBAdapter;

namespace SwenetDev.Comparers
{
	/// <summary>
	/// Compares two ModuleInfo objects based on the name of the submitter for each.
	/// </summary>
	public class SubmitterComparer: IComparer
	{
		public SubmitterComparer()
		{

		}
		#region IComparer Members

		public int Compare(object x, object y)
		{
			try {	//Compares the submitter names of two ModuleInfo objects

				Modules.ModuleInfo obj1 = (Modules.ModuleInfo)x;
				Modules.ModuleInfo obj2 = (Modules.ModuleInfo)y;

				return obj1.Submitter.CompareTo( obj2.Submitter );

			} catch( Exception e ) {

				throw new Exception( "SubmitterComparer Error" );

			}
		}

		#endregion
	}
}
