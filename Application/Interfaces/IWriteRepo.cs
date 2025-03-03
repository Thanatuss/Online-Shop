using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.Utilities;

namespace Infrastructore.Interfaces
{
    public interface IWriteRepo
    {
        Task<OperationHandler> Add<T>(T entity);
    }
}
