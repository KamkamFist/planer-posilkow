using System;
using Microsoft.Maui.Controls;

namespace MauiApp2
{
    public partial class NewPage1 : ContentPage
    {
        public NewPage1()
        {
            InitializeComponent();
        }

        private async Task OpenAddMealPage(string day, Action<string, string> updateAction)
        {
            var page = new AddMealPage(day);
            page.MealAdded += (mealType, ingredients) =>
            {
                updateAction(mealType, ingredients);
            };
            await Navigation.PushAsync(page);
        }

        private async void OnMondayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("PONIEDZIA£EK", (type, ing) =>
            {
                UpdateMealLabel(type, ing, MondayBreakfastLabel, MondayLunchLabel, MondayDinnerLabel);
            });

        private async void OnTuesdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("WTOREK", (type, ing) =>
            {
                UpdateMealLabel(type, ing, TuesdayBreakfastLabel, TuesdayLunchLabel, TuesdayDinnerLabel);
            });

        private async void OnWednesdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("ŒRODA", (type, ing) =>
            {
                UpdateMealLabel(type, ing, WednesdayBreakfastLabel, WednesdayLunchLabel, WednesdayDinnerLabel);
            });

        private async void OnThursdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("CZWARTEK", (type, ing) =>
            {
                UpdateMealLabel(type, ing, ThursdayBreakfastLabel, ThursdayLunchLabel, ThursdayDinnerLabel);
            });

        private async void OnFridayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("PI¥TEK", (type, ing) =>
            {
                UpdateMealLabel(type, ing, FridayBreakfastLabel, FridayLunchLabel, FridayDinnerLabel);
            });

        private async void OnSaturdayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("SOBOTA", (type, ing) =>
            {
                UpdateMealLabel(type, ing, SaturdayBreakfastLabel, SaturdayLunchLabel, SaturdayDinnerLabel);
            });

        private async void OnSundayAddClicked(object sender, EventArgs e) =>
            await OpenAddMealPage("NIEDZIELA", (type, ing) =>
            {
                UpdateMealLabel(type, ing, SundayBreakfastLabel, SundayLunchLabel, SundayDinnerLabel);
            });

        private void UpdateMealLabel(string type, string ingredients, Label breakfast, Label lunch, Label dinner)
        {
            switch (type)
            {
                case "Œniadanie":
                    breakfast.Text = $"Œniadanie: {ingredients}";
                    break;
                case "Obiad":
                    lunch.Text = $"Obiad: {ingredients}";
                    break;
                case "Kolacja":
                    dinner.Text = $"Kolacja: {ingredients}";
                    break;
            }
        }
    }
}