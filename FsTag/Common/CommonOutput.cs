﻿using System.Diagnostics.CodeAnalysis;

using FsTag.Filters;
using FsTag.Helpers;

namespace FsTag.Common;

public static class CommonOutput
{
    public static bool ErrorIfFilterNullAndNotAll([NotNullWhen(false)] PathFilter? filter)
    {
        if (filter == null)
        {
            WriteFormatter.Error("Expected a filter. Maybe you forgot -a?");

            return true;
        }

        return false;
    }

    public static void WarnXIgnoredBecauseYIsSpecified(object? x, bool y)
    {
        if (y)
        {
            WriteFormatter.Warning($"{nameof(x)} will be ignored because {nameof(y)} is specified.");
        }
    }
}