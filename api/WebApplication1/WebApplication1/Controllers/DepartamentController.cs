using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartamentController(IConfiguration configuration)
        {
           _configuration = configuration;
            
        }
     
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select DepartmentId, DepartmentName from
                            dbo.Department
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }
        [HttpPost]

        public JsonResult Post(Departament dep)
        {
            string storedProcedure = "PR_I_DEPARTMENT";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(storedProcedure, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;

                    
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartamentName);

                    myCommand.ExecuteNonQuery();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }


        [HttpPut]

        public JsonResult Put(Departament dep)
        {
            string storedProcedure = "PR_U_DEPARTMENT";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(storedProcedure, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;


                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartamentName);
                    myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartamentId);

                    myCommand.ExecuteNonQuery();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }


        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            string storedProcedure = "PR_D_DEPARTMENT";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(storedProcedure, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;


                   
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);

                    myCommand.ExecuteNonQuery();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
