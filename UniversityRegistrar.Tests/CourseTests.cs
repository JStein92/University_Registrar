using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Tests
{
  [TestClass]
  public class CourseTests : IDisposable
  {
    public CourseTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_registrar_test;";
    }
    public void Dispose()
    {
        Course.DeleteAll();
        Student.DeleteAll();
    }

    [TestMethod]
    public void GetAll_ReturnEmptyDatabase_Courses()
    {
      int expected = 0;
      int actual = Course.GetAll().Count;

      Assert.AreEqual(expected,actual);
    }

    [TestMethod]
    public void Save_ReturnsSavedCourses_List()
    {

      Course newCourse = new Course("Math", 101);
      newCourse.Save();

      List<Course> expected = new List<Course> {newCourse};
      List<Course> actual = Course.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Find_FindsCourseById_Course()
    {

      Course newCourse = new Course("Math", 101);

      newCourse.Save();
      Course expected = newCourse;
      Course actual = Course.Find(newCourse.GetId());

      Assert.AreEqual(expected,actual);
    }

    [TestMethod]
    public void Delete_DeletesACourse_List()
    {
      Course newCourse = new Course("Math", 101);
      newCourse.Save();

      Course newCourse2 = new Course("English", 201);
      newCourse2.Save();

      newCourse.Delete();

      List<Course> expected = new List<Course>{newCourse2};
      List<Course> actual = Course.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddStudent_AddStudentToCourse_ListOfStudents()
    {
      Course newCourse = new Course("Math",101);
      newCourse.Save();

      DateTime testTime = new DateTime(1999,04,22,12,00,00);
      Student newStudent = new Student("Sam", testTime);
      newStudent.Save();

      newCourse.AddStudent(newStudent);

      List<Student> expected = new List<Student>{newStudent};
      List<Student> actual = newCourse.GetAllStudents();


      CollectionAssert.AreEqual(expected,actual);
    }

    [TestMethod]
    public void Update_UpdatesCourseDetails_Course()
    {
      Course newCourse = new Course("Math",101);
      newCourse.Save();

      newCourse.Update("Eng", 201);

      Course updateCourse = new Course("Eng", 201);
      string expected = updateCourse.GetName();
      //Console.WriteLine("EXPECTED " + expected.GetName() + expected.GetNumber());

      string actual = Course.Find(newCourse.GetId()).GetName();
    //  Console.WriteLine("ACTUAL " + actual.GetName() + actual.GetNumber());

      Assert.AreEqual(expected, actual);
    }
  }

}
