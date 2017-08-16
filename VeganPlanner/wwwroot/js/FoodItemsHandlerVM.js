define([], function () {  
    
    function ItemVM () {
        var self = this;

        self.ItemID = ko.observable();
        self.Name = ko.observable();
        self.IsRecipe = ko.observable();
        self.ServingSize = ko.observable();
        self.ServingUnits = ko.observable();
        self.Category = ko.observable();
        self.UserID = ko.observable();
        self.CaloriesPerServing = ko.observable();
        self.ProteinPerServing = ko.observable();
        self.IsPantryItem = ko.observable();
        self.IsGF = ko.observable();
        self.RecipeID = ko.observable();
        self.recipe = ko.observable(new RecipeVM());

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
            self.RecipeID(data.recipeID);
            self.recipe().load(data.recipe);
        }
    }

    function RecipeVM() {
        var self = this;

        self.RecipeID = ko.observable();
        self.Notes = ko.observable();
        self.Servings = ko.observable();
        self.Ingredients = ko.observableArray();
        self.Instructions = ko.observableArray();

        self.load = function (data) {
            self.RecipeID(data.recipeID);
            self.Notes(data.notes);
            self.Servings(data.servings);
            self.Ingredients(data.ingredients);
            self.Instructions(data.instructions);
        }
    }

    function IngredientVM () {
        var self = this;

        self.IngredientID = ko.observable();
        self.Item = ko.observable(new ItemVM());
        self.ItemID = ko.observable();
        self.Quantity = ko.observable();
        self.RecipeID = ko.observable();
        self.Units = ko.observable();

        self.load = function (data) {
            self.IngredientID(data.ingredientID);
            self.Item().load(data.item);
            self.ItemID(data.itemID);
            self.Quantity(data.quantity);
            self.RecipeID(data.recipeID);
            self.Units(data.units);
        }
    }

    function StepVM () {
        var self = this;

        self.StepID = ko.observable();
        self.RecipeID = ko.observable();
        self.Description = ko.observable();
        self.Order = ko.observable();

        self.load = function (data) {
            self.StepID(data.stepID);
            self.RecipeID(data.recipeID);
            self.Description(data.description);
            self.Order(data.order);
        }
    }

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
        self.DeleteItem = ko.observable();
        self.IngredientList = ko.observableArray();

        

        self.UnitList = [
            { UnitName: "lb"},
            { UnitName: "Cup"},
            { UnitName: "tsp"}, {UnitName: "Tbsp"}, {UnitName: "oz"}
        ];    

        self.addIngredient = function () {
            self.EditItem().recipe.ingredients.push(new IngredientVM());
            
        };

        self.removeIngredient = function (ingredient) {
            self.EditItem.recipe.Ingredients.remove(ingredient);
        };

        self.addStep = function () {
            
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

                //self.Items(data);
                 for(var i = 0; i < data.items.length ; i+=1){
                     self.Items.push(new ItemVM(data.items[i]));
                 }
                    

                    //if (element && !ko.dataFor(element))
                      // ko.applyBindings(self, element);
                }
            });  
        }

        self.showDetails = function (item) {
            self.DetailItem(item);
            $("#details-modal").modal("show");
        }

        self.showEdit = function (item) {
            self.EditItem(item);
            
            $("#edit-modal").modal("show");
        }

        
        self.saveEdit = function (item) {

            $.ajax({
                type: 'post',
                dataType: 'json',
                url: 'Items/Edit',
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
        
        // public void testComputed(CString test) { do stuff that... }
        /*self.testComputed = ko.pureComputed(function () {
            self.test();
            ko.ignoreDependencies(function () {
                // do stuff that won't trigger testComputed's reevaluation.
            });
        });*/

        self.deleteConfirm = function (item) {
            swal({
                title: "Delete " + item.name() + "?",
                text: "You will not be able to recover this food item.",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Delete",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: true
            },
                function (isConfirm) {
                    
                    if (isConfirm) {
                        
                        $.ajax({
                            type: 'post',
                            dataType: 'json',
                            url: 'Items/DeleteConfirmed',
                            data: { "json": ko.toJSON(item) },
                            success: function (json) {
                                if (json === "Delete confirmed") {
                                    self.Items.remove(item);
                                    swal({
                                        type: "success",
                                        title: "Deleted!",
                                        timer: 2000,
                                        allowOutsideClick: true,
                                        showConfirmButton: false
                                    });
                                } else {
                                    swal({
                                        type: "error",
                                        title: "Erorr Occurred",
                                        text: "You cannot delete a food item that is part of an existing recipe.",
                                        timer: 2000,
                                        showConfirmButton: false
                                    });
                                }
                            },
                            error: function () {
                                alert("error");
                            },

                        }); 
                        
                    }
                });
        }
       
    
     
    }

    return FoodItemsHandlerVM;
});