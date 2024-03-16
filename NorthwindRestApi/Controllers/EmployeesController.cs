using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private northwindContext db;

        public EmployeesController(northwindContext dbparametri)
        {
            db = dbparametri;
        }

        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            var tyontekijat = db.Employees.ToList();

            return Ok(tyontekijat);
        }

        [HttpGet("{id}")]
        public ActionResult GetOneEmployee(int id)
        {
            try
            {
                var tyontekija= db.Employees.Find(id);

                if (tyontekija != null)
                {
                    return Ok(tyontekija);
                }
                else
                {
                    return BadRequest("Työntekijää ei löydy");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("something went wrong" + ex.InnerException);
            }
        }

        [HttpPost]
        public ActionResult AddNew([FromBody] Employee emp)
        {
            try
            {
                db.Employees.Add(emp);
                db.SaveChanges();
                return Ok($"Lisättiin uusi työntekijä {emp.FirstName}+{emp.LastName}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        // Poisto toiminto
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {

                var tyontekija = db.Employees.Find(id);

                if (tyontekija != null)
                {  // työntekijä löytyy
                    db.Employees.Remove(tyontekija);
                    db.SaveChanges();
                    return Ok(tyontekija.EmployeeId + " poistettiin.");
                }

                return NotFound("Työntekijää" + " ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        [HttpPut("{id}")]
        public ActionResult EditEmployee(int id, [FromBody] Employee emp)
        {
            var tyontekija = db.Employees.Find(id);
            if (tyontekija != null)
            {

                tyontekija.FirstName = emp.FirstName;
                tyontekija.LastName = emp.LastName;
                tyontekija.Title = emp.Title;
                tyontekija.TitleOfCourtesy = emp.TitleOfCourtesy;
                tyontekija.BirthDate = emp.BirthDate;
                tyontekija.HireDate = emp.HireDate;
                tyontekija.Address = emp.Address;
                tyontekija.City = emp.City;
                tyontekija.Region = emp.Region;
                tyontekija.PostalCode = emp.PostalCode;
                tyontekija.Country = emp.Country;
                tyontekija.HomePhone = emp.HomePhone;
                tyontekija.Extension = emp.Extension;
                tyontekija.Photo = emp.Photo;
                tyontekija.Notes = emp.Notes;
                tyontekija.ReportsTo = emp.ReportsTo;
                tyontekija.PhotoPath = emp.PhotoPath;
                tyontekija.HireDate = emp.HireDate;

                db.SaveChanges();
                return Ok("Muokattu työntekijää " + tyontekija.EmployeeId + tyontekija.FirstName + tyontekija.LastName);
            }

            return NotFound("Työntekijää ei löytynyt id:llä " + id);
        }

        [HttpGet("employeename/{ename}")]
        public ActionResult GetByName(string ename)
        {
            try
            {

                

                var emp = db.Employees.Where(c => c.LastName.Contains(ename));

                


               
                
                return Ok(emp);
                
                //Palauttaa tyhjän olion mikäli tietoa ei löydy emp ei null.
                //Jatkokehitykseen mikäli vastaavuutta ei löydy niin ilmoittaa käyttäjälle

                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}

