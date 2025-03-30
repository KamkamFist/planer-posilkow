using System;
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
            if (MealTypePicker.SelectedItem == null || string.IsNullOrWhiteSpace(MealIngredientsEntry.Text))
            {
                await DisplayAlert("B³¹d", "Wybierz typ posi³ku i wpisz sk³adniki.", "OK");
                return;
            }

            var mealType = MealTypePicker.SelectedItem.ToString();
            var ingredients = MealIngredientsEntry.Text.Trim();

            MealAdded?.Invoke(mealType, ingredients);
            await Navigation.PopAsync();
        }
    }
}