using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTransify.src.Models.Transfer
{
    public class TransferResult
    {
        public string? SourcePlaylistId { get; set; }
        public string? DestinationPlaylistId { get; set; }
        public List<string>? FailedTracks { get; set; }
        public int TotalTracks { get; set; }
        public int TransferredTracks { get; set; }
        public int FailedCount => FailedTracks?.Count ?? 0;
        public bool IsSuccessful => FailedCount == 0;
    }
}