using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FrontendThrowAway.Components.Pages.MusicCatalog.Contributors.list
{
    public partial class Contributors
    {

        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        [Inject] 
        private IJSRuntime JS { get; set; } = default!;
        
        private List<ContributorDto> _contributors = new();
        private bool isLoading = true;
        private string? errorMessage;
        private string _searchString = string.Empty;

        private IEnumerable<ContributorDto> FilteredContributors =>
            _contributors.Where(c =>
                string.IsNullOrWhiteSpace(_searchString) ||
                (c.DisplayName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.LegalName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ?? false));

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _contributors = await Http.GetFromJsonAsync<List<ContributorDto>>("api/contributor")
                               ?? new();
            }
            catch (Exception ex)
            {
                errorMessage = $"Could not fetch contributors: {ex.Message}";
            }
            finally
            {
                isLoading = false;
            }
        }

        private void EditContributor(Guid id)
        {
            Navigation.NavigateTo($"/contributors/{id}/edit");
        }

        private async Task DeleteContributor(Guid id)
        {
            var confirm = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this contributor?");
            if (!confirm) return;

            try
            {
                var response = await Http.DeleteAsync($"api/contributor/{id}");

                if (response.IsSuccessStatusCode)
                {
                    _contributors = _contributors.Where(c => c.Id != id).ToList();
                }
                else
                {
                    errorMessage = "Failed to delete contributor.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        } 
    }
}
