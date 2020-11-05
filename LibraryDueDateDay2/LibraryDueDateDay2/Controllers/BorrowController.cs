using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDueDateDay2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryDueDateDay2.Controllers
{
    public class BorrowController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public static void ExtendDueDateForBorrowByID(string bookID)
        {
            using LibraryContext context = new LibraryContext();
            Borrow requiredBorrow = context.Borrows.Where(borrow => borrow.BookID == int.Parse(bookID)).SingleOrDefault();
            requiredBorrow.DueDate = requiredBorrow.DueDate.AddDays(7);
            context.SaveChanges();
        }
        public static void ReturnBorrowByID(string bookID)
        {
            using LibraryContext context = new LibraryContext();
            Borrow requiredBorrow = context.Borrows.Where(borrow => borrow.BookID == int.Parse(bookID)).SingleOrDefault();

            requiredBorrow.ReturnedDate = DateTime.Today;
            context.SaveChanges();
        }

        public static void CreateBorrow()
        {
            using LibraryContext context = new LibraryContext();
            context.Borrows.Add(new Borrow()
            {
                CheckedOutDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14),
                ReturnedDate = null
            });
            context.SaveChanges();
        }
    }
}
