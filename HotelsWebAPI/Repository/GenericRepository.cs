using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelsApplication.Data;
using HotelsApplication.Exceptions;
using HotelsApplication.Interfaces;
using HotelsApplication.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace HotelsApplication.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelsDBContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(HotelsDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<T> GetAsync(int id)
        {
            if (id < 0)
                return null;

            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await _context.Set<T>().ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParams)
        {
            var totalCount = await _context.Set<T>().CountAsync();
            int startIndex = (queryParams.PageNumber - 1) * queryParams.PageSize;
            var items = await _context.Set<T>().Skip(startIndex).Take(queryParams.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
            return new PagedResult<TResult> 
            { 
                Items = items,
                TotalCount = totalCount,
                RecordNumber = queryParams.PageSize,
                PageNumber = queryParams.PageNumber,
            };
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update<T>(entity);
            await _context.SaveChangesAsync();
        }
        
    }
}
