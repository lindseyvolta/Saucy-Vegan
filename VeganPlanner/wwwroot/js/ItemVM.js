define(["js/RecipeVM"], function (RecipeVM) {

    function ItemVM() {
        var self = this;

        self.ItemID = ko.observable();
        self.Name = ko.observable().extend({ required: "Enter a Name" });
        self.IsRecipe = ko.observable(false);
        self.ServingSize = ko.observable();
        self.ServingUnits = ko.observable();
        self.Category = ko.observable();
        self.UserID = ko.observable();
        self.CaloriesPerServing = ko.observable();
        self.ProteinPerServing = ko.observable();
        self.IsPantryItem = ko.observable();
        self.IsGF = ko.observable();
        self.RecipeID = ko.observable();
        self.Recipe = new RecipeVM();

        self.CreatedByAdmin = ko.pureComputed(function () {
            if (self.UserID() == "lvolta@umich.edu" || self.UserID() == "jvolta@vtechnologies.com") {
                return true;
            } else {
                return false;
            }
        });

        self.load = function (data) {
            self.ItemID(data.itemID);
            self.Name(data.name)
            self.IsRecipe(data.isRecipe);
            self.ServingSize(data.servingSize);
            self.ServingUnits(data.servingUnits);
            self.Category(data.category);
            self.UserID(data.userID);
            self.CaloriesPerServing(data.caloriesPerServing);
            self.ProteinPerServing(data.proteinPerServing);
            self.IsPantryItem(data.isPantryItem);
            self.IsGF(data.isGF);
            if (self.IsRecipe()) {
                self.RecipeID(data.recipeID);
                self.Recipe.load(data.recipe);
            }
        }
    }

    return ItemVM;
});