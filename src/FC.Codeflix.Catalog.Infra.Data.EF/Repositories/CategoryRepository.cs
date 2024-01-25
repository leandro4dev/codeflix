using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CodeflixCatalogDbContext _context;

    public CategoryRepository(CodeflixCatalogDbContext context)
    {
        _context = context;
    }

    public async Task Insert(Category aggregate, CancellationToken cancellationToken)
    {
        await _context.AddAsync(aggregate, cancellationToken);
    }

    public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id, 
                cancellationToken
            );

        NotFoundException.ThrowIfNull(
            category, 
            $"Category '{id}' not found"
        );

        return category!;
    }

    public Task Update(Category aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Categories.Update(aggregate));
    }

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Categories.Remove(aggregate));
    }


    public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var total = await _context.Categories.CountAsync();

        var items = await _context.Categories.AsNoTracking()
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync();

        return new SearchOutput<Category>(
            input.Page,
            input.PerPage,
            total,
            items
        );
    }

    
}
