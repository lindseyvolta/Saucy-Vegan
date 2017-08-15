define([], function () {
    /**
     * View model that handles the "My Kitchen -> Food Items" view.
     */
    function FoodItemsHandlerVM() {
        var self = this;

        self.SearchString = ko.observable();
        self.ItemCategory = ko.observable();
        self.Items = ko.observableArray();
        self.DetailItem = ko.observable();
        self.EditItem = ko.observable();
        self.Ingredients = ko.observableArray();

        self.UnitList = [
            { UnitName: "lb"},
            { UnitName: "Cup"},
            { UnitName: "tsp"}, {UnitName: "tbsp"}, {UnitName: "oz"}
        ];    

        self.addIngredient = function (item) {
            //self.Ingredients.push(new Ingredient());
            
        };

        self.removeIngredient = function (ingredient) {
            self.EditItem.recipe.Ingredients.remove(ingredient);
        };

        self.addStep = function (item) {
            self.EditItem.recipe.instructions.push(new instruction());
        };

        self.removeStep = function (instruction) {
            self.EditItem.recipe.instructions.remove(instruction);
        };

        self.populateData = function (element) {
            $.ajax({
                type: "GET",
                url: "Items/GetItems",
                data: { searchString: self.SearchString(), itemCategory: self.ItemCategory() },
                success: function (data) {
                    ko.mapping.fromJS(data.items, {}, self.Items);

                    if (element && !ko.dataFor(element))
                        ko.applyBindings(self, element);
                }
            });  
        }

        self.showDetails = function (item) {
            self.DetailItem(item);
            $("#details-modal").modal("show");
        }

        self.showEdit = function (item) {
            self.EditItem(item);
            //self.Ingredients(item.recipe.ingredients);
            $("#edit-modal").modal("show");
        }

        
        self.saveEdit = function (item) {

            $.ajax({
                type: 'post',
                dataType: 'json',
                url: 'Items/Test',
                data: { "json": ko.toJSON(item) },
                success: function (json) {
                    if (json) {
                        alert('ok - ' + json);
                        $("#edit-modal").modal("toggle");
                    } else {
                        alert('failed');
                    }
                },
                error: function () {
                    alert("error");
                },
                
            }); 
        } 

        /*self.saveEdit = function (item) {
            $.post({
                url: 'Items/Edit',
                data: { "itemJson": ko.toJSON(item) },
                success: function (result) {
                    alert(result);
                    $("edit-modal").modal("toggle");
                },
                error: function () {
                    alert("error"+result);
                }
                
            });
        }*/

    
     
    }

    return FoodItemsHandlerVM;
});