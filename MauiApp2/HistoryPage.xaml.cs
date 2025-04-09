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

        private async void OnMealSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Meal selectedMeal)
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
    }
}
