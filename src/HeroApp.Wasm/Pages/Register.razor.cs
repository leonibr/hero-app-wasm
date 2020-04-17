using HeroApp.AppShared.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroApp.AppShared.Authentication.Register;
using HeroApp.Domain;
using HeroApp.Wasm.Shared;
using HeroApp.Wasm.Shared.Components;

namespace HeroApp.Wasm.Pages
{
    public partial class Register : OwningComponentBase<IHttpService>
    {

        [Inject] Radzen.DialogService DialogService { get; set; }
        [Inject] NavigationManager Navigator { get; set; }
        [Inject] ITokenService TokenService { get; set; }

        public bool isLoading = false;
        Command registerCommand = new Command()
        {
            Name = "ASDF",
            Email = "asdf@asdf.com",
            City = "sadf",
            ConfirmPassword = "asdfasdf",
            Password = "asdfasdf",
            State = "SS",
            Whatsapp = "asdfasdf"
        };


        void OnMakingRequest(object sender, IsMakingRequestEventArgs args)
        {
            isLoading = args.IsLoading;
            StateHasChanged();
            Console.WriteLine("isLoading> " + isLoading.ToString());
        }

        protected override async Task OnInitializedAsync()
        {
            Service.IsMakingRequest += OnMakingRequest;
            await Task.CompletedTask;
        }

        async Task HandleRegister()
        {

            var apiResponse = await Service.Post<Command, ApiResponse<Result>>(registerCommand, "/api/authenticate/register");


            if (apiResponse.Succeeded)
            {
                DebugPrint.Log(apiResponse);
                await TokenService.StoreToken(new AppShared.Model.LocalUserInfo()
                {
                    Id = apiResponse.Result.UserId,
                    AccessToken = apiResponse.Result.AccessToken
                });




                try
                {

                    var response = await DialogService.OpenAsync<SimpleConfirm>($"Your access is granted!",
                         new Dictionary<string, object>() { { "Description", new string[] {
              //  $"The Id: {apiResponse.Result.UserId} is your identifier",
                "You will need create your incidents. You will be redirected to your Profile page.",
                "Keep your password safe!"
             }  } }
                        );

                    if((bool)response)
                    {
                        Navigator.NavigateTo("/profile", forceLoad: true);
                    } else
                    {
                        Navigator.NavigateTo("/", forceLoad: true);

                    }

                }
                catch (Exception ex)
                {
                    DebugPrint.Log("Error on HandleRegister.");
                    DebugPrint.Log($"HandleRegister:: [] \n{ex.Message}");

                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    isLoading = false;
                }
            }
            else
            {
                var response = await DialogService.OpenAsync<SimpleConfirm>($"There was a problem!",
                         new Dictionary<string, object>() { { "Description", apiResponse.Message.Split('\n').ToArray() } }
        );
            }
        }

        public void Dispose()
        {
            Service.IsMakingRequest -= OnMakingRequest;
        }
    }
}
