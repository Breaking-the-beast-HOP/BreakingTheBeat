using Frontend.Components.Pages.MusicCatalog.MusicReleases.Models;
using Frontend.Components.Pages.MusicCatalog.SharedModels;
using Frontend.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace Frontend.Components.Pages.MusicCatalog.MusicReleases.Edit;

public partial class EditRelease
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    protected HttpClient Http { get; set; } = default!;

    private ReleaseModel _model = new();

    private List<LanguageDto> _languages = new();
    private List<GenreDto> _genres = new();
    private List<TrackDto> _tracks = new();

    private GenreDto? _selectedGenre;
    private LanguageDto? _selectedLanguage;

    private List<ContributionEditor.ContributionRow> _contributions = new();

    private List<ContributionEditor.ContributorOption> _availableContributors = new();
    private List<ContributionEditor.ContributorRoleDto> _contributionRoles = new();

    private TrackDetailsDto? _selectedTrackPreview;
    private bool _isLoadingTrackPreview = false;
    private string? _trackPreviewError;

    private bool _isLoadingLookups = true;
    private bool _isSaving = false;
    private string? _errorMessage;
    private string? _successMessage;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _languages = await Http.GetFromJsonAsync<List<LanguageDto>>("api/Language") ?? new();
            _genres = await Http.GetFromJsonAsync<List<GenreDto>>("api/Genre") ?? new();
            _tracks = await Http.GetFromJsonAsync<List<TrackDto>>("api/Track") ?? new();

            _contributionRoles = await Http.GetFromJsonAsync<List<ContributionEditor.ContributorRoleDto>>("api/ContributorRole") ?? new();

            _availableContributors = await Http.GetFromJsonAsync<List<ContributionEditor.ContributorOption>>("api/Contributor") ?? new();

            var release = await Http.GetFromJsonAsync<ReleaseDetailsDto>($"api/Release/{Id}");

            if (release is null)
            {
                _errorMessage = "Release not found.";
                return;
            }

            MapReleaseToForm(release);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Could not load release: {ex.Message}";
        }
        finally
        {
            _isLoadingLookups = false;
        }
    }

    private void MapReleaseToForm(ReleaseDetailsDto release)
    {
        _model = new ReleaseModel
        {
            Id = release.Id,
            Title = release.Title,
            Version = release.Version,
            EAN = release.EAN,
            ReleaseDate = release.ReleaseDate,
            GenreId = release.GenreId,
            LanguageId = release.LanguageId,
            Status = release.status,
            Volumes = release.Volumes
                    .OrderBy(v => v.VolumeNumber)
                    .Select(v => new ReleaseVolumeModel
                    {
                        Id = v.Id,
                        VolumeNumber = v.VolumeNumber,
                        Tracks = v.Tracks
                            .OrderBy(t => t.TrackNumber)
                            .Select(t =>
                            {
                                var selectedTrack = _tracks.FirstOrDefault(x => x.Id == t.TrackId);

                                return new ReleaseTrackModel
                                {
                                    TrackNumber = t.TrackNumber,
                                    TrackId = t.TrackId,
                                    SelectedTrack = selectedTrack
                                };
                            })
                            .ToList()
                    })
                    .ToList()
                    
        };

        _selectedGenre = _genres.FirstOrDefault(g => g.Id == release.GenreId);
        _selectedLanguage = _languages.FirstOrDefault(l => l.Id == release.LanguageId);

        _contributions = release.Contributions
          .Select(c =>
          {
              var selectedContributor = _availableContributors
                  .FirstOrDefault(x => x.Id == c.ContributorId);

              return new ContributionEditor.ContributionRow
              {
                  ContributorId = c.ContributorId,

                  // brug det property-navn din ContributionRow har:
                  Contributor = selectedContributor,

                  Roles = c.Roles
                          .Select(r => _contributionRoles.FirstOrDefault(role => role.Id == r.Id))
                          .Where(r => r is not null)
                          .Select(r => r!)
                          .ToList()
              };
          })
          .ToList();

        if (!_model.Volumes.Any())
        {
            _model.Volumes.Add(new ReleaseVolumeModel
            {
                VolumeNumber = 1,
                Tracks = new()
                {
                    new ReleaseTrackModel { TrackNumber = 1 }
                }
            });
        }
    }

    private Task SaveDraft()
    {
        return SaveRelease(ReleaseStatusDto.Draft);
    }

    private Task SubmitToQc()
    {
        return SaveRelease(ReleaseStatusDto.ReadyForQc);
    }

    private async Task WithdrawFromQc()
    {
        _isSaving = true;
        _errorMessage = null;
        _successMessage = null;

        try
        {
            var response = await Http.PostAsync($"api/Release/{Id}/withdraw", null);

            if (response.IsSuccessStatusCode)
                _successMessage = "Release withdrawn from QC.";
            else
                _errorMessage = "Could not withdraw release.";
        }
        catch (Exception ex)
        {
            _errorMessage = $"An error occurred: {ex.Message}";
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task SaveRelease(ReleaseStatusDto status)
    {
        _errorMessage = null;
        _successMessage = null;

        if (string.IsNullOrWhiteSpace(_model.Title))
        {
            _errorMessage = "Title is required.";
            return;
        }

        if (_model.GenreId is null || _model.GenreId == Guid.Empty)
        {
            _errorMessage = "Genre is required.";
            return;
        }

        if (_model.LanguageId is null || _model.LanguageId == Guid.Empty)
        {
            _errorMessage = "Metadata language is required.";
            return;
        }

        if (status == ReleaseStatusDto.ReadyForQc)
            _model.Status = ReleaseStatusDto.ReadyForQc;
        else
            _model.Status = ReleaseStatusDto.Draft;

        if (!_model.Volumes.Any())
        {
            _errorMessage = "At least one volume is required.";
            return;
        }

        if (_model.Volumes.Any(v => !v.Tracks.Any()))
        {
            _errorMessage = "Each volume must contain at least one track.";
            return;
        }

        if (_model.Volumes.SelectMany(v => v.Tracks).Any(t => t.TrackId is null || t.TrackId == Guid.Empty))
        {
            _errorMessage = "All track rows must have a selected track.";
            return;
        }

        try
        {
            _isSaving = true;

            _model.Contributions = _contributions
                .Where(c => c.ContributorId.HasValue && c.Roles.Any())
                .Select(c => new ContributionRequest
                {
                    ContributorId = c.ContributorId!.Value,
                    RoleIds = c.Roles.Select(r => r.Id).ToList()
                })
                .ToList();

            var response = await Http.PutAsJsonAsync($"api/Release/{Id}", _model);

            if (response.IsSuccessStatusCode)
            {
                _successMessage = "Release updated successfully.";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                _errorMessage = string.IsNullOrWhiteSpace(error)
                    ? "Failed to update release."
                    : error;
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"An error occurred while saving: {ex.Message}";
        }
        finally
        {
            _isSaving = false;
        }
    }

    private Task<IEnumerable<GenreDto>> SearchGenres(string value, CancellationToken token)
    {
        var result = string.IsNullOrWhiteSpace(value)
            ? _genres
            : _genres.Where(g => g.Name.Contains(value, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(result.AsEnumerable());
    }

    private Task<IEnumerable<LanguageDto>> SearchLanguages(string value, CancellationToken token)
    {
        var result = string.IsNullOrWhiteSpace(value)
            ? _languages
            : _languages.Where(l =>
                l.Name.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                l.ISO.Contains(value, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(result.AsEnumerable());
    }

    private Task<IEnumerable<TrackDto>> SearchTracks(string value, CancellationToken token)
    {
        var result = string.IsNullOrWhiteSpace(value)
            ? _tracks
            : _tracks.Where(t =>
                t.Title.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                (!string.IsNullOrWhiteSpace(t.ISRC) &&
                 t.ISRC.Contains(value, StringComparison.OrdinalIgnoreCase)));

        return Task.FromResult(result.AsEnumerable());
    }

    private Task OnGenreChanged(GenreDto? genre)
    {
        _selectedGenre = genre;
        _model.GenreId = genre?.Id;
        return Task.CompletedTask;
    }

    private Task OnLanguageChanged(LanguageDto? language)
    {
        _selectedLanguage = language;
        _model.LanguageId = language?.Id;
        return Task.CompletedTask;
    }

    private Task OnTrackChanged(ReleaseTrackModel row, TrackDto? track)
    {
        row.SelectedTrack = track;
        row.TrackId = track?.Id;
        return Task.CompletedTask;
    }

    private async Task PreviewTrack(Guid? trackId)
    {
        if (trackId is null || trackId == Guid.Empty)
            return;

        _trackPreviewError = null;
        _selectedTrackPreview = null;
        _isLoadingTrackPreview = true;

        try
        {
            _selectedTrackPreview =
                await Http.GetFromJsonAsync<TrackDetailsDto>($"api/track/{trackId}");
        }
        catch (Exception ex)
        {
            _trackPreviewError = $"Could not fetch track details: {ex.Message}";
        }
        finally
        {
            _isLoadingTrackPreview = false;
        }
    }

    private void AddVolume()
    {
        _model.Volumes.Add(new ReleaseVolumeModel
        {
            VolumeNumber = _model.Volumes.Count + 1,
            Tracks = new()
            {
                new ReleaseTrackModel { TrackNumber = 1 }
            }
        });
    }

    private void RemoveVolume(ReleaseVolumeModel volume)
    {
        if (_model.Volumes.Count <= 1)
            return;

        _model.Volumes.Remove(volume);

        for (var i = 0; i < _model.Volumes.Count; i++)
            _model.Volumes[i].VolumeNumber = i + 1;
    }

    private void AddTrack(ReleaseVolumeModel volume)
    {
        volume.Tracks.Add(new ReleaseTrackModel
        {
            TrackNumber = volume.Tracks.Count + 1
        });
    }

    private void RemoveTrack(ReleaseVolumeModel volume, ReleaseTrackModel track)
    {
        if (volume.Tracks.Count <= 1)
            return;

        volume.Tracks.Remove(track);

        for (var i = 0; i < volume.Tracks.Count; i++)
            volume.Tracks[i].TrackNumber = i + 1;
    }

    private string FormatDuration(int? durationSeconds)
    {
        if (durationSeconds is null || durationSeconds <= 0)
            return "00:00";

        var ts = TimeSpan.FromSeconds(durationSeconds.Value);
        return ts.ToString(@"mm\:ss");
    }

    private string DisplayTrackSubtitle(TrackDto track)
    {
        if (!string.IsNullOrWhiteSpace(track.ISRC))
            return track.ISRC;

        if (!string.IsNullOrWhiteSpace(track.Version))
            return track.Version;

        return "No ISRC";
    }


}


