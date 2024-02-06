# Ordering API ASP.NET

This project is a practice to learn more about ASP.NET Core and C#.

I'm using the MVC pattern and Test Driven Development (TDD) for this project. TDD is a way to develop software by writing tests first.
I'm confortable enough with TDD, but MVC is new to me. I thought i knew it, but I didn't.

### Features

 - [x] List orders
 - [ ] See details of order
 - [x] Order product
 - [x] Cancel order of product

### Non-Functional Requirements

 - [x] Send orders recently made to a microservice via Message Queue


### Why use Decimal instead of Double type?

The `double` type is not as precise as `decimal`. Due to this, `decimal` is recommended for when number precision is needed, for example currency, one of the domain object used in this project.

### References 
- [Patterns of Enterprise Application Architecture - Martin Fowler](https://martinfowler.com/books/eaa.html)
- [Floating-point numeric types (C# reference)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)
- [Gui Archictecture](https://martinfowler.com/eaaDev/uiArchs.html#ModelViewController)
- [observer pattern vs MVC](https://stackoverflow.com/questions/15563005/observer-pattern-vs-mvc)
- [MVC: Model View Controller -- does the View call the Model?](https://stackoverflow.com/questions/2621725/mvc-model-view-controller-does-the-view-call-the-model)

### Credits

Made with :heart: by matheusinit 
