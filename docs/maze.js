
var maze;
var size;
var lineLength;
var cursorStartPositionX;
var cursorStartPositionY;;
var cursorFillColor = "rgba(40, 167, 69, .80)";
var lineFillColor = "rgba(255, 255, 255, 1)";
var visited = [];

// Canvas variables
// We have two canvases on top of each other to make it easier to clear the path when moving 
var baseCanvas = null;
var baseCanvasContext = null;
var pathCanvas = null;
var pathCanvasContext = null;

var cursorCurrentPositionX = cursorStartPositionX;
var cursorCurrentPositionY = cursorStartPositionY;
var cursorFinishPositionX;
var cursorFinishPositionY;

var baseApiUrl = "https://maze-generator.azurewebsites.net/api";

$(document).ready(function () {
    $('#mazeForm').on('submit', function (event) {
        event.preventDefault();
        generateMaze();
    });

    window.onkeydown = processKey;

    baseCanvas = document.getElementById('baseCanvas');
    if (baseCanvas.getContext) {
        baseCanvasContext = baseCanvas.getContext('2d');
    }

    pathCanvas = document.getElementById('pathCanvas');
    if (pathCanvas.getContext) {
        pathCanvasContext = pathCanvas.getContext('2d');
    }

    $('#mazeSize').keyup(function () {
        var s = this;
        s.validity.valid ? $('#generateButton').prop('disabled', false) : $('#generateButton').prop('disabled', true);
    });
});

function generateMaze() {
    $('#generateButton').prop('disabled', true);
    size = $('#mazeSize').val();
    if(size <= 15) {
        lineLength = 35;
    } else if(size <= 25) {
        lineLength = 30;
    }  else if (size <= 35) {
        lineLength = 25;
    } else if(size <= 45) {
        lineLength = 20;
    } else if(size <= 55) {
        lineLength = 16;
    } else if(size <= 65) {
        lineLength = 14;
    } else {
        lineLength = 13;
    }

    cursorStartPositionX = lineLength + (lineLength / 2);
    cursorStartPositionY = lineLength + (lineLength / 2);

    baseCanvas.width = pathCanvas.width = size * lineLength + lineLength * 2;
    baseCanvas.height = pathCanvas.height = size * lineLength + lineLength * 2;
    cursorFinishPositionX = cursorStartPositionX + ((size - 1) * lineLength);
    cursorFinishPositionY = cursorStartPositionY + ((size - 1) * lineLength);
    loadMaze();
}

function getSolution() {
    $('#spinner').show();
    var data = JSON.stringify(maze);
    $.ajax({
        type: "POST",
        contentType: 'application/json',
        dataType: 'json',
        url: baseApiUrl + "/maze/solution",
        data: data,
        success: function (result) {
            drawSolution(result.reverse());
            $('#solveButton').hide();
            $('#spinner').hide();
        },
    });
}

function processKey(e) {
    if ([32, 37, 38, 39, 40].indexOf(e.keyCode) > -1) {
        e.preventDefault();
    }

    dx = 0;
    dy = 0;

    // The up arrow was pressed, so move up.
    if (e.keyCode == 38) {
        dy = -lineLength;
    }

    // The down arrow was pressed, so move down.
    if (e.keyCode == 40) {
        dy = lineLength;
    }

    // The left arrow was pressed, so move left.
    if (e.keyCode == 37) {
        dx = -lineLength;
    }

    // The right arrow was pressed, so move right.
    if (e.keyCode == 39) {
        dx = lineLength;
    }

    if (dx != 0 || dy != 0) {


        if (!checkForCollision(dx, dy)) {

            // visited.push({'x': 30, 'y': 30 });
            var nextPositionX = cursorCurrentPositionX + dx;
            var nextPositionY = cursorCurrentPositionY + dy;

            var matchedItems = visited.filter(function (line) {
                return line.x0 === nextPositionX && line.y0 === nextPositionY;
            });

            if (matchedItems && matchedItems.length > 0) {
                visited.splice(visited.length - 1, 1);
            }
            else {
                visited.push({ 'x0': cursorCurrentPositionX, 'y0': cursorCurrentPositionY, 'x1': nextPositionX, 'y1': nextPositionY });
            }

            redrawPath();
            cursorCurrentPositionX += dx;
            cursorCurrentPositionY += dy;

            if (cursorCurrentPositionX == cursorFinishPositionX && cursorCurrentPositionY == cursorFinishPositionY) {
                $('#myModal').modal("show");
            }
        }
    }
}

function redrawPath() {

    clearCanvas(pathCanvas);
    pathCanvasContext.strokeStyle = cursorFillColor;
    pathCanvasContext.lineWidth = 6;
    pathCanvasContext.lineJoin = "miter";
    for (var i = 0; i < visited.length; i++) {
        var point = visited[i];
        pathCanvasContext.beginPath();
        pathCanvasContext.moveTo(point.x0, point.y0);
        pathCanvasContext.lineTo(point.x1, point.y1);
        pathCanvasContext.stroke();
    }
}

