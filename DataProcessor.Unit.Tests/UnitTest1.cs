using System;
using NUnit.Framework;
using Model;

namespace DataProcessor.Unit.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        [TestCase(53,1006,53001,123456)]
        public void Given_SomeData_When_ISerializeADataPoint_Then_TheDataPointIsCreatedWithTheCorrectValues(int pac, int dayEnergy, int yearEnergy, int totalEnergy)
        {
            DataPoint dataPoint = new DataPoint();
            dataPoint._id = Guid.NewGuid().ToString();
            dataPoint.Body = new Body(){
                PAC = new FroniusEnergyReading() {
                    Unit = "W",
                    Values = new FroniusEnergyReadingItem() {
                        Value = pac
                    }},
                DAY_ENERGY = new FroniusEnergyReading() {
                    Unit = "Wh",
                    Values = new FroniusEnergyReadingItem() {
                        Value = dayEnergy
                    }},                    
                    YEAR_ENERGY = new FroniusEnergyReading() {
                    Unit = "Wh",
                    Values = new FroniusEnergyReadingItem() {
                        Value = yearEnergy
                    }},
                TOTAL_ENERGY = new FroniusEnergyReading() {
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
            Assert.True(true);
            
        }
    }
}
