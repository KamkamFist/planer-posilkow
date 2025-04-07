using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace MauiApp2
{
    public partial class NewPage1 : ContentPage
    {
        public NewPage1()
        {
            InitializeComponent();
        }
        private void AddMealToDay(string day)
        {
           
            }

        private async void OnHistoryButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistoryPage());
        }
        private async Task OpenAddMealPage(string day, Action<string, string> updateAction)
        {
            var page = new AddMealPage(day);
            page.MealAdded += (mealType, name) =>
            {
                updateAction(mealType, name);
            };
            await Navigation.PushAsync(page);
        }

        private async void OnMondayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("PONIEDZIA£EK", (type, name) =>
            {
                UpdateMealLabel(type, name, MondayBreakfastLabel, MondayLunchLabel, MondayDinnerLabel);
            });

        private async void OnTuesdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("WTOREK", (type, name) =>
            {
                UpdateMealLabel(type, name, TuesdayBreakfastLabel, TuesdayLunchLabel, TuesdayDinnerLabel);
            });

        private async void OnWednesdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("ŒRODA", (type, name) =>
            {
                UpdateMealLabel(type, name, WednesdayBreakfastLabel, WednesdayLunchLabel, WednesdayDinnerLabel);
            });

        private async void OnThursdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("CZWARTEK", (type, name) =>
            {
                UpdateMealLabel(type, name, ThursdayBreakfastLabel, ThursdayLunchLabel, ThursdayDinnerLabel);
            });

        private async void OnFridayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("PI¥TEK", (type, name) =>
            {
                UpdateMealLabel(type, name, FridayBreakfastLabel, FridayLunchLabel, FridayDinnerLabel);
            });

        private async void OnSaturdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("SOBOTA", (type, name) =>
            {
                UpdateMealLabel(type, name, SaturdayBreakfastLabel, SaturdayLunchLabel, SaturdayDinnerLabel);
            });

        private async void OnSundayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("NIEDZIELA", (type, name) =>
            {
                UpdateMealLabel(type, name, SundayBreakfastLabel, SundayLunchLabel, SundayDinnerLabel);
            });

        private void UpdateMealLabel(string meal, string name, Label breakfast, Label lunch, Label dinner)
        {
            if (breakfast == null || lunch == null || dinner == null) return;

            Label mealLabel = meal switch
            {
                "Œniadanie" => breakfast,
                "Obiad" => lunch,
                "Kolacja" => dinner,
                _ => null
            };

            if (mealLabel != null)
            {
                mealLabel.Text = $"{meal}: {name}";
            }
        }

        // Wczytanie posi³ków z pliku
        private List<Meal> LoadMealsFromFile()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "meals.json");
            if (!File.Exists(filePath))
                return new List<Meal>();

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();
        }

        // Wczytanie posi³ków i wyœwietlenie ich
        private void DisplayMeals()
        {
            var meals = LoadMealsFromFile();

            foreach (var meal in meals)
            {
                // Przypisanie posi³ku do odpowiedniego dnia
                switch (meal.MealType)
                {
                    case "Œniadanie":
                        UpdateMealLabel(meal.MealType, meal.Name, MondayBreakfastLabel, TuesdayBreakfastLabel, WednesdayBreakfastLabel);
                        break;
                    case "Obiad":
                        UpdateMealLabel(meal.MealType, meal.Name, MondayLunchLabel, TuesdayLunchLabel, WednesdayLunchLabel);
                        break;
                    case "Kolacja":
                        UpdateMealLabel(meal.MealType, meal.Name, MondayDinnerLabel, TuesdayDinnerLabel, WednesdayDinnerLabel);
                        break;
                }
            }
        }
    }
}
