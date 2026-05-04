using FrontendThrowAway.Components.Pages.MusicCatalog.MusicReleases.Models;
using FrontendThrowAway.Components.Shared;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;

namespace FrontendThrowAway.Components.Pages.MusicCatalog.MusicReleases.Details
{
    public partial class ReleaseDetails
    {

        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        [Parameter]
        public Guid Id { get; set; }

        private ReleaseDetailsDto? _release;
        private bool _isLoading = true;
        private string? _errorMessage;

        private List<ContributionDisplay.ContributionListItem> _contributionItems = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _release = await Http.GetFromJsonAsync<ReleaseDetailsDto>($"api/release/{Id}");

                if (_release is not null)
                {
                    _contributionItems = _release.Contributions
                        .Select(c => new ContributionDisplay.ContributionListItem
                        {
                            ContributorName = c.ContributorName,
                            Roles = c.Roles
                                        .Select(r => new ContributionDisplay.ContributionListRole
                                        {
                                            Id = r.Id,
                                            Name = r.Name
                                        })
            .ToList()
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                _errorMessage = $"Could not fetch release: {ex.Message}";
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task DeleteRelease(Guid id)
        {
            try
            {
                var response = await Http.DeleteAsync($"api/release/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Navigation.NavigateTo("/releases");
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

        private string FormatDuration(int? durationSeconds)
        {
            if (durationSeconds is null || durationSeconds <= 0)
                return "00:00";

            var ts = TimeSpan.FromSeconds(durationSeconds.Value);

            if (ts.TotalHours >= 1)
                return ts.ToString(@"hh\:mm\:ss");

            return ts.ToString(@"mm\:ss");
        }

        private string FormatDateOnly(DateTime? value)
        {
            return value?.ToString("dd-MM-yyyy") ?? "-";
        }

        private string DisplayTrackSubtitle(ReleaseTrackDto track)
        {
            if (!string.IsNullOrWhiteSpace(track.ISRC))
                return track.ISRC;

            if (!string.IsNullOrWhiteSpace(track.Version))
                return track.Version;

            return "No ISRC";
        }


    }
}
