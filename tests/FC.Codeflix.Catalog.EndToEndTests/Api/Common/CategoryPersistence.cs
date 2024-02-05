using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Common;

public class CategoryPersistence
{
    private readonly CodeflixCatalogDbContext _context;

    public CategoryPersistence(CodeflixCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<DomainEntity.Category?> GetById(Guid id)
    {
        return await _context.Categories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task InsertList(List<DomainEntity.Category> categories)
    {
        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();
    }

        
    public async Task Insert(DomainEntity.Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    } 
}
