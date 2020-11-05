﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDueDateDay2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryDueDateDay2.Controllers
{
    public class BookController : Controller
    {
        //public static List<Book> Books = new List<Book>();
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult Create(string title, string authorID, string publicationDate)
        {
            ViewBag.authors = AuthorController.GetAuthors();
            if (Request.Method == "POST")
            {
                try
                {
                    Book createdBook = CreateBook(title, authorID, publicationDate);

                    ViewBag.addMessage = $"You have successfully created {createdBook.Title}.";
                }
                catch (Exception e)
                {
                    ViewBag.authorID = authorID;
                    ViewBag.bookTitle = title;
                    ViewBag.addMessage = $"Unable to create book: {e.Message}";
                }                
            }                
            return View();
        }

        public IActionResult List()
        {
            ViewBag.list = GetBooks();
            ViewBag.overdueBooks = GetOverdueBooks();
            return View();
        }

        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id.Trim()) || string.IsNullOrWhiteSpace(id.Trim()))
            {
                ViewBag.errorMessage = "No book selected.";
            }
            else
            {
                try
                {
                    ViewBag.bookDetails = GetBookByID(id);
                }
                catch
                {

                }
            }
            return View();
        }

        public IActionResult Extend(string id)
        {
            ExtendDueDateForBookByID(id);
            return RedirectToAction("Details", new Dictionary<string, string>() { { "id", id } });
        }

        public IActionResult Return(string id)
        {
            ReturnBookByID(id);
            return RedirectToAction("Details", new Dictionary<string, string>() { { "id", id } });
        }

        public IActionResult Delete(string id)
        {
            DeleteBookByID(id);
            return RedirectToAction("List");
        }
        public IActionResult Borrow(string id)
        {
            CreateBorrow(id);
            return RedirectToAction("List");
        }

        public Book CreateBook(string title, string authorID, string publicationDate)
        {
            int parsedAuthorID;
            DateTime parsedPublicationdate;

            // Trim the values so we don't need to do it a bunch of times later.
            authorID = authorID != null ? authorID.Trim() : null;
            title = title != null ? title.Trim() : null;
            publicationDate = publicationDate != null ? publicationDate.Trim() : null;
            // Check for individual validation cases and throw an exception if they fail.

            // No value for authorID.
            if (string.IsNullOrWhiteSpace(authorID) || string.IsNullOrEmpty(authorID))
            {
                throw new Exception("AuthorID Not Provided");
            }

            // Author ID fails parse.
            if (!int.TryParse(authorID, out parsedAuthorID))
            {
                throw new Exception("Author ID Not Valid");
            }

            // No value for title.
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrEmpty(title))
            {
                throw new Exception("Title Not Provided");
            }

            if (string.IsNullOrWhiteSpace(publicationDate) || string.IsNullOrEmpty(publicationDate))
            {
                throw new Exception("Publication Date  Not Provided");
            }

            // publicationDate fails parse.
            if (!DateTime.TryParse(publicationDate, out parsedPublicationdate))
            {
                throw new Exception("Publication Date Not Valid");
            }           
            using LibraryContext context = new LibraryContext();

            if (!context.Authors.Any(x => x.ID == parsedAuthorID))
            {
                throw new Exception("Author Does Not Exist in DB");
            }
            if (!context.Books.Any(x => x.Title == title))
            {
                Book newBook = new Book()
                {
                    AuthorID = int.Parse(authorID),
                    Title = title.Trim(),
                    PublicationDate = DateTime.Parse(publicationDate)
                };

                context.Books.Add(newBook);
                context.SaveChanges();

                return newBook;
            }
            else
            {
                throw new Exception("Book already exists.");
            }
        }
        public Book GetBookByID(string id)
        {
            using LibraryContext context = new LibraryContext();
            return context.Books.Where(book => book.ID == int.Parse(id))
                .Include(book => book.Borrows).Include(x => x.Author).SingleOrDefault();
        }
        public void ExtendDueDateForBookByID(string id)
        {
            BorrowController.ExtendDueDateForBorrowByID(id);
        }

        public void ReturnBookByID(string id)
        {
            BorrowController.ReturnBorrowByID(id);
        }
        public void CreateBorrow(string id)
        {
            BorrowController.CreateBorrow(id);
        }

        public void DeleteBookByID(string id)
        {
            using LibraryContext context = new LibraryContext();
            context.Books.Remove(context.Books.Where(book => book.ID == int.Parse(id)).SingleOrDefault());
            context.SaveChanges();
        }

        public List<Book> GetBooks()
        {
            using LibraryContext context = new LibraryContext();
            return context.Books.Include(book => book.Borrows).Include(x => x.Author).ToList();
        }

        public List<Book> GetOverdueBooks()
        {
            using LibraryContext context = new LibraryContext();
            /*
            List<int> overdueBookIDs = context.Borrows.Where(borrow => borrow.DueDate < DateTime.Today && borrow.ReturnedDate == null).Select(x => x.BookID).ToList();
            //Citation
            //https://stackoverflow.com/questions/14257360/linq-select-objects-in-list-where-exists-in-a-b-c/14257379
            //Referenced code from above source to check if element in the list
            List<Book> overdueBooks = context.Books.Where(book => overdueBookIDs.Contains(book.ID)).ToList();
            //End Citation
            */
            List<Book> overdueBooks = context.Borrows.Where(borrow => borrow.DueDate < DateTime.Today && borrow.ReturnedDate == null).Select(borrow => borrow.Book).ToList();
            return overdueBooks;
        }
    }
}
