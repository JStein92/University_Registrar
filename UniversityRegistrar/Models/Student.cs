using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace UniversityRegistrar.Models
{
  public class Student
  {
    private int _id;
    private string _name;
    private DateTime _enrollmentDate;

    public Student(string name, DateTime enrollmentDate, int id = 0)
    {
      _id = id;
      _name = name;
      _enrollmentDate = enrollmentDate;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public DateTime GetEnrollmentDate()
    {
      return _enrollmentDate;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool nameEquality = this.GetName() == newStudent.GetName();
        bool enrollmentDateEquality = this.GetEnrollmentDate() == newStudent.GetEnrollmentDate();

        return (idEquality && nameEquality && enrollmentDateEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        DateTime enrollmentDate = rdr.GetDateTime(2);
        Student newStudent = new Student(name,enrollmentDate,id);
        allStudents.Add(newStudent);
      }
      conn.Close();
      if (conn!=null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students (name, enrollment_date) VALUES (@name, @enrollment_date);";

      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = _name;
      cmd.Parameters.Add(nameParameter);

      MySqlParameter enrollmentDateParameter = new MySqlParameter();
      enrollmentDateParameter.ParameterName = "@enrollment_date";
      enrollmentDateParameter.Value = _enrollmentDate;
      cmd.Parameters.Add(enrollmentDateParameter);

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
      cmd.CommandText = @"DELETE FROM students; DELETE FROM courses_students;";
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Student Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students WHERE id = @thisId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = id;
      cmd.Parameters.Add(idParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newId = 0;
      string name = "";
      DateTime enrollDate = DateTime.Now;

      while(rdr.Read())
      {
         newId = rdr.GetInt32(0);
         name = rdr.GetString(1);
         enrollDate = rdr.GetDateTime(2);
      }
      Student foundStudent = new Student(name,enrollDate,newId);
      return foundStudent;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd= new MySqlCommand("DELETE FROM students WHERE id = @thisId; DELETE FROM courses_students WHERE student_id = @thisId;", conn);

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = _id;
      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddCourse(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@courseId, @studentId);";

      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@courseId";
      course_id.Value = newCourse.GetId();
      cmd.Parameters.Add(course_id);

      MySqlParameter student_id = new MySqlParameter();
      student_id.ParameterName = "@studentId";
      student_id.Value = _id;
      cmd.Parameters.Add(student_id);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddDepartment(Department newDepartment)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO departments_students (department_id, student_id) VALUES (@departmentId, @studentId);";

      MySqlParameter department_id = new MySqlParameter();
      department_id.ParameterName = "@departmentId";
      department_id.Value = newDepartment.GetId();
      cmd.Parameters.Add(department_id);

      MySqlParameter student_id = new MySqlParameter();
      student_id.ParameterName = "@studentId";
      student_id.Value = _id;
      cmd.Parameters.Add(student_id);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public Department GetDepartment()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT departments.* FROM students
        JOIN departments_students ON (students.id = departments_students.student_id)
        JOIN departments ON (departments_students.department_id = departments.id)
        WHERE students.id = @studentId;";

      MySqlParameter studentId = new MySqlParameter();
      studentId.ParameterName = "@studentId";
      studentId.Value = _id;
      cmd.Parameters.Add(studentId);

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

    public List<Course> GetAllCourses()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT courses.* FROM students
        JOIN courses_students ON (students.id = courses_students.student_id)
        JOIN courses ON (courses_students.course_id = courses.id)
        WHERE students.id = @studentId;";

      MySqlParameter studentId = new MySqlParameter();
      studentId.ParameterName = "@studentId";
      studentId.Value = _id;
      cmd.Parameters.Add(studentId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Course> allCourses = new List<Course>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int number = rdr.GetInt32(2);
        Course newCourse = new Course(name, number, id);
        allCourses.Add(newCourse);
      }
      conn.Close();
      return allCourses;
    }

    public void Update(string name, DateTime enrollmentDate)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"UPDATE students SET name = @name, enrollment_date = @enrollmentDate WHERE id = @thisId;";

      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = name;
      cmd.Parameters.Add(nameParameter);

      MySqlParameter enrollmentDateParameter = new MySqlParameter();
      enrollmentDateParameter.ParameterName = "@enrollmentDate";
      enrollmentDateParameter.Value = enrollmentDate;
      cmd.Parameters.Add(enrollmentDateParameter);

      MySqlParameter studentId = new MySqlParameter();
      studentId.ParameterName = "@thisId";
      studentId.Value = _id;
      cmd.Parameters.Add(studentId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
