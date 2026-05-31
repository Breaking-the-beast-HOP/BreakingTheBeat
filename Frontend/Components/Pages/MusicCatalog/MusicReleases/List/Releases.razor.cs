using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Frontend.Components.Pages.MusicCatalog.MusicReleases.List
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

        private string? _selectedGenreFilter;
        private string? _selectedStatusFilter;
        private int _currentPage = 0;

        private IEnumerable<ReleaseDto> FilteredReleases =>
            _releases.Where(r =>
                (string.IsNullOrWhiteSpace(_searchString) ||
                 (r.Title?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                 (r.EAN?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false)) &&
                (string.IsNullOrWhiteSpace(_selectedGenreFilter) || r.GenreName == _selectedGenreFilter) &&
                (string.IsNullOrWhiteSpace(_selectedStatusFilter) ||
                 r.Status.ToString() == _selectedStatusFilter));

        private void ClearFilters()
        {
            _searchString = string.Empty;
            _selectedGenreFilter = null;
            _selectedStatusFilter = null;
        }

        private static string GetStatusLabel(string status) => status switch
        {
            "Draft" => "Draft",
            "Submitted" => "Submitted to QC",
            "InQC" => "In QC",
            "Approved" => "Approved",
            "Rejected" => "Rejected",
            _ => status
        };

        private static (string icon, Color color, string label, string tooltip) GetStatusDisplay(ReleaseStatus status)
            => status switch
            {
                ReleaseStatus.Draft => (Icons.Material.Filled.Edit, Color.Warning, "Draft", "Kladde – kræver handling før indsendelse"),
                ReleaseStatus.Submitted => (Icons.Material.Filled.Send, Color.Info, "Submitted", "Indsendt til QC – afventer behandling"),
                ReleaseStatus.InQC => (Icons.Material.Filled.Lock, Color.Secondary, "In QC", "Låst under kvalitetskontrol – afvent"),
                ReleaseStatus.Approved => (Icons.Material.Filled.CheckCircle, Color.Success, "Approved", "Godkendt – alt er i orden"),
                ReleaseStatus.Rejected => (Icons.Material.Filled.Cancel, Color.Error, "Rejected", "Afvist – rettelser krævet før genindsendelse"),
                _ => (Icons.Material.Filled.Help, Color.Default, "Unknown", "")
            };

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

     
        public enum ReleaseStatus
        {
            Draft,
            Submitted,
            InQC,
            Approved,
            Rejected
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
            public ReleaseStatus Status { get; set; } = ReleaseStatus.Draft; 
        }
    }
}
