using System;

namespace MusicTransify.src.Contracts.Mapper
{
    public interface IMappable
    {
        Dictionary<string, string> ToMap();
    }
}