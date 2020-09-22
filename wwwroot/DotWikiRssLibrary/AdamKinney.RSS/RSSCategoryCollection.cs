using System;
using System.Collections;

namespace AdamKinney.RSS
{
	public class RSSCategoryCollection  : CollectionBase
	{
		public RSSCategory this[ int index ]  
		{
			get{ return( (RSSCategory) List[index] ); }
			set{ List[index] = value; }
		}

		public int Add(RSSCategory value)
		{
			return List.Add(value);
		}

		public int IndexOf( RSSCategory value )  
		{
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, RSSCategory value )  
		{
			List.Insert( index, value );
		}

		public void Remove( RSSCategory value )  
		{
			List.Remove( value );
		}

		public bool Contains( RSSCategory value )  
		{
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  
		{
			if ( value.GetType() != Type.GetType("AdamKinney.RSS.RSSCategory") )
				throw new ArgumentException( "value must be of type RSSCategory.", "value" );
		}

		protected override void OnRemove( int index, Object value )  
		{
			if ( value.GetType() != Type.GetType("AdamKinney.RSS.RSSCategory") )
				throw new ArgumentException( "value must be of type RSSCategory.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if ( newValue.GetType() != Type.GetType("AdamKinney.RSS.RSSCategory") )
				throw new ArgumentException( "newValue must be of type RSSCategory.", "newValue" );
		}

		protected override void OnValidate( Object value )  
		{
			if ( value.GetType() != Type.GetType("AdamKinney.RSS.RSSCategory") )
				throw new ArgumentException( "value must be of type RSSCategory." );
		}
	}
}
