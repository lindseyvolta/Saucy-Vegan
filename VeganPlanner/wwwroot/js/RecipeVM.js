define(["js/IngredientVM", "js/StepVM"], function (IngredientVM, StepVM) {

    function RecipeVM() {
        var self = this;

        self.RecipeID = ko.observable();
        self.Notes = ko.observable();
        self.Servings = ko.observable();
        self.Ingredients = ko.observableArray().extend({ deferred: true });
        self.Instructions = ko.observableArray();

        self.load = function (data) {
            self.RecipeID(data.recipeID);
            self.Notes(data.notes);
            self.Servings(data.servings);

            for (var i = 0; i < data.ingredients.length; i += 1) {
                var anIngredient = new IngredientVM();
                anIngredient.load(data.ingredients[i]);
                self.Ingredients.push(anIngredient);
            }

            for (var i = 0; i < data.instructions.length; i += 1) {
                var anInstruction = new StepVM();
                anInstruction.load(data.instructions[i]);
                self.Instructions.push(anInstruction);
            }
        }
    }

    return RecipeVM;
});