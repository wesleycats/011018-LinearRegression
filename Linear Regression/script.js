const canvas = document.createElement("canvas");
canvas.width = 600;
canvas.height = 600;
const context = canvas.getContext('2d');
document.body.appendChild(canvas);

var allPoints = [];
var pointSize = 5;

function xSum(array) {
  let xSum = 0;
  for (let i = 0; i < array.length; i++)
  {
    xSum += array[i].x;
  }
  return xSum;
}

function ySum(array) {
  let ySum = 0;
  for (let i = 0; i < array.length; i++)
  {
    ySum += array[i].y;
  }
  return ySum;
}

function xySum(array) {
  let xySum = 0;
  for (let i = 0; i < array.length; i++)
  {
    let x = array[i].x;
    let y = array[i].y;
    xySum += (x * y);
  }
  return xySum;
}

function xSquaredSum(array) {
  let xSquaredSum = 0;
  for (let i = 0; i < array.length; i++)
  {
    xSquaredSum += (array[i].x * array[i].x);
  }
  return xSquaredSum;
}

function ySquaredSum(array) {
  let ySquaredSum = 0;
  for (let i = 0; i < array.length; i++)
  {
    ySquaredSum += (array[i].y * array[i].y);
  }
  return ySquaredSum;
}

function getSlope(array) {
  return ((array.length * xySum(array) - (xSum(array) * ySum(array))) / ((array.length * xSquaredSum(array)) - (xSum(array) * xSum(array))));
}

function getYIntercept(array) {
  return ((((xSquaredSum(array) * ySum(array))) - ((xSum(array)) * xySum(array))) / ((array.length * xSquaredSum(array)) - (xSum(array) * xSum(array))));
}

function getCoefficient(array) {
  return ((((array.length * xySum(array) - xSum(array) * ySum(array)) * (array.length * xySum(array) - xSum(array) * ySum(array))) / (((array.length * xSquaredSum(array)) - (xSum(array) * xSum(array))) * ((array.length * ySquaredSum(array)) - (ySum(array) * ySum(array))))));
}

function OnClick(event) {
  // returns when out of bounds
  if (event.clientX > canvas.width || event.clientX < 0 || event.clientY > canvas.height || event.client < 0) return;

  // removes a point if clicked mouse position is equal
  for (let i = 0; i < allPoints.length; i++) {
    if (event.clientX == allPoints[i].x && event.clientY == allPoints[i].y) {
      allPoints.splice(i, 1);
      return;
    }
  }

  // spawns new point
  let point = new Point(event.clientX, event.clientY, pointSize);
  allPoints.push(point);
}

function OnKeyDown(event) {
  // clears everything if spacebar is pressed
  if (event.keyCode == 32) {
    allPoints = [];
  }
}

function SpawnRandomPoints(amount) {
  for (let i = 0; i < amount; i++) {
    let point = new Point(Math.random() * canvas.width, Math.random() * canvas.height, pointSize);
    allPoints.push(point);
    document.getElementById('amount').value = 0;
  }
}

// map points from graph coordinates to the screen
let graph_size = {width: 10, height: 10};
function to_screen(x, y) {
  return {x: (x/graph_size.width)*canvas.width, y: -(y/graph_size.height)*canvas.height + canvas.height};
}

// map points from screen coordinates to the graph
function to_graph(x, y) {
  return {x: x/canvas.width*graph_size.width, y: graph_size.height - y/canvas.height*graph_size.height};
}

// draw the graph's grid lines
function draw_grid() {
  context.strokeStyle = "black";
  for (let j = 0; j <= graph_size.width; j++) {

    // x lines
    context.beginPath();
    let p = to_screen(j, 0);
    context.moveTo(p.x, p.y);
    p = to_screen(j, graph_size.height);
    context.lineTo(p.x, p.y);
    context.stroke();

    // y lines
    context.beginPath();
    p = to_screen(0, j);
    context.moveTo(p.x, p.y);
    p = to_screen(graph_size.width, j);
    context.lineTo(p.x, p.y);
    context.stroke();
  }
}

function drawParameters(x, y, xSteps, ySteps) {
  context.strokeStyle = "black";
}

setInterval(function () {
  context.clearRect(0,0,canvas.width,canvas.height);
  update();
}, 10);

function update() {
  // updates the coefficient
  coefficient = document.getElementById('coefficient').innerHTML = "<b>correlation coefficient = </b>" + getCoefficient(allPoints);

  // creates the prediction line
  let l = new Line(getSlope(allPoints), getYIntercept(allPoints));

  // draws all points
  for (let i = 0; i < allPoints.length; i++)
  {
    allPoints[i].draw();
  }
  draw_grid();
  l.draw();
}

// checks for user input
document.addEventListener("click", OnClick);
document.addEventListener("keydown", OnKeyDown)
