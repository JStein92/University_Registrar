using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using UniversityRegistrar.Models;

namespace UniversityRegistrar.Tests
{
  [TestClass]
  public class DepartmentTests : IDisposable
  {
    public DepartmentTests()
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
    public void GetAll_ReturnEmptyDatabase_Departments()
    {
      int expected = 0;
      int actual = Department.GetAll().Count;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Save_SaveDepartment_Department()
    {
      Department newDepartment = new Department("Science");
      newDepartment.Save();

      List<Department> actual = Department.GetAll();
      List<Department> expected = new List<Department>{newDepartment};

      CollectionAssert.AreEqual(expected,actual);
    }

    [TestMethod]
    public void Delete_DeletesADepartment_List()
    {
      Department newDepartment = new Department("Science");
      newDepartment.Save();

      Department newDepartment2 = new Department("Education");
      newDepartment2.Save();

      newDepartment.Delete();

      List<Department> expected = new List<Department>{newDepartment2};
      List<Department> actual = Department.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

  }
}
