using System;

namespace Bio.Domain.Interfaces
{
    public interface ITokenService
    {
        string CreateToken<TEntity>(TEntity entity) where TEntity : class;
    }
}
