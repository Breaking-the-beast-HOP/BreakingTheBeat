using Frontend.Components.Pages.MusicCatalog.SharedModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Frontend.Components.Pages.MusicCatalog.Tracks.List
{

    public partial class Tracks
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;
        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        private List<TrackDto> _tracks = new();
        private bool isLoading = true;
        private string? errorMessage;

        private int _currentPage = 0;


        private string _searchString = string.Empty;
        private string? _selectedGenreFilter;
        private string? _selectedLanguageFilter;

        private IEnumerable<TrackDto> FilteredTracks =>
            _tracks
                .Where(t =>
                    string.IsNullOrWhiteSpace(_searchString) ||
                    (t.Title?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (t.Version?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (t.ISRC?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false))
                .Where(t =>
                    string.IsNullOrWhiteSpace(_selectedGenreFilter) ||
                    t.GenreName == _selectedGenreFilter)
                .Where(t =>
                    string.IsNullOrWhiteSpace(_selectedLanguageFilter) ||
                    t.AudioLanguageName == _selectedLanguageFilter)
                .OrderBy(t => t.Title);

        private void ClearFilters()
        {
            _searchString = string.Empty;
            _selectedGenreFilter = null;
            _selectedLanguageFilter = null;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _tracks = await Http.GetFromJsonAsync<List<TrackDto>>("api/track")
                         ?? new List<TrackDto>();
            }
            catch (Exception ex)
            {
                errorMessage = $"Could not fetch tracks: {ex.Message}";
            }
            finally
            {
                isLoading = false;
            }
        }

        private string FormatDuration(int durationSeconds)
        {
            var ts = TimeSpan.FromSeconds(durationSeconds);
            return ts.ToString(@"mm\:ss");
        }

        private void OnRowClicked(TableRowClickEventArgs<TrackDto> args)
        {
            Navigation.NavigateTo($"/tracks/{args.Item.Id}");
        }

        private void GoToEdit(Guid id)
        {
            Navigation.NavigateTo($"/tracks/edit/{id}");
        }
        private async Task DeleteTrack(Guid id)
        {
            try
            {
                var response = await Http.DeleteAsync($"api/track/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var trackToRemove = _tracks.FirstOrDefault(t => t.Id == id);
                    if (trackToRemove is not null)
                    {
                        _tracks.Remove(trackToRemove);
                    }
                }
                else
                {
                    errorMessage = $"Could not delete track. Status code: {(int)response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Could not delete track: {ex.Message}";
            }
        }



    }
}
