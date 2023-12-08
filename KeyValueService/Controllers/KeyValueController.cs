using KeyValueService.Data;
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
        /// Retrieves a key-value pair by the specified key.
        /// </summary>
        /// <param name="key">The key to search for in the key-value store.</param>
        /// <returns>
        ///   <para>Returns a NotFound response if the key does not exist.</para>
        ///   <para>Returns an Ok response with the retrieved key-value pair and a success message if found.</para>
        /// </returns>
        [HttpGet("keys/{key}")]
        public async Task<IActionResult> GetByKey(string key)
        {
            var keyValue = await _dbContext.KeyValues.FindAsync(key);

            if (keyValue == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Key does not exist.",
                });
            }

            return Ok(new ApiResponse<KeyValue>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Key-Value retrieved successfully.",
                Data = keyValue
            });
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


        /// <summary>
        /// Updates the value of an existing key in the key-value store.
        /// </summary>
        /// <param name="key">The key to update.</param>
        /// <param name="value">The new value for the key.</param>
        /// <returns>
        ///   <para>Returns a NotFound response if the key does not exist.</para>
        ///   <para>Returns an Ok response with the updated key-value pair and a success message if updated successfully.</para>
        /// </returns>
        [HttpPatch("keys/{key}/{value}")]
        public async Task<IActionResult> UpdateValue(string key, string value)
        {
            var keyValues = await _dbContext.KeyValues.FindAsync(key);

            if (keyValues == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Key does not exist.",
                });
            }

            keyValues.Value = value;
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<KeyValue>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Value updated successfully.",
                Data = keyValues
            });
        }
    }
}
