
//Width and height
var WIDTH = 900;
var HEIGHT = 350;
var dataElement = $('#graphData').data("energyreadings");
var dataArray = JSON.parse("[" + dataElement + "]");
var MARGINS = {
    top: 20,
    right: 50,
    bottom: 40,
    left: 50
};

// Create canvas to write to
var svg = d3.select("#graph")
            .append("svg")
            .attr("width", WIDTH)   
            .attr("height", HEIGHT); 

// Create scalings between input data and canvas size
var xScale = d3.time.scale()
    .domain([d3.min(dataArray, function (d) { return d[0]; }), d3.max(dataArray, function (d) { return d[0]; })])
    .range([MARGINS.left, WIDTH - MARGINS.right]);

var yScale1 = d3.scale.linear()
    .domain([d3.min(dataArray, function (d) { return d[1]; }), d3.max(dataArray, function (d) { return d[1]; })])
    .range([HEIGHT - MARGINS.top, MARGINS.bottom]);

var yScale2 = d3.scale.linear()
    .domain([d3.min(dataArray, function (d) { return d[2]; }), d3.max(dataArray, function (d) { return d[2]; })])
    .range([HEIGHT - MARGINS.top, MARGINS.bottom]);

var xRange = d3.scale.ordinal().rangeRoundBands([MARGINS.left, WIDTH - MARGINS.right], 0.1).domain(dataArray.map(function (d) {
    return d[0];
}));

var yRange1 = d3.scale.linear().range([HEIGHT - MARGINS.top, MARGINS.bottom]).domain([0,
  d3.max(dataArray, function (d) {
      return d[1];
  })]);

// Create chart for first set of data
svg.selectAll('rect')
  .data(dataArray)
  .enter()
  .append('rect')
  .attr('x', function (d) { // sets the x position of the bar
      return xRange(d[0]);
  })
  .attr('y', function (d) { // sets the y position of the bar
      return yRange1(d[1]);
  })
  .attr('width', xRange.rangeBand()) // sets the width of bar
  .attr('height', function (d) {      // sets the height of bar
      return ((HEIGHT - MARGINS.bottom) - yRange1(d[1]));
  })
  .attr('fill', 'red')   // fills the bar with grey color
  .on('mouseover', function (d) {
      d3.select(this)
          .attr('fill', 'blue');
  })
  .on('mouseout', function (d) {
      d3.select(this)
          .attr('fill', 'red');
  });

var xAxis = d3.svg.axis()
    .scale(xScale)
    .orient("bottom")
    .ticks(12);

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

var yRange2 = d3.scale.linear().range([HEIGHT - MARGINS.top, MARGINS.bottom]).domain([0,
  d3.max(dataArray, function (d) {
      return d[2];
  })]);

var lineFunc = d3.svg.line()
  .x(function (d) {
      return xScale(d[0]);
  })
  .y(function (d) {
      return yRange2(d[2]);
  })
  .interpolate('linear');

svg.append('svg:path')
  .attr('d', lineFunc(dataArray))
  .attr('stroke', 'blue')
  .attr('stroke-width', 2)
  .attr('fill', 'none');

// Create x-axis label
svg.append("text")
    .attr("text-anchor", "end")
    .attr("x", WIDTH/2 + 30 )
    .attr("y", HEIGHT - 6)
    .text("Time of day");

// Create left y-axis label
svg.append("text")
    .attr("text-anchor", "end")
    .attr("class", "yaxis1")
    .attr("y", 12)
    .attr("x", -90)
    .attr("dy", ".55em")
    .attr("transform", "rotate(-90)")
    .text("Energy Production (Wh)");

// Create right y-axis label
svg.append("text")
    .attr("text-anchor", "end")
    .attr("class", "yaxis2")
    .attr("y", WIDTH-12)
    .attr("x", -90)
    .attr("dy", ".55em")
    .attr("transform", "rotate(-90)")
    .text("Current Output (W)");

