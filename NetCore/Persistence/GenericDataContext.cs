using System;
using CrudDatastore;

namespace Persistence
{
    internal class GenericDataContext : DataContextBase
    {
        public GenericDataContext(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}
