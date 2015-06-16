using System;
using NUnit.Framework;
using Model;
using DataProcessor.Utility;
using Newtonsoft.Json;
using DataProcessor.Unit.Tests.Properties;

namespace DataProcessor.Unit.Tests
{
    [TestFixture]
    public class Serialization
    {

        [Test]
        public void Given_SomeTestData_When_IDeserializeADataPoint_Then_TheDataPointIsCreatedInAnObjectWithTheCorrectValues()
        {

            // Arrange
            var json = Resources.SampleJsonData;  

            // Act
            var dataPoint = JsonConvert.DeserializeObject<DataPoint>(json);

            // Assert
            Assert.IsNotNull(dataPoint);
            var timeStamp = DateTime.Parse("2015-05-20T06:25:52+02:00");
            Assert.AreEqual(timeStamp, dataPoint.Head.Timestamp);
            Assert.AreEqual("Inverter", dataPoint.Head.RequestArguments.Query);
            Assert.AreEqual("System", dataPoint.Head.RequestArguments.Scope);
            Assert.AreEqual(0, dataPoint.Head.Status.Code);
            Assert.AreEqual("", dataPoint.Head.Status.Reason);
            Assert.AreEqual("", dataPoint.Head.Status.UserMessage);
            Assert.AreEqual("W", dataPoint.Body.CurrentReading.Unit);
            Assert.AreEqual(53, dataPoint.Body.CurrentReading.Values.Value);
            Assert.AreEqual("Wh", dataPoint.Body.DayEnergy.Unit);
            Assert.AreEqual(29, dataPoint.Body.DayEnergy.Values.Value);
            Assert.AreEqual("Wh", dataPoint.Body.YearEnergy.Unit);
            Assert.AreEqual(55022, dataPoint.Body.YearEnergy.Values.Value);
            Assert.AreEqual("Wh", dataPoint.Body.TotalEnergy.Unit);
            Assert.AreEqual(119233, dataPoint.Body.TotalEnergy.Values.Value);

        }

        [Test]
        [TestCase(53,1006,53001,123456)]
        public void Given_SomeData_When_ISerializeADataPoint_Then_TheDataPointIsCreatedAsAStringWithTheCorrectValues(int pac, int dayEnergy, int yearEnergy, int totalEnergy)
        {
            // Arrange
            DataPoint dataPoint = new DataPoint();
            dataPoint._id = Guid.NewGuid().ToString();
            dataPoint.Body = new Body(){
                CurrentReading = new FroniusEnergyReading() {
                    Unit = "W",
                    Values = new FroniusEnergyReadingItem() {
                        Value = pac
                    }},
                DayEnergy = new FroniusEnergyReading() {
                    Unit = "Wh",
                    Values = new FroniusEnergyReadingItem() {
                        Value = dayEnergy
                    }},                    
                    YearEnergy = new FroniusEnergyReading() {
                    Unit = "Wh",
                    Values = new FroniusEnergyReadingItem() {
                        Value = yearEnergy
                    }},
                TotalEnergy = new FroniusEnergyReading() {
                    Unit = "Wh",
                    Values = new FroniusEnergyReadingItem() {
                        Value = totalEnergy
                    }}};
            dataPoint.Head = new Head() {
                Timestamp = DateTime.Now,
                RequestArguments = new RequestArguments() {
                    Query = "Inverter",
                    Scope = "System"
                },
                Status = new Status() {
                    Code = 0,
                    Reason = "",
                    UserMessage = ""
                }
            };

            // Act
            var json = JsonConvert.SerializeObject(dataPoint);

            // Assert 
            Assert.IsTrue(json.IndexOf("PAC") < json.IndexOf("DAY_ENERGY"), "PAC and DAY_ENERGY are out of order");
            Assert.IsTrue(json.IndexOf("DAY_ENERGY") < json.IndexOf("YEAR_ENERGY"), "DAY_ENERGY and YEAR_ENERGY are out of order");
            Assert.IsTrue(json.IndexOf("YEAR_ENERGY") < json.IndexOf("TOTAL_ENERGY"), "YEAR_ENERGY and TOTAL_ENERGY are out of order");
            Assert.IsTrue(json.IndexOf("Head") < json.IndexOf("Body"), "Head and Body are out of order");
            
        }

    }
}
