namespace DemoPK41;

using DemoPK41.Data;
using DemoPK41.Models;

public partial class Form1 : Form
{
    private readonly List<Recipe> _recipes = new();

    public Form1()
    {
        InitializeComponents();
        InitializeDatabase();
        LoadRecipes();
        lstRecipes.SelectedIndexChanged += ShowRecipeDetails;
    }

    private void InitializeDatabase()
    {
        try
        {
            using var context = new ApplicationDbContext();
            context.Database.EnsureCreated();

            if (context.Recipes.Any()) return;

            var defaultRecipe = new Recipe
            {
                Name = "Жареные пельмени от Демида",
                CookingTime = 15,
                Ingredients = "Пельмени\nТоматный соус\n400мл воды\n\nСоль, перец по вкусу",
                Instructions = "1. Ставим сковороду на высокий огонь, наливаем масла, равномерно размазываем\n" +
                               "2. Высыпаем замороженные пельмени на сковороду, обжариваем до появления корочки с двух сторон\n" +
                               "3. Заливаем в сковороду томатный соус, и сразу после выливаем туда воду, сыплем соль и перец по вкусу, накрываем крышкой и даём постоять 10 минут\n" +
                               "4. Когда 10 минут прошли - убираем крышку и выпариваем воду\n" +
                               "5. Ахуенные пельмени готовы! Приятного аппетита!"
            };

            context.Recipes.Add(defaultRecipe);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Ошибка при инициализации базы данных", ex);
        }
    }

    private void AddRecipe_Click(object sender, EventArgs e)
    {
        if (!ValidateRecipeInputs()) return;

        var newRecipe = new Recipe
        {
            Name = txtName.Text.Trim(),
            Ingredients = txtIngredients.Text.Trim(),
            Instructions = txtInstructions.Text.Trim(),
            CookingTime = (int)numTime.Value
        };

        try
        {
            using var context = new ApplicationDbContext();
            context.Recipes.Add(newRecipe);
            context.SaveChanges();

            _recipes.Add(newRecipe);
            lstRecipes.Items.Add(newRecipe.Name);

            MessageBox.Show("Рецепт успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearRecipeInputs();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Ошибка при добавлении рецепта", ex);
        }
    }

    private void ShowRecipeDetails(object sender, EventArgs e)
    {
        if (lstRecipes.SelectedIndex == -1) return;

        var selectedRecipe = _recipes[lstRecipes.SelectedIndex];
        using var detailsForm = new DetailsRecipe(selectedRecipe);
        detailsForm.ShowDialog();
    }

    private void LoadRecipes()
    {
        try
        {
            using var context = new ApplicationDbContext();
            var recipesFromDb = context.Recipes.ToList();

            foreach (var recipe in recipesFromDb)
            {
                _recipes.Add(recipe);
                lstRecipes.Items.Add(recipe.Name);
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Ошибка при загрузке рецептов", ex);
        }
    }

    private bool ValidateRecipeInputs()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Введите название рецепта!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtIngredients.Text) || string.IsNullOrWhiteSpace(txtInstructions.Text))
        {
            MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        return true;
    }

    private void ClearRecipeInputs()
    {
        txtName.Clear();
        txtIngredients.Clear();
        txtInstructions.Clear();
        numTime.Value = numTime.Minimum;
    }

    private void ShowErrorMessage(string message, Exception ex)
    {
        MessageBox.Show($"{message}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
