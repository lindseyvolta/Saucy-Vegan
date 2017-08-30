define([], function () {

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

    return StepVM;
});