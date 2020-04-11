using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroApp.Wasm.Shared.Components
{
    public class FiComponentBase: ComponentBase
    {
        [Parameter]
        public int Size { get; set; } = 24;

        [Parameter]
        public string StrokeColor { get; set; } = "#e02041";
    }
}
