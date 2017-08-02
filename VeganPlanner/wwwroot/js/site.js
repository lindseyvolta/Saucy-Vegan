require(["js/FoodItemsHandlerVM"], function (FoodItemsHandlerVM) {
    /**
     * Populates knockout data in the specified element with a view model.
     * @param {String} elementName - The name of the element to populate.
     * @param {Function} viewModelDefinition - The view model definition to populate the element with.
     */
    function populateData(elementName, viewModelDefinition) {
        var element = document.getElementById(elementName);

        if (element) {
            var viewModel = ko.dataFor(element) || new viewModelDefinition();

            viewModel.populateData(element);
        }
    }

    // When the document loads, apply appropriate view models to their elements.
    $(document).ready(function () {
        populateData("FoodItems", FoodItemsHandlerVM);
    });
});