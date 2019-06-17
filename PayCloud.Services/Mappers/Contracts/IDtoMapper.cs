using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services.Mappers
{
    public interface IDtoMapper<TEntity, TDto>
    {
        TDto MapFrom(TEntity entity);
    }
}
