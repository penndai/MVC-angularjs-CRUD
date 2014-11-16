using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Test.Models;

namespace Test.Controllers
{
    public class CustomerController : ApiController
    {
        private CustomerContext db = new CustomerContext();

        // GET api/Customer

        public IEnumerable<Customer> GetCustomers(string q = null, string sort = null, bool desc = false, int? limit = null, int offset = 0)
        {

            var list = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<Customer>();

            IQueryable<Customer> items = string.IsNullOrEmpty(sort) ? list.OrderBy(o => o.FirstName)
                : list.OrderBy(String.Format("it.{0} {1}", sort, desc ? "DESC" : "ASC"));

            if (!string.IsNullOrEmpty(q) && q != "undefined") 
                items = items.Where(t => t.FirstName.Contains(q));

            if (offset > 0) items = items.Skip(offset);
            if (limit.HasValue) items = items.Take(limit.Value);
            return items;

            //var list = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<Customer>();
            //IEnumerable<Customer> customers = 
            //    string.IsNullOrEmpty(sort) ? list.OrderBy(x => x.ID) : list.OrderBy(string.Format("it.{0} {1}", sort, desc ? "DESC" : "ASC"));

            //return customers;
        }
        // Get all customers only
        //public IEnumerable<Customer> GetCustomers()
        //{
        //    return db.Customers.AsEnumerable();
        //}

        // GET api/Customer/5
        public Customer GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return customer;
        }

        // PUT api/Customer/5
        public HttpResponseMessage PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != customer.ID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Customer
        public HttpResponseMessage PostCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, customer);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = customer.ID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Customer/5
        public HttpResponseMessage DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Customers.Remove(customer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, customer);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}