using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using VeganPlanner.Models;

namespace VeganPlanner.Migrations
{
    [DbContext(typeof(VeganPlannerContext))]
    [Migration("20170829144223_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VeganPlanner.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ItemID");

                    b.Property<decimal>("Quantity");

                    b.Property<int>("RecipeID");

                    b.Property<string>("Units");

                    b.HasKey("IngredientID");

                    b.HasIndex("ItemID");

                    b.HasIndex("RecipeID");

                    b.ToTable("Ingredient");
                });

            modelBuilder.Entity("VeganPlanner.Models.Item", b =>
                {
                    b.Property<int>("ItemID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CaloriesPerServing");

                    b.Property<string>("Category");

                    b.Property<bool>("IsGF");

                    b.Property<bool>("IsPantryItem");

                    b.Property<bool>("IsRecipe");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProteinPerServing");

                    b.Property<int?>("RecipeID");

                    b.Property<decimal>("ServingSize");

                    b.Property<string>("ServingUnits");

                    b.Property<string>("UserID");

                    b.HasKey("ItemID");

                    b.HasIndex("RecipeID");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("VeganPlanner.Models.Meal", b =>
                {
                    b.Property<int>("MealID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CaloriesPerServing");

                    b.Property<string>("NickName");

                    b.Property<string>("Notes");

                    b.Property<int>("ProteinPerServing");

                    b.Property<string>("UserID");

                    b.HasKey("MealID");

                    b.ToTable("Meal");
                });

            modelBuilder.Entity("VeganPlanner.Models.MealComponent", b =>
                {
                    b.Property<int>("MealComponentID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FoodItemID");

                    b.Property<int>("MealID");

                    b.HasKey("MealComponentID");

                    b.HasIndex("MealID");

                    b.ToTable("MealComponent");
                });

            modelBuilder.Entity("VeganPlanner.Models.Recipe", b =>
                {
                    b.Property<int>("RecipeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Notes");

                    b.Property<int>("Servings");

                    b.HasKey("RecipeID");

                    b.ToTable("Recipe");
                });

            modelBuilder.Entity("VeganPlanner.Models.Step", b =>
                {
                    b.Property<int>("StepID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("Order");

                    b.Property<int>("RecipeID");

                    b.HasKey("StepID");

                    b.HasIndex("RecipeID");

                    b.ToTable("Step");
                });

            modelBuilder.Entity("VeganPlanner.Models.Ingredient", b =>
                {
                    b.HasOne("VeganPlanner.Models.Item", "item")
                        .WithMany()
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VeganPlanner.Models.Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VeganPlanner.Models.Item", b =>
                {
                    b.HasOne("VeganPlanner.Models.Recipe", "recipe")
                        .WithMany()
                        .HasForeignKey("RecipeID");
                });

            modelBuilder.Entity("VeganPlanner.Models.MealComponent", b =>
                {
                    b.HasOne("VeganPlanner.Models.Meal")
                        .WithMany("MealComponents")
                        .HasForeignKey("MealID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VeganPlanner.Models.Step", b =>
                {
                    b.HasOne("VeganPlanner.Models.Recipe")
                        .WithMany("Instructions")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
