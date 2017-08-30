define([], function () {

    function MealComponentVM() {
        var self = this;

        self.MealComponentID = ko.observable();
        self.MealID = ko.observable();
        self.FoodItemID = ko.observable();
        self.FoodItem = ko.observable();

        self.load = function (data) {
            self.MealComponentID(data.mealComponentID);
            self.MealID(data.mealID);
            self.FoodItemID(data.foodItemID);
            var im = new ItemVM();
            im.load(data.foodItem);
            self.FoodItem(im);
        }
    }


    function MealVM() {
        var self = this;

        self.MealID = ko.observable();
        self.NickName = ko.observable();
        self.UserID = ko.observable();
        self.CaloriesPerServing = ko.observable();
        self.ProteinPerServing = ko.observable();
        self.Notes = ko.observable();
        self.MealComponents = ko.observableArray().extend({ deferred: true });

        self.CreatedByAdmin = ko.pureComputed(function () {
            if (self.UserID() == "lvolta@umich.edu" || self.UserID() == "jvolta@vtechnologies.com") {
                return true;
            } else {
                return false;
            }
        });

        self.load = function (data) {
            self.MealID(data.mealID);
            self.NickName(data.nickName);
            self.UserID(data.userID);
            self.CaloriesPerServing(data.caloriesPerServing);
            self.ProteinPerServing(data.proteinPerServing);
            self.Notes(data.notes);

            for (var i = 0; i < data.mealComponents.length; i += 1) {
                var aMealComponent = new MealComponentVM();
                aMealComponent.load(data.mealComponents[i]);
                self.MealComponents.push(aMealComponent);
            }
        }
    }

        function MealsHandlerVM() {
            var self = this;

            window.instance = self;

            self.Meals = ko.observableArray();
            self.SearchString = ko.observable();

            self.populateData = function (element) {
                $.ajax({
                    type: "GET",
                    url: "Meals/GetMeals",
                    data: { searchString: self.SearchString() },
                    success: function (data) {

                        self.Meals.removeAll();

                        for (var i = 0; i < data.meals.length; i += 1) {
                            var ameal = new MealVM();
                            ameal.load(data.meals[i])
                            self.Meals.push(ameal);
                        }

                        if (element && !ko.dataFor(element))
                            ko.applyBindings(self, element);
                    }
                });
            }
        }

       return MealsHandlerVM;
});