using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Persistance
{
    public interface IApplicationDbContext
    {

        int SaveChanges();
    }
}
