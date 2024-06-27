﻿namespace SalesWebApiTutorial.Models;

public class Orderline
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    //FK
    public int OrderId { get; set; }
    public int ItemId { get; set; }

    public virtual Order? Order { get; set; }
    public virtual Item? Item { get; set; }
    
}
