using FrontendThrowAway.Components.Shared;
using FrontendThrowAway.Components.Pages.MusicCatalog.MusicReleases.Models;
using FrontendThrowAway.Components.Pages.MusicCatalog.SharedModels;
using Microsoft.AspNetCore.Components;

namespace FrontendThrowAway.Components.Pages.MusicCatalog.MusicReleases.Create
{
    public partial class CreateRelease
    {

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
            _model.Volumes.Add(new ReleaseVolumeModel
            {
                VolumeNumber = 1,
                Tracks = new()
        {
            new ReleaseTrackModel { TrackNumber = 1 }
        }
            });

            try
            {
                _languages = await Http.GetFromJsonAsync<List<LanguageDto>>("api/Language") ?? new();
                _genres = await Http.GetFromJsonAsync<List<GenreDto>>("api/Genre") ?? new();
                _tracks = await Http.GetFromJsonAsync<List<TrackDto>>("api/Track") ?? new();

                try
                {
                    _contributionRoles =
                        await Http.GetFromJsonAsync<List<ContributionEditor.ContributorRoleDto>>("api/ContributorRole") ?? new();
                }
                catch (Exception ex)
                {
                    _errorMessage = $"Could not load contributor roles: {ex.Message}";
                }

                try
                {
                    _availableContributors =
                        await Http.GetFromJsonAsync<List<ContributionEditor.ContributorOption>>("api/Contributor") ?? new();
                }
                catch (Exception ex)
                {
                    _errorMessage = $"Could not load contributors: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                _errorMessage = $"Could not load lookup data: {ex.Message}";
            }
            finally
            {
                _isLoadingLookups = false;
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

        private async Task SaveRelease()
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

                var response = await Http.PostAsJsonAsync("api/Release", _model);

                if (response.IsSuccessStatusCode)
                {
                    _successMessage = "Release created successfully.";

                    ResetForm();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _errorMessage = string.IsNullOrWhiteSpace(error)
                        ? "Failed to save release."
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

        private void ResetForm()
        {
            _model = new ReleaseModel();
            _model.Volumes.Add(new ReleaseVolumeModel
            {
                VolumeNumber = 1,
                Tracks = new()
        {
            new ReleaseTrackModel { TrackNumber = 1 }
        }
            });

            _contributions.Clear();

            _selectedGenre = null;
            _selectedLanguage = null;
            _selectedTrackPreview = null;
            _trackPreviewError = null;
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
}


