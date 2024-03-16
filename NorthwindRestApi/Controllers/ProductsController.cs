using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        
        //northwindContext db = new northwindContext();
        private northwindContext db;

        public ProductsController(northwindContext dbparametri)
        {
            db = dbparametri;
        }

        [HttpGet]
        public ActionResult GetAllProducts()
        {
            var tuotteet = db.Products.ToList();

            return Ok(tuotteet);
        }

        [HttpGet("{id}")]
        public ActionResult GetOneProduct(int id)
        {
            try
            {
                var tuote = db.Products.Find(id);

                if (tuote != null)
                {
                    return Ok(tuote);
                }
                else
                {
                    return BadRequest("Tuotetta ei löydy");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("something went wrong" + ex.InnerException);
            }
        }

        [HttpPost]
        public ActionResult AddNew([FromBody] Product prod)
        {
            try
            {
                db.Products.Add(prod);
                db.SaveChanges();
                return Ok($"Lisättiin uusi tuote {prod.ProductName}");
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

                var tuote = db.Products.Find(id);

                if (tuote != null)
                {  // tuote löytyy
                    db.Products.Remove(tuote);
                    db.SaveChanges();
                    return Ok(tuote.ProductName + " poistettiin.");
                }

                return NotFound("Tuotetta" + " ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        [HttpPut("{id}")]
        public ActionResult EditProduct(int id, [FromBody] Product product)
        {
            var tuote = db.Products.Find(id);
            if (tuote != null)
            {

                tuote.ProductName = product.ProductName;
                tuote.SupplierId = product.SupplierId;
                tuote.CategoryId = product.CategoryId;
                tuote.QuantityPerUnit = product.QuantityPerUnit;
                tuote.UnitPrice = product.UnitPrice;
                tuote.UnitsInStock = product.UnitsInStock;
                tuote.UnitsOnOrder = product.UnitsOnOrder;
                tuote.ReorderLevel = product.ReorderLevel;
                tuote.Discontinued = product.Discontinued;
                tuote.ImageLink = product.ImageLink;

                db.SaveChanges();
                return Ok("Muokattu tuotetta " + tuote.ProductName);
            }

            return NotFound("Tuotetta ei löytynyt id:llä " + id);
        }

        [HttpGet("productname/{pname}")]
        public ActionResult GetByName(string pname)
        {
            try
            {

                var pro = db.Products.Where(c => c.ProductName.Contains(pname));

                

                return Ok(pro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

