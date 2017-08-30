
define(["js/ItemVM"], function (ItemVM) {

    function IngredientVM() {
        var self = this;

        self.IngredientID = ko.observable();
        self.Item = ko.observable();
        self.ItemID = ko.observable();
        self.Quantity = ko.observable();
        self.RecipeID = ko.observable();
        self.Units = ko.observable();

        self.load = function (data) {
            self.IngredientID(data.ingredientID);
            var im = new ItemVM();
            im.load(data.item);
            self.Item(im)
            self.ItemID(data.itemID);
            self.Quantity(data.quantity);
            self.RecipeID(data.recipeID);
            self.Units(data.units);
        }
    }
    return IngredientVM;
});
