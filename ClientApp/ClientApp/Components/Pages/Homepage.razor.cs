
using ClientApp.Models;
using ClientApp.Services;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Components.Pages
{
    public partial class Homepage
    {
        [Inject] HouseService HouseService { get; set; } = null!;
        public IEnumerable<House> Houses { get; set; } = null!;
        public bool HasNoHouses => (Houses?.Count() ?? 0 ) == 0;
        protected override async Task OnInitializedAsync()
        {
            await RefreshHousesAsync();
            await base.OnInitializedAsync();
        }

        private async Task RefreshHousesAsync() => 
            Houses = await HouseService.GetHousesAsync();
    }
}
