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
â”œâ”€â”€ Model/                # Domain models
â”‚   â”œâ”€â”€ Recept.cs         # Recipe
â”‚   â”œâ”€â”€ Ingrediens.cs     # Ingredient
â”‚   â”œâ”€â”€ Vara.cs           # Goods/product
â”‚   â”œâ”€â”€ Naringsvarde.cs   # Nutritional value
â”‚   â””â”€â”€ ReceptIngrediens.cs
â”œâ”€â”€ ViewModel/            # MVVM ViewModels
â”‚   â”œâ”€â”€ VMRecipePage.cs
â”‚   â”œâ”€â”€ VMIngredientPage.cs
â”‚   â”œâ”€â”€ VMNewRecipe.cs
â”‚   â”œâ”€â”€ VMOvrigtPage.cs
â”‚   â””â”€â”€ VMAddSingleVara.cs
â”œâ”€â”€ Pages/                # WPF pages
â”‚   â”œâ”€â”€ RecipePage.xaml
â”‚   â”œâ”€â”€ IngredientPage.xaml
â”‚   â”œâ”€â”€ NewRecipe.xaml
â”‚   â”œâ”€â”€ ShoppingList.xaml
â”‚   â””â”€â”€ ...
â”œâ”€â”€ Other/                # Utilities
â”‚   â”œâ”€â”€ SaveLoad.cs
â”‚   â”œâ”€â”€ RelayCommand.cs
â”‚   â””â”€â”€ DependencyHelper.cs
â””â”€â”€ MainWindow.xaml
```

## How to Run

Open `ReceptApp.sln` in Visual Studio and run the project.
