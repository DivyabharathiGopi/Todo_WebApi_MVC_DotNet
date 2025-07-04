using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todo.Models;

namespace Todo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Simulating database with static list
        private static List<TodoItem> _todoList = new();
        private static int _idCounter = 1;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var todoListViewModel = new TodoViewModel
            {
                TodoList = _todoList,
                Todo = new TodoItem()
            };

            return View(todoListViewModel);
        }

        [HttpGet]
        public JsonResult PopulateForm(int id)
        {
            var todo = _todoList.FirstOrDefault(t => t.Id == id);
            return Json(todo);
        }

        [HttpPost]
        public RedirectResult Insert(TodoItem todo)
        {
            if (!string.IsNullOrWhiteSpace(todo.Name))
            {
                todo.Id = _idCounter++;
                _todoList.Add(todo);
            }

            return Redirect("/");
        }

        [HttpPost]
        public RedirectResult Update(TodoItem todo)
        {
            var existing = _todoList.FirstOrDefault(t => t.Id == todo.Id);
            if (existing != null)
            {
                existing.Name = todo.Name;
            }

            return Redirect("/");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var todo = _todoList.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                _todoList.Remove(todo);
            }

            return Json(new { success = true });
        }
    }
}




// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Data.Sqlite;
// using Microsoft.Extensions.Logging;
// using Todo.Models;


// namespace Todo.Controllers
// {
//     public class HomeController : Controller
//     {
//         private readonly ILogger<HomeController> _logger;

//         public HomeController(ILogger<HomeController> logger)
//         {
//             _logger = logger;
//         }

//         public IActionResult Index()
//         {
//             var todoListViewModel = GetAllTodos();
//             return View(todoListViewModel);
//         }

//         [HttpGet]
//         public JsonResult PopulateForm(int id)
//         {
//             var todo = GetById(id);
//             return Json(todo);
//         }


//         internal TodoViewModel GetAllTodos()
//         {
//             List<TodoItem> todoList = new();

//             using (SqliteConnection con =
//                    new SqliteConnection("Data Source=db.sqlite"))
//             {
//                 using (var tableCmd = con.CreateCommand())
//                 {
//                     con.Open();
//                     tableCmd.CommandText = "SELECT * FROM todo";

//                     using (var reader = tableCmd.ExecuteReader())
//                     {
//                         if (reader.HasRows)
//                         {
//                             while (reader.Read())
//                             {
//                                 todoList.Add(
//                                     new TodoItem
//                                     {
//                                         Id = reader.GetInt32(0),
//                                         Name = reader.GetString(1)
//                                     });
//                             }
//                         }
//                         else
//                         {
//                             return new TodoViewModel
//                             {
//                                 TodoList = todoList
//                             };
//                         }
//                     };
//                 }
//             }

//             return new TodoViewModel
//             {
//                 TodoList = todoList
//             };
//         }

//         internal TodoItem GetById(int id)
//         {
//             TodoItem todo = new();

//             using (var connection =
//                    new SqliteConnection("Data Source=db.sqlite"))
//             {
//                 using (var tableCmd = connection.CreateCommand())
//                 {
//                     connection.Open();
//                     tableCmd.CommandText = $"SELECT * FROM todo Where Id = '{id}'";

//                     using (var reader = tableCmd.ExecuteReader())
//                     {
//                         if (reader.HasRows)
//                         {
//                             reader.Read();
//                             todo.Id = reader.GetInt32(0);
//                             todo.Name = reader.GetString(1);
//                         }
//                         else
//                         {
//                             return todo;
//                         }
//                     };
//                 }
//             }

//             return todo;
//         }

//         public RedirectResult Insert(TodoItem todo)
//         {
//             using (SqliteConnection con =
//                    new SqliteConnection("Data Source=db.sqlite"))
//             {
//                 using (var tableCmd = con.CreateCommand())
//                 {
//                     con.Open();
//                     tableCmd.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";
//                     try
//                     {
//                         tableCmd.ExecuteNonQuery();
//                     }
//                     catch (Exception ex)
//                     {
//                         Console.WriteLine(ex.Message);
//                     }
//                 }
//             }
//             return Redirect("http://localhost:5010");
//         }

//         [HttpPost]
//         public JsonResult Delete(int id)
//         {
//             using (SqliteConnection con =
//                    new SqliteConnection("Data Source=db.sqlite"))
//             {
//                 using (var tableCmd = con.CreateCommand())
//                 {
//                     con.Open();
//                     tableCmd.CommandText = $"DELETE from todo WHERE Id = '{id}'";
//                     tableCmd.ExecuteNonQuery();
//                 }
//             }

//             return Json(new {});
//         }

//         public RedirectResult Update(TodoItem todo)
//         {
//             using (SqliteConnection con =
//                    new SqliteConnection("Data Source=db.sqlite"))
//             {
//                 using (var tableCmd = con.CreateCommand())
//                 {
//                     con.Open();
//                     tableCmd.CommandText = $"UPDATE todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}'";
//                     try
//                     {
//                         tableCmd.ExecuteNonQuery();
//                     }
//                     catch (Exception ex)
//                     {
//                         Console.WriteLine(ex.Message);
//                     }
//                 }
//             }

//             return Redirect("http://localhost:5010");
//         }
//     }
// }


