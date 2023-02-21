using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var nextRecipeId = 0;
var recipeDict = new ConcurrentDictionary<int, Recipe>();

app.MapGet("/recipes", () => recipeDict.Values);

app.MapPost("/recipes", (CreateOrUpdateRecipeDto newRecipe) => {
    var newId = Interlocked.Increment(ref nextRecipeId);

    var recipeToAdd = new Recipe
    {
        Id = newId,
        Title = newRecipe.Title,
        Ingredients = newRecipe.Ingredients,
        Description = newRecipe.Description,
        ImageLink = newRecipe.ImageLink
    };

    if (!recipeDict.TryAdd(newId, recipeToAdd)) 
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }

    return Results.Created($"/recipes/{newId}", recipeToAdd);
});

app.MapDelete("/recipes/{id}", (int id) => 
{
    if (!recipeDict.TryRemove(id, out var _))
    {
        return Results.NotFound();
    }
    return Results.NoContent();
});

app.MapGet("/recipes/filter-by-title", (string title) => 
{
    var res = recipeDict.Where(recipe => recipe.Value.Title.ToLower().Contains(title.ToLower()));
    if (res.Count() == 0)
    {
        return Results.NoContent();
    }
    return Results.Ok(res);
});

app.MapGet("/recipes/filter-by-ingredient", (string ingredient) => {
    var res = new LinkedList<Recipe>();
    bool valid = false;
    foreach (Recipe rec in recipeDict.Values)
    {
        foreach(Ingredient ing in rec.Ingredients!)
        {
            if (ing.Name.ToLower().Contains(ingredient.ToLower()))
            {
                valid = true;
            }
        }
        if (valid) res.AddLast(rec);
        valid = false;
    }
    if (res.Count() < 1)
    {
        return Results.NoContent();
    }
    return Results.Ok(res);
});

app.MapPut("/recipes/{id}", (int id, CreateOrUpdateRecipeDto recipeToUpdate) => 
{
    if (!recipeDict.TryGetValue(id, out Recipe? recipe)) { return Results.NotFound(); };

    recipe.Title = recipeToUpdate.Title;
    recipe.Ingredients = recipeToUpdate.Ingredients;
    recipe.Description = recipeToUpdate.Description;
    recipe.ImageLink = recipeToUpdate.ImageLink;

    return Results.Ok(recipe);
});

app.Run();

class Recipe

{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public LinkedList<Ingredient>? Ingredients { get; set; } = new LinkedList<Ingredient>();
    public string Description { get; set; } = "";
    public string? ImageLink { get; set; } = "";
}

class Ingredient

{
    public string Name { get; set; } = "";
    public string MeasureUnit { get; set; } = "";
    public int Quantity { get; set; }
}

record CreateOrUpdateRecipeDto(string Title, LinkedList<Ingredient>? Ingredients, string Description, string? ImageLink);