using HeroApp.AppShared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using HeroApp.Domain;
using HeroApp.Wasm.Shared.Components;
using AutoMapper;

namespace HeroApp.Wasm.Pages
{
    [Authorize]
    public partial class Profile : OwningComponentBase<IHttpService>
    {
        [Inject]
        private IMapper mapper { get; set; }

        [Inject]
        private Radzen.DialogService DialogService { get; set; }

        [Inject]
        private ILocalStorageService LocalStorageService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }


        public IEnumerable<Incident> Incidents { get; set; } = new List<Incident>();

        protected override async Task OnInitializedAsync()
        {
            var response = await Service.Get<ApiResponse<IEnumerable<AppShared.Profile.GetProfile.Result>>>("/api/profile");
            if (response.Succeeded)
            {
                Incidents =  mapper.Map<IEnumerable<Domain.Incident>>(response.Result);
            }
        }



        async Task handleDeleteIncident(long id)
        {
            var confirm = await DialogService.OpenAsync<SimpleConfirm>(
                "Delete for good!",
                new Dictionary<string, object>() { { "description", new string[] {
                    "Do you really want to delete this incident?",
                    "This action can not be undone!"
                } } });

            if ((bool)confirm)
            {

                var response = await Service.Delete<ApiResponse<AppShared.Incident.Delete.Result>>("/api/incidents", $"/{id}");

                if (!response.Succeeded)
                {
                    var dialogResponse = await DialogService.OpenAsync<SimpleConfirm>("Error", new Dictionary<string, object>() { { "description", response.Errors.ToArray() } });
                }
            }

        }

        async Task HandleLogout()
        {

            await LocalStorageService.ClearAsync();
            NavigationManager.NavigateTo("/");

        }

    }
}
