
//Width and height
var WIDTH = 900;
var HEIGHT = 350;
var dataArray;
var targetDate = $("#targetDate").data("isodate");
$('#totalProduction').text("0");
$('#maximumProduction').text("0");
$('#averageProduction').text("0");
d3.json("/Report/MonthGraphData?targetDate=" + targetDate, function (error, json) {
    if (error) return console.warn(error);
    dataArray = json.data;
    targetDate = new Date(json.targetDate);
    $('#totalProduction').text(json.totalProduction);
    $('#maximumProduction').text(json.maximumProduction);
    $('#averageProduction').text(json.averageProduction);
    visualizeit();
});

function visualizeit() {

    var MARGINS = {
        top: 20,
        right: 10,
        bottom: 40,
        left: 60
    };

    // Create canvas to write to
    var svg = d3.select("#graph")
                .append("svg")
                .attr("width", WIDTH)
                .attr("height", HEIGHT);

    // Create display scalings between input data and canvas size
    var xScale = d3.time.scale.utc()
        .domain([d3.min(dataArray, function (d) { return d.timestamp; }), d3.max(dataArray, function (d) { return d.timestamp; })])
        .range([MARGINS.left, WIDTH - MARGINS.right]);

    var xRange = d3.scale.ordinal()
        .rangeRoundBands([MARGINS.left, WIDTH - MARGINS.right], 0.1)
        .domain(dataArray.map(function (d) { return d.timestamp; }));

    var yScale1 = d3.scale.linear()
        .domain([d3.min(dataArray, function (d) { return d.dayEnergy; }), d3.max(dataArray, function (d) { return d.dayEnergy; })])
        .range([HEIGHT - MARGINS.top, MARGINS.bottom]);

    var yRange1 = d3.time.scale.utc().range([HEIGHT - MARGINS.top, MARGINS.bottom]).domain([0,
        d3.max(dataArray, function (d) { return d.dayEnergy; })]);

    // Create chart - Energy Production (Wh)
    svg.selectAll('rect')
      .data(dataArray)
      .enter()
      .append('rect')
      .attr('x', function (d) { // sets the x position of the bar
          return xRange(d.timestamp);
      })
      .attr('y', function (d) { // sets the y position of the bar
          return yRange1(d.dayEnergy);
      })
      .attr('width', xRange.rangeBand()) // sets the width of bar
      .attr('height', function (d) {      // sets the height of bar
          return ((HEIGHT - MARGINS.bottom) - yRange1(d.dayEnergy));
      })
      .attr('fill', 'red')   // fills the bar with red color
      .on('mouseover', function (d) {
          d3.select(this)
              .attr('fill', 'blue');
      })
      .on('mouseout', function (d) {
          d3.select(this)
              .attr('fill', 'red');
      });

    // Append axes
    var xAxis = d3.svg.axis()
        .scale(xScale)
        .orient("bottom")
        .tickFormat(d3.time.format("%d"));

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
        .attr("fill", "black")
        .attr('transform', 'translate(' + (MARGINS.left) + ',' + (MARGINS.top - MARGINS.bottom) + ')')
        .call(yAxis1);

    // Create x-axis label
    svg.append("text")
        .attr("text-anchor", "end")
        .attr("x", WIDTH / 2 + 30)
        .attr("y", HEIGHT - 6)
        .text("Day");

    // Create left y-axis label - Total Production (Wh)
    svg.append("text")
        .attr("text-anchor", "end")
        .attr("fill", "red")
        .attr("y", 4)
        .attr("x", -120)
        .attr("dy", ".55em")
        .attr("transform", "rotate(-90)")
        .text("Total Production (Wh)");

    // Create title
    svg.append("text")
        .attr("text-anchor", "middle")
        .attr("x", WIDTH * 0.5)
        .attr("y", 10)
        .text("Graph of production for the month of " + moment(targetDate).format("MMMM YYYY"));

}