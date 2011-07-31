﻿using Ookii.CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace Ookii.CommandLine.Tests
{
    
    
    /// <summary>
    ///This is a test class for CommandLineParserTest and is intended
    ///to contain all CommandLineParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CommandLineParserTest
    {
        #region Nested types

        class EmptyArguments
        {
        }

        [System.ComponentModel.Description("Test arguments description.")]
        class TestArguments
        {
            private TestArguments(string notAnArg)
            {
            }

            public TestArguments([System.ComponentModel.Description("Arg1 description.")] string arg1, [System.ComponentModel.Description("Arg2 description."), ArgumentName("other"), ValueDescription("Number")] int arg2 = 42, bool notSwitch = false)
            {
                Arg1 = arg1;
                Arg2 = arg2;
                NotSwitch = notSwitch;
            }

            public string Arg1 { get; private set; }

            public int Arg2 { get; private set; }

            public bool NotSwitch { get; private set; }

            [CommandLineArgument()]
            public string Arg3 { get; set; }

            [CommandLineArgument("other2", DefaultValue = 47, ValueDescription = "Number", Position = 1), System.ComponentModel.Description("Arg4 description.")]
            public int Arg4 { get; set; }

            [CommandLineArgument(Position = 0), System.ComponentModel.Description("Arg5 description.")]
            public float Arg5 { get; set; }

            [CommandLineArgument(IsRequired = true), System.ComponentModel.Description("Arg6 description.")]
            public string Arg6 { get; set; }

            [CommandLineArgument()]
            public bool Arg7 { get; set; }

            [CommandLineArgument(Position=2)]
            public DayOfWeek[] Arg8 { get; set; }

            [CommandLineArgument()]
            public int? Arg9 { get; set; }

            [CommandLineArgument]
            public bool[] Arg10 { get; set; }

            [CommandLineArgument]
            public bool? Arg11 { get; set; }

            public string NotAnArg { get; set; }

            [CommandLineArgument()]
            private string NotAnArg2 { get; set; }

            [CommandLineArgument()]
            public static string NotAnArg3 { get; set; }
        }

        class MultipleConstructorsArguments
        {
            public MultipleConstructorsArguments() { }
            public MultipleConstructorsArguments(string notArg1, int notArg2) { }
            [CommandLineConstructor]
            public MultipleConstructorsArguments(string arg1) { }
        }

        #endregion

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CommandLineParser Constructor
        ///</summary>
        [TestMethod()]
        public void ConstructorEmptyArgumentsTest()
        {
            Type argumentsType = typeof(EmptyArguments);
            CommandLineParser target = new CommandLineParser(argumentsType);
            Assert.AreEqual(CultureInfo.CurrentCulture, target.Culture);
            Assert.AreEqual(false, target.AllowDuplicateArguments);
            Assert.AreEqual(true, target.AllowWhiteSpaceValueSeparator);
            CollectionAssert.AreEqual(CommandLineParser.DefaultArgumentNamePrefixes.ToArray(), target.ArgumentNamePrefixes);
            Assert.AreEqual(argumentsType, target.ArgumentsType);
            Assert.AreEqual(string.Empty, target.Description);
            Assert.AreEqual(0, target.Arguments.Count);
        }

        [TestMethod()]
        public void ConstructorTest()
        {
            Type argumentsType = typeof(TestArguments);
            CommandLineParser target = new CommandLineParser(argumentsType);
            Assert.AreEqual(CultureInfo.CurrentCulture, target.Culture);
            Assert.AreEqual(false, target.AllowDuplicateArguments);
            Assert.AreEqual(true, target.AllowWhiteSpaceValueSeparator);
            CollectionAssert.AreEqual(CommandLineParser.DefaultArgumentNamePrefixes.ToArray(), target.ArgumentNamePrefixes);
            Assert.AreEqual(argumentsType, target.ArgumentsType);
            Assert.AreEqual("Test arguments description.", target.Description);
            Assert.AreEqual(12, target.Arguments.Count);
            IEnumerator<CommandLineArgument> args = target.Arguments.GetEnumerator();
            TestArgument(args, "arg1", typeof(string), 0, true, null, "Arg1 description.", "String", false);
            TestArgument(args, "Arg10", typeof(bool[]), null, false, null, "", "Boolean", true);
            TestArgument(args, "Arg11", typeof(bool?), null, false, null, "", "Boolean", true);
            TestArgument(args, "Arg3", typeof(string), null, false, null, "", "String", false);
            TestArgument(args, "Arg5", typeof(float), 3, false, null, "Arg5 description.", "Single", false);
            TestArgument(args, "Arg6", typeof(string), null, true, null, "Arg6 description.", "String", false);
            TestArgument(args, "Arg7", typeof(bool), null, false, null, "", "Boolean", true);
            TestArgument(args, "Arg8", typeof(DayOfWeek[]), 5, false, null, "", "DayOfWeek", false);
            TestArgument(args, "Arg9", typeof(int?), null, false, null, "", "Int32", false);
            TestArgument(args, "notSwitch", typeof(bool), 2, false, false, "", "Boolean", false);
            TestArgument(args, "other", typeof(int), 1, false, 42, "Arg2 description.", "Number", false);
            TestArgument(args, "other2", typeof(int), 4, false, 47, "Arg4 description.", "Number", false);
        }

        [TestMethod]
        public void ConstructorMultipleArgumentConstructorsTest()
        {
            Type argumentsType = typeof(MultipleConstructorsArguments);
            CommandLineParser target = new CommandLineParser(argumentsType);
            Assert.AreEqual(CultureInfo.CurrentCulture, target.Culture);
            Assert.AreEqual(false, target.AllowDuplicateArguments);
            Assert.AreEqual(true, target.AllowWhiteSpaceValueSeparator);
            CollectionAssert.AreEqual(CommandLineParser.DefaultArgumentNamePrefixes.ToArray(), target.ArgumentNamePrefixes);
            Assert.AreEqual(argumentsType, target.ArgumentsType);
            Assert.AreEqual("", target.Description);
            Assert.AreEqual(1, target.Arguments.Count);
            IEnumerator<CommandLineArgument> args = target.Arguments.GetEnumerator();
            TestArgument(args, "arg1", typeof(string), 0, true, null, "", "String", false);

        }

        [TestMethod]
        public void ParseTest()
        {
            Type argumentsType = typeof(TestArguments);
            CommandLineParser target = new CommandLineParser(argumentsType, new[] { "/", "-" }) { Culture = CultureInfo.InvariantCulture };

            // Only required arguments
            TestParse(target, "val1 2 /arg6 val6", "val1", 2, arg6: "val6");
            // Make sure negative numbers are accepted, and not considered an argument name.
            TestParse(target, "val1 -2 /arg6 val6", "val1", -2, arg6: "val6");
            // All positional arguments except array
            TestParse(target, "val1 2 true 5.5 4 /arg6 arg6", "val1", 2, true, arg4: 4, arg5: 5.5f, arg6: "arg6");
            // All positional arguments including array
            TestParse(target, "val1 2 true 5.5 4 /arg6 arg6 Monday Tuesday", "val1", 2, true, arg4: 4, arg5: 5.5f, arg6: "arg6", arg8: new[] { DayOfWeek.Monday, DayOfWeek.Tuesday });
            // All positional arguments including array, which is specified by name first and then by position
            TestParse(target, "val1 2 true 5.5 4 /arg6 arg6 /arg8 Monday Tuesday", "val1", 2, true, arg4: 4, arg5: 5.5f, arg6: "arg6", arg8: new[] { DayOfWeek.Monday, DayOfWeek.Tuesday });
            // Some positional arguments using names, in order
            TestParse(target, "/arg1 val1 2 true /arg5 5.5 4 /arg6 arg6", "val1", 2, true, arg4: 4, arg5: 5.5f, arg6: "arg6");
            // Some position arguments using names, out of order (also uses : and - for one of them to mix things up)
            TestParse(target, "/other 2 val1 -arg5:5.5 true 4 /arg6 arg6", "val1", 2, true, arg4: 4, arg5: 5.5f, arg6: "arg6");
            // All arguments
            TestParse(target, "val1 2 true /arg3 val3 -other2:4 5.5 /arg6 val6 /arg7 /arg8 Monday /arg8 Tuesday /arg9 9 /arg10 /arg10 /arg10:false /arg11:false", "val1", 2, true, "val3", 4, 5.5f, "val6", true, new[] { DayOfWeek.Monday, DayOfWeek.Tuesday }, 9, new[] { true, true, false }, false);
        }

        private static void TestArgument(IEnumerator<CommandLineArgument> arguments, string name, Type type, int? position, bool isRequired, object defaultValue, string description, string valueDescription, bool isSwitch)
        {
            arguments.MoveNext();
            CommandLineArgument argument = arguments.Current;
            Assert.AreEqual(name, argument.ArgumentName);
            Assert.AreEqual(type, argument.ArgumentType);
            Assert.AreEqual(position, argument.Position);
            Assert.AreEqual(isRequired, argument.IsRequired);
            Assert.AreEqual(description, argument.Description);
            Assert.AreEqual(valueDescription, argument.ValueDescription);
            Assert.AreEqual(isSwitch, argument.IsSwitch);
            Assert.AreEqual(defaultValue, argument.DefaultValue);
            Assert.AreEqual(null, argument.Value);
            Assert.AreEqual(false, argument.HasValue);
        }

        private static void TestParse(CommandLineParser target, string commandLine, string arg1 = null, int arg2 = 42, bool notSwitch = false, string arg3 = null, int arg4 = 47, float arg5 = 0.0f, string arg6 = null, bool arg7 = false, DayOfWeek[] arg8 = null, int? arg9 = null, bool[] arg10 = null, bool? arg11 = null)
        {
            string[] args = commandLine.Split(' '); // not using quoted arguments in the tests, so this is fine.
            TestArguments result = (TestArguments)target.Parse(args);
            Assert.AreEqual(arg1, result.Arg1);
            Assert.AreEqual(arg2, result.Arg2);
            Assert.AreEqual(arg3, result.Arg3);
            Assert.AreEqual(arg4, result.Arg4);
            Assert.AreEqual(arg5, result.Arg5);
            Assert.AreEqual(arg6, result.Arg6);
            Assert.AreEqual(arg7, result.Arg7);
            CollectionAssert.AreEqual(arg8, result.Arg8);
            Assert.AreEqual(arg9, result.Arg9);
            CollectionAssert.AreEqual(arg10, result.Arg10);
            Assert.AreEqual(arg11, result.Arg11);

        }
    }
}