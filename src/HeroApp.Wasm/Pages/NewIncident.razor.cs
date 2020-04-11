using HeroApp.AppShared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroApp.AppShared.Incident.NewIncident;
using HeroApp.Wasm.Shared;
using HeroApp.Domain;
using HeroApp.Wasm.Shared.Components;

namespace HeroApp.Wasm.Pages
{
    public partial class NewIncident: OwningComponentBase<IHttpService>
    {
        [Inject]
        public Radzen.DialogService DialogService { get; set; }
        [Inject]
        public NavigationManager Navigator { get; set; }
        public Command Command { get; set; } = new Command();
        public bool IsLoading { get; set; } = false;



        public async Task HandleSubmit ()
        {
            DebugPrint.Log("HandleSubmit Clicked");
            var apiResponse = await Service.Post<Command, ApiResponse<Result>>(Command, "/api/incidents");
            
            if (apiResponse.Succeeded)
            {
                DebugPrint.Log("HandleSubmit Clicked");
                Navigator.NavigateTo("/profile");
            } else
            {
                var response = await DialogService.OpenAsync<SimpleConfirm>($"Something is wrong!",
     new Dictionary<string, object>() { { "Description", apiResponse.Errors  } }
    );
            }
        }
    }
}
