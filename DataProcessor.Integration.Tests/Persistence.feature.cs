﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SolarApp.DataProcessor.Integration.Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Persistence")]
    public partial class PersistenceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Persistence.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Persistence", "In order to keep data\r\nAs an automated system\r\nI want to be able to store data in" +
                    " MongoDb", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Store a random value to a setting entry in the database and retrieve it")]
        public virtual void StoreARandomValueToASettingEntryInTheDatabaseAndRetrieveIt()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Store a random value to a setting entry in the database and retrieve it", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
 testRunner.Given("I want to store some random value", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
 testRunner.When("I persist the setting to the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 9
 testRunner.Then("the random value should be retrievable from the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Store a data point in the database and retrieve it")]
        public virtual void StoreADataPointInTheDatabaseAndRetrieveIt()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Store a data point in the database and retrieve it", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Time",
                        "CurrentReading",
                        "DayEnergy",
                        "YearEnergy",
                        "TotalEnergy",
                        "FileName"});
            table1.AddRow(new string[] {
                        "[Now]",
                        "321",
                        "100",
                        "1000",
                        "10000",
                        "[Random]"});
#line 12
 testRunner.Given("I have a data point with values:", ((string)(null)), table1, "Given ");
#line 15
 testRunner.When("I persist the data point to the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Time",
                        "CurrentReading",
                        "DayEnergy",
                        "YearEnergy",
                        "TotalEnergy"});
            table2.AddRow(new string[] {
                        "[Now]",
                        "321",
                        "100",
                        "1000",
                        "10000"});
#line 16
 testRunner.Then("I can retrieve a data point with values:", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Calculates the average reading for a specified hour across two days where data is" +
            " provided")]
        public virtual void CalculatesTheAverageReadingForASpecifiedHourAcrossTwoDaysWhereDataIsProvided()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Calculates the average reading for a specified hour across two days where data is" +
                    " provided", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Time",
                        "CurrentReading",
                        "Comment"});
            table3.AddRow(new string[] {
                        "2015-01-15 09:23:00",
                        "100",
                        "Included"});
            table3.AddRow(new string[] {
                        "2015-01-15 09:33:00",
                        "200",
                        "Included"});
            table3.AddRow(new string[] {
                        "2015-01-14 09:46:00",
                        "300",
                        "Included"});
            table3.AddRow(new string[] {
                        "2015-01-15 10:02:01",
                        "2500",
                        "Excluded"});
#line 21
 testRunner.Given("I have a data points with values:", ((string)(null)), table3, "Given ");
#line 27
 testRunner.When("I calculate the mean for hour 9", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.Then("The calculated average value is 200", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Calculates the average reading as null for a specified hour across two days where" +
            " no data")]
        public virtual void CalculatesTheAverageReadingAsNullForASpecifiedHourAcrossTwoDaysWhereNoData()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Calculates the average reading as null for a specified hour across two days where" +
                    " no data", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Time",
                        "CurrentReading",
                        "Comment"});
            table4.AddRow(new string[] {
                        "2015-01-15 09:23:00",
                        "100",
                        "Excluded"});
            table4.AddRow(new string[] {
                        "2015-01-15 09:33:00",
                        "200",
                        "Excluded"});
            table4.AddRow(new string[] {
                        "2015-01-14 09:46:00",
                        "300",
                        "Excluded"});
            table4.AddRow(new string[] {
                        "2015-01-15 10:00:01",
                        "2500",
                        "Excluded"});
#line 31
 testRunner.Given("I have a data points with values:", ((string)(null)), table4, "Given ");
#line 37
 testRunner.When("I calculate the mean for hour 8", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
 testRunner.Then("The calculated average value is null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Calculates the latest reading across two days where data is provided")]
        public virtual void CalculatesTheLatestReadingAcrossTwoDaysWhereDataIsProvided()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Calculates the latest reading across two days where data is provided", ((string[])(null)));
#line 40
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Time",
                        "CurrentReading"});
            table5.AddRow(new string[] {
                        "2018-06-15 09:23:00",
                        "100"});
            table5.AddRow(new string[] {
                        "2018-06-15 09:33:00",
                        "200"});
            table5.AddRow(new string[] {
                        "2018-06-14 09:46:00",
                        "300"});
            table5.AddRow(new string[] {
                        "2018-06-15 10:00:01",
                        "2500"});
#line 41
 testRunner.Given("I have a data points with values:", ((string)(null)), table5, "Given ");
#line 47
 testRunner.When("I calculate the latest date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 48
 testRunner.Then("The calculated latest date is \'2018-06-15 10:00:01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Calculates the energy output for the period stated")]
        public virtual void CalculatesTheEnergyOutputForThePeriodStated()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Calculates the energy output for the period stated", ((string[])(null)));
#line 50
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Time",
                        "CurrentReading",
                        "DayEnergy"});
            table6.AddRow(new string[] {
                        "2015-06-14 23:59:59",
                        "50",
                        "36"});
            table6.AddRow(new string[] {
                        "2015-06-15 09:00:00",
                        "100",
                        "5"});
            table6.AddRow(new string[] {
                        "2015-06-15 09:15:00",
                        "200",
                        "15"});
            table6.AddRow(new string[] {
                        "2015-06-15 09:30:00",
                        "300",
                        "27"});
            table6.AddRow(new string[] {
                        "2015-06-15 09:45:01",
                        "400",
                        "38"});
            table6.AddRow(new string[] {
                        "2015-06-16 00:00:01",
                        "50",
                        "1"});
#line 51
 testRunner.Given("I have a data points with values:", ((string)(null)), table6, "Given ");
#line 59
 testRunner.When("I calculate the energy output for 2015-06-15", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Timestamp",
                        "CurrentEnergy",
                        "DayEnergy"});
            table7.AddRow(new string[] {
                        "2015-06-15 09:00:00",
                        "100",
                        "5"});
            table7.AddRow(new string[] {
                        "2015-06-15 09:15:00",
                        "200",
                        "15"});
            table7.AddRow(new string[] {
                        "2015-06-15 09:30:00",
                        "300",
                        "27"});
            table7.AddRow(new string[] {
                        "2015-06-15 09:45:01",
                        "400",
                        "38"});
#line 60
 testRunner.Then("The energy readings returned have values:", ((string)(null)), table7, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Gets the sun up, set and azimuth times for a specified date")]
        public virtual void GetsTheSunUpSetAndAzimuthTimesForASpecifiedDate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Gets the sun up, set and azimuth times for a specified date", ((string[])(null)));
#line 67
this.ScenarioSetup(scenarioInfo);
#line 68
 testRunner.Given("I have suntimes for utc date 2015-08-30", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 69
 testRunner.When("I request the suntime for the utc date 2015-08-30", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 70
 testRunner.Then("I have a sunrise of 05:11:00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 71
 testRunner.And("I have a sunset of 19:02:00", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
 testRunner.And("I have a sun azimuth time of 12:06:30", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
