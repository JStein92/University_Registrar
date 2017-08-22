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
      Department department = Department.Find(int.Parse(Request.Form["department"]));
      Course newCourse = new Course(name, number);
      Console.WriteLine(department.GetName()); // Not seeing department in coursedetails.CShtml
      newCourse.AddDepartment(department);
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
      newStudent.AddDepartment(Department.Find(int.Parse(Request.Form["department"])));

      return View("students", Student.GetAll());
    }

    [HttpGet("/add/course")]
    public ActionResult AddCourse()
    {

      return View(Department.GetAll());
    }

    [HttpGet("/add/student")]
    public ActionResult AddStudent()
    {
      return View(Department.GetAll());
    }

    [HttpGet("/studentdetail/{id}")]
    public ActionResult StudentDetails(int id)
    {
      Dictionary<string, object> model = new Dictionary<string,object>{};
      model.Add("allCourses", Course.GetAll());

      Student foundStudent = Student.Find(id);
      model.Add("student", foundStudent);

      return View(model);
    }

    [HttpPost("/studentdetail/{id}")]
    public ActionResult StudentDetailsNewCourse(int id)
    {
      Dictionary<string, object> model = new Dictionary<string,object>{};
      model.Add("allCourses", Course.GetAll());

      Course foundCourse = Course.Find(int.Parse(Request.Form["course"]));

      Student foundStudent = Student.Find(id);
      foundStudent.AddCourse(foundCourse);

      model.Add("student", foundStudent);

      return View("StudentDetails",model);
    }

    [HttpPost("/students/update")]
    public ActionResult StudentUpdate()
    {
      Student foundStudent = Student.Find(int.Parse(Request.Form["studentId"]));
      foundStudent.Update(Request.Form["name"], DateTime.Parse(Request.Form["date"]));

      return View("students",Student.GetAll());
    }

    [HttpGet("/coursedetail/{id}")]
    public ActionResult CourseDetails(int id)
    {
      Dictionary <string, object> model = new Dictionary<string,object>{};


      Course foundCourse = Course.Find(id);
      model.Add("course", foundCourse);
      model.Add("allDepartments", Department.GetAll());
      return View(model);
    }

    [HttpPost("/courses/update")]
    public ActionResult CourseUpdate()
    {
      Course newCourse = Course.Find(int.Parse(Request.Form["courseId"]));
      newCourse.Update(Request.Form["name"], int.Parse(Request.Form["number"]));

      return View("Courses", Course.GetAll());
    }

    [HttpGet("/delete/courses")]
    public ActionResult CoursesDelete()
    {
      Course.DeleteAll();

      return View("Success");
    }

    [HttpGet("/delete/students")]
    public ActionResult StudentsDelete()
    {
      Student.DeleteAll();

      return View("Success");
    }
    [HttpGet("/delete/{id}")]
    public ActionResult CourseDelete(int id)
    {
      Course foundCourse = Course.Find(id);
      foundCourse.Delete();
      return View ("courses", Course.GetAll());
    }
    [HttpGet("/delete/student/{id}")]
    public ActionResult StudentDelete(int id)
    {
      Student foundStudent = Student.Find(id);
      foundStudent.Delete();

      return View("students", Student.GetAll());
    }
    [HttpPost("/departments")]
    public ActionResult DepartmentsPost()
    {
      Department newDepartment = new Department(Request.Form["name"]);
      newDepartment.Save();
      return View("Departments", Department.GetAll());
    }
    [HttpGet("/Departments")]
    public ActionResult Departments()
    {
      return View(Department.GetAll());
    }
    [HttpGet("/AddDepartment")]
    public ActionResult AddDepartment()
    {
      return View();
    }
  }
}
