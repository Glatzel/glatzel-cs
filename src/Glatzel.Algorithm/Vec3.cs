﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Glatzel.Algorithm;

public struct Vec3 : IEquatable<Vec3>, IFormattable
{
    private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

    public Vec3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public Vec3(double scalar)
    {
        X = scalar;
        Y = scalar;
        Z = scalar;
    }

    public Vec3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vec3(double[] v)
    {
        if (v.Length != 3)
        {
            string msg = $"Lenth of input is not 3, get {v.Length}.";
            Log.Error(msg);
            throw new ArgumentException(msg);
        }

        X = v[0];
        Y = v[1];
        Z = v[2];
    }

    public static Vec3 One => new(1, 1, 1);
    public static Vec3 UnitX => new(1, 0, 0);
    public static Vec3 UnitY => new(0, 1, 0);
    public static Vec3 UnitZ => new(0, 0, 1);
    public static Vec3 Zero => new(0, 0, 0);
    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }

    public double this[int idx]
    {
        readonly get
        {
            if (idx == 0)
            {
                return X;
            }
            else if (idx == 1)
            {
                return Y;
            }
            else if (idx == 2)
            {
                return Z;
            }
            else
            {
                throw new ArgumentException($"Unknown index: {idx}");
            }
        }
        set
        {
            if (idx == 0)
            {
                X = value;
            }
            else if (idx == 1)
            {
                Y = value;
            }
            else if (idx == 2)
            {
                Z = value;
            }
            else
            {
                throw new ArgumentException($"Unknown index: {idx}");
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Add(Vec3 v1, Vec3 v2)
    {
        return new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Add(Vec3 v1, double t)
    {
        return new(v1.X + t, v1.Y + t, v1.Z + t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Add(Vec3 u, double t, ref Vec3 outv)
    {
        outv.X = u.X + t;
        outv.Y = u.Y + t;
        outv.Z = u.Z + t;
        return outv;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Add(Vec3 u, Vec3 v, ref Vec3 outv)
    {
        outv.X = u.X + v.X;
        outv.Y = u.Y + v.Y;
        outv.Z = u.Z + v.Z;
        return outv;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Cross(Vec3 u, Vec3 v)
    {
        return new Vec3(
            (u.Y * v.Z) - (u.Z * v.Y),
            (u.Z * v.X) - (u.X * v.Z),
            (u.X * v.Y) - (u.Y * v.X)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Divide(Vec3 v, double t, ref Vec3 outv)
    {
        outv.X = v.X / t;
        outv.Y = v.Y / t;
        outv.Z = v.Z / t;
        return outv;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Divide(Vec3 v, double t)
    {
        return new(v.X / t, v.Y / t, v.Z / t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Divide(Vec3 v, Vec3 t, ref Vec3 outv)
    {
        outv.X = v.X / t.X;
        outv.Y = v.Y / t.Y;
        outv.Z = v.Z / t.Z;
        return outv;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Divide(Vec3 v, Vec3 t)
    {
        return new(v.X / t.X, v.Y / t.Y, v.Z / t.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Dot(Vec3 u, Vec3 v)
    {
        return (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Length(Vec3 v)
    {
        return Math.Sqrt((v.X * v.X) + (v.Y * v.Y) + (v.Z * v.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Length2(Vec3 v)
    {
        return (v.X * v.X) + (v.Y * v.Y) + (v.Z * v.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Multiply(Vec3 v, double t, ref Vec3 outv)
    {
        outv.X = v.X * t;
        outv.Y = v.Y * t;
        outv.Z = v.Z * t;
        return outv;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Multiply(Vec3 v, double t)
    {
        return new(v.X * t, v.Y * t, v.Z * t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Multiply(Vec3 u, Vec3 v, ref Vec3 outv)
    {
        outv.X = u.X * v.X;
        outv.Y = u.Y * v.Y;
        outv.Z = u.Z * v.Z;
        return outv;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Multiply(Vec3 u, Vec3 v)
    {
        return new(u.X * v.X, u.Y * v.Y, u.Z * v.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Normalize(Vec3 v)
    {
        double length = v.Length();
        return new Vec3(v.X / length, v.Y / length, v.Z / length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator -(Vec3 v) => new(-v.X, -v.Y, -v.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator -(Vec3 left, Vec3 right) =>
        new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator -(double left, Vec3 right) =>
        new(left - right.X, left - right.Y, left - right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator -(Vec3 left, double right) =>
        new(left.X - right, left.Y - right, left.Z - right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vec3 left, Vec3 right) => !(left == right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(Vec3 left, Vec3 right) =>
        new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(double left, Vec3 right) =>
        new(left * right.X, left * right.Y, left * right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(Vec3 left, double right) =>
        new(left.X * right, left.Y * right, left.Z * right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator /(Vec3 left, Vec3 right) =>
        new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator /(double left, Vec3 right) =>
        new(left / right.X, left / right.Y, left / right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator /(Vec3 left, double right) =>
        new(left.X / right, left.Y / right, left.Z / right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator +(Vec3 left, Vec3 right) =>
        new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator +(double left, Vec3 right) =>
        new(left + right.X, left + right.Y, left + right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator +(Vec3 left, double right) =>
        new(left.X + right, left.Y + right, left.Z + right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vec3 left, Vec3 right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Subtract(Vec3 v1, Vec3 v2)
    {
        return new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Subtract(Vec3 v1, double t)
    {
        return new(v1.X - t, v1.Y - t, v1.Z - t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Subtract(Vec3 u, double t, ref Vec3 outv)
    {
        outv.X = u.X - t;
        outv.Y = u.Y - t;
        outv.Z = u.Z - t;
        return outv;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Subtract(Vec3 u, Vec3 v, ref Vec3 outv)
    {
        outv.X = u.X - v.X;
        outv.Y = u.Y - v.Y;
        outv.Z = u.Z - v.Z;
        return outv;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3 Add(double v)
    {
        X += v;
        Y += v;
        Z += v;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3 Add(Vec3 v)
    {
        X += v.X;
        Y += v.Y;
        Z += v.Z;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3 Divide(double t)
    {
        X /= t;
        Y /= t;
        Z /= t;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3 Divide(Vec3 v)
    {
        X /= v.X;
        Y /= v.Y;
        Z /= v.Z;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Vec3 other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly bool Equals(object obj)
    {
        return obj is Vec3 vec3 && Equals(vec3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double Length()
    {
        return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double Length2()
    {
        return (X * X) + (Y * Y) + (Z * Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3 Multiply(double t)
    {
        X *= t;
        Y *= t;
        Z *= t;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3 Multiply(Vec3 v)
    {
        X *= v.X;
        Y *= v.Y;
        Z *= v.Z;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3 Normalize()
    {
        double length = Length();
        X /= length;
        Y /= length;
        Z /= length;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3 Subtract(double v)
    {
        X -= v;
        Y -= v;
        Z -= v;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3 Subtract(Vec3 v)
    {
        X -= v.X;
        Y -= v.Y;
        Z -= v.Z;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => ToString("G", CultureInfo.CurrentCulture);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format, IFormatProvider formatProvider)
    {
        string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return $"Vec3<{X.ToString(format, formatProvider)}{separator} {Y.ToString(format, formatProvider)}{separator} {Z.ToString(format, formatProvider)}>";
    }
}
