﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Glatzel.Algorithm;

public struct BoundingBox : IEquatable<BoundingBox>, IFormattable
{
    private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

    public BoundingBox()
    {
        MinPt = new Vec3(0, 0, 0);
        MaxPt = new Vec3(1, 1, 1);
    }

    public BoundingBox(double minPt, double maxPt)
    {
        MinPt = new Vec3(minPt);
        MaxPt = new Vec3(maxPt);
    }

    public BoundingBox(Vec3 minPt, Vec3 maxPt)
    {
        MinPt = minPt;
        MaxPt = maxPt;
    }

    public BoundingBox(double[] minPt, double[] maxPt)
    {
        MinPt = new Vec3(minPt);
        MaxPt = new Vec3(maxPt);
    }

    public Vec3 MaxPt { readonly get; set; }
    public Vec3 MinPt { readonly get; set; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Intersect(params BoundingBox[] bboxs)
    {
        Vec3 minPt = new();
        Vec3 maxPt = new();
        List<BoundingBox> listBBox = [.. bboxs];
        minPt.X = listBBox.Min(p => p.MaxPt.X);
        minPt.Y = listBBox.Min(p => p.MaxPt.Y);
        minPt.Z = listBBox.Min(p => p.MaxPt.Z);

        maxPt.X = listBBox.Max(p => p.MinPt.X);
        maxPt.Y = listBBox.Max(p => p.MinPt.Y);
        maxPt.Z = listBBox.Max(p => p.MinPt.Z);

        BoundingBox outbbox = new(maxPt, minPt);
        outbbox.Check();
        return outbbox;
    }

    //https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection#aabb_vs._aabb
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIntersect(BoundingBox bbox1, BoundingBox bbox2) =>
        bbox1.MinPt.X <= bbox2.MaxPt.X
        && bbox1.MaxPt.X >= bbox2.MinPt.X
        && bbox1.MinPt.Y <= bbox2.MaxPt.Y
        && bbox1.MaxPt.Y >= bbox2.MinPt.Y
        && bbox1.MinPt.Z <= bbox2.MaxPt.Z
        && bbox1.MaxPt.Z >= bbox2.MinPt.Z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Offset(BoundingBox bbox, double offset)
    {
        return Offset(bbox, new Vec3(-offset / 2.0), new Vec3(offset / 2.0));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Offset(BoundingBox bbox, Vec3 MinOffset, Vec3 MaxOffset)
    {
        var minPt = bbox.MinPt.Add(MinOffset);
        var maxPt = bbox.MaxPt.Add(MaxOffset);
        return new(minPt, maxPt);
    }

    public static bool operator !=(BoundingBox left, BoundingBox right)
    {
        return !(left == right);
    }

    public static bool operator ==(BoundingBox left, BoundingBox right)
    {
        return left.Equals(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Scale(BoundingBox bbox, double scale)
    {
        return Scale(bbox, new Vec3(scale), bbox.Center());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Scale(BoundingBox bbox, double scale, Vec3 origin)
    {
        return Scale(bbox, new Vec3(scale), origin);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Scale(BoundingBox bbox, Vec3 scale)
    {
        return Scale(bbox, scale, bbox.Center());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Scale(BoundingBox bbox, Vec3 scale, Vec3 origin)
    {
        scale.Subtract(1);
        var minPt = new Vec3(
            bbox.MinPt.X + ((bbox.MinPt.X - origin.X) * scale.X),
            bbox.MinPt.Y + ((bbox.MinPt.Y - origin.Y) * scale.Y),
            bbox.MinPt.Z + ((bbox.MinPt.Z - origin.Z) * scale.Z)
        );
        var maxPt = new Vec3(
            bbox.MaxPt.X + ((bbox.MaxPt.X - origin.X) * scale.X),
            bbox.MaxPt.Y + ((bbox.MaxPt.Y - origin.Y) * scale.Y),
            bbox.MaxPt.Z + ((bbox.MaxPt.Z - origin.Z) * scale.Z)
        );
        return new(minPt, maxPt);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Union(params BoundingBox[] bboxs)
    {
        Vec3 maxpt = new();
        Vec3 Minpt = new();
        List<BoundingBox> listBBox = [.. bboxs];
        maxpt.X = listBBox.Max(p => p.MaxPt.X);
        maxpt.Y = listBBox.Max(p => p.MaxPt.Y);
        maxpt.Z = listBBox.Max(p => p.MaxPt.Z);

        Minpt.X = listBBox.Min(p => p.MinPt.X);
        Minpt.Y = listBBox.Min(p => p.MinPt.Y);
        Minpt.Z = listBBox.Min(p => p.MinPt.Z);

        return new BoundingBox(Minpt, maxpt);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vec3 Center() => new(MidX(), MidY(), MidZ());

    public readonly void Check()
    {
        if (MaxPt.X < MinPt.X || MaxPt.Y < MinPt.Y || MaxPt.Z < MinPt.Z)
        {
            string msg =
                $"MaxPt({MaxPt.X}, {MaxPt.Y}, {MaxPt.Z})< MinPt({MinPt.X}, {MinPt.Y}, {MinPt.Z})";
            Log.Error(msg);
            throw new ArithmeticException(msg);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(BoundingBox other) => MinPt == other.MinPt && MaxPt == other.MaxPt;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly bool Equals(object obj) =>
        obj is BoundingBox boundingBox && Equals(boundingBox);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode() =>
        HashCode.Combine(MinPt.GetHashCode(), MaxPt.GetHashCode());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double LengthX() => MaxPt.X - MinPt.X;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double LengthY() => MaxPt.Y - MinPt.Y;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double LengthZ() => MaxPt.Z - MinPt.Z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Axis MaxAxis()
    {
        if (LengthX() >= LengthY() && LengthX() >= LengthZ())
            return Axis.X;
        else if (LengthY() >= LengthZ())
            return Axis.Y;
        else
            return Axis.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Axis MinAxis()
    {
        if (LengthX() <= LengthY() && LengthX() <= LengthZ())
            return Axis.X;
        else if (LengthY() <= LengthZ())
            return Axis.Y;
        else
            return Axis.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double MidX() => (MaxPt.X + MinPt.X) / 2.0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double MidY() => (MaxPt.Y + MinPt.Y) / 2.0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double MidZ() => (MaxPt.Z + MinPt.Z) / 2.0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundingBox Offset(double offset)
    {
        Offset(new Vec3(-offset / 2.0), new Vec3(offset / 2.0));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundingBox Offset(Vec3 MinOffset, Vec3 MaxOffset)
    {
        MinPt = MinPt.Add(MinOffset);
        MaxPt = MaxPt.Add(MaxOffset);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundingBox Scale(double scale)
    {
        Scale(new Vec3(scale), Center());
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundingBox Scale(double scale, Vec3 origin)
    {
        Scale(new Vec3(scale), origin);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundingBox Scale(Vec3 scale)
    {
        Scale(scale, Center());
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BoundingBox Scale(Vec3 scale, Vec3 origin)
    {
        scale.Subtract(1);
        MinPt = new Vec3(
            MinPt.X + ((MinPt.X - origin.X) * scale.X),
            MinPt.Y + ((MinPt.Y - origin.Y) * scale.Y),
            MinPt.Z + ((MinPt.Z - origin.Z) * scale.Z)
        );
        MaxPt = new Vec3(
            MaxPt.X + ((MaxPt.X - origin.X) * scale.X),
            MaxPt.Y + ((MaxPt.Y - origin.Y) * scale.Y),
            MaxPt.Z + ((MaxPt.Z - origin.Z) * scale.Z)
        );
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double Volume() => LengthX() * LengthY() * LengthZ();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly string ToString(string format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        return $"BoundingBox(Min: {MinPt.ToString(format, formatProvider)}, Max: {MaxPt.ToString(format, formatProvider)})";
    }
}
