using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ECommerce.Domain.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        /// <summary>
        /// Поиск сущности по Id
        /// </summary>
        /// <param name="id">Id сущности</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includesProperties">Делегаты для подключения навигационных свойств</param>
        /// <returns></returns>
        Task<T?> GetByIdAsync(Guid id,
         CancellationToken cancellationToken = default,
         params Expression<Func<T, object>>[]? includesProperties);

        /// <summary>
        /// Получение всего списка сущностей
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IReadOnlyList<T>> ListAllAsync(
            CancellationToken cancellationToken = default);

        /// </summary>
        /// Добавление новой сущности
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync(T entity,
         CancellationToken cancellationToken = default);


        // <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="entity">Сущность, которую следует удалить</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(T entity,
         CancellationToken cancellationToken = default);

    }
}
