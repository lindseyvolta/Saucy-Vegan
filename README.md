# Saucy Vegan
# Lindsey Volta

After learning about web development at V-Technologies, I really wanted to try to build my own web application from scratch.  My mom has been looking for a way to organize and share her low-carb vegan menu ideas. Together we came up with a design for a web application that would allow users to easily build meals, plan their dinners, and get a shopping list. I am also a long-time vegan and foodie so I was really excited about this idea.
 
There are three main objects that the application will work with: Food Items (which may be recipes), Meals (which consist of Food Items), and Meal Schedule (meals and number of servings assigned to calendar dates). I used Microsoft’s ASP.NET Core framework, designing the Food Item model and controller in C#, with Entity Framework seamlessly managing the database. I also added the ASP.NET Identity framework to implement individual user accounts and authentication.
 
I then built the user interface for the CRUD (Create, Read, Update and Delete) operations for the Food Item objects. When a Food Item is a recipe, the UI adds two tables with lists of ingredients and steps which the user can add/edit/insert in-line. I use Knockout.js to keep the client side (JavaScript) model in sync the view (HTML). I began working on meals using the Chosen jQuery plugin, which allows users to manage the list of Food Items in a meal.
 
That's as far as I got this summer, but I plan on working on it while at school this fall and winter. Once I finish the basic functionality I will learn how to host it on Azure. I have so many ideas about how this basic design can be expanded by adding things like user-ratings, automated suggestions, connection to a free food list API like Spoonacular, and maybe even a mobile app!
