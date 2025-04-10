using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace MauiApp2
{
    public partial class ShoppingListPage : ContentPage
    {
        private const string MealsFileName = "meals.json"; // Nazwa pliku z posi³kami
        private const string ShoppingListFileName = "shoppingList.json"; // Nazwa pliku z list¹ zakupów
        public List<ShoppingItem> ShoppingList { get; set; }

        public ShoppingListPage()
        {
            InitializeComponent();
            LoadShoppingList();
        }

        // Wczytanie listy zakupów z pliku
        private void LoadShoppingList()
        {
            var shoppingFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "shoppinglist.json");

            if (File.Exists(shoppingFilePath))
            {
                // Jeœli istnieje zapisany plik listy zakupów, to go wczytaj
                var savedJson = File.ReadAllText(shoppingFilePath);
                ShoppingList = JsonSerializer.Deserialize<List<ShoppingItem>>(savedJson) ?? new List<ShoppingItem>();
            }
            else
            {
                // Jeœli nie istnieje – generujemy listê z meals.json
                var mealsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "meals.json");

                if (!File.Exists(mealsFilePath))
                {
                    ShoppingList = new List<ShoppingItem>();
                    return;
                }

                var json = File.ReadAllText(mealsFilePath);
                var meals = JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();

                var ingredientsSet = new HashSet<string>();

                foreach (var meal in meals)
                {
                    var lines = meal.Ingredients.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        var trimmed = line.Trim();
                        if (!string.IsNullOrWhiteSpace(trimmed))
                        {
                            ingredientsSet.Add(trimmed);
                        }
                    }
                }

                ShoppingList = ingredientsSet.Select(i => new ShoppingItem { Name = i, IsChecked = false }).ToList();

                SaveShoppingList(); // zapisujemy do pliku po wygenerowaniu
            }

            RefreshShoppingListView();
        }
        // Zapisz listê zakupów do pliku
        private void SaveShoppingList()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "shoppinglist.json");
            var json = JsonSerializer.Serialize(ShoppingList);
            File.WriteAllText(filePath, json);
        }


        // Obs³uguje zmianê stanu checkboxa
        private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var item = checkBox?.BindingContext as ShoppingItem;

            if (item != null)
            {
                item.IsChecked = e.Value;
                SaveShoppingList(); // Zapisz po ka¿dej zmianie
            }
        }

        // Obs³uguje klikniêcie na element w liœcie
        // Usuwanie pojedynczego produktu po tapniêciu
        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is ShoppingItem selectedItem)
            {
                bool delete = await DisplayAlert("Usuñ produkt", $"Czy na pewno chcesz usun¹æ {selectedItem.Name}?", "Tak", "Nie");
                if (delete)
                {
                    ShoppingList.Remove(selectedItem);
                    SaveShoppingList(); // Zapisz po usuniêciu
                    RefreshShoppingListView();
                }
            }

            // Resetujemy zaznaczenie, ¿eby nie zostawa³o "na niebiesko"
            ((ListView)sender).SelectedItem = null;
        }

        // Usuwanie zaznaczonych (checkbox) produktów
        private void OnRemoveSelectedButtonClicked(object sender, EventArgs e)
        {
            // Kopiujemy zaznaczone elementy do usuniêcia
            var itemsToRemove = ShoppingList.Where(item => item.IsChecked).ToList();

            if (itemsToRemove.Count == 0)
            {
                DisplayAlert("Brak zaznaczenia", "Zaznacz produkty do usuniêcia.", "OK");
                return;
            }

            foreach (var item in itemsToRemove)
            {
                ShoppingList.Remove(item);
            }

            SaveShoppingList(); // Zapisz zmiany
            RefreshShoppingListView(); // Odœwie¿ listê
        }

        // Pomocnicza metoda do odœwie¿ania widoku listy
        private void RefreshShoppingListView()
        {
            ShoppingListView.ItemsSource = null;
            ShoppingListView.ItemsSource = ShoppingList;
        }


        // Klasa do przechowywania elementów listy zakupów
        public class ShoppingItem
        {
            public string Name { get; set; }
            public bool IsChecked { get; set; }
        }
        private void LoadShoppingListFromMeals()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "meals.json");

            if (!File.Exists(filePath))
            {
                return;
            }

            var json = File.ReadAllText(filePath);
            var meals = JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();

            var ingredientsSet = new HashSet<string>();

            foreach (var meal in meals)
            {
                var lines = meal.Ingredients.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmed))
                    {
                        ingredientsSet.Add(trimmed); // dodaje unikalnie
                    }
                }
            }

            ShoppingList = ingredientsSet.Select(i => new ShoppingItem { Name = i, IsChecked = false }).ToList();

            SaveShoppingList(); // Zapisz od razu, jeœli chcesz zachowaæ listê na sta³e
            RefreshShoppingListView();
        }
protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadShoppingList();
        }
    }
    

    }
