using System;
using Dapper;
using KarveDapper;
using KarveDapper.Extensions;

 
namespace DataAccessLayer.DataObjects
{
	/// <summary>
	/// Represents a VIASPEDIPRO.
	/// NOTE: This class is generated from a T4 template - you should not modify it manually.
	/// </summary>
    [Table("VIASPEDIPRO")]
	public class VIASPEDIPRO 
	{
	
	/// <summary>
    ///  Set or get the CODIGO property.
    /// </summary>
    
		public byte CODIGO { get; set; }
 
	/// <summary>
    ///  Set or get the NOMBRE property.
    /// </summary>
    
		public string NOMBRE { get; set; }
 
	/// <summary>
    ///  Set or get the USUARIO property.
    /// </summary>
    
		public string USUARIO { get; set; }
 
	/// <summary>
    ///  Set or get the ULTMODI property.
    /// </summary>
    
		public string ULTMODI { get; set; }
	}
}
