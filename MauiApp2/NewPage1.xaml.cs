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
            // Subskrypcja komunikatu z HistoryPage
            MessagingCenter.Subscribe<HistoryPage, (Meal meal, string day)>(this, "MealReAdd", (sender, tuple) =>
            {
                var (meal, day) = tuple;
                AddMealFromHistory(meal, day); // Dodajemy posi�ek do odpowiedniego dnia
            });
        }

        private void AddMealToDay(string day)
        {
            // Metoda ta jest niepotrzebna w tej wersji kodu, ale mo�e by� u�ywana w innych miejscach.
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
            await OpenAddMealPage("PONIEDZIA�EK", (type, name) =>
            {
                UpdateMealLabel(type, name, MondayBreakfastLabel, MondayLunchLabel, MondayDinnerLabel);
            });

        private async void OnTuesdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("WTOREK", (type, name) =>
            {
                UpdateMealLabel(type, name, TuesdayBreakfastLabel, TuesdayLunchLabel, TuesdayDinnerLabel);
            });

        private async void OnWednesdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("�RODA", (type, name) =>
            {
                UpdateMealLabel(type, name, WednesdayBreakfastLabel, WednesdayLunchLabel, WednesdayDinnerLabel);
            });

        private async void OnThursdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("CZWARTEK", (type, name) =>
            {
                UpdateMealLabel(type, name, ThursdayBreakfastLabel, ThursdayLunchLabel, ThursdayDinnerLabel);
            });

        private async void OnFridayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("PI�TEK", (type, name) =>
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
                "�niadanie" => breakfast,
                "Obiad" => lunch,
                "Kolacja" => dinner,
                _ => null
            };

            if (mealLabel != null)
            {
                mealLabel.Text = $"{meal}: {name}";
            }
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

        // Wczytanie posi�k�w i wy�wietlenie ich
        private void DisplayMeals()
        {
            var meals = LoadMealsFromFile();

            foreach (var meal in meals)
            {
                // Przypisanie posi�ku do odpowiedniego dnia
                switch (meal.MealType)
                {
                    case "�niadanie":
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

        // Metoda do dodania posi�ku z historii do odpowiedniego dnia
        private void AddMealFromHistory(Meal meal, string day)
        {
            switch (day)
            {
                case "PONIEDZIA�EK":
                    UpdateMealLabel(meal.MealType, meal.Name, MondayBreakfastLabel, MondayLunchLabel, MondayDinnerLabel);
                    break;
                case "WTOREK":
                    UpdateMealLabel(meal.MealType, meal.Name, TuesdayBreakfastLabel, TuesdayLunchLabel, TuesdayDinnerLabel);
                    break;
                case "�RODA":
                    UpdateMealLabel(meal.MealType, meal.Name, WednesdayBreakfastLabel, WednesdayLunchLabel, WednesdayDinnerLabel);
                    break;
                case "CZWARTEK":
                    UpdateMealLabel(meal.MealType, meal.Name, ThursdayBreakfastLabel, ThursdayLunchLabel, ThursdayDinnerLabel);
                    break;
                case "PI�TEK":
                    UpdateMealLabel(meal.MealType, meal.Name, FridayBreakfastLabel, FridayLunchLabel, FridayDinnerLabel);
                    break;
                case "SOBOTA":
                    UpdateMealLabel(meal.MealType, meal.Name, SaturdayBreakfastLabel, SaturdayLunchLabel, SaturdayDinnerLabel);
                    break;
                case "NIEDZIELA":
                    UpdateMealLabel(meal.MealType, meal.Name, SundayBreakfastLabel, SundayLunchLabel, SundayDinnerLabel);
                    break;
            }
        }
        private async void OnShoppingListButtonClicked(object sender, EventArgs e)
        {
            // Przechodzimy do strony z list� zakup�w
            await Navigation.PushAsync(new ShoppingListPage());
        }
    }
    }
    
