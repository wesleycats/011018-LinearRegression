const canvas = document.getElementById("canvas");
const context = canvas.getContext('2d');
const randomAmount = document.getElementById('random-amount');
const preciseAmount = document.getElementById('precise-amount');

var rect = canvas.getBoundingClientRect();
var allPoints = [];
var pointSize = 5;
var randomAmountLimit = 300;
var parameters, xParameter, yParameter, l;
var debug = true;

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
  return ((((array.length * xySum(array) - xSum(array) * ySum(array)) * (array.length * xySum(array) - xSum(array) * ySum(array))) / (((array.length * xSquaredSum(array)) - (xSum(array) * xSum(array)))
  * ((array.length * ySquaredSum(array)) - (ySum(array) * ySum(array))))));
}

function OnClick(event) {
  // returns when out of bounds
  let realX = event.clientX - rect.left;
  let realY = event.clientY - rect.top;
  if (realX < 0 || realX > canvas.width || realY < 0 || realY > canvas.height) return;

  let pointDiameter = Math.round(pointSize);

  // removes a point if clicked mouse position is in point borders
  for (let i = 0; i < allPoints.length; i++) {
    if (realX >= (allPoints[i].x - pointDiameter) && realX <= (allPoints[i].x + pointDiameter) && realY >= (allPoints[i].y - pointDiameter) && realY <= (allPoints[i].y + pointDiameter)) {
      allPoints.splice(i, 1);
      return;
    }
  }

  // spawns new point
  let point = new Point(realX, realY, pointSize);
  allPoints.push(point);
}

function OnKeyDown(event) {
  // clears everything if spacebar is pressed
  switch (event.keyCode) {
    case 32:
      {
        allPoints = [];
      }
      break;
    case 88:
      {
        allPoints.splice(allPoints.length-1, 1);
      }
      break;
  }
  // debug
  // console.log(event.keyCode);
}

function OnMouseMove(event) {
  let realX = event.clientX - rect.left;
  let realY = event.clientY - rect.top;
  if (realX < 0 || realX > canvas.width || realY < 0 || realY > canvas.height) return;

  mousePos = document.getElementById('mouse-pos').innerHTML = "Mouse position: [" + realX + ", " + realY + "]";
}

function SpawnRandomPoints(amount) {
  if (!amount) return;

  // spawns amount of random points
  for (let i = 0; i < amount; i++) {
    let point = new Point(Math.floor((Math.random() * rect.right) + rect.left), Math.floor((Math.random() * rect.bottom) + rect.top), pointSize);
    allPoints.push(point);
  }
}

function SpawnPrecisePoint(position) {
  if (!position) return;

  let point = new Point(xParameter, yParameter, pointSize);
  allPoints.push(point);
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

setInterval(function () {
  context.clearRect(0,0,canvas.width,canvas.height);
  update();
}, 10);

// returns the int or 0
function EnsuresInt(input) {
  newValue = parseInt(input);
  if (Number.isInteger(newValue))
  {
    return newValue;
  }
  else {
    return 0;
  }
}

function RemoveSpaces(input) {
  return input.replace(" ", "");
}

function isOnCanvas(input, axis) {
  if (input <= 0)
  {
    return 0;
  }
  else if (input >= canvas.width && axis == "x")
  {
    return canvas.width;
  }
  else if (input >= canvas.height && axis == "y")
  {
    return canvas.height;
  }

  return input;
}

function update() {

  preciseAmount.value = RemoveSpaces(preciseAmount.value);
  randomAmount.value = RemoveSpaces(randomAmount.value);

  // gets x and y values out the userinput
  parameters = preciseAmount.value.split(",");
  xParameter = parameters[0];
  yParameter = parameters[1];

  xParameter = EnsuresInt(xParameter);
  yParameter = EnsuresInt(yParameter);
  randomAmount.value = EnsuresInt(randomAmount.value);

  xParameter = isOnCanvas(xParameter, "x");
  yParameter = isOnCanvas(yParameter, "y");

  // ensures there is a comma between the parameters
  preciseAmount.value = xParameter + "," + yParameter;

  // ensures limited amount of random spawned points
  if (randomAmount.value > randomAmountLimit)
  {
    randomAmount.value = randomAmountLimit;
  }

  if (allPoints.length > 1)
  {
    // updates the coefficient
    coefficient = document.getElementById('coefficient').innerHTML = "<b>Correlation Coefficient = </b>" + parseFloat((getCoefficient(allPoints)).toFixed(5));

    // creates the prediction line
    l = new Line(getSlope(allPoints), getYIntercept(allPoints));
  }


  // draws all points
  for (let i = 0; i < allPoints.length; i++)
  {
    allPoints[i].draw();
  }
  draw_grid();
  if (l == null) return;
  l.draw();
}

// checks for user input
document.addEventListener("click", OnClick);
document.addEventListener("keydown", OnKeyDown)
document.addEventListener("mousemove",OnMouseMove);
