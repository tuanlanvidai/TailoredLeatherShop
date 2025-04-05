using ASPdotNET_CUOIMON.Models.DAO;
using ASPdotNET_CUOIMON.Models.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.Repositories
{
	public class CategoryRepository
	{
        private CategoryDAO dao = new CategoryDAO();

        public List<CategoryView> GetAll()
        {
            var list = dao.GetAll();
            return list.Select(c => new CategoryView
            {
                CategoryId = c.category_id,
                Name = c.name
            }).ToList();
        }


        public CategoryView GetById(int id)
        {
            var cat = dao.GetById(id);
            if (cat == null) return null;

            return new CategoryView
            {
                CategoryId = cat.category_id,
                Name = cat.name
            };
        }

    }
}