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
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MealsFileName);

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var meals = JsonSerializer.Deserialize<List<Meal>>(json) ?? new List<Meal>();

                // Na podstawie sk³adników z posi³ków tworzymy listê zakupów
                var shoppingItems = new List<ShoppingItem>();
                foreach (var meal in meals)
                {
                    var ingredients = meal.Ingredients.Split(','); // Zak³adamy, ¿e sk³adniki s¹ rozdzielone przecinkiem
                    foreach (var ingredient in ingredients)
                    {
                        var ingredientTrimmed = ingredient.Trim();
                        if (!shoppingItems.Any(item => item.Name == ingredientTrimmed))
                        {
                            shoppingItems.Add(new ShoppingItem { Name = ingredientTrimmed, IsChecked = false });
                        }
                        else
                        {
                            // Dodajemy drugi, trzeci itd. jeœli sk³adnik ju¿ istnieje w liœcie
                            shoppingItems.Add(new ShoppingItem { Name = ingredientTrimmed, IsChecked = false });
                        }
                    }
                }

                ShoppingList = shoppingItems;
            }
            else
            {
                ShoppingList = new List<ShoppingItem>();
            }

            ShoppingListView.ItemsSource = ShoppingList;
        }

        // Zapisz listê zakupów do pliku
        private void SaveShoppingList()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ShoppingListFileName);
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
        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as ShoppingItem;
            if (selectedItem != null)
            {
                bool delete = await DisplayAlert("Usuñ produkt", $"Czy na pewno chcesz usun¹æ {selectedItem.Name}?", "Tak", "Nie");
                if (delete)
                {
                    ShoppingList.Remove(selectedItem);
                    SaveShoppingList(); // Zapisz po usuniêciu
                    ShoppingListView.ItemsSource = null;
                    ShoppingListView.ItemsSource = ShoppingList;
                }
            }
        }

        // Obs³uguje klikniêcie przycisku "Usuñ zaznaczone"
        private void OnRemoveSelectedButtonClicked(object sender, EventArgs e)
        {
            var selectedItems = new List<ShoppingItem>();

            // Zbieramy wszystkie zaznaczone elementy
            foreach (var item in ShoppingList)
            {
                if (item.IsChecked)
                {
                    selectedItems.Add(item);
                }
            }

            // Usuwamy zaznaczone elementy
            foreach (var item in selectedItems)
            {
                ShoppingList.Remove(item);
            }

            // Zapisujemy po usuniêciu
            SaveShoppingList();

            // Odœwie¿amy widok
            ShoppingListView.ItemsSource = null;
            ShoppingListView.ItemsSource = ShoppingList;
        }
    }

    // Klasa do przechowywania elementów listy zakupów
    public class ShoppingItem
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
