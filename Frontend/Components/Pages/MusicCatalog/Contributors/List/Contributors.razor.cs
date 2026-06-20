using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Frontend.Components.Pages.MusicCatalog.Contributors.List
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
        private ContributorFilter _filter = ContributorFilter.All;

        private IEnumerable<ContributorDto> FilteredContributors
        {
            get
            {
                var contributors = _contributors.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(_searchString))
                {
                    contributors = contributors.Where(c =>
                        Contains(c.DisplayName, _searchString) ||
                        Contains(c.LegalName, _searchString) ||
                        Contains(c.IPI, _searchString) ||
                        Contains(c.ISNI, _searchString));
                }

                contributors = _filter switch
                {
                    ContributorFilter.WithIPI => contributors
                        .Where(c => !string.IsNullOrWhiteSpace(c.IPI)),

                    ContributorFilter.WithISNI => contributors
                        .Where(c => !string.IsNullOrWhiteSpace(c.ISNI)),

                    ContributorFilter.MissingIdentifiers => contributors
                        .Where(c =>
                            string.IsNullOrWhiteSpace(c.IPI) ||
                            string.IsNullOrWhiteSpace(c.ISNI)),

                    _ => contributors
                };

                return contributors;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _contributors =
                    await Http.GetFromJsonAsync<List<ContributorDto>>("api/contributor")
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
            var confirm = await JS.InvokeAsync<bool>(
                "confirm",
                "Are you sure you want to delete this contributor?");

            if (!confirm)
                return;

            try
            {
                var response = await Http.DeleteAsync($"api/contributor/{id}");

                if (response.IsSuccessStatusCode)
                {
                    _contributors = _contributors
                        .Where(c => c.Id != id)
                        .ToList();
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

        private static bool Contains(string? value, string search)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                   value.Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        private static string DisplayValue(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value;
        }

        private static string GetInitials(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "?";

            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
                return parts[0][0].ToString().ToUpper();

            return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
        }

        private enum ContributorFilter
        {
            All,
            WithIPI,
            WithISNI,
            MissingIdentifiers
        }
    }
}