// <copyright file="PlatformBaseExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Reflection;

namespace DrasticOverlay
{
    /// <summary>
    /// Platform Base Extensions.
    /// </summary>
    public static class PlatformBaseExtensions
    {
        public static Dictionary<string, string?> GetFieldValues(this object obj)
            => obj.GetType().GetFieldValues();

        public static Dictionary<string, string?> GetFieldValues(this Type type)
        {
            return type
                      .GetFields(BindingFlags.Public | BindingFlags.Static)
                      .Where(f => f.FieldType == typeof(string))
                      .ToDictionary(
                          f => f.Name,
                          f => f.GetValue(null) as string);
        }
    }
}