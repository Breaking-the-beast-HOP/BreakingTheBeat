using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace Frontend.Components.Pages.PodcastCatalog.Contributors.Create;

public partial class CreateContributor
{
    [Inject]
    protected HttpClient Http { get; set; } = default!;

    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    private PodcastContributorModel _model = new();

    private bool _isSaving;
    private bool _showSuccessDialog;

    private Guid? _createdContributorId;
    private string? _errorMessage;

    private async Task SaveContributor()
    {
        _errorMessage = null;

        if (string.IsNullOrWhiteSpace(_model.DisplayName))
        {
            _errorMessage = "Display name is required.";
            return;
        }

        try
        {
            _isSaving = true;

            var response = await Http.PostAsJsonAsync("api/podcasts/Contributor", _model);

            if (response.IsSuccessStatusCode)
            {
                _createdContributorId =
                    await response.Content.ReadFromJsonAsync<Guid>();

                _showSuccessDialog = true;
                return;
            }

            var error = await response.Content.ReadAsStringAsync();

            _errorMessage = string.IsNullOrWhiteSpace(error)
                ? "Failed to save contributor."
                : error;
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

    private void GoToCreatedContributor()
    {
        if (_createdContributorId is null)
            return;

        Navigation.NavigateTo($"/podcast-contributors/{_createdContributorId}");
    }

    private void GoToContributors()
    {
        Navigation.NavigateTo("/podcast-contributors");
    }

    private void CreateAnother()
    {
        _model = new PodcastContributorModel();

        _createdContributorId = null;
        _showSuccessDialog = false;
        _errorMessage = null;
    }
}
public class PodcastContributorModel
{
    public string DisplayName { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? Title { get; set; }
    public string? Biography { get; set; }
    public string? Expertise { get; set; }
    public string? ProfileUrl { get; set; }
}