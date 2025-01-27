﻿using Laraue.Core.DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Laraue.Core.DataAccess.EFCore.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Create pagination by <see cref="IPaginatedRequest"/>. Extension for EF core package.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task<IPaginatedResult<TEntity>> PaginateAsync<TEntity>(
            this IQueryable<TEntity> query,
            IPaginatedRequest request,
            CancellationToken ct = default)
            where TEntity : class
        {
            var total = await query.LongCountAsync(ct);
            var skip = (request.Page - 1) * request.PerPage;

            var data = await query.Skip(skip)
                .Take(request.PerPage)
                .AsNoTracking()
                .ToListAsync(ct);

            return new PaginatedResult<TEntity>(request.Page, request.PerPage, total, data);
        }

        /// <summary>
        /// Create pagination by <see cref="IPaginatedRequest"/>. Extension for EF core package.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IPaginatedResult<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query, IPaginatedRequest request)
            where TEntity : class
        {
            var total = query.LongCount();
            var skip = (request.Page - 1) * request.PerPage;

            var data = query.Skip(skip)
                .Take(request.PerPage)
                .AsNoTracking()
                .ToList();

            return new PaginatedResult<TEntity>(request.Page, request.PerPage, total, data);
        }
    }
}