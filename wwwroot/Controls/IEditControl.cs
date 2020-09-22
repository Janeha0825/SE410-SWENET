using System;

namespace SwenetDev.Controls {
	/// <summary>
	/// Interface for the user controls for editing module pieces.
	/// </summary>
	public interface IEditControl {
		bool validate();
		bool hasDuplicates();
		void initList( int moduleID );
		void insertAll( int moduleID, bool removePrevious );
	}
}
