using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SurfergraphyApi.Models;

namespace SurfergraphyApi.Controllers
{
    public class MembersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Members
        public IQueryable<Member> GetMembers()
        {
            return db.Members;
        }

        // GET: api/Members/5
        [ResponseType(typeof(Member))]
        public IHttpActionResult GetMember(string id)
        {
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        // POST: api/Login
        [Route("api/Members/Login")]
        public IHttpActionResult LoginMember(LoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!MemberExists(model.Id))
            {
                return NotFound();
            }

            var member = db.Members.Find(model.Id);
            member.LoginToken = model.LoginToken;
            member.LoginDate = DateTime.Now;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(member);
        }

        // POST: api/Members
        [ResponseType(typeof(Member))]
        public IHttpActionResult PostMember(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var member = new Member() { Id = model.Id, Email = model.Email, JoinType = model.JoinType, Name = model.Name, ImageUrl = model.ImageUrl, JoinDate = DateTime.Now, DeletedDate = DateTime.Now, LoginDate = DateTime.Now, Deleted = false, Grade = 0, Wave = 0, LoginToken = "", PushToken = "" };

            db.Members.Add(member);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MemberExists(member.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = member.Id }, member);
        }

        // DELETE: api/Members/5
        [ResponseType(typeof(Member))]
        public IHttpActionResult DeleteMember(string id)
        {
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }

            db.Members.Remove(member);
            db.SaveChanges();

            return Ok(member);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(string id)
        {
            return db.Members.Count(e => e.Id == id) > 0;
        }
    }
}