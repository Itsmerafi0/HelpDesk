﻿using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using API.Contracs;
using API.Models;
using API.ViewModel.Account;
using API.ViewModel.Response;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller")]


    public class BaseController<TModel, TViewModel> : ControllerBase
        {
            private readonly IGeneralRepository<TModel> _repository;
            private readonly IMapper<TModel, TViewModel> _mapper;

            public BaseController(IGeneralRepository<TModel> repository, IMapper<TModel, TViewModel> mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

        [HttpGet]
        [Authorize(Roles ="Admin")]
            public IActionResult GetAll()
            {
                var models = _repository.GetAll();
                if (!models.Any())
                {
                    return NotFound(new ResponseVM<TViewModel>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data not found"
                    });
                }

                var resultConverted = models.Select(_mapper.Map).ToList();

                return Ok(new ResponseVM<List<TViewModel>>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Success",
                    Data = resultConverted
                });
            }

            [HttpGet("{guid}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetByGuid(Guid guid)
            {
                var model = _repository.GetByGuid(guid);
                if (model is null)
                {
                    return NotFound(new ResponseVM<TViewModel>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data not found"
                    });
                }

                var resultConverted = _mapper.Map(model);

                return Ok(new ResponseVM<TViewModel>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Success",
                    Data = resultConverted
                });
            }

            [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult Create(TViewModel viewModel)
            {
                var model = _mapper.Map(viewModel);
                var result = _repository.Create(model);
                if (result is null)
                {
                    return NotFound(new ResponseVM<TViewModel>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Create Failed"
                    });
                }

                return Ok(new ResponseVM<TModel>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Create Success",
                    Data = result
                });
            }

            [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(TViewModel viewModel)
            {
                var model = _mapper.Map(viewModel);
                var isUpdated = _repository.Update(model);
                if (!isUpdated)
                {
                    return NotFound(new ResponseVM<TViewModel>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Update Failed"
                    });
                }

                return Ok(new ResponseVM<TModel>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Update Success"
                });
            }

            [HttpDelete("{guid}")]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(Guid guid)
            {
                var isDeleted = _repository.Delete(guid);
                if (!isDeleted)
                {
                    return NotFound(new ResponseVM<TViewModel>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Delete Failed"
                    });
                }

                return Ok(new ResponseVM<TModel>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Delete Success"
                });
            }
        }
    
}