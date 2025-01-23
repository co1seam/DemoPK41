namespace DemoPK41;
using Microsoft.Data.Sqlite;

public partial class Form1 : Form
{
    private List<Recipe> _recipes = new List<Recipe>();
    
    private TextBox txtName;
    private RichTextBox txtIngredients;
    private RichTextBox txtInstructions;
    private NumericUpDown numTime;
    private ListBox lstRecipes;

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
            using (var connection = new SqliteConnection("Data Source=./recipes.db"))
            {
                connection.Open();
                
                string checkTableQuery = @"
                SELECT name FROM sqlite_master 
                WHERE type='table' AND name='Recipes';";

                using (var checkTableCommand = new SqliteCommand(checkTableQuery, connection))
                {
                    var tableExists = checkTableCommand.ExecuteScalar() != null;

                    if (!tableExists)
                    {
                        string createTableQuery = @"
                        CREATE TABLE Recipes (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Ingredients TEXT NOT NULL,
                            Instructions TEXT NOT NULL,
                            CookingTime INTEGER NOT NULL
                        );";

                        using (var createTableCommand = new SqliteCommand(createTableQuery, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при инициализации базы данных: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void InitializeComponents()
    {
        this.Text = "Кулинарная книга";
        this.Size = new System.Drawing.Size(600, 450);
        
        txtName = new TextBox { Location = new System.Drawing.Point(120, 20), Width = 200 };
        txtIngredients = new RichTextBox { Location = new System.Drawing.Point(120, 60), Height = 80, Width = 300 };
        txtInstructions = new RichTextBox { Location = new System.Drawing.Point(120, 150), Height = 80, Width = 300 };
        numTime = new NumericUpDown { Location = new System.Drawing.Point(120, 240), Minimum = 1, Maximum = 600 };
        lstRecipes = new ListBox { Location = new System.Drawing.Point(20, 280), Width = 400, Height = 120 };
            
        var btnAdd = new Button 
        { 
            Text = "Добавить рецепт",
            Location = new System.Drawing.Point(450, 20),
            Size = new System.Drawing.Size(120, 30)
        };
        
        var labels = new[]
        {
            new Label { Text = "Название:", Location = new System.Drawing.Point(20, 20) },
            new Label { Text = "Ингредиенты:", Location = new System.Drawing.Point(20, 60) },
            new Label { Text = "Инструкции:", Location = new System.Drawing.Point(20, 150) },
            new Label { Text = "Время (мин):", Location = new System.Drawing.Point(20, 240) }
        };
        
        this.Controls.AddRange(new Control[] 
        {
            txtName, txtIngredients, txtInstructions, numTime,
            btnAdd, lstRecipes
        });
        this.Controls.AddRange(labels);
        
        btnAdd.Click += AddRecipe_Click;
    }
    
    private void AddRecipe_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Введите название рецепта!", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtIngredients.Text) ||
            string.IsNullOrWhiteSpace(txtInstructions.Text))
        {
            MessageBox.Show("Заполните все поля!", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var newRecipe = new Recipe
        {
            Name = txtName.Text,
            Ingredients = txtIngredients.Text,
            Instructions = txtInstructions.Text,
            CookingTime = (int)numTime.Value
        };

        try
        {
            using (var connection = new SqliteConnection("Data Source=./recipes.db"))
            {
                connection.Open();
                string insertQuery = @"
                INSERT INTO Recipes (Name, Ingredients, Instructions, CookingTime)
                VALUES (@Name, @Ingredients, @Instructions, @CookingTime)";
            
                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", newRecipe.Name);
                    command.Parameters.AddWithValue("@Ingredients", newRecipe.Ingredients);
                    command.Parameters.AddWithValue("@Instructions", newRecipe.Instructions);
                    command.Parameters.AddWithValue("@CookingTime", newRecipe.CookingTime);
                    command.ExecuteNonQuery();
                }
            }

            _recipes.Add(newRecipe);
            lstRecipes.Items.Add(newRecipe.Name);

            MessageBox.Show("Рецепт успешно добавлен!", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Очистка полей
            txtName.Clear();
            txtIngredients.Clear();
            txtInstructions.Clear();
            numTime.Value = numTime.Minimum;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при добавлении рецепта: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    
    private void ShowRecipeDetails(object sender, EventArgs e)
    {
        if (lstRecipes.SelectedIndex == -1) return;

        var selected = _recipes[lstRecipes.SelectedIndex];
        var detailsForm = new DetailsRecipe(selected);
        detailsForm.ShowDialog();
    }

    private void LoadRecipes()
    {
        try
        {
            using (var connection = new SqliteConnection("Data Source=./recipes.db"))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Recipes";
                
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var recipe = new Recipe
                        {
                            Name = reader["Name"].ToString(),
                            Ingredients = reader["Ingredients"].ToString(),
                            Instructions = reader["Instructions"].ToString(),
                            CookingTime = Convert.ToInt32(reader["CookingTime"])
                        };

                        _recipes.Add(recipe);
                        lstRecipes.Items.Add(recipe.Name);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке рецептов: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}