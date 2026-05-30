using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace Frontend.Components.Pages.PodcastCatalog.Contributors.List;

public partial class Contributors
{
    [Inject]
    protected HttpClient Http { get; set; } = default!;

    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    private List<PodcastContributorDto> _contributors = new();
    private string? _searchString;
    private string? _errorMessage;
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _contributors =
                await Http.GetFromJsonAsync<List<PodcastContributorDto>>("api/podcasts/contributor")
                ?? new List<PodcastContributorDto>();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load podcast contributors: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private IEnumerable<PodcastContributorDto> FilteredContributors =>
        string.IsNullOrWhiteSpace(_searchString)
            ? _contributors
            : _contributors.Where(c =>
                Contains(c.DisplayName, _searchString) ||
                Contains(c.FullName, _searchString) ||
                Contains(c.Title, _searchString) ||
                Contains(c.Expertise, _searchString) ||
                Contains(c.Biography, _searchString));

    private static bool Contains(string? value, string search)
    {
        return !string.IsNullOrWhiteSpace(value)
            && value.Contains(search, StringComparison.OrdinalIgnoreCase);
    }

    private void EditContributor(Guid id)
    {
        Navigation.NavigateTo($"/podcast-contributors/{id}/edit");
    }

    private async Task DeleteContributor(Guid id)
    {
        // Implementér først når DELETE endpointet findes
        await Task.CompletedTask;
    }

    private class PodcastContributorDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Title { get; set; }
        public string? Biography { get; set; }
        public string? Expertise { get; set; }
        public string? ProfileUrl { get; set; }
    }
}