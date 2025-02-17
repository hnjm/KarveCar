using System;
using KarveDapper.Extensions;
namespace DataAccessLayer.DataObjects
{
	/// <summary>
	/// Represents a CLAVEPTO.
	/// NOTE: This class is generated from a T4 template - you should not modify it manually.
	/// </summary>
	[Table("CLAVEPTO")]
	public class CLAVEPTO 
	{
	
	/// <summary>
    ///  Set or get the COD_CLAVE property.
    /// </summary>
        [Key]
        [FieldSize("2")]
		public string COD_CLAVE { get; set; }
 
	/// <summary>
    ///  Set or get the ULTMODI property.
    /// </summary>
    
		public string ULTMODI { get; set; }
 
	/// <summary>
    ///  Set or get the USUARIO property.
    /// </summary>
    
		public string USUARIO { get; set; }
 
	/// <summary>
    ///  Set or get the NOMBRE property.
    /// </summary>
    
		public string NOMBRE { get; set; }
	}
}
