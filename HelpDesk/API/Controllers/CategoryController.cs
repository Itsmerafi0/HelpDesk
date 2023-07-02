using API.Contracs;
using API.Models;
using API.ViewModel.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CategoryController : BaseController<Category, CategoryVM>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper<Category, CategoryVM> _categoryMapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper<Category, CategoryVM> categoryMapper) : base(categoryRepository, categoryMapper)
    {
        _categoryRepository = categoryRepository;
        _categoryMapper = categoryMapper;
    }
}
