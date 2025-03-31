using System;
using System.IO;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace MauiApp2
{
    public partial class AddMealPage : ContentPage
    {
        private string _day;

        public event Action<string, string> MealAdded;

        public AddMealPage(string day)
        {
            InitializeComponent();
            _day = day;
        }

        private async void OnSaveMealClicked(object sender, EventArgs e)
        {
            if (MealTypePicker.SelectedItem == null || string.IsNullOrWhiteSpace(MealNameEntry.Text) || string.IsNullOrWhiteSpace(MealIngredientsEntry.Text))
            {
                await DisplayAlert("B³¹d", "Wybierz typ posi³ku, wpisz nazwê posi³ku i sk³adniki.", "OK");
                return;
            }

            var mealType = MealTypePicker.SelectedItem.ToString();
            var name = MealNameEntry.Text.Trim();
            var ingredients = MealIngredientsEntry.Text.Trim();

            // Tworzymy obiekt Meal
            var meal = new Meal
            {
                MealType = mealType,
                Name = name,
                Ingredients = ingredients
            };

            // Zapisujemy meal do pliku
            SaveMealToFile(meal);

            // Powiadamiamy, ¿e posi³ek zosta³ dodany
            MealAdded?.Invoke(mealType, name);

            await Navigation.PopAsync();
        }

        private void SaveMealToFile(Meal meal)
        {
            // Œcie¿ka do pliku w lokalnym katalogu aplikacji
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "meals.json");

            // Sprawdzamy, czy plik ju¿ istnieje
            var meals = new List<Meal>();

            if (File.Exists(filePath))
            {
                // Jeœli plik istnieje, wczytujemy jego zawartoœæ
                var json = File.ReadAllText(filePath);
                meals = JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();
            }

            // Dodajemy nowy posi³ek do listy
            meals.Add(meal);

            // Zapisujemy zaktualizowan¹ listê do pliku
            var updatedJson = JsonSerializer.Serialize(meals);
            File.WriteAllText(filePath, updatedJson);
        }
    }
}
