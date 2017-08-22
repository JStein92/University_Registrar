using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System;

namespace UniversityRegistrar.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
    [HttpGet("/students")]
    public ActionResult Students()
    {
      return View(Student.GetAll());
    }
    [HttpGet("/courses")]
    public ActionResult Courses()
    {
      return View(Course.GetAll());
    }
    [HttpPost("/courses")]
    public ActionResult CoursesPost()
    {
      string name = Request.Form["name"];
      int number = int.Parse(Request.Form["number"]);
      Course newCourse = new Course(name, number);
      newCourse.Save();

      return View("Courses", Course.GetAll());
    }
    [HttpPost("/students")]
    public ActionResult StudentsPost()
    {
      string newName = Request.Form["name"];
      DateTime newEnrollmentDate = DateTime.Parse(Request.Form["date"]);

      Student newStudent = new Student(newName, newEnrollmentDate);
      newStudent.Save();
      newStudent.AddCourse(Course.Find(int.Parse(Request.Form["course"])));

      return View("students", Student.GetAll());
    }

    [HttpGet("/add/course")]
    public ActionResult AddCourse()
    {
      return View();
    }

    [HttpGet("/add/student")]
    public ActionResult AddStudent()
    {
      return View(Course.GetAll());
    }

  }
}
