using Microsoft.AspNetCore.Components;

namespace FrontendThrowAway.Components.Pages.MusicCatalog.Contributors.Create
{

    public partial class CreateContributor
    {

            [Inject]
            protected HttpClient Http { get; set; } = default!;

            [Inject]
            protected NavigationManager Navigation { get; set; } = default!;

            private ContributorModel _model = new();

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

                    var response = await Http.PostAsJsonAsync("api/contributor", _model);

                    if (response.IsSuccessStatusCode)
                    {
                        _createdContributorId = await response.Content.ReadFromJsonAsync<Guid>();
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

                Navigation.NavigateTo($"/contributors/{_createdContributorId}");
            }

            private void GoToContributors()
            {
                Navigation.NavigateTo("/contributors");
            }

            private void CreateAnother()
            {
                _model = new ContributorModel();
                _createdContributorId = null;
                _showSuccessDialog = false;
                _errorMessage = null;
            }
        
    }
}

