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
                type: "POST",
                url: "Items/Edit",
                data: ko.toJSON(item),
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#edit-modal").modal("toggle");
                }
            });  
        }

        
     
    }

    return FoodItemsHandlerVM;
});