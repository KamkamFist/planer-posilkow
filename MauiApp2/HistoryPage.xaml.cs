using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace MauiApp2
{
    public partial class HistoryPage : ContentPage
    {
        private List<Meal> favorites = new();

        public HistoryPage()
        {
            InitializeComponent();
            LoadFavorites();
            DisplayMealsHistory();
        }

        private List<Meal> LoadMealsFromFile()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "meals.json");
            if (!File.Exists(filePath))
                return new List<Meal>();

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();
        }

        private void DisplayMealsHistory()
        {
            var meals = LoadMealsFromFile();
            MealsListView.ItemsSource = meals;
        }

        private async void OnMealSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Meal selectedMeal)
            {
                bool confirm = await DisplayAlert("Dodaj ponownie", $"Dodaæ {selectedMeal.MealType}: {selectedMeal.Name}?", "Tak", "Nie");
                if (confirm)
                {
                    string chosenDay = await DisplayActionSheet("Wybierz dzieñ", "Anuluj",
                        null, "PONIEDZIA£EK", "WTOREK", "ŒRODA", "CZWARTEK", "PI¥TEK", "SOBOTA", "NIEDZIELA");

                    if (!string.IsNullOrEmpty(chosenDay) && chosenDay != "Anuluj")
                    {
                        MessagingCenter.Send(this, "MealReAdd", (selectedMeal, chosenDay));
                        await DisplayAlert("Dodano", "Posi³ek zosta³ dodany ponownie.", "OK");
                        await Navigation.PopAsync();
                    }
                }

                MealsListView.SelectedItem = null;
            }
        }

        private async void OnFavoriteClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.CommandParameter is Meal meal)
            {
                if (IsFavorite(meal))
                {
                    favorites.RemoveAll(m => m.Name == meal.Name && m.MealType == meal.MealType);
                    button.Source = "heart_outline.png";
                }
                else
                {
                    favorites.Add(meal);
                    button.Source = "heart_filled.png";
                }

                SaveFavorites();
                await DisplayAlert("Ulubione", $"\"{meal.Name}\" {(IsFavorite(meal) ? "dodano do" : "usuniêto z")} ulubionych.", "OK");
            }
        }

        private string GetFavoritesFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favorites.json");
        }

        private void LoadFavorites()
        {
            var path = GetFavoritesFilePath();
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                favorites = JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();
            }
        }

        private void SaveFavorites()
        {
            var json = JsonSerializer.Serialize(favorites);
            File.WriteAllText(GetFavoritesFilePath(), json);
        }

        private bool IsFavorite(Meal meal)
        {
            return favorites.Any(m => m.Name == meal.Name && m.MealType == meal.MealType);
        }
    }
}
