using FrontendThrowAway.Components.Pages.MusicCatalog.SharedModels;
using FrontendThrowAway.Components.Pages.MusicCatalog.Tracks.Models;
using FrontendThrowAway.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace FrontendThrowAway.Components.Pages.MusicCatalog.Tracks.Create
{
    public partial class CreateTrack
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;
        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        private TrackModel _model = new();

        private List<LanguageDto> _languages = new();
        private List<GenreDto> _genres = new();

        private GenreDto? _selectedGenre;
        private LanguageDto? _selectedMetadataLanguage;
        private LanguageDto? _selectedAudioLanguage;

        private List<ContributionEditor.ContributionRow> _contributions = new();

        private List<ContributionEditor.ContributorOption> _availableContributors = new();
        private List<ContributionEditor.ContributorRoleDto> _contributionRoles = new();

        private bool _isLoadingLookups = true;
        private bool _isSaving = false;
        private string? _errorMessage;
        private bool _showSuccessDialog;
        private Guid? _createdTrackId;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _languages = await Http.GetFromJsonAsync<List<LanguageDto>>("api/Language") ?? new();
                _genres = await Http.GetFromJsonAsync<List<GenreDto>>("api/Genre") ?? new();

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

        private Task OnGenreChanged(GenreDto? genre)
        {
            _selectedGenre = genre;
            _model.GenreId = genre?.Id;
            return Task.CompletedTask;
        }

        private Task OnMetadataLanguageChanged(LanguageDto? language)
        {
            _selectedMetadataLanguage = language;
            _model.MetadataLanguageId = language?.Id;
            return Task.CompletedTask;
        }

        private Task OnAudioLanguageChanged(LanguageDto? language)
        {
            _selectedAudioLanguage = language;
            _model.AudioLanguageId = language?.Id;
            return Task.CompletedTask;
        }

        private async Task SaveTrack()
        {
            _errorMessage = null;


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

            if (_model.MetadataLanguageId is null || _model.MetadataLanguageId == Guid.Empty)
            {
                _errorMessage = "Metadata language is required.";
                return;
            }

            if (_model.AudioLanguageId is null || _model.AudioLanguageId == Guid.Empty)
            {
                _errorMessage = "Audio language is required.";
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

                var response = await Http.PostAsJsonAsync("api/Track", _model);

                if (response.IsSuccessStatusCode)
                {
                    _createdTrackId = await response.Content.ReadFromJsonAsync<Guid>();
                    _showSuccessDialog = true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _errorMessage = string.IsNullOrWhiteSpace(error)
                        ? "Failed to save track."
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

        private void GoToCreatedTrack()
        {
            if (_createdTrackId is null)
                return;

            Navigation.NavigateTo($"/tracks/{_createdTrackId}");
        }

        private void GoToTracks()
        {
            Navigation.NavigateTo("/tracks");
        }

        private void CreateAnother()
        {
            ResetForm();

            _createdTrackId = null;
            _showSuccessDialog = false;
            _errorMessage = null;
        }
        private void ResetForm()
        {
            _model = new TrackModel();

            _selectedGenre = null;
            _selectedMetadataLanguage = null;
            _selectedAudioLanguage = null;

            _contributions.Clear();
        }

    }
}
