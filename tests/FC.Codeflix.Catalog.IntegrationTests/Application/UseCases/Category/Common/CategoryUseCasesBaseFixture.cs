﻿using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.IntegrationTests.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;

public class CategoryUseCasesBaseFixture : BaseFixture
{
    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
        {
            categoryName = Faker.Commerce.Categories(1)[0];
        }

        if (categoryName.Length > 255)
        {
            categoryName = categoryName[..255]; //primeiros 255 characters
        }

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10000)
        {
            categoryDescription = categoryDescription[..10_000];
        }

        return categoryDescription;
    }

    public bool GetRandomBoolean()
    {
        return new Random().NextDouble() < 0.5;
    }

    public DomainEntity.Category GetExampleCategory()
    {
        return new DomainEntity.Category(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );
    }


    public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
    {
        return Enumerable.Range(1, length)
            .Select(_ => GetExampleCategory()).ToList();
    }
}
