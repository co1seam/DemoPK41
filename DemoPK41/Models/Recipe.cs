namespace DemoPK41.Models;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Ingredients { get; set; }
    public string Instructions { get; set; }
    public int CookingTime { get; set; }
}