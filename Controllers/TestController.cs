using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirdIsAWord.Data.Entities;
using BirdIsAWord.Data;

namespace BirdIsAWord.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController:ControllerBase
    {
        private readonly IRepositoryAsync<Sensor> _repository;

        public TestController(IRepositoryAsync<Sensor> repository)
        {
            _repository = repository;
        }
        [HttpGet("proxytable")]
        public async Task<IActionResult> GetProximityTable()
        {
            var results = await _repository.ListAllAsync();
            return Ok(results);
        }
        
    }
}
