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
        private const string MealsFileName = "meals.json"; // Nazwa pliku z posi�kami
        private const string ShoppingListFileName = "shoppingList.json"; // Nazwa pliku z list� zakup�w
        public List<ShoppingItem> ShoppingList { get; set; }

        public ShoppingListPage()
        {
            InitializeComponent();
            LoadShoppingList();
        }

        // Wczytanie listy zakup�w z pliku
        private void LoadShoppingList()
        {
            var shoppingFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "shoppinglist.json");

            if (File.Exists(shoppingFilePath))
            {
                // Je�li istnieje zapisany plik listy zakup�w, to go wczytaj
                var savedJson = File.ReadAllText(shoppingFilePath);
                ShoppingList = JsonSerializer.Deserialize<List<ShoppingItem>>(savedJson) ?? new List<ShoppingItem>();
            }
            else
            {
                // Je�li nie istnieje � generujemy list� z meals.json
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
        // Zapisz list� zakup�w do pliku
        private void SaveShoppingList()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "shoppinglist.json");
            var json = JsonSerializer.Serialize(ShoppingList);
            File.WriteAllText(filePath, json);
        }


        // Obs�uguje zmian� stanu checkboxa
        private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var item = checkBox?.BindingContext as ShoppingItem;

            if (item != null)
            {
                item.IsChecked = e.Value;
                SaveShoppingList(); // Zapisz po ka�dej zmianie
            }
        }

        // Obs�uguje klikni�cie na element w li�cie
        // Usuwanie pojedynczego produktu po tapni�ciu
        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is ShoppingItem selectedItem)
            {
                bool delete = await DisplayAlert("Usu� produkt", $"Czy na pewno chcesz usun�� {selectedItem.Name}?", "Tak", "Nie");
                if (delete)
                {
                    ShoppingList.Remove(selectedItem);
                    SaveShoppingList(); // Zapisz po usuni�ciu
                    RefreshShoppingListView();
                }
            }

            // Resetujemy zaznaczenie, �eby nie zostawa�o "na niebiesko"
            ((ListView)sender).SelectedItem = null;
        }

        // Usuwanie zaznaczonych (checkbox) produkt�w
        private void OnRemoveSelectedButtonClicked(object sender, EventArgs e)
        {
            // Kopiujemy zaznaczone elementy do usuni�cia
            var itemsToRemove = ShoppingList.Where(item => item.IsChecked).ToList();

            if (itemsToRemove.Count == 0)
            {
                DisplayAlert("Brak zaznaczenia", "Zaznacz produkty do usuni�cia.", "OK");
                return;
            }

            foreach (var item in itemsToRemove)
            {
                ShoppingList.Remove(item);
            }

            SaveShoppingList(); // Zapisz zmiany
            RefreshShoppingListView(); // Od�wie� list�
        }

        // Pomocnicza metoda do od�wie�ania widoku listy
        private void RefreshShoppingListView()
        {
            ShoppingListView.ItemsSource = null;
            ShoppingListView.ItemsSource = ShoppingList;
        }


        // Klasa do przechowywania element�w listy zakup�w
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

            SaveShoppingList(); // Zapisz od razu, je�li chcesz zachowa� list� na sta�e
            RefreshShoppingListView();
        }
protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadShoppingList();
        }
    }
    

    }
