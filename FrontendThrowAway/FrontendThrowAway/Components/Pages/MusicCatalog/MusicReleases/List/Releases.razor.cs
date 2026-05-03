using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FrontendThrowAway.Components.Pages.MusicCatalog.MusicReleases.List
{

    public partial class Releases
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        private List<ReleaseDto> _releases = new();
        private bool _isLoading = true;
        private string? _errorMessage;
        private string _searchString = string.Empty;

        private IEnumerable<ReleaseDto> FilteredReleases =>
            _releases.Where(r =>
                string.IsNullOrWhiteSpace(_searchString) ||
                (r.Title?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (r.EAN?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false));

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _releases = await Http.GetFromJsonAsync<List<ReleaseDto>>("api/release")
                            ?? new List<ReleaseDto>();
            }
            catch (Exception ex)
            {
                _errorMessage = $"Could not fetch releases: {ex.Message}";
            }
            finally
            {
                _isLoading = false;
            }
        }

        private string FormatReleaseYear(DateTime? releaseDate)
        {
            return releaseDate?.Year.ToString() ?? "-";
        }

        private void OnRowClicked(TableRowClickEventArgs<ReleaseDto> args)
        {
            Navigation.NavigateTo($"/releases/{args.Item.Id}");
        }

        private void GoToEdit(Guid id)
        {
            Navigation.NavigateTo($"/releases/edit/{id}");
        }

        private async Task DeleteRelease(Guid id)
        {
            try
            {
                var response = await Http.DeleteAsync($"api/release/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var releaseToRemove = _releases.FirstOrDefault(r => r.Id == id);
                    if (releaseToRemove is not null)
                        _releases.Remove(releaseToRemove);
                }
                else
                {
                    _errorMessage = $"Could not delete release. Status code: {(int)response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                _errorMessage = $"Could not delete release: {ex.Message}";
            }
        }

        public class ReleaseDto
        {
            public Guid Id { get; set; }
            public string? Title { get; set; }
            public string? Version { get; set; }
            public string? EAN { get; set; }
            public DateTime? ReleaseDate { get; set; }

            public string? GenreName { get; set; }
            public string? LanguageName { get; set; }
        }
    }
}
