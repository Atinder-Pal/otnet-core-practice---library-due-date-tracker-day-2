# dotnet-core-practice---library-due-date-tracker-day-2-Atinder-Pal

**Purpose:** This practice is meant to challenge your mastery of ASP.NET Web Application (Model - View - Controller) 
and how well you are able to use MVC to create a CRUD application. 
The goal in this assignment is to create a tool that will help keep track of all the books that are checked out of a library.
This is a cumulative activity. Using code from ASP.NET Core Practice - Library Due Date Tracker Day 1 as a starting point. 
We will be adding onto this code for a consequent assignment.

**Author:** Atinder Pal

**Requirements:**
Generate the following ERD using code-first database generation techniques:
![ERD](image.png)

Ensure all controller methods interface with Entity Framework and the database, and not the list from ASP.NET Core Practice - Library Due Date Tracker Day 1.
Add a “LibraryContext” class (Context):
All requisite methods and properties to function as a context.
Database connection string to a database called “mvc_library”.
Ensure the delete behaviour for “Author” is “Restrict”.
Ensure the delete behaviour for “Book” is “Cascade”.
Seed the database with:
At least 5 “Authors” of your choice.
At least 3 “Books” by the same Author.
These three books must have a “CheckoutDate” equal to December 25, 2019.
One book must be returned on-time with no extension.
One book must be returned on-time with one extension.
One book must not have been returned at all!
Add migrations and update the database once this and the models are completed.
Add a scaffolded controller and views for the “Author” model (using “LibraryContext”).
Add a “GetAuthors()” method that will return a list of authors (for use in the Create “Book” view).
Do NOT use scaffolding for the “Book” or “Borrow” models, continue to use the manually generated Controller and Views from ASP.NET Core Practice - Library Due Date Tracker Day 1.
“BorrowController” (Controller) class created:
Add a “ExtendDueDateForBorrowByID()” method that will extend the “DueDate” by 7 days from today.
Add a “ReturnBorrowByID()” method that will set the “Borrow”s “ReturnedDate” to today.
Add a “CreateBorrow()” method that will accept a “Book.ID” as a parameter and create a borrow for it.
The “CheckOutDate” will be today.
The “DueDate” will be 14 days from today.
The “ReturnedDate” will be null.
“BookController” (Controller) class modified:
Remove the “Books” property (static list of Books).
Modify “ExtendDueDateForBookByID()” to call “BorrowController.ExtendDueDateForBorrowByID()”.
Modify “ReturnBookByID()” to call “BorrowController.ReturnBorrowByID()”.
Modify “DeleteBookByID()” to delete a book.
Add a “GetBooks()” method to get a list of all books.
Add a “GetOverdueBooks()” method to get a list of all books that have a due date prior to the current date.
Modify “GetBookByID()” to get a specific book from the database.
Ensure that the necessary virtual properties are populated on all ‘Get’ methods before returning results.
Ensure that “CreateBook()” is no longer accepting an ID, as it is database generated.
“Book” “Create” (View) modified:
Have a dropdown (select) to select the “Author”.
Populate the dropdown (select) based on the “Author” table (Call “AuthorController.GetAuthors()”).
Remove the field for “ID”.
“Book” “List” (View) modified:
Modify the output to account for the new models (show the property values from the related tables).
“Book” “Details” (View) modified:
Modify the output to account for the new models (show the property values from the related tables).
Add a Borrow Book button that will create a Borrow for the book.

*Edit views for “Book”s and “Borrow”s are NOT necessary. The “Return” / “Extend” / “Delete” buttons on the Details view will suffice.



**Link to Trello Board:** https://trello.com/b/i393Lv3l/library-due-date-tracker-day-2
