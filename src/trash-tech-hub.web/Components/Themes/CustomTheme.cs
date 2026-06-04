using TrashTechHub.Core.DTOs;
using MudBlazor;

namespace TrashTechHub.Web.Components.Themes;

public static class CustomTheme
{
    public static MudTheme mudTheme = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#FF6B35",           // Bright Tech Orange
            Secondary = "#374151",         // Dark slate grey (contrast elements)
            Tertiary = "#2C3E50",          // Slate Blue (accent)
            Background = "#F0F2F5",        // Icy grey-white page background
            AppbarBackground = "#FFFFFF",
            AppbarText = "#1A1D21",
            Surface = "#FFFFFF",           // White cards
            TextPrimary = "#1A1D21",       // Near-black for readability
            TextSecondary = "#6B7280",     // Muted grey
            DrawerBackground = "#FFFFFF",
            ActionDefault = "#FF6B35",
            LinesDefault = "#E5E7EB",      // Light grey borders
            Divider = "#E5E7EB"
        },
        PaletteDark = new PaletteDark()
        {
            Primary = "#FF6B35",
            Secondary = "#374151",
            Tertiary = "#2C3E50",
            Background = "#F0F2F5",
            AppbarBackground = "#FFFFFF",
            AppbarText = "#1A1D21",
            Surface = "#FFFFFF",
            TextPrimary = "#1A1D21",
            TextSecondary = "#6B7280",
            DrawerBackground = "#FFFFFF",
            ActionDefault = "#FF6B35",
            LinesDefault = "#E5E7EB",
            Divider = "#E5E7EB"
        },
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "6px",
        },
        Typography = new Typography()
        {
            Default = new DefaultTypography()
            {
                FontFamily = new[] { "Outfit", "sans-serif" }
            }
        }
    };
}
