﻿using KeyValueService.Data;
using KeyValueService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KeyValueService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyValueController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public KeyValueController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        /// <summary>
        /// Adds a new key-value pair to the store.
        /// </summary>
        /// <param name="keyValue">The key-value pair to add.</param>
        /// <returns>
        ///   <para>Returns a Conflict response if the key already exists.</para>
        ///   <para>Returns an Ok response with the added key-value pair and a success message if added successfully.</para>
        /// </returns>
        [HttpPost("keys")]
        public async Task<IActionResult> AddKeyValue(KeyValue keyValue)
        {
            if (_dbContext.KeyValues.Any(k => k.Key == keyValue.Key))
            {
                return Conflict(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "The key is already exists.",
                });
            }

            await _dbContext.KeyValues.AddAsync(keyValue);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<KeyValue>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Key-Value added successfully.",
                Data = keyValue
            });
        }
    }
}
