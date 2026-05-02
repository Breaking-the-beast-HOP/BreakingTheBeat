using FrontendThrowAway.Components.Component;
using FrontendThrowAway.Models.Common;
using FrontendThrowAway.Models.Tracks.EditTrack;
using Microsoft.AspNetCore.Components;

namespace FrontendThrowAway.Pages;

public partial class EditTrack
{
    [Parameter]
    public Guid Id { get; set; }

    // ----------------------------
    // STATE
    // ----------------------------

    private TrackModel _model = new();

    // Lookup data (dropdowns/autocomplete)
    private List<LanguageDto> _languages = new();
    private List<GenreDto> _genres = new();

    // Contribution editor
    private List<ContributionEditor.ContributionRow> _contributions = new();
    private List<ContributionEditor.ContributorOption> _availableContributors = new();
    private List<ContributionEditor.ContributorRoleDto> _contributionRoles = new();

    // Selected values (bound to UI)
    private GenreDto? _selectedGenre;
    private LanguageDto? _selectedMetadataLanguage;
    private LanguageDto? _selectedAudioLanguage;

    // UI state
    private bool _isLoading = true;
    private bool _isSaving;

    private string? _errorMessage;
    private string? _successMessage;

    // ----------------------------
    // LIFECYCLE
    // ----------------------------

    protected override async Task OnInitializedAsync()
    {
        try
        {
            // 1. Load dropdown/lookup data first
            await LoadLookupsAsync();

            // 2. Load track
            var track = await Http.GetFromJsonAsync<TrackEditDto>($"api/track/{Id}");

            if (track is null)
            {
                _errorMessage = "Track not found.";
                return;
            }

            // 3. Map data → UI model
            SetModel(track);

            // 4. Set selected dropdown values
            SetSelectedLookups(track);

            // 5. Map contributions
            SetContributions(track);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Could not load track: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    // ----------------------------
    // DATA LOADING
    // ----------------------------

    private async Task LoadLookupsAsync()
    {
        // NOTE:
        // Hvis disse lister er tomme, vil dine autocompletes ALDRIG vise noget
        _languages = await Http.GetFromJsonAsync<List<LanguageDto>>("api/Language") ?? new();
        _genres = await Http.GetFromJsonAsync<List<GenreDto>>("api/Genre") ?? new();

        _contributionRoles =
            await Http.GetFromJsonAsync<List<ContributionEditor.ContributorRoleDto>>("api/ContributorRole")
            ?? new();

        _availableContributors =
            await Http.GetFromJsonAsync<List<ContributionEditor.ContributorOption>>("api/Contributor")
            ?? new();
    }

    // ----------------------------
    // MAPPING
    // ----------------------------

    private void SetModel(TrackEditDto track)
    {
        // Domain → UI model
        _model = new TrackModel
        {
            Title = track.Title,
            Version = track.Version,
            ISRC = track.ISRC,
            GenreId = track.GenreId,
            MetadataLanguageId = track.MetadataLanguageId,
            AudioLanguageId = track.AudioLanguageId
        };
    }

    private void SetSelectedLookups(TrackEditDto track)
    {
        // IMPORTANT:
        // Hvis disse bliver null → så vises der INTET i UI
        _selectedGenre = _genres.FirstOrDefault(g => g.Id == track.GenreId);
        _selectedMetadataLanguage = _languages.FirstOrDefault(l => l.Id == track.MetadataLanguageId);
        _selectedAudioLanguage = _languages.FirstOrDefault(l => l.Id == track.AudioLanguageId);
    }

    private void SetContributions(TrackEditDto track)
    {
        _contributions = track.Contributions
            .Select(c => new ContributionEditor.ContributionRow
            {
                ContributorId = c.ContributorId,

                // Finder contributor fra lookup-listen
                Contributor = _availableContributors
                    .FirstOrDefault(x => x.Id == c.ContributorId),

                // Mapper roller
                Roles = c.Roles
                    .Select(r => new ContributionEditor.ContributorRoleDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .ToList()
            })
            .ToList();
    }

    // ----------------------------
    // SEARCH (Autocomplete)
    // ----------------------------

    private Task<IEnumerable<GenreDto>> SearchGenres(string value, CancellationToken token)
    {
        // Hvis tom → vis alle
        var result = string.IsNullOrWhiteSpace(value)
            ? _genres
            : _genres.Where(g =>
                g.Name.Contains(value, StringComparison.OrdinalIgnoreCase));

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

    // ----------------------------
    // UI EVENTS
    // ----------------------------

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

    // ----------------------------
    // SAVE
    // ----------------------------

    private async Task UpdateTrack()
    {
        _errorMessage = null;
        _successMessage = null;

        // Basic validation
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

            // Mapper contributions tilbage til request
            _model.Contributions = _contributions
                .Where(c => c.ContributorId.HasValue && c.Roles.Any())
                .Select(c => new UpdateContributionRequest
                {
                    ContributorId = c.ContributorId!.Value,
                    RoleIds = c.Roles.Select(r => r.Id).ToList()
                })
                .ToList();

            var response = await Http.PutAsJsonAsync($"api/track/{Id}", _model);

            if (response.IsSuccessStatusCode)
            {
                _successMessage = "Track updated successfully.";

                // Naviger væk → derfor ser du sjældent success message
                Navigation.NavigateTo($"/tracks/{Id}");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();

                _errorMessage = string.IsNullOrWhiteSpace(error)
                    ? "Failed to update track."
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
}