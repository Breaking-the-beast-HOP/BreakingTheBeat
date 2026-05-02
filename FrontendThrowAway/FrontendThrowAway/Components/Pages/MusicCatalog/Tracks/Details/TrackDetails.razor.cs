using FrontendThrowAway.Components.Shared;
using FrontendThrowAway.Components.Pages.MusicCatalog.SharedModels;
using Microsoft.AspNetCore.Components;

namespace FrontendThrowAway.Components.Pages.MusicCatalog.Tracks.Details
{
    public partial class TrackDetails
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;
        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;
        
        [Parameter]
        public Guid Id { get; set; }

        private TrackDetailsDto? _track;
        private bool _isLoading = true;
        private string? _errorMessage;

        private List<ContributionDisplay.ContributionListItem> _contributionItems = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _track = await Http.GetFromJsonAsync<TrackDetailsDto>($"api/track/{Id}");

                if (_track is not null)
                {
                    _contributionItems = _track.Contributions
                        .Select(c => new ContributionDisplay.ContributionListItem
                        {
                            ContributorId = c.ContributorId,
                            ContributorName = c.ContributorName,
                            Roles = c.Roles.Select(r => new ContributionDisplay.ContributionListRole
                            {
                                Id = r.Id,
                                Name = r.Name
                            }).ToList()
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                _errorMessage = $"Could not fetch track: {ex.Message}";
            }
            finally
            {
                _isLoading = false;
            }
        }

        private string FormatDuration(int durationSeconds)
        {
            var ts = TimeSpan.FromSeconds(durationSeconds);
            return ts.ToString(@"mm\:ss");
        }

        private string FormatDate(DateTime? value)
        {
            return value?.ToString("dd-MM-yyyy HH:mm") ?? "-";
        }

        private string? errorMessage;
        private async Task DeleteTrack(Guid id)
        {
            try
            {
                var response = await Http.DeleteAsync($"api/track/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Navigation.NavigateTo("/tracks");
                }
                else
                {
                    _errorMessage = $"Could not delete track. Status code: {(int)response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                _errorMessage = $"Could not delete track: {ex.Message}";
            }
        }
    }
}
