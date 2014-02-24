﻿using System;
using System.Linq.Expressions;
using SwissKnife;

namespace TinyDdd.FluentValidation
{
    internal static class MessageKeyBuilder
    {
        private static readonly Identifier.ConversionOptions _conversionOptions = new Identifier.ConversionOptions
        {
            StaticMemberConversion = Identifier.StaticMemberConversion.ParentTypeName
        };

        internal static string From(Expression<Func<string>> resourceSelector)
        {
            System.Diagnostics.Debug.Assert(resourceSelector != null);

            return Identifier.ToString(resourceSelector, _conversionOptions);
        }
    }
}
