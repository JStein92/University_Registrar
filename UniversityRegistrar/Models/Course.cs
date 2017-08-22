using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace UniversityRegistrar.Models
{
  public class Course
  {
    private int _id;
    private string _name;
    private int _number;

    public Course(string name, int number, int id = 0)
    {
      _id = id;
      _name = name;
      _number = number;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public int GetNumber()
    {
      return _number;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetId() == newCourse.GetId();
        bool nameEquality = this.GetName() == newCourse.GetName();
        bool numberEquality = this.GetNumber() == newCourse.GetNumber();

        return (idEquality && nameEquality && numberEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int number = rdr.GetInt32(2);
        Course newCourse = new Course(name,number,id);
        allCourses.Add(newCourse);
      }
      conn.Close();
      if (conn!=null)
      {
        conn.Dispose();
      }
      return allCourses;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses (name, number) VALUES (@name, @number);";

      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = _name;
      cmd.Parameters.Add(nameParameter);

      MySqlParameter numberParameter = new MySqlParameter();
      numberParameter.ParameterName = "@number";
      numberParameter.Value = _number;
      cmd.Parameters.Add(numberParameter);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM courses; DELETE FROM courses_students;";
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Course Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses WHERE id = @thisId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = id;
      cmd.Parameters.Add(idParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newId = 0;
      string name = "";
      int number = 0;

      while(rdr.Read())
      {
         newId = rdr.GetInt32(0);
         name = rdr.GetString(1);
         number = rdr.GetInt32(2);
      }
      Course foundCourse = new Course(name,number,newId);
      return foundCourse;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand("DELETE FROM courses WHERE id = @thisId; DELETE FROM courses_students WHERE course_id = @thisId;", conn);

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = this._id;
      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddStudent(Student newStudent)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@courseId, @studentId);";

      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@courseId";
      course_id.Value = _id;
      cmd.Parameters.Add(course_id);

      MySqlParameter student_id = new MySqlParameter();
      student_id.ParameterName = "@studentId";
      student_id.Value = newStudent.GetId();
      cmd.Parameters.Add(student_id);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public List<Student> GetAllStudents()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText= @"SELECT students.*
      FROM courses
      JOIN courses_students ON (courses.id = courses_students.course_id)
      JOIN students ON (courses_students.student_id = students.id)
      WHERE courses.id = @courseId;";

      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@courseId";
      course_id.Value = _id;
      cmd.Parameters.Add(course_id);

      List<Student> allStudents = new List<Student>{};
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        DateTime enrollDate = rdr.GetDateTime(2);
        Student newStudent = new Student(name,enrollDate,id);
        allStudents.Add(newStudent);
      }
      conn.Close();

      return allStudents;
    }

    public void Update(string name, int number)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText= @"UPDATE courses SET name = @name, number = @number WHERE id = @thisId;";

      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = name;
      cmd.Parameters.Add(nameParameter);

      MySqlParameter numberParameter= new MySqlParameter();
      numberParameter.ParameterName = "@number";
      numberParameter.Value = number;
      cmd.Parameters.Add(numberParameter);

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddDepartment(Department newDepartment)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses_departments (course_id, department_id) VALUES (@courseId, @departmentId);";

      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@courseId";
      course_id.Value = _id;
      cmd.Parameters.Add(course_id);

      MySqlParameter department_id = new MySqlParameter();
      department_id.ParameterName = "@departmentId";
      department_id.Value = newDepartment.GetId();
      cmd.Parameters.Add(department_id);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public Department GetDepartment()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT departments.* FROM courses
        JOIN courses_departments ON (courses.id = courses_departments.course_id)
        JOIN departments ON (courses_departments.department_id = departments.id)
        WHERE courses.id = @courseId;";

      MySqlParameter courseId = new MySqlParameter();
      courseId.ParameterName = "@courseId";
      courseId.Value = _id;
      cmd.Parameters.Add(courseId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int id = 0;
      string name = "";

      while(rdr.Read())
      {
         id = rdr.GetInt32(0);
         name = rdr.GetString(1);
      }
      Department newDepartment = new Department(name, id);
      conn.Close();
      return newDepartment;
    }


  }
}
