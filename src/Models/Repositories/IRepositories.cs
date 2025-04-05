using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPdotNET_CUOIMON.Models.Repositories
{
    public interface IRepositories<T>
    {
        List<T> GetAll();
        T GetById(int id);
        bool Create(T model);
        bool Update(T model);
        bool Delete(int id);
    }
}
