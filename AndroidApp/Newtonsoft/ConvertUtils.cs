﻿#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
#if !(NET20 || NET35 || SILVERLIGHT || PORTABLE40 || PORTABLE)
using System.Numerics;
#endif
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal enum PrimitiveTypeCode
  {
    Empty,
    Object,
    Char,
    CharNullable,
    Boolean,
    BooleanNullable,
    SByte,
    SByteNullable,
    Int16,
    Int16Nullable,
    UInt16,
    UInt16Nullable,
    Int32,
    Int32Nullable,
    Byte,
    ByteNullable,
    UInt32,
    UInt32Nullable,
    Int64,
    Int64Nullable,
    UInt64,
    UInt64Nullable,
    Single,
    SingleNullable,
    Double,
    DoubleNullable,
    DateTime,
    DateTimeNullable,
#if !NET20
    DateTimeOffset,
    DateTimeOffsetNullable,
#endif
    Decimal,
    DecimalNullable,
    Guid,
    GuidNullable,
    TimeSpan,
    TimeSpanNullable,
#if !(PORTABLE || NET35 || NET20 || WINDOWS_PHONE || SILVERLIGHT)
    BigInteger,
    BigIntegerNullable,
#endif
    Uri,
    String,
    Bytes,
    DBNull
  }

  internal class TypeInformation
  {
    public Type Type { get; set; }
    public PrimitiveTypeCode TypeCode { get; set; }
  }

  internal enum ParseResult
  {
    None,
    Success,
    Overflow,
    Invalid
  }

  internal static class ConvertUtils
  {
    private static readonly Dictionary<Type, PrimitiveTypeCode> TypeCodeMap =
      new Dictionary<Type, PrimitiveTypeCode>
        {
          { typeof(char), PrimitiveTypeCode.Char },
          { typeof(char?), PrimitiveTypeCode.CharNullable },
          { typeof(bool), PrimitiveTypeCode.Boolean },
          { typeof(bool?), PrimitiveTypeCode.BooleanNullable },
          { typeof(sbyte), PrimitiveTypeCode.SByte },
          { typeof(sbyte?), PrimitiveTypeCode.SByteNullable },
          { typeof(short), PrimitiveTypeCode.Int16 },
          { typeof(short?), PrimitiveTypeCode.Int16Nullable },
          { typeof(ushort), PrimitiveTypeCode.UInt16 },
          { typeof(ushort?), PrimitiveTypeCode.UInt16Nullable },
          { typeof(int), PrimitiveTypeCode.Int32 },
          { typeof(int?), PrimitiveTypeCode.Int32Nullable },
          { typeof(byte), PrimitiveTypeCode.Byte },
          { typeof(byte?), PrimitiveTypeCode.ByteNullable },
          { typeof(uint), PrimitiveTypeCode.UInt32 },
          { typeof(uint?), PrimitiveTypeCode.UInt32Nullable },
          { typeof(long), PrimitiveTypeCode.Int64 },
          { typeof(long?), PrimitiveTypeCode.Int64Nullable },
          { typeof(ulong), PrimitiveTypeCode.UInt64 },
          { typeof(ulong?), PrimitiveTypeCode.UInt64Nullable },
          { typeof(float), PrimitiveTypeCode.Single },
          { typeof(float?), PrimitiveTypeCode.SingleNullable },
          { typeof(double), PrimitiveTypeCode.Double },
          { typeof(double?), PrimitiveTypeCode.DoubleNullable },
          { typeof(DateTime), PrimitiveTypeCode.DateTime },
          { typeof(DateTime?), PrimitiveTypeCode.DateTimeNullable },
#if !NET20
          { typeof(DateTimeOffset), PrimitiveTypeCode.DateTimeOffset },
          { typeof(DateTimeOffset?), PrimitiveTypeCode.DateTimeOffsetNullable },
#endif
          { typeof(decimal), PrimitiveTypeCode.Decimal },
          { typeof(decimal?), PrimitiveTypeCode.DecimalNullable },
          { typeof(Guid), PrimitiveTypeCode.Guid },
          { typeof(Guid?), PrimitiveTypeCode.GuidNullable },
          { typeof(TimeSpan), PrimitiveTypeCode.TimeSpan },
          { typeof(TimeSpan?), PrimitiveTypeCode.TimeSpanNullable },
#if !(PORTABLE || PORTABLE40 || NET35 || NET20 || WINDOWS_PHONE || SILVERLIGHT)
          { typeof(BigInteger), PrimitiveTypeCode.BigInteger },
          { typeof(BigInteger?), PrimitiveTypeCode.BigIntegerNullable },
#endif
          { typeof(Uri), PrimitiveTypeCode.Uri },
          { typeof(string), PrimitiveTypeCode.String },
          { typeof(byte[]), PrimitiveTypeCode.Bytes },
#if !(PORTABLE || PORTABLE40 || NETFX_CORE)
          { typeof(DBNull), PrimitiveTypeCode.DBNull }
#endif
        };

    private static readonly List<TypeInformation> PrimitiveTypeCodes = new List<TypeInformation>
      {
        new TypeInformation { Type = typeof(object), TypeCode = PrimitiveTypeCode.Empty },
        new TypeInformation { Type = typeof(object), TypeCode = PrimitiveTypeCode.Object },
        new TypeInformation { Type = typeof(object), TypeCode = PrimitiveTypeCode.DBNull },
        new TypeInformation { Type = typeof(bool), TypeCode = PrimitiveTypeCode.Boolean },
        new TypeInformation { Type = typeof(char), TypeCode = PrimitiveTypeCode.Char },
        new TypeInformation { Type = typeof(sbyte), TypeCode = PrimitiveTypeCode.SByte },
        new TypeInformation { Type = typeof(byte), TypeCode = PrimitiveTypeCode.Byte },
        new TypeInformation { Type = typeof(short), TypeCode = PrimitiveTypeCode.Int16 },
        new TypeInformation { Type = typeof(ushort), TypeCode = PrimitiveTypeCode.UInt16 },
        new TypeInformation { Type = typeof(int), TypeCode = PrimitiveTypeCode.Int32 },
        new TypeInformation { Type = typeof(uint), TypeCode = PrimitiveTypeCode.UInt32 },
        new TypeInformation { Type = typeof(long), TypeCode = PrimitiveTypeCode.Int64 },
        new TypeInformation { Type = typeof(ulong), TypeCode = PrimitiveTypeCode.UInt64 },
        new TypeInformation { Type = typeof(float), TypeCode = PrimitiveTypeCode.Single },
        new TypeInformation { Type = typeof(double), TypeCode = PrimitiveTypeCode.Double },
        new TypeInformation { Type = typeof(decimal), TypeCode = PrimitiveTypeCode.Decimal },
        new TypeInformation { Type = typeof(DateTime), TypeCode = PrimitiveTypeCode.DateTime },
        new TypeInformation { Type = typeof(object), TypeCode = PrimitiveTypeCode.Empty }, // no 17 in TypeCode for some reason
        new TypeInformation { Type = typeof(string), TypeCode = PrimitiveTypeCode.String }
      };

    public static PrimitiveTypeCode GetTypeCode(Type t)
    {
      PrimitiveTypeCode typeCode;
      if (TypeCodeMap.TryGetValue(t, out typeCode))
        return typeCode;

      if (t.IsEnum)
        return GetTypeCode(Enum.GetUnderlyingType(t));

      // performance?
      if (ReflectionUtils.IsNullableType(t))
      {
        Type nonNullable = Nullable.GetUnderlyingType(t);
        if (nonNullable.IsEnum)
        {
          Type nullableUnderlyingType = typeof(Nullable<>).MakeGenericType(Enum.GetUnderlyingType(nonNullable));
          return GetTypeCode(nullableUnderlyingType);
        }
      }

      return PrimitiveTypeCode.Object;
    }

    public static PrimitiveTypeCode GetTypeCode(object o)
    {
      return GetTypeCode(o.GetType());
    }

#if !(NETFX_CORE || PORTABLE)
    public static TypeInformation GetTypeInformation(IConvertible convertable)
    {
      TypeInformation typeInformation = PrimitiveTypeCodes[(int)convertable.GetTypeCode()];
      return typeInformation;
    }
#endif

    public static bool IsConvertible(Type t)
    {
#if !(NETFX_CORE || PORTABLE)
      return typeof(IConvertible).IsAssignableFrom(t);
#else
      return (
        t == typeof(bool) || t == typeof(byte) || t == typeof(char) || t == typeof(DateTime) || t == typeof(decimal) || t == typeof(double) || t == typeof(short) || t == typeof(int) ||
        t == typeof(long) || t == typeof(sbyte) || t == typeof(float) || t == typeof(string) || t == typeof(ushort) || t == typeof(uint) || t == typeof(ulong) || t.IsEnum());
#endif
    }

    public static TimeSpan ParseTimeSpan(string input)
    {
#if !(NET35 || NET20 || PORTABLE40)
      return TimeSpan.Parse((string)input, CultureInfo.InvariantCulture);
#else
      return TimeSpan.Parse((string)input);
#endif
    }

    internal struct TypeConvertKey : IEquatable<TypeConvertKey>
    {
      private readonly Type _initialType;
      private readonly Type _targetType;

      public Type InitialType
      {
        get { return _initialType; }
      }

      public Type TargetType
      {
        get { return _targetType; }
      }

      public TypeConvertKey(Type initialType, Type targetType)
      {
        _initialType = initialType;
        _targetType = targetType;
      }

      public override int GetHashCode()
      {
        return _initialType.GetHashCode() ^ _targetType.GetHashCode();
      }

      public override bool Equals(object obj)
      {
        if (!(obj is TypeConvertKey))
          return false;

        return Equals((TypeConvertKey)obj);
      }

      public bool Equals(TypeConvertKey other)
      {
        return (_initialType == other._initialType && _targetType == other._targetType);
      }
    }

#if !(NET20 || NET35 || SILVERLIGHT || PORTABLE || PORTABLE40)
    internal static BigInteger ToBigInteger(object value)
    {
      if (value is BigInteger)
        return (BigInteger)value;
      if (value is string)
        return BigInteger.Parse((string)value);
      if (value is float)
        return new BigInteger((float)value);
      if (value is double)
        return new BigInteger((double)value);
      if (value is decimal)
        return new BigInteger((decimal)value);
      if (value is int)
        return new BigInteger((int)value);
      if (value is long)
        return new BigInteger((long)value);
      if (value is uint)
        return new BigInteger((uint)value);
      if (value is ulong)
        return new BigInteger((ulong)value);
      if (value is byte[])
        return new BigInteger((byte[])value);

      throw new InvalidCastException("Cannot convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
    }

    public static object FromBigInteger(BigInteger i, Type targetType)
    {
      if (targetType == typeof(decimal))
        return (decimal)i;
      if (targetType == typeof(double))
        return (double)i;
      if (targetType == typeof(float))
        return (float)i;
      if (targetType == typeof(ulong))
        return (ulong)i;

      try
      {
        return System.Convert.ChangeType((long)i, targetType, CultureInfo.InvariantCulture);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Can not convert from BigInteger to {0}.".FormatWith(CultureInfo.InvariantCulture, targetType), ex);
      }
    }
#endif


    public static bool IsInteger(object value)
    {
      switch (GetTypeCode(value))
      {
        case PrimitiveTypeCode.SByte:
        case PrimitiveTypeCode.Byte:
        case PrimitiveTypeCode.Int16:
        case PrimitiveTypeCode.UInt16:
        case PrimitiveTypeCode.Int32:
        case PrimitiveTypeCode.UInt32:
        case PrimitiveTypeCode.Int64:
        case PrimitiveTypeCode.UInt64:
          return true;
        default:
          return false;
      }
    }

    public static int Int32Parse(char[] chars, int start, int length)
    {
      if (length == 0)
        throw new FormatException("Input string was not in a correct format.");

      bool isNegative = (chars[start] == '-');

      if (isNegative)
      {
        // text just a negative sign
        if (length == 1)
          throw new FormatException("Input string was not in a correct format.");

        start++;
        length--;
      }

      int result = 0;
      int end = start + length;

      for (int i = start; i < end; i++)
      {
        int c = chars[i] - '0';

        if (c < 0 || c > 9)
          throw new FormatException("Input string was not in a correct format.");

        int newValue = (10*result) - c;

        // overflow has caused the number to loop around
        if (newValue > result)
          throw new OverflowException();

        result = newValue;
      }

      // go from negative to positive to avoids overflow
      // negative can be slightly bigger than positive
      if (!isNegative)
      {
        // negative integer can be one bigger than positive
        if (result == int.MinValue)
          throw new OverflowException();

        result = -result;
      }

      return result;
    }

    public static ParseResult Int64TryParse(char[] chars, int start, int length, out long value)
    {
      value = 0;

      if (length == 0)
        return ParseResult.Invalid;

      bool isNegative = (chars[start] == '-');

      if (isNegative)
      {
        // text just a negative sign
        if (length == 1)
          return ParseResult.Invalid;

        start++;
        length--;
      }

      int end = start + length;

      for (int i = start; i < end; i++)
      {
        int c = chars[i] - '0';

        if (c < 0 || c > 9)
          return ParseResult.Invalid;

        long newValue = (10*value) - c;

        // overflow has caused the number to loop around
        if (newValue > value)
          return ParseResult.Overflow;

        value = newValue;
      }

      // go from negative to positive to avoids overflow
      // negative can be slightly bigger than positive
      if (!isNegative)
      {
        // negative integer can be one bigger than positive
        if (value == long.MinValue)
          return ParseResult.Overflow;

        value = -value;
      }

      return ParseResult.Success;
    }
  }
}