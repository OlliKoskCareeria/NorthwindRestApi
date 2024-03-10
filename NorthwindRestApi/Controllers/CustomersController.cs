using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

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


    }
}
