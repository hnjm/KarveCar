using System;
 
namespace DataAccessLayer.DataObjects
{
	/// <summary>
	/// Represents a HORARIO_CHOFER_DIA.
	/// NOTE: This class is generated from a T4 template - you should not modify it manually.
	/// </summary>
	public class HORARIO_CHOFER_DIA 
	{
	
	/// <summary>
    ///  Set or get the CHOFER property.
    /// </summary>
    
		public string CHOFER { get; set; }
 
	/// <summary>
    ///  Set or get the DIA property.
    /// </summary>
    
		public DateTime DIA { get; set; }
 
	/// <summary>
    ///  Set or get the DE property.
    /// </summary>
    
		public TimeSpan? DE { get; set; }
 
	/// <summary>
    ///  Set or get the A property.
    /// </summary>
    
		public TimeSpan? A { get; set; }
 
	/// <summary>
    ///  Set or get the LINEA property.
    /// </summary>
    
		public Int32 LINEA { get; set; }
	}
}
