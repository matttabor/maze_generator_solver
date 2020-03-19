using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Maze.Models;
using AutoMapper;
using MazeApi.Models;
using Maze.Solvers;

namespace maze_api.Controllers
{
    [ApiController]
    public class MazeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<MazeController> _logger;

        public MazeController(ILogger<MazeController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("api/maze/{size}")]
        public IActionResult GetMaze(int size)
        {
            try
            {
                var maze = new MazeModel(size);
                maze.GenerateMaze();
                var model = _mapper.Map<MazeViewModel>(maze);
                return Ok(model);
            } 
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("api/maze/solution")]
        public IActionResult GetSolution(MazeViewModel model)
        {

            try 
            {
                var solver = MazeSolverFactory.CreateMazeSolver(MazeSolverType.BFS);
                var maze = _mapper.Map<MazeModel>(model);
                var solution = solver.SolveMaze(maze);
                var hash = new HashSet<int>();
                int currentCellIndex = maze.MaxIndex;
                int start = 0;
                while (currentCellIndex != 0)
                {
                    hash.Add(currentCellIndex);
                    currentCellIndex = solution[currentCellIndex];
                }
                hash.Add(start);
                return Ok(hash);
            } 
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
