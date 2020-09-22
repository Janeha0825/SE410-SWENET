using System;
using System.Collections;
using SwenetDev.DBAdapter;

namespace SwenetDev.Comparers
{
	/// <summary>
	/// Compares two ModuleInfo objects based on the dates they were submitted.
	/// </summary>
	public class DateComparer: IComparer
	{
		public DateComparer()
		{
		
		}
		#region IComparer Members

		public int Compare(object x, object y)
		{
			try {	//Compares the two DateTime members of each ModuleInfo object

				Modules.ModuleInfo obj1 = (Modules.ModuleInfo)x;
				Modules.ModuleInfo obj2 = (Modules.ModuleInfo)y;

				return obj1.Date.CompareTo(obj2.Date);

			} catch( Exception e ) {

				throw new Exception( "DateComparer Error" );

			}
		}

		#endregion
	}
}
