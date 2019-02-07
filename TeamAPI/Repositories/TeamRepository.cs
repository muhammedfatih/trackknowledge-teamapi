using DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamAPI.Models;

namespace TeamAPI.Repositories
{
	public class TeamRepository : IRepository<Team>
	{
		public void Delete(Team entity)
		{
			using (TeamDBContext db = new TeamDBContext()) {
				var record = db.Teams.Find(entity.Id);
				db.Teams.Remove(record);
				db.SaveChanges();
			}
		}

		public Team Get(object id)
		{
			using (TeamDBContext db = new TeamDBContext()) {
				return db.Teams.Find(id);
			}
		}

		public Team Insert(Team entity)
		{
			using (TeamDBContext db = new TeamDBContext()) {
				db.Teams.Add(entity);
				db.SaveChanges();
				return entity;
			}
		}

		public List<Team> List()
		{
			using (TeamDBContext db = new TeamDBContext()) {
				return db.Teams.ToList();
			}
		}

		public void Update(Team entity)
		{
			using (TeamDBContext db = new TeamDBContext()) {
				entity.UpdatedAt = DateTime.Now;
				db.SaveChanges();
			}
		}
	}
}