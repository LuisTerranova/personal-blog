using MudBlazor;

namespace personal_blog.front.Themes;

public static class CustomTheme
{
    public static MudTheme mudTheme = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#E43D12",
            Secondary = "#D6536D",
            Tertiary = "#EFB11D",
            Background = "#EBE9E1",
            AppbarBackground = "#EBE9E1",
        },
        PaletteDark = new PaletteDark()
        {

        },

        LayoutProperties = new LayoutProperties()
        {
            DrawerMiniWidthLeft = "260px",
            DrawerMiniWidthRight = "300px",
        },
        
        Typography = new  Typography()
        {
            Default = new DefaultTypography()
            {
                FontFamily = new[] { "Outfit", "sans-serif"}
            }
        }
    };
}