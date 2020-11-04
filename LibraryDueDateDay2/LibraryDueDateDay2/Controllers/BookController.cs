using System;
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

        public IActionResult Create(string id, string title, string author, string publicationDate, string checkedOutDate)
        {
            if (id != null || title != null || author != null || publicationDate != null || checkedOutDate != null)
            {
                try
                {
                    Book createdBook = CreateBook(id, title, author, publicationDate, checkedOutDate);
                    ViewBag.addMessage = $"You have successfully checked out {createdBook.Title} until {createdBook.DueDate}.";
                }
                catch (Exception e)
                {
                    ViewBag.addMessage = $"Unable to check out book: {e.Message}";
                }
            }
            return View();
        }

        public IActionResult List()
        {
            ViewBag.list = Books;
            return View();
        }

        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id.Trim()) || string.IsNullOrWhiteSpace(id.Trim()) || Books.Any(x => x.ID == int.Parse(id.Trim())) == false)
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

        public Book CreateBook(string title, string authorID, string publicationDate)
        {
            using (LibraryContext context = new LibraryContext())
            {
                if (! context.Books.Any(x => x.Title == title))
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
        }
        public Book GetBookByID(string id)
        {
            using (LibraryContext context = new LibraryContext())
            {
                return context.Books.Where(book => book.ID == int.Parse(id))
                    .Include(book => book.Borrows).SingleOrDefault();
            }
        }
        public void ExtendDueDateForBookByID(string id)
        {
            BorrowController.ExtendDueDateForBorrowByID(id);
        }

        public void ReturnBookByID(string id)
        {
            BorrowController.ReturnBorrowByID(id);
        }

        public void DeleteBookByID(string id)
        {
            using (LibraryContext context = new LibraryContext())
            {
                context.Books.Remove(context.Books.Where(book => book.ID == int.Parse(id)).SingleOrDefault());
                context.SaveChanges();
            }
        }

        public List<Book> GetBooks()
        {           
            using (LibraryContext context = new LibraryContext())
            {
                return context.Books.Include(book => book.Borrows).ToList();
            }           
        }

        public List<Book> GetOverdueBooks()
        {
            using (LibraryContext context = new LibraryContext())
            {
                /*
                List<int> overdueBookIDs = context.Borrows.Where(borrow => borrow.DueDate < DateTime.Today && borrow.ReturnedDate == null).Select(x => x.BookID).ToList();
                //Citation
                //https://stackoverflow.com/questions/14257360/linq-select-objects-in-list-where-exists-in-a-b-c/14257379
                //Referenced code from above source to check if element in the list
                List<Book> overdueBooks = context.Books.Where(book => overdueBookIDs.Contains(book.ID)).ToList();
                //End Citation
                */
                List<Book> overdueBooks = context.Borrows.Where(borrow => borrow.DueDate < DateTime.Today && borrow.ReturnedDate == null).Select(borrow => borrow.Book).Include(book => book.Borrows).ToList();
                return overdueBooks;
            }
        }
    }
}