function clearCanvas(canvas) {
    var ctx = canvas.getContext('2d');
    ctx.save();
    ctx.globalCompositeOperation = 'copy';
    ctx.strokeStyle = 'transparent';
    ctx.beginPath();
    ctx.lineTo(0, 0);
    ctx.stroke();
    ctx.restore();
}

function getIndexFromCurrentPosition(cursorCurrentPositionX, cursorCurrentPositionY) {
    // convert current x y coordinates to an array index
    var realPositionX = cursorCurrentPositionX - (lineLength / 2) - lineLength;
    var realPositionY = cursorCurrentPositionY - (lineLength / 2) - lineLength;
    var row = realPositionY / lineLength;
    var col = realPositionX / lineLength;
    return row * size + col;
}

// Checks if moving from current cursor position going to cause a collision with a wall
function checkForCollision(dx, dy) {

    var realPositionX = cursorCurrentPositionX - (lineLength / 2) - lineLength;
    var realPositionY = cursorCurrentPositionY - (lineLength / 2) - lineLength;
    var row = realPositionY / lineLength;
    var col = realPositionX / lineLength;
    var currentCellIndex = getIndexFromCurrentPosition(cursorCurrentPositionX, cursorCurrentPositionY);
    var cell = maze.rooms[currentCellIndex];

    if (dy < 0) {
        if (row == 0 && col == 0) {
            return true; // even though north door is open in starting cell we treat it as if it is closed
        }
        // need to check north wall
        return cell.north.isClosed;
    } else if (dy > 0) {
        // need to check south wall
        return cell.south.isClosed;
    } else if (dx < 0) {
        // need to check west wall
        return cell.west.isClosed;
    } else if (dx > 0) {
        // need to check east wall
        return cell.east.isClosed;
    } else {
        return false; // should never get here?
    }
}

function loadMaze() {
    $('#spinner').show();
    $.ajax({
        url: baseApiUrl + "/maze/" + size,
        contentType: "application/json",
        dataType: 'json',
        success: function (result) {
            maze = result;
            drawMaze();
            $('#spinner').hide();
            $('#generateButton').prop('disabled', false);
            $('#solveButton').show();
        }
    });
}

function drawMaze() {
    visited = [];
    if (pathCanvasContext) {
        pathCanvasContext.clearRect(0, 0, pathCanvas.width, pathCanvas.height);
        cursorCurrentPositionX = cursorStartPositionX;
        cursorCurrentPositionY = cursorStartPositionY;
    }

    if (baseCanvasContext) {

        baseCanvasContext.clearRect(0, 0, baseCanvas.width, baseCanvas.height);
        baseCanvasContext.fillStyle = lineFillColor;
        baseCanvasContext.beginPath();
        var x = lineLength, y = lineLength;

        var size = maze.size;
        // draw the north border wall
        for (var i = 0; i < size; i++) {
            baseCanvasContext.moveTo(x, y);
            x += lineLength;
            if (maze.rooms[i].north.isClosed) {
                baseCanvasContext.lineTo(x, y);
                baseCanvasContext.stroke();
            }
        }

        var row = 0;
        // draw the interior walls
        for (var i = 0; i < maze.numOfRooms; i += size) // rows
        {
            x = lineLength;
            y = row * lineLength + lineLength;

            for (var j = i; j < i + size; ++j) // Prints the west walls
            {
                baseCanvasContext.moveTo(x, y);

                if (maze.rooms[j].west.isClosed) {
                    baseCanvasContext.lineTo(x, y + lineLength);
                    baseCanvasContext.stroke();
                }

                x += lineLength;
            }

            baseCanvasContext.moveTo(x, y);
            baseCanvasContext.lineTo(x, y + lineLength);
            baseCanvasContext.stroke();

            x = lineLength;
            y = row * lineLength + lineLength + lineLength;
            for (var j = i; j < i + size; ++j) // Prints the south wall
            {
                baseCanvasContext.moveTo(x, y);

                if (maze.rooms[j].south.isClosed) {
                    baseCanvasContext.lineTo(x + lineLength, y);
                    baseCanvasContext.stroke();

                }

                x += lineLength;
            }
            //Console.WriteLine("+"); // Right most border

            row++;
        }

        // draw starting position
        // start always begins in the first cell
        pathCanvasContext.moveTo(cursorStartPositionX, cursorStartPositionY);
        pathCanvasContext.beginPath();
        pathCanvasContext.fillStyle = cursorFillColor;
        pathCanvasContext.arc(cursorStartPositionX, cursorStartPositionY, 6, 0, Math.PI * 2, true);
        pathCanvasContext.fill();

    }
}

function drawSolution(solution) {
    pathCanvasContext.moveTo(cursorStartPositionX, cursorStartPositionY);
    pathCanvasContext.beginPath();
    pathCanvasContext.strokeStyle = "red";
    pathCanvasContext.lineWidth = 4;

    for (var i = 0; i < solution.length; i++) {
        // convert index to screen position
        var index = solution[i];

        var row = parseInt(index / size);
        var col = index % size;
        var x = col * lineLength + (lineLength + lineLength / 2);
        var y = row * lineLength + (lineLength + lineLength / 2);
        pathCanvasContext.lineTo(x, y);
        pathCanvasContext.stroke();

        pathCanvasContext.moveTo(x, y);
    }
}