namespace DemoPK41;

public partial class DetailsRecipe : Form
{
    public DetailsRecipe(Recipe recipe)
    {
        this.Text = recipe.Name;
        this.Size = new System.Drawing.Size(400, 400);

        var txtDetails = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            Text = $"Ингредиенты:\n{recipe.Ingredients}\n\n" +
                   $"Инструкции:\n{recipe.Instructions}\n\n" +
                   $"Время приготовления: {recipe.CookingTime} мин"
        };
        
        this.Controls.Add(txtDetails);
    }
}