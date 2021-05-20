using System.Collections;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public Ingredient[] recipe;
    public bool recipeFound;

    int a, b;

    [ContextMenu("PRINT!")]
    private void PRINT()
    {
        int bitA = 15;
        int bitB = 3;

        print(bitA | bitB);

        // 0 1111 == 9
        // 0 0011 == 3
        // 0 0011 == 1
        // 0 0000 == 5 
        // 0 0001 == 1
        // 0 1111 == 15
        // 1 1110 == 30
        //11 1100 == 60
    }

    private void Update()
    {
        recipeFound = CheckRecipe(recipe);




    }


    IEnumerator Foo(int i)
    {
        yield return new WaitUntil(() => false);


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