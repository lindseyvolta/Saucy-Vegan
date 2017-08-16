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
        self.DeleteItem = ko.observable();
        self.Ingredients = ko.observableArray();

        

        self.UnitList = [
            { UnitName: "lb"},
            { UnitName: "Cup"},
            { UnitName: "tsp"}, {UnitName: "Tbsp"}, {UnitName: "oz"}
        ];    

        self.addIngredient = function (item) {
            //self.Ingredients.push(new Ingredient());
            
        };

        self.removeIngredient = function (ingredient) {
            self.EditItem.recipe.Ingredients.remove(ingredient);
        };

        self.addStep = function () {
            var new_step = new Step();
            new_step.RecipeID = self.EditItem.recipeID;
            new_step.Description = "";
            new_step.Order = 0;
            self.EditItem.recipe.instructions.push(new Step());
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