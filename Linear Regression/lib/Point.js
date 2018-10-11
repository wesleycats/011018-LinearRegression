class Point {
  constructor(x,y,r,color,label) {
    this.x = x;
    this.y = y;
    this.r = r || 20;
    this.color = color || "black";
    this.label = label || "";
  }

  draw() {
    context.beginPath();
    context.fillStyle = this.color;
    context.strokeStyle = "black";
    context.arc(this.x,this.y,this.r,0,2*Math.PI);
    context.closePath();
    context.fill();
    context.stroke();
    context.font = "20px Arial";
    context.fillText(this.label,this.x-this.r/2,this.y-this.r);
  }

  drag() {
    let drag = false;
    let mousePosition = {};

    canvas.addEventListener('mousedown', (evt)=> {
      let rect = canvas.getBoundingClientRect();
      mousePosition = {
    			x: evt.clientX - rect.left,
    			y: evt.clientY - rect.top
  		};

      //if (mouseX - this.x <= this.r && mouseY - this.y <= this.r && this.x - mouseX <= this.r && this.y - mouseY <= this.r)
      if (this.distance(mousePosition) < this.r)
      {
        drag = true;
      }
      else
      {
        drag = false;
      }
    });

    canvas.addEventListener('mousemove', (evt)=> {
      let rect = canvas.getBoundingClientRect();
      mousePosition = {
    			x: evt.clientX - rect.left,
    			y: evt.clientY - rect.top
  		}

      if (this.distance(mousePosition) < this.r)
      {
    		canvas.style.cursor = 'pointer';
    		evt.stopImmediatePropagation();
  	   }
       else
       {
    		canvas.style.cursor = "default";
  	   }


      if (drag)
      {
        this.x = mousePosition.x;
        this.y = mousePosition.y;
      }
    });

    canvas.addEventListener('mouseup', (evt)=> {
      drag = false;
    });
  }
}
