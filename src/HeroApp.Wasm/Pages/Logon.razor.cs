using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroApp.AppShared.Authentication.Login;
using HeroApp.AppShared.Services;
using HeroApp.Wasm.Shared;
using HeroApp.Wasm.Shared.Components;
using Microsoft.AspNetCore.Components;

namespace HeroApp.Wasm.Pages
{

    public partial class Logon : OwningComponentBase<IHttpService>
    {
        private bool isLoading = false;
        private Command command = new Command();

       
       
        protected override void OnInitialized()
        {
            Service.IsMakingRequest += (sender, evnt) =>
            {
                isLoading = evnt.IsLoading;
                DebugPrint.Log("IsMakingRequest: " + evnt.IsLoading.ToString());

            };

        }
        //private RenderFragment ShowSpinner(bool yes)
        //{
        //    var icon = new RenderFragment((builder) => {
        //        builder.OpenComponent(1, typeof(Spinner));
        //        builder.AddAttribute(2,"Size", "24");
        //        builder.CloseComponent();               
                
        //        });
        //    DebugPrint.Log("Show Spinner: " + yes.ToString());
           
        //    return yes ? icon : null;


        //}

        private async Task HandleValidSubmit()
        {

            DebugPrint.Log("HandleValidSubmit clicked");
            isLoading = true;
            StateHasChanged();
           // ShowSpinner(true);
            //await Task.Delay(2500);
            //ShowSpinner(false);
            //DebugPrint.Log("HandleValidSubmit Fim");


        }
    }
}
