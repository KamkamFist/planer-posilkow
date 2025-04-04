using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace MauiApp2
{
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
            DisplayMealsHistory();
        }

        // Wczytanie posi�k�w z pliku
        private List<Meal> LoadMealsFromFile()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "meals.json");
            if (!File.Exists(filePath))
                return new List<Meal>();

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();
        }

        // Wy�wietlanie historii posi�k�w
        private void DisplayMealsHistory()
        {
            var meals = LoadMealsFromFile();
            MealsListView.ItemsSource = meals;
        }

        // Obs�uguje klikni�cie przycisku "Dodaj do dnia"
        private async void OnAddToDayClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Meal selectedMeal)
            {
                var page = new NewPage1();
                await Navigation.PushAsync(page);
                page.AddMealToDay(selectedMeal);
            }
        }
    }
}