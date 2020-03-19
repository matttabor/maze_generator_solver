using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Maze.Models;

namespace maze_api.Controllers
{
    [ApiController]
    public class MazeController : ControllerBase
    {

        private readonly ILogger<MazeController> _logger;

        public MazeController(ILogger<MazeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("api/maze/{size}")]
        public IActionResult GetMaze(int size)
        {
            var maze = new MazeModel(size);
            maze.GenerateMaze();
            return Ok(maze);
        }

    }
}
