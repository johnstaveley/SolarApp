
//Width and height
var WIDTH = 900;
var HEIGHT = 350;
var dataArray;
var targetDate = $("#targetDate").data("isodate");
$('#totalProduction').text("0");
$('#maximumProduction').text("0");
d3.json("/Report/DayGraphData?targetDate=" + targetDate, function (error, json) {
    if (error) return console.warn(error);
    dataArray = json.data;
    targetDate = new Date(json.targetDate);
    $('#totalProduction').text(json.totalProduction);
    $('#maximumProduction').text(json.maximumProduction);
    $('#sunrise').text(moment(json.sunrise).format("HH:mm"));
    $('#sunset').text(moment(json.sunset).format("HH:mm")); 
    $('#sunAzimuth').text(moment(json.sunAzimuth).format("HH:mm"));
    visualizeit();
});

function visualizeit() {

    var MARGINS = {
        top: 20,
        right: 55,
        bottom: 40,
        left: 52
    };

    // Create canvas to write to
    var svg = d3.select("#graph")
                .append("svg")
                .attr("width", WIDTH)
                .attr("height", HEIGHT);

    var xRange = d3.scale.ordinal()
        .rangeRoundBands([MARGINS.left, WIDTH - MARGINS.right])
        .domain(dataArray.map(function (d) {
            return d.timestamp;
        }));

    // Create chart 1 - Energy Production (Wh)
    var yRange1 = d3.scale.linear().range([HEIGHT - MARGINS.top, MARGINS.bottom]).domain([0,
      d3.max(dataArray, function (d) {
          return d.currentEnergy;
      })]);

    var lineFunc1 = d3.svg.line()
      .x(function (d) {
          return xRange(d.timestamp);
      })
      .y(function (d) {
          return yRange1(d.currentEnergy) + (MARGINS.top - MARGINS.bottom);
      })
      .interpolate('cardinal');

    svg.append('svg:path')
      .attr('d', lineFunc1(dataArray))
      .attr('stroke', 'red')
      .attr('stroke-width', 2)
      .attr('fill', 'none');

    // Create chart 2 - Instananeous output (W)
    var yRange2 = d3.scale.linear().range([HEIGHT - MARGINS.top, MARGINS.bottom]).domain([0,
      d3.max(dataArray, function (d) {
          return d.dayEnergyInstant;
      })]);

    var lineFunc2 = d3.svg.line()
      .x(function (d) {
          return xRange(d.timestamp);
      })
      .y(function (d) {
          return yRange2(d.dayEnergyInstant) + (MARGINS.top - MARGINS.bottom);
      })
      .interpolate('linear');

    svg.append('svg:path')
      .attr('d', lineFunc2(dataArray))
      .attr('stroke', 'blue')
      .attr('stroke-width', 2)
      .attr('fill', 'none');

    // Create display scalings between input data and canvas size
    var xScale = d3.time.scale.utc()
        .domain([d3.min(dataArray, function (d) { return d.timestamp; }), d3.max(dataArray, function (d) { return d.timestamp; })])
        .range([MARGINS.left + xRange.rangeBand(), WIDTH - MARGINS.right - xRange.rangeBand()]);

    var yScale1 = d3.scale.linear()
        .domain([d3.min(dataArray, function (d) { return d.currentEnergy; }), d3.max(dataArray, function (d) { return d.currentEnergy; })])
        .range([HEIGHT - MARGINS.top, MARGINS.bottom]);

    var yScale2 = d3.scale.linear()
        .domain([d3.min(dataArray, function (d) { return d.dayEnergyInstant; }), d3.max(dataArray, function (d) { return d.dayEnergyInstant; })])
        .range([HEIGHT - MARGINS.top, MARGINS.bottom]);

    // Append axes
    var xAxis = d3.svg.axis()
        .scale(xScale)
        .orient("bottom")
        .ticks(12)
        .tickFormat(d3.time.format("%H"));

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
        .attr("fill", "blue")
        .attr('transform', 'translate(' + (MARGINS.left) + ',' + (MARGINS.top - MARGINS.bottom) + ')')
        .call(yAxis1);

    var yAxis2 = d3.svg.axis()
        .scale(yScale2)
        .orient("right")
        .ticks(5);

    svg.append("g")
        .attr("class", "axis")
        .attr("fill", "red")
        .attr('transform', 'translate(' + (WIDTH - MARGINS.right) + ',' + (MARGINS.top - MARGINS.bottom) + ')')
        .call(yAxis2);

    // Create x-axis label
    svg.append("text")
        .attr("text-anchor", "end")
        .attr("x", WIDTH / 2 + 30)
        .attr("y", HEIGHT - 6)
        .text("Time of day");

    // Create left y-axis label - Current output (W)
    svg.append("text")
        .attr("text-anchor", "end")
        .attr("fill", "blue")
        .attr("y", 4)
        .attr("x", -120)
        .attr("dy", ".55em")
        .attr("transform", "rotate(-90)")
        .text("Current Output (W)");

    // Create right y-axis label - Instantaneous Production (Wh)
    svg.append("text")
        .attr("text-anchor", "end")
        .attr("fill", "red")
        .attr("y", WIDTH - 12)
        .attr("x", -90)
        .attr("dy", ".55em")
        .attr("transform", "rotate(-90)")
        .text("Energy Production (Wh)");

    // Create title
    svg.append("text")
        .attr("text-anchor", "start")
        .attr("x", WIDTH * 0.63)
        .attr("y", 50)
        .text("Graph of instantaneous energy output");
    svg.append("text")
        .attr("text-anchor", "start")
        .attr("x", WIDTH * 0.63)
        .attr("y", 70)
        .text("and production for " + moment(targetDate).format("dddd DD/MM/YYYY"));
}