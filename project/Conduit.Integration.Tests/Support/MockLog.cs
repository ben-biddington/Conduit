﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Conduit.Integration.Tests.Support
{
    class MockLog
    {
        private readonly List<string> _messages = new List<string>();

        internal Action<string> Fun()
        {
            return m => _messages.Add(m);
        }

        internal void MustHaveMessageLike(string expected)
        {
            Assert.True(_messages.Any(it => Regex.IsMatch(it, expected)), 
                $"Expected message <{expected}>, got:{Environment.NewLine}{Environment.NewLine}{string.Join(Environment.NewLine, _messages.ToArray())}");
        }
    }
}