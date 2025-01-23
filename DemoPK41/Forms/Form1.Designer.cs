using DemoPK41.Models;

namespace DemoPK41;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private TextBox txtName;
    private RichTextBox txtIngredients;
    private RichTextBox txtInstructions;
    private NumericUpDown numTime;
    private ListBox lstRecipes;

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Form1";
    }

    private void InitializeComponents()
    {
        Text = "Кулинарная книга";
        Size = new Size(600, 450);

        txtName = new TextBox { Location = new Point(120, 20), Width = 200 };
        txtIngredients = new RichTextBox { Location = new Point(120, 60), Height = 80, Width = 300 };
        txtInstructions = new RichTextBox { Location = new Point(120, 150), Height = 80, Width = 300 };
        numTime = new NumericUpDown { Location = new Point(120, 240), Minimum = 1, Maximum = 600 };
        lstRecipes = new ListBox { Location = new Point(20, 280), Width = 400, Height = 120 };

        var btnAdd = new Button
        {
            Text = "Добавить рецепт",
            Location = new Point(450, 20),
            Size = new Size(120, 30)
        };

        var labels = new[]
        {
            new Label { Text = "Название:", Location = new Point(20, 20) },
            new Label { Text = "Ингредиенты:", Location = new Point(20, 60) },
            new Label { Text = "Инструкции:", Location = new Point(20, 150) },
            new Label { Text = "Время (мин):", Location = new Point(20, 240) }
        };

        Controls.AddRange(
        [
            txtName, txtIngredients, txtInstructions, numTime,
            btnAdd, lstRecipes
        ]);
        Controls.AddRange(labels);

        btnAdd.Click += AddRecipe_Click;
    }

    #endregion
}