using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Tests
{
  [TestClass]
  public class StudentTests : IDisposable
  {
    public StudentTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_registrar_test;";
    }
    public void Dispose()
    {
        Course.DeleteAll();
        Student.DeleteAll();
        Department.DeleteAll();
    }

    [TestMethod]
    public void GetAll_ReturnEmptyDatabase_Students()
    {
      int expected = 0;
      int actual = Student.GetAll().Count;

      Assert.AreEqual(expected,actual);
    }

    [TestMethod]
    public void Save_ReturnsSavedStudents_List()
    {
      DateTime testTime = new DateTime(1999,04,22,12,00,00);
      Student newStudent = new Student("Sam", testTime);
      newStudent.Save();

      List<Student> expected = new List<Student> {newStudent};
      List<Student> actual = Student.GetAll();

      // foreach(var student in expected)
      // {
      //   Console.WriteLine("EXPECTED: " + student.GetName() + " ID: " + student.GetId() + " EnrollDate: " + student.GetEnrollmentDate());
      // }
      // foreach(var student in actual)
      // {
      //   Console.WriteLine("ACTUAL: " + student.GetName() + " ID: " + student.GetId() + " EnrollDate: " + student.GetEnrollmentDate());
      // }

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Find_FindsStudentById_Student()
    {
      DateTime testTime = new DateTime(1999,04,22,12,00,00);
      Student newStudent = new Student("Sam", testTime);

      newStudent.Save();
      Student expected = newStudent;
      Student actual = Student.Find(newStudent.GetId());

      Assert.AreEqual(expected,actual);
    }

    [TestMethod]
    public void Delete_DeletesAStudent_List()
    {
      DateTime testTime = new DateTime(1999,04,22,12,00,00);
      Student newStudent = new Student("Sam", testTime);
      newStudent.Save();

      DateTime testTime2 = new DateTime(2000,05,22,12,00,00);
      Student newStudent2 = new Student("Sally", testTime2);
      newStudent2.Save();

      newStudent.Delete();

      List<Student> expected = new List<Student>{newStudent2};
      List<Student> actual = Student.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddCourse_AddCourseToStudent_ListOfCourses()
    {
      Course newCourse = new Course("Math", 101);
      newCourse.Save();

      DateTime testTime = new DateTime(1999,04,22,12,00,00);
      Student newStudent = new Student("Sam", testTime);
      newStudent.Save();

      newStudent.AddCourse(newCourse);

      List<Course> expected = new List<Course>{newCourse};
      List<Course> actual = newStudent.GetAllCourses();

      // foreach(var course in expected)
      // {
      //   Console.WriteLine("EXPECTED: " + course.GetName() + " ID: " + course.GetId());
      // }
      // foreach(var course in actual)
      // {
      //   Console.WriteLine("ACTUAL: " + course.GetName() + " ID: " + course.GetId());
      // }

      CollectionAssert.AreEqual(expected,actual);
    }

    [TestMethod]
    public void Update_UpdatesStudentDetails_Student()
    {
      DateTime testTime = new DateTime(1999,04,22,12,00,00);
      Student newStudent = new Student("Sam", testTime);
      newStudent.Save();

      DateTime updatedTime = new DateTime(2004,03,11,12,00,00);
      newStudent.Update("Samuel", updatedTime);

      Student expectedStudent = new Student("Samuel", updatedTime);
      string expected = expectedStudent.GetName();
      //Console.WriteLine("EXPECTED " + expected.GetName() + expected.GetNumber());

      string actual = Student.Find(newStudent.GetId()).GetName();
    //  Console.WriteLine("ACTUAL " + actual.GetName() + actual.GetNumber());

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddDepartment_AddDepartmentToStudent_ListOfDepartments()
    {
      Department newDepartment = new Department("Science");
      newDepartment.Save();

      DateTime testTime = new DateTime(1999,04,22,12,00,00);
      Student newStudent = new Student("Sam", testTime);
      newStudent.Save();

      newStudent.AddDepartment(newDepartment);

      string expected = newDepartment.GetName();
      string actual = newStudent.GetDepartment().GetName();

      Assert.AreEqual(expected,actual);
    }


  }

}
