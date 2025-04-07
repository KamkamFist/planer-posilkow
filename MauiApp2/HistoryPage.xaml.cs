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

        // Wczytanie posi³ków z pliku
        private List<Meal> LoadMealsFromFile()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "meals.json");
            if (!File.Exists(filePath))
                return new List<Meal>();

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();
        }

        // Wyœwietlanie historii posi³ków
        private void DisplayMealsHistory()
        {
            var meals = LoadMealsFromFile();
            MealsListView.ItemsSource = meals;
        }
    }
}
