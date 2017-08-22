define([], function () {

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

    function RecipeVM() {
        var self = this;

        self.RecipeID = ko.observable();
        self.Notes = ko.observable();
        self.Servings = ko.observable();
        self.Ingredients = ko.observableArray().extend({deferred: true});
        self.Instructions = ko.observableArray();

        self.load = function (data) {
            self.RecipeID(data.recipeID);
            self.Notes(data.notes);
            self.Servings(data.servings);
            //self.Ingredients(data.ingredients);

            for (var i = 0; i < data.ingredients.length; i += 1) {
                var anIngredient = new IngredientVM();
                anIngredient.load(data.ingredients[i]);
                self.Ingredients.push(anIngredient);
            }
            //self.Instructions(data.instructions);

            for (var i = 0; i < data.instructions.length; i += 1) {
                var anInstruction = new StepVM();
                anInstruction.load(data.instructions[i]);
                self.Instructions.push(anInstruction);
            }
        }
    }

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

    function StepVM() {
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

        window.instance = self;

        self.SearchString = ko.observable();
        self.ItemCategory = ko.observable();
        self.Items = ko.observableArray();
        self.DetailItem = ko.observable();
        self.EditItem = ko.observable();
        self.DeleteItem = ko.observable();
        self.ItemsDropDown = ko.observableArray().extend({ deferred: true });
        self.CategoryDropDown = ko.observableArray().extend({ deferred: true });
        self.ModalTitle = ko.observable();


        self.UnitList = [
            { UnitName: "lb"},
            { UnitName: "Cup"},
            { UnitName: "tsp"}, {UnitName: "Tbsp"}, {UnitName: "oz"}
        ];    

        self.addIngredient = function () {
            self.EditItem().Recipe.Ingredients.push(new IngredientVM());
        };

        self.removeIngredient = function (ingredient) {
            self.EditItem().Recipe.Ingredients.remove(ingredient);
        };

        self.addStep = function (index) {
            var newStep = new StepVM();
            newStep.Order = index;
            self.EditItem().Recipe.Instructions.push(newStep);
        };

        self.removeStep = function (instruction) {
            self.EditItem().Recipe.Instructions.remove(instruction);
        };

        self.populateData = function (element) {            
            $.ajax({
                type: "GET",
                url: "Items/GetItems",
                data: { searchString: self.SearchString(), itemCategory: self.ItemCategory() },
                success: function (data) {

                    self.Items.removeAll();

                    for (var i = 0; i < data.items.length; i += 1){
                         var anitem = new ItemVM();
                         anitem.load(data.items[i])
                         self.Items.push(anitem);
                     }
                    
                     if (element && !ko.dataFor(element))
                        ko.applyBindings(self, element);

                 
                     self.GetCategoriesDropDownList();
                }

            });  
        }


        self.showDetails = function (item) {
            self.DetailItem(item);
            $("#details-modal").modal("show");
        }


        self.GetCategoriesDropDownList = function () {
            $.ajax({
                type: "GET",
                url: "Items/GetCategoriesDropDown",
                success: function (data) {
                    var array = [];                                
                    for (var i = 0; i < data.categoryQuery.length; i += 1) {
                        array.push(data.categoryQuery[i]);
                    }   

                    self.CategoryDropDown(array);                 
                }
            });
        }

        self.GetItemsDropDownListNew = function () {
            var array = [];
            for (var i = 0; i < self.Items().length; i += 1) {
                if (self.Items()[i].IsRecipe() == false)
                    array.push(self.Items()[i]);
            }

            self.ItemsDropDown(array);

        }


        self.showEdit = function (item) {
            if (item == null) {
                self.ModalTitle = "Create";
                var item = new ItemVM();
            } else {
                self.ModalTitle = "Edit";
                
            }

            
            self.ItemsDropDown.removeAll();
            self.GetItemsDropDownListNew();
            self.EditItem(item);
            $("#edit-modal").modal("show");
        }

        
        
        self.saveEdit = function (item) {
            //If we are creating a new food item
            if (item.ItemID._latestValue === undefined) {
                $.ajax({
                    type: 'post',
                    dataType: 'json',
                    url: 'Items/Create',
                    data: { "json": ko.toJSON(item) },
                    success: function (json) {
                        if (json == "Created") {
                            swal({
                                type: "success",
                                title: "Saved",
                                text: item.Name() + " has been created.",
                                timer: 2000,
                                showConfirmButton: false
                            });
                            $("#edit-modal").modal("toggle");
                            self.populateData();                            
                        } else {
                            alert('failed');
                        }
                    },
                    error: function () {
                        alert("error");
                    },

                }); 

            } else {
                $.ajax({
                    type: 'post',
                    dataType: 'json',
                    url: 'Items/Edit',
                    data: { "json": ko.toJSON(item) },
                    success: function (json) {
                        if (json) {
                            swal({
                                type: "success",
                                title: "Saved",
                                text: item.Name() + " has been edited.",
                                timer: 2000,
                                showConfirmButton: false
                            });
                            $("#edit-modal").modal("toggle");
                            self.populateData();  
                        } else {
                            alert('failed');
                        }
                    },
                    error: function () {
                        alert("error");
                    },

                }); 
            }

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
                title: "Delete " + item.Name() + "?",
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