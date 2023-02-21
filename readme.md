# Recipe Management

## Introduction

You job is to implement a web API for storing cooking recipes.

## Data Handling

Each recipe consists of:

* Title (string, mandatory)
* Optional list of ingredients (each ingredient consists of a name, a unit-of-measure, and a quantity)
* Description (string, mandatory)
* Link to a title image (string, optional)

The system has to add an ID (integer) to each created recipe so that it can easily be identified.

The API stores recipes in an in-memory collection. You do not need to store data in a database (yet).

## Operations

Your API must support the following operations:

* Add a recipe.
* Get a list of all recipes.
* Delete a recipe (identified using its ID)
* Get a list of recipes filtered by title. The filter string must be *contained* in the recipe's title *or* in the recipe's description.
* Get a list of recipes filtered by ingredient. A recipe must be included if it contains at least on ingredients that contains the given filter string.
* Replace a recipe (identified using its ID). In this operation, all data of the recipe must be overwritten with the provided, new data.

It is part of the exercise to design the web API (URL structure, body structure, query string parameters, HTTP methods).

## Implementation

* Use the latest version of C# and .NET.
* Implement the web API with ASP.NET minimal API.
