﻿using Sewer56.StructuredDiff.Interfaces;

namespace Persona.Merger.Patching.Tbl.FieldResolvers;

public struct IntResolver : IEncoderFieldResolver
{
    public bool Resolve(nuint offset, out int moveBy, out int length)
    {
        // All data are u32s.
        var fourByteAligned = offset / 4 * 4;
        moveBy = (int)(offset - fourByteAligned);
        length = 4;
        return true;
    }
}