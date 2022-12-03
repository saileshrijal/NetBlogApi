using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Repositories.Interfaces
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
    }
}
