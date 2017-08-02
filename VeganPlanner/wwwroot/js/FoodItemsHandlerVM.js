define([], function () {
    /**
     * View model that handles the "My Kitchen -> Food Items" view.
     */
    function FoodItemsHandlerVM() {
        var self = this;

        self.SearchString = ko.observable();
        self.Items = ko.observableArray();

        self.populateData = function (element) {
            $.ajax({
                type: "GET",
                url: "Items/GetItems",
                data: { searchString: self.SearchString() },
                success: function (data) {
                    ko.mapping.fromJS(data.items, {}, self.Items);

                    if (element && !ko.dataFor(element))
                        ko.applyBindings(self, element);
                }
            });  
        }
    }

    return FoodItemsHandlerVM;
});