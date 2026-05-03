using FrontendThrowAway.Components.Shared;
using FrontendThrowAway.Components.Pages.MusicCatalog.MusicReleases.Models;
using FrontendThrowAway.Components.Pages.MusicCatalog.SharedModels;
using Microsoft.AspNetCore.Components;

namespace FrontendThrowAway.Components.Pages.MusicCatalog.Contributors.Create
{

    public partial class CreateContributor
    {

        [Inject]
        protected HttpClient Http { get; set; } = default!;

        private ContributorModel _model = new();
        private bool _isSaving = false;
        private string? _errorMessage;
        private string? _successMessage;

        private async Task SaveContributor()
        {
            _errorMessage = null;
            _successMessage = null;

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
                    _successMessage = "Contributor created successfully.";
                    _model = new ContributorModel();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _errorMessage = string.IsNullOrWhiteSpace(error)
                        ? "Failed to save contributor."
                        : error;
                }
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


    }
}
