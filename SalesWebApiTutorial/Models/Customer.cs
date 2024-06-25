using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesWebApiTutorial.Models;

public class Customer
{
    public int Id { get; set; } //it will be a PK by default
    [StringLength(30)] //make sure our table will have a varchar(30)
    public string Name { get; set; } = string.Empty;
    [StringLength(30)]
    public string City { get; set; } = string.Empty;
    [StringLength(2)]
    public string State { get; set; } = string.Empty;
    [Column(TypeName ="decimal(11,2)")] //to make sure we pass decimal(11,2) to the table
    public decimal Sales { get; set; }
    [Column(TypeName ="bit")] //change type to bit 
    public bool Active { get; set; } 



}
