using System;

namespace eazy.rest.extension.EfCore.Entity
{
    public interface IGenericPro<T>
    {
        T Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}