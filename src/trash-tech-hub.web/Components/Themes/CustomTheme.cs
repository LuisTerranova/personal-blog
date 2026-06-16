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
            Secondary = "#4B5563",
            Tertiary = "#4F46E5",
            Background = "#0B0F19",
            AppbarBackground = "#0B0F19",
            AppbarText = "#F3F4F6",
            Surface = "#111827",
            TextPrimary = "#F3F4F6",
            TextSecondary = "#9CA3AF",
            DrawerBackground = "#111827",
            ActionDefault = "#FF6B35",
            LinesDefault = "#1F2937",
            Divider = "#1F2937"
        },
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "6px",
        },
        Typography = new Typography()
        {
            Default = new DefaultTypography()
            {
                FontFamily = new[] { "Inter", "sans-serif" },
                FontSize = "0.95rem",
                FontWeight = "400",
                LineHeight = "1.6"
            },
            Body1 = new Body1Typography()
            {
                FontFamily = new[] { "Inter", "sans-serif" },
                FontSize = "0.95rem",
                FontWeight = "400",
                LineHeight = "1.6"
            },
            Body2 = new Body2Typography()
            {
                FontFamily = new[] { "Inter", "sans-serif" },
                FontSize = "0.875rem",
                FontWeight = "400",
                LineHeight = "1.5"
            },
            Subtitle1 = new Subtitle1Typography()
            {
                FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" },
                FontSize = "1rem",
                FontWeight = "500",
                LineHeight = "1.5"
            },
            Subtitle2 = new Subtitle2Typography()
            {
                FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" },
                FontSize = "0.875rem",
                FontWeight = "500",
                LineHeight = "1.55"
            },
            Caption = new CaptionTypography()
            {
                FontFamily = new[] { "Inter", "sans-serif" },
                FontSize = "0.75rem",
                FontWeight = "400",
                LineHeight = "1.4"
            },
            Overline = new OverlineTypography()
            {
                FontFamily = new[] { "Inter", "sans-serif" },
                FontSize = "0.75rem",
                FontWeight = "500",
                LineHeight = "2",
                LetterSpacing = "0.15em"
            },
            H1 = new H1Typography()
            {
                FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" },
                FontSize = "3rem",
                FontWeight = "800",
                LineHeight = "1.05",
                LetterSpacing = "-0.02em"
            },
            H2 = new H2Typography()
            {
                FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" },
                FontSize = "2.25rem",
                FontWeight = "800",
                LineHeight = "1.1",
                LetterSpacing = "-0.015em"
            },
            H3 = new H3Typography()
            {
                FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" },
                FontSize = "1.75rem",
                FontWeight = "700",
                LineHeight = "1.15",
                LetterSpacing = "-0.01em"
            },
            H4 = new H4Typography()
            {
                FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" },
                FontSize = "1.35rem",
                FontWeight = "700",
                LineHeight = "1.2",
                LetterSpacing = "-0.005em"
            },
            H5 = new H5Typography()
            {
                FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" },
                FontSize = "1.1rem",
                FontWeight = "700",
                LineHeight = "1.25"
            },
            H6 = new H6Typography()
            {
                FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" },
                FontSize = "0.95rem",
                FontWeight = "600",
                LineHeight = "1.3"
            },
            Button = new ButtonTypography()
            {
                FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" },
                FontWeight = "700",
                LetterSpacing = "0.04em"
            }
        }
    };
}
