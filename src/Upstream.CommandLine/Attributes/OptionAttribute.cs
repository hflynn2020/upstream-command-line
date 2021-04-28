﻿using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Reflection;

namespace Upstream.CommandLine
{
    public class OptionAttribute : CommandSymbolAttribute
    {
        private static readonly object _uninitializedDefaultValue = new();
        private object _defaultValue = _uninitializedDefaultValue;

        public OptionAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }

        public string[]? Aliases { get; set; }

        public SymbolType? Type { get; set; }

        public bool IsRequired { get; set; } = false;

        public object DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                SetDefaultValue = () => DefaultValue;
            }
        }

        public Func<object?>? SetDefaultValue { get; private set; }

        public bool HasDefaultValue => DefaultValue != _uninitializedDefaultValue;

        public override Symbol GetSymbol(PropertyInfo property)
        {
            var aliases = Aliases?.Length > 0 ? Aliases : new[] { property.Name.ToKebabCase() };

            var option = new Option(
                aliases,
                description: Description,
                getDefaultValue: SetDefaultValue,
                argumentType: property.PropertyType);

            if (IsRequired)
            {
                option.IsRequired = true;
            }

            return option;
        }
    }
}
