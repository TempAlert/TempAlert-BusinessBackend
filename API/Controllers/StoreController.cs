﻿using API.Dtos;
using API.Helpers.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StoreController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StoreController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<Store>>> GetAll([FromQuery] Params productParams)
        {
            var result = await _unitOfWork.Stores.GetAllWithPaginationAsync(productParams.PageIndex, productParams.PageSize, productParams.Search);

            var stores = _mapper.Map<List<Store>>(result.registers);

            return new Pager<Store>(stores, result.totallyRegister, productParams.PageIndex, productParams.PageSize, productParams.Search);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Store>> GetById(Guid id)
        {
            var store = await _unitOfWork.Stores.GetByIdAsync(id);

            if (store == null)
                return NotFound(new ApiResponse(404));

            return Ok(store);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Store>> Post(AddStoreDto storeDto)
        {
            var storeExistWithName = _unitOfWork.Stores.Find(u => u.Name.ToLower() == storeDto.Name.ToLower()).FirstOrDefault();

            if (storeExistWithName != null)
                return BadRequest(new ApiResponse(400, $"Store with name {storeDto.Name} already exists"));

            var store = _mapper.Map<Store>(storeDto);

            store.CreatedDate = DateTime.Now;

            _unitOfWork.Stores.Add(store);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(Post), store);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Store>> Put(Guid id, [FromBody] UpdateStoreDto storeDto)
        {
            var store = await _unitOfWork.Stores.GetByIdAsync(id);

            if (store == null)
                return NotFound(new ApiResponse(404));

            var storesExistWithName = _unitOfWork.Stores.Find(u => u.Name.ToLower() == storeDto.Name.ToLower()).FirstOrDefault();

            if (storesExistWithName != null && storesExistWithName.Id != id)
                return BadRequest(new ApiResponse(400, $"Store with name {storeDto.Name} already exists"));

            var updateStore = _mapper.Map<Store>(storeDto);

            updateStore.Id = id;

            _unitOfWork.Stores.Detach(store);
            _unitOfWork.Stores.Update(updateStore);
            await _unitOfWork.SaveAsync();

            return updateStore;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var store = await _unitOfWork.Stores.GetByIdAsync(id);

            if (store == null)
                return NotFound(new ApiResponse(404));

            _unitOfWork.Stores.Remove(store);
            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                Id = id,
            });
        }

    }
}
