using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using System.Linq.Expressions;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //northwindContext db = new northwindContext();
        private northwindContext db;

        public CustomersController(northwindContext dbparametri)
        {
            db = dbparametri;
        }

        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            var asiakkaat = db.Customers.ToList();

            return Ok(asiakkaat);
        }

        [HttpGet("{id}")]
        public ActionResult GetOneCustomer(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);

                if (asiakas != null)
                {
                    return Ok(asiakas);
                }
                else
                {
                    return BadRequest("Asiakasta ei löydy");
                }
            }
            catch(Exception ex)
            {
                return BadRequest("something went wrong" + ex.InnerException);
            }
        }

        [HttpPost]
        public ActionResult AddNew([FromBody] Customer cust)
        {
            try
            {
                db.Customers.Add(cust);
                db.SaveChanges();
                return Ok($"Lisättiin uusi asiakas {cust.CompanyName} from {cust.City}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää: " + e.InnerException);
            }
        }

        // Poisto toiminto
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {

                var asiakas = db.Customers.Find(id);

                if (asiakas != null)
                {  // asiakas löytyy
                    db.Customers.Remove(asiakas);
                    db.SaveChanges();
                    return Ok(asiakas.CompanyName + " poistettiin.");
                }

                return NotFound("Asiakasta id:llä " + id + " ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        [HttpPut("{id}")]
        public ActionResult EditCustomer(string id, [FromBody] Customer customer)
        {
            var asiakas = db.Customers.Find(id);
            if (asiakas != null)
            {

                asiakas.CompanyName = customer.CompanyName;
                asiakas.ContactName = customer.ContactName;
                asiakas.ContactTitle = customer.ContactTitle;
                asiakas.Address = customer.Address;
                asiakas.City = customer.City;
                asiakas.Region = customer.Region;
                asiakas.PostalCode = customer.PostalCode;
                asiakas.Country = customer.Country;
                asiakas.Phone = customer.Phone;
                asiakas.Fax = customer.Fax;

                db.SaveChanges();
                return Ok("Muokattu asiakasta " + asiakas.CompanyName);
            }

            return NotFound("Asikasta ei löytynyt id:llä " + id);
        }

        [HttpGet("companyname/{cname}")]
        public ActionResult GetByName(string cname)
        {
            try
            {
                
                var cust = db.Customers.Where(c => c.CompanyName.Contains(cname));
                
                //var cust = from c in db.Customers where c.CompanyName.Contains(cname) select c; <-- sama mutta traditional


                // var cust = db.Customers.Where(c => c.CompanyName == cname); <--- perfect match

                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
