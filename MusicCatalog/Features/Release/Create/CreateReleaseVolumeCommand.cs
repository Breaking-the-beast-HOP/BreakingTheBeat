using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalog.Features.Release.Create
{
    public record CreateReleaseVolumeCommand(int VolumeNumber, List<CreateReleaseTrackCommand> Tracks);
}
