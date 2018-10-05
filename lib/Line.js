class Line {
  constructor(slope, yIntercept,color) {
    this.slope = slope;
    this.yIntercept = yIntercept;
    this.color = color || "black";
  }

  calcY(x) {
    return this.slope * x + this.yIntercept;
  }

  draw() {
    context.strokeStyle = this.color;
    context.beginPath();
    context.moveTo(0, this.calcY(0));
    context.lineTo(800, this.calcY(800));
    context.stroke();
    context.closePath();
  }

  drawInner(startX, endX, lineWidth) {
    context.beginPath();
    context.moveTo(startX, this.calcY(startX));
    context.lineTo(endX, this.calcY(endX));
    context.lineWidth = lineWidth || 2;
    context.strokeStyle = this.color;
    context.stroke();
    context.closePath();
  }

  drawInnerCenter() {
    context.beginPath();
    context.moveTo(0, this.calcY(0));
    context.lineTo(800, this.calcY(800));
    context.lineWidth = 2;
    context.strokeStyle = "red";
    context.stroke();
    context.closePath();
  }

  intersection (m){
    var ans = {};
    ans.x = (m.yIntercept - this.yIntercept)/(this.slope-m.slope);
    ans.y = (ans.x * this.slope) + this.yIntercept
    return ans;
  }
}
