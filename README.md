# ReceptApp

A desktop recipe management application built with C# and WPF using the MVVM pattern. Manage recipes, ingredients, nutritional values, and generate shopping lists. "Recept" means "Recipe" in Swedish.

## Technologies

- C#, WPF (XAML), .NET, MVVM pattern

## Features

- **Recipe management** - Create, view, and edit recipes
- **Ingredient tracking** - Manage ingredients with quantities
- **Nutritional values** - Track nutritional information per ingredient
- **Shopping list** - Generate shopping lists from recipes
- **Price management** - Track and update ingredient prices
- **Save/Load** - Persistent data storage

## Project Structure

```
ReceptApp/
├── Model/                # Domain models
│   ├── Recept.cs         # Recipe
│   ├── Ingrediens.cs     # Ingredient
│   ├── Vara.cs           # Goods/product
│   ├── Naringsvarde.cs   # Nutritional value
│   └── ReceptIngrediens.cs
├── ViewModel/            # MVVM ViewModels
│   ├── VMRecipePage.cs
│   ├── VMIngredientPage.cs
│   ├── VMNewRecipe.cs
│   ├── VMOvrigtPage.cs
│   └── VMAddSingleVara.cs
├── Pages/                # WPF pages
│   ├── RecipePage.xaml
│   ├── IngredientPage.xaml
│   ├── NewRecipe.xaml
│   ├── ShoppingList.xaml
│   └── ...
├── Other/                # Utilities
│   ├── SaveLoad.cs
│   ├── RelayCommand.cs
│   └── DependencyHelper.cs
└── MainWindow.xaml
```

## How to Run

Open `ReceptApp.sln` in Visual Studio and run the project.
