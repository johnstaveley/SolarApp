
//Width and height
var WIDTH = 800;
var HEIGHT = 350;
var barPadding = 1;

var dataElement = $('#graphData').data("energyreadings");
var dataArray = JSON.parse("[" + dataElement + "]");
var MARGINS = {
    top: 20,
    right: 20,
    bottom: 20,
    left: 50
};

var svg = d3.select("#graph")
            .append("svg")
            .attr("width", WIDTH)   
            .attr("height", HEIGHT); 

var xScale = d3.scale.linear()
    .domain([d3.min(dataArray, function (d) { return d[0]; }), d3.max(dataArray, function (d) { return d[0]; })])
    .range([MARGINS.left, WIDTH - MARGINS.right]);
var yScale1 = d3.scale.linear()
    .domain([d3.min(dataArray, function (d) { return d[1]; }), d3.max(dataArray, function (d) { return d[1]; })])
    .range([HEIGHT - MARGINS.top, MARGINS.bottom]);
var yScale2 = d3.scale.linear()
    .domain([d3.min(dataArray, function (d) { return d[2]; }), d3.max(dataArray, function (d) { return d[2]; })])
    .range([HEIGHT - MARGINS.top, MARGINS.bottom]);

svg.selectAll("rect") // <-- returns an empty collection which is then added to
   .data(dataArray)
   .enter()
   .append("rect")
   .attr("x", function (d, i) {
       return i * (WIDTH / dataArray.length - barPadding);
   })
    .attr("height", function (d) {
        return d[1];
    })
    .attr("width", 5)
    .attr("y", function (d) {
        return HEIGHT - d[1];  //Height minus data value
    })
    .attr("fill", "teal")
;

var xAxis = d3.svg.axis()
    .scale(xScale)
    .orient("bottom")
    .ticks(20);

svg.append("g")
    .attr("class", "axis")
    .attr('transform', 'translate(0,' + (HEIGHT - MARGINS.bottom) + ')')
    .call(xAxis);

var yAxis1 = d3.svg.axis()
    .scale(yScale1)
    .orient("left")
    .ticks(5);

svg.append("g")
    .attr("class", "axis")
    .attr('transform', 'translate(' + (MARGINS.left) + ',0)')
    .call(yAxis1);

var yAxis2 = d3.svg.axis()
    .scale(yScale2)
    .orient("right")
    .ticks(5);

svg.append("g")
    .attr("class", "axis")
    .attr('transform', 'translate(' + (WIDTH - MARGINS.right) + ',0)')
    .call(yAxis2);

var lineFunc = d3.svg.line()
  .x(function (d) {
      return xScale(d[0]);
  })
  .y(function (d) {
      return yScale2(d[2]);
  })
  .interpolate('linear');

svg.append('svg:path')
  .attr('d', lineFunc(dataArray))
  .attr('stroke', 'blue')
  .attr('stroke-width', 2)
  .attr('fill', 'none');



