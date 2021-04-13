using UnityEngine;

public class Crafting : MonoBehaviour
{
    public Ingredient[] recipe;
    public bool recipeFound;

    private void Update()
    {
        recipeFound = CheckRecipe(recipe);
    }

    private bool CheckRecipe(params Ingredient[] ingredients)
    {
        int foundIngredients = 0;
        bool recipeFound = false;

        foreach (var slot in GetComponentsInChildren<Slot>())
        {
            if (recipeFound)
            {
                if (slot.ingredient != Ingredient.Empty)
                    recipeFound = false;
                break;
            }

            if (slot.ingredient == ingredients[foundIngredients])
                foundIngredients++;

            else if (slot.ingredient != Ingredient.Empty)
                break;

            if (foundIngredients == ingredients.Length)
                recipeFound = true;
        }
        if (recipeFound)
            return true;
        return false;
    }
}

public enum Ingredient { Empty, Hops, Wheat, Barley, Snake };